import os
import requests
import pyotp
import qrcode

from cs50 import SQL
from flask import Flask, flash, redirect, render_template, request, session
from flask_session import Session
from tempfile import mkdtemp
from werkzeug.exceptions import default_exceptions, HTTPException, InternalServerError
from werkzeug.security import check_password_hash, generate_password_hash
from datetime import datetime
from helpers import apology, login_required, lookup, usd
import SQL as SQLQuery


# Configure application
app = Flask(__name__)


# Ensure templates are auto-reloaded
app.config["TEMPLATES_AUTO_RELOAD"] = True


# Ensure responses aren't cached
@app.after_request
def after_request(response):
    response.headers["Cache-Control"] = "no-cache, no-store, must-revalidate"
    response.headers["Expires"] = 0
    response.headers["Pragma"] = "no-cache"
    return response


# Custom filter
app.jinja_env.filters["usd"] = usd


# Configure session to use filesystem (instead of signed cookies)
app.config["SESSION_FILE_DIR"] = mkdtemp()
app.config["SESSION_PERMANENT"] = False
app.config["SESSION_TYPE"] = "filesystem"
Session(app)


# Configure CS50 Library to use SQLite database
db = SQL("sqlite:///finance.db")


# Make sure API key is set
if not os.environ.get("API_KEY"):
    raise RuntimeError("API_KEY not set")


def validateLogin():
    if isEmptyLoginInformation():
        return apology("must provide username/password", 403)

    rows = getExistentUser()
    if not isUserPasswordValid(rows, request.form.get("password")):
        return apology("invalid username and/or password", 403)

    user2FACode = request.form.get("2FA")
    person2FAKey = get2FAKey(rows[0]["id"])
    if person2FAKey != None:
        if user2FACode != "":
            if not is2FAKeyValid(person2FAKey, user2FACode):
                return apology("invalid 2FA code", 403)
        else:
            return apology("You must provide your 2FA code to have access", 403)
    elif user2FACode != "":
        return apology("You have insert 2FA validation but your account has no 2FA autentication", 403)

    setSessionId(rows[0]["id"])
    return redirect("/")


def isEmptyLoginInformation():
    return (not request.form.get("username")) or (not request.form.get("password"))


def getExistentUser():
    return db.execute(SQLQuery.getSQLExistentUser(), request.form.get("username"))


def isUserPasswordValid(rows, password):
    return (len(rows) == 1) and (check_password_hash(rows[0]["hash"], password))


def setSessionId(id):
    session["user_id"] = id
    return


def hasEnoughtMoney(latestPrice, qtStock):
    userCash = getUserCash()
    return (latestPrice * qtStock) <= userCash


def insertStock(stock, qtStock):
    db.execute(SQLQuery.getSQLInsertMarketShares(), stock["symbol"], stock["name"], qtStock, stock["price"], session["user_id"])
    return


def buyUserStock(stock, qtStock):
    if userHasStock(stock["symbol"]):
        updateUserStock(stock["symbol"], qtStock)
        updateUserAVGPrice(stock["symbol"], stock["price"], qtStock)
        updateUserCash(qtStock, -stock["price"])
    else:
        insertStock(stock, qtStock)
        updateUserCash(qtStock, -stock["price"])

    saveHistory(stock["symbol"], stock["price"], qtStock, 0)
    return


def userHasStock(symbol):
    rows = db.execute(SQLQuery.getSQLUserHasStock(), symbol, session["user_id"])
    return rows[0]["COUNT"] > 0


def updateUserAVGPrice(symbol, latestPrice, qtStock):
    userStock = getUserStock(symbol)
    userPriceAVG = (userStock["PRICE"] + (latestPrice * qtStock)) / (userStock["SHARES"] + qtStock)
    db.execute(SQLQuery.getSQLUpdateUserAVGPrice(), userPriceAVG, session["user_id"])
    return


def getUserStock(symbol):
    rows = db.execute(SQLQuery.getSQLUSERStock(), symbol, session["user_id"])
    stock = {}
    if len(rows) > 0:
        stock["PRICE"] = rows[0]["price"]
        stock["SHARES"] = rows[0]["shares"]

    return stock


def getAllSymbols():
    # Get all IEB valid Symbols
    response = requests.get(f'https://api.iex.cloud/v1/data/CORE/REF_DATA?token={os.environ.get("API_KEY").strip()}')
    response = response.json()
    symbolList = [{}]

    for value in response:
        if value["symbol"] != '':
            symbol = {}
            symbol["symbol"] = value["symbol"]
            symbolList.append(symbol)

    return symbolList


def getUserSymbols():
    rows = db.execute(SQLQuery.getSQLUSERSymbols(), session['user_id'])

    stockList = [{}]
    for row in rows:
        stock = {}
        stock["symbol"] = row["symbol"]
        stockList.append(stock)

    return stockList


def canSellStock(symbol, qtStock):
    stock = getUserStock(symbol)
    if stock != {}:
        return (stock["SHARES"] > 0) and (stock["SHARES"] >= qtStock)
    else:
        return False


def sellStock(symbol, qtStock, latestPrice):
    updateUserStock(symbol, -qtStock)
    deleteUnusedUserStock(symbol)
    updateUserCash(qtStock, latestPrice)

    saveHistory(symbol, latestPrice, qtStock, 1)
    return


def updateUserStock(symbol, qtStock):
    db.execute(SQLQuery.getSQLUpdateUserStock(), qtStock, session["user_id"], symbol, session["user_id"], symbol)
    return


def deleteUnusedUserStock(symbol):
    stock = getUserStock(symbol)
    if stock["SHARES"] == 0:
        deleteUserShare(symbol)
    return


def deleteUserShare(symbol):
    db.execute(SQLQuery.getSQLDeleteUserShare(), session["user_id"], symbol)
    return


def getUserCash():
    userCash = db.execute(SQLQuery.getSQLUserCash(), session["user_id"])
    return float(userCash[0]["cash"])


def updateUserCash(qtStock, latestPrice):
    userCash = getUserCash()
    userCash += qtStock * latestPrice
    db.execute(SQLQuery.getSQLUpdateUserCash(), userCash, session["user_id"])
    return


def getOperation(operation):
    match operation:
        case 0:
            return "BOUGHT"
        case 1:
            return "SOLD"
        case _:
            raise Exception("operation not implemented in application.py - getOperation(operation)")


def getHistoryList():
    rows = db.execute(SQLQuery.getSQLHistoryList(), session['user_id'])

    historyList = [{}]
    for row in rows:
        history = {}
        history["ID"] = row["id"]
        history["DATE"] = row["date"]
        history["SYMBOL"] = row["symbol"]
        history["PRICE"] = usd(row["price"])

        history["OPERATION"] = getOperation(row["operation"])
        if history["OPERATION"] == 'SOLD':
            history["SHARES"] = -row["shares"]
        else:
            history["SHARES"] = row["shares"]

        history["TOTAL"] = usd(row["price"] * row["shares"])
        historyList.append(history)

    return historyList


def saveHistory(symbol, latestPrice, qtStock, operation):
    db.execute(SQLQuery.getSQLSaveHistory(), datetime.now(), operation, symbol.upper(), latestPrice, qtStock,  session['user_id'])
    return

def getImageFilename():
    return 'static/img/'

def get2FAKey(idUser):
    returningValue = None
    rows = db.execute(SQLQuery.getSQLIs2FAActive(), idUser)
    if rows[0]["COUNT"] > 0:
        returningValue = rows[0]["twofa"]
    return returningValue

def save2FAKey(person2FAKey):
    db.execute(SQLQuery.getSQLSave2FAKey(), person2FAKey, session['user_id'])
    return

def is2FAKeyValid(person2FAKey, app2FACode):
    totp = pyotp.TOTP(person2FAKey)
    return totp.verify(app2FACode)

def delete2FAKey():
    db.execute(SQLQuery.getSQLDelete2FAKey(), session['user_id'])
    return

def removeFilesInFolder(filepath):
    for filename in os.listdir(filepath):
        file = os.path.join(filepath, filename)
        if os.path.isfile(file) or os.path.islink(file):
            os.unlink(file)
        else:
            shutil.rmtree(file)

    return

@app.route("/")
@login_required
def index():
    """Show portfolio of stocks"""

    # Get all stock market shares
    rows = db.execute(SQLQuery.getSQLConsultMarketShares(), session["user_id"])

    # Populate the dictionary with all users market shares
    marketSharesList = [{}]
    for marketShares in rows:
        # Parse de DB info to "stock" Dictionary
        stock = {}
        stock["ID"] = marketShares["id"]
        stock["SYMBOL"] = marketShares["symbol"]
        stock["COMPANY_NAME"] = marketShares["companyName"]

        shares = marketShares["shares"]
        stock["SHARES"] = shares

        price = marketShares["price"]
        stock["PRICE"] = usd(price)
        stock["TOTAL"] = usd(price * shares)

        marketSharesList.append(stock)

    return render_template("portfolio.html", marketSharesList=marketSharesList, cash=getUserCash())


@app.route("/buy", methods=["GET", "POST"])
@login_required
def buy():
    """Buy shares of stock"""
    if request.method == "GET":
        return render_template("buy.html", stockList=getAllSymbols())
    else:
        symbol = request.form.get("symbol")
        qtStock = request.form.get("qtStock")
        if int(qtStock) <= 0:
            flash('Invalid quantity', 'error')
            return redirect("/buy")

        response = lookup(symbol)
        if response:
            if hasEnoughtMoney(float(response["price"]), float(qtStock)):
                buyUserStock(response, float(qtStock))
                flash(f'You have bought {qtStock} {symbol} for {usd(response["price"] * float(qtStock))} successfully')
                return redirect("/")
            
        flash(f'{symbol.upper()} not exists', 'error')
        return redirect("/")


@app.route("/history")
@login_required
def history():
    """Show history of transactions"""
    return render_template("history.html", historyList=getHistoryList())


@app.route("/login", methods=["GET", "POST"])
def login():
    """Log user in"""
    session.clear()

    if request.method == "POST":
        return validateLogin()
    else:
        return render_template("login.html")


@app.route("/logout")
def logout():
    """Log user out"""
    # Forget any user_id
    session.clear()

    # Redirect user to login form
    return redirect("/")


@app.route("/quote", methods=["GET", "POST"])
@login_required
def quote():
    """Get stock quote."""
    if request.method == "GET":
        return render_template("quote.html", stockList=getAllSymbols())
    else:
        searchValue = request.form.get("symbol")
        response = lookup(searchValue)
        if response:
            stock = {}
            stock["symbol"] = response['symbol']
            stock["companyName"] = response['name']
            stock["latestPrice"] = usd(response['price'])
            return render_template("quote.html", searchValue=searchValue, stockList=getAllSymbols(), stock=stock)
        else:
            flash(f'{searchValue.upper()} is not a valid symbol', 'warning')
            return redirect("/quote")


@app.route("/register", methods=["GET", "POST"])
def register():
    """Register user"""
    if request.method == "GET":
        return render_template("register.html")
    else:
        # Query database for username

        # Validate registration
        rows = db.execute(SQLQuery.getSQLLastUserID(), request.form.get("username"))

        if rows[0]['COUNT'] > 0:
            return apology("Existent username. Choose another or log in", 403)

        # Set new UserID
        newUserId = rows[0]['LAST_USER_ID']
        if newUserId != None:
            newUserId =+ 1
        else:
            newUserId = 1

        # Validate passwords
        if request.form.get("password") != request.form.get("confirmation_password"):
            return apology("Passwords must be the same.")

        # Insert new user
        db.execute(SQLQuery.getSQLInsertNewUser(), request.form.get("username"), generate_password_hash(request.form.get("password")))

        # Remember which user has logged in
        session["user_id"] = newUserId

        # Redirect user to home page
        return redirect("/")


@app.route("/sell", methods=["GET", "POST"])
@login_required
def sell():
    """Sell shares of stock"""
    if request.method == "GET":
        return render_template("sell.html", stockList=getUserSymbols())
    else:
        symbol = request.form.get("symbol")
        qtStock = request.form.get("qtStock")
        if int(qtStock) <= 0:
            flash('Invalid quantity', 'error')
            return redirect("/sell")

        response = lookup(symbol)
        if response:
            if userHasStock(symbol):
                if canSellStock(symbol, int(qtStock)):
                    sellStock(symbol, float(qtStock), float(response["price"]))
                    flash(f'You have sold {qtStock} {symbol} for {usd(float(response["price"]) * float(qtStock))} successfully')
                    return redirect("/")
                else:
                    flash('Insufficient shares to sell', 'error')
                    return redirect("/sell")
            else:
                flash(f'You have none {symbol.upper()}', 'warning')
                return redirect("/sell")

        flash('Invalid symbol', 'warning')
        return redirect("/sell")

@app.route("/settings")
@login_required
def settings():
    return render_template("settings.html")

@app.route("/2FA", methods=["GET", "POST"])
@login_required
def twoFA():
    removeFilesInFolder(getImageFilename())
    person2FAKey = get2FAKey(session['user_id'])

    if request.method == "GET":
        if person2FAKey != None:
             return render_template("2FA.html", is2FAActive=True)
        else:
            # Generate uri for user to scan the qrCode
            person2FAKey = pyotp.random_base32()
            uri = pyotp.TOTP(person2FAKey).provisioning_uri(name='C$50 Finance', issuer_name='by Allan')

            # Generate file for load into html
            filename = os.path.join(getImageFilename(), '2FA.jpg')
            qrcode.make(uri).save(filename)

            # Save key to database
            save2FAKey(person2FAKey)

            return render_template("2FA.html", is2FAActive=False)
    else:
        #Post Request
        app2FACode = request.form.get("2FA")
        if is2FAKeyValid(person2FAKey, app2FACode):
            isRemove2FA = request.form.get("remove2FA")
        else:
            flash('Invalid code', 'error')
            return redirect("/2FA")

        if isRemove2FA != None:
            # Continue removing 2FA
            delete2FAKey()
            flash('2FA key was successfully removed!')
        else:
            # Add 2FA to the account
            save2FAKey(person2FAKey)
            flash('2FA key was successfully added!')

        return redirect("/")


def errorhandler(e):
    """Handle error"""
    if not isinstance(e, HTTPException):
        e = InternalServerError()
    return apology(e.name, e.code)


# Listen for errors
for code in default_exceptions:
    app.errorhandler(code)(errorhandler)