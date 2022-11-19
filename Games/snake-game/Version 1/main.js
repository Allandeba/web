import {outsideGrid} from './grid.js'
import {update as updateSnake, draw as drawSnake, snakeSpeed, getSnakeHead, snakeIntersection} from './snake.js'
import {update as updateFood, draw as drawFood} from './food.js'
import {update as updateScore, draw as drawScore} from './informations.js'

const gameBoard = document.getElementById('game-board')
const gameOverScreen = document.getElementById('game-over')
let lastRenderTime = 0
let gameOver = false

function mainAnimation(currentTime) {
  if (gameOver) {
    finishGame()
    return
  }

  window.requestAnimationFrame(mainAnimation) 
  const secondsSinceLastRender = (currentTime - lastRenderTime) / 1000
  if (secondsSinceLastRender < 1 / snakeSpeed) return

  lastRenderTime = currentTime

  update()
  draw()
  checkGameOver()
}

requestAnimationFrame(mainAnimation);

function update() {
  document.getElementById('game-board').innerHTML = '';
  updateSnake()
  updateFood()
  updateScore()
}

function draw() {
  drawSnake(gameBoard)
  drawFood(gameBoard)
  drawScore()
}

function checkGameOver() {
  gameOver = (outsideGrid(getSnakeHead()) || snakeIntersection())
}

function finishGame() {
  if (gameOver) {
    const gameOverText = document.createElement('h2')
    gameOverText.innerText = 'Game over...'
    gameOverText.classList.add('game-over')
    gameOverScreen.appendChild(gameOverText)

    const restartButton = document.createElement('button')
    restartButton.innerText = 'Restart'
    restartButton.classList.add('game-over-button')
    restartButton.onclick = () => location.reload()
    gameOverScreen.appendChild(restartButton)

    return
  }
}

