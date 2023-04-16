def getSQLExistentUser():
    return ("SELECT * " +
            "  FROM USERS " +
            " WHERE USERS.USERNAME LIKE ?")

def getSQLInsertMarketShares():
    return ("INSERT INTO MARKETSHARES ( " +
               "            SYMBOL," +
               "            COMPANYNAME," +
               "            SHARES," +
               "            PRICE," +
               "            IDUSER) " +
               "     VALUES ( " +
               "            ?," +
               "            ?," +
               "            ?," +
               "            ?," +
               "            ?)")

def getSQLUserHasStock():
    return("SELECT COUNT(*) AS COUNT " +
           "  FROM MARKETSHARES " +
           " WHERE MARKETSHARES.SYMBOL LIKE ? " +
           "   AND MARKETSHARES.IDUSER = ?")

def getSQLUpdateUserAVGPrice():
    return ("UPDATE MARKETSHARES " +
            "   SET PRICE = ? " +
            " WHERE MARKETSHARES.IDUSER = ?")

def getSQLUSERStock():
    return ("SELECT PRICE, " +
            "       SHARES " +
            "  FROM MARKETSHARES " +
            " WHERE MARKETSHARES.SYMBOL LIKE ? " +
            "   AND MARKETSHARES.IDUSER = ?")

def getSQLUSERSymbols():
    return ("SELECT SYMBOL " +
            "  FROM MARKETSHARES " +
            " WHERE MARKETSHARES.IDUSER = ?")

def getSQLUpdateUserStock():
    return ("UPDATE MARKETSHARES " +
            "   SET SHARES = ? + (SELECT SHARES " +
            "                       FROM MARKETSHARES USER_STOCKS " +
            "                      WHERE USER_STOCKS.IDUSER = ? " +
            "                        AND USER_STOCKS.SYMBOL LIKE ?) " +
            " WHERE MARKETSHARES.IDUSER = ? " +
            "   AND MARKETSHARES.SYMBOL LIKE ?")

def getSQLDeleteUserShare():
    return ("DELETE FROM MARKETSHARES " +
            " WHERE MARKETSHARES.IDUSER = ? " +
            "   AND MARKETSHARES.SYMBOL LIKE ?")

def getSQLUserCash():
    return ("SELECT CASH " +
            "  FROM USERS " +
            " WHERE USERS.ID = ?")

def getSQLUpdateUserCash():
    return ("UPDATE USERS " +
            "   SET CASH = ? " +
            " WHERE USERS.ID = ?")

def getSQLHistoryList():
    return ("SELECT ID, " +
            "       DATE, " +
            "       OPERATION, " +
            "       SYMBOL, " +
            "       PRICE, " +
            "       SHARES " +
            "  FROM MARKETSHARESHISTORY " +
            " WHERE IDUSER = ?")

def getSQLSaveHistory():
    return ("INSERT INTO MARKETSHARESHISTORY (" +
            "            DATE, " +
            "            OPERATION, " +
            "            SYMBOL, " +
            "            PRICE, " +
            "            SHARES, " +
            "            IDUSER) " +
            "     VALUES (" +
            "            ?," +
            "            ?," +
            "            ?," +
            "            ?," +
            "            ?," +
            "            ?)")

def getSQLConsultMarketShares():
    return ("SELECT * " +
            "  FROM MARKETSHARES " +
            " WHERE MARKETSHARES.IDUSER = ?")

def getSQLLastUserID():
    return ("SELECT COUNT(*) AS COUNT, " +
            "       MAX(ID) AS LAST_USER_ID " +
            "  FROM USERS " +
            " WHERE USERS.USERNAME = LOWER(?)")

def getSQLInsertNewUser():
    return ("INSERT INTO USERS ( " +
            "            USERNAME, " +
            "            HASH) " +
            "     VALUES (?, " +
            "             ?)")

def getSQLIs2FAActive():
    return ("SELECT TWOFA, " +
            "       COUNT(*) AS COUNT " +
            "  FROM USERS " +
            " WHERE USERS.ID = ? " +
            "   AND TWOFA IS NOT NULL")

def getSQLSave2FAKey():
    return ("UPDATE USERS " +
            "   SET TWOFA = ? " +
            " WHERE USERS.ID = ?")

def getSQLDelete2FAKey():
    return ("UPDATE USERS " +
            "   SET TWOFA = NULL " +
            " WHERE USERS.ID = ?")