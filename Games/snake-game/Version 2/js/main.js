import {} from './input.js'
import { snake, SNAKE_SPEED, getSnakeHead, isOnSnake, isSnakeOutsideCanvas } from './snake.js'
import { food } from './food.js'

const CANVAS = document.getElementById('canvas')
const CTX = CANVAS.getContext('2d')

let lastRenderTime = 0

function animate(currentTime) {
  if (isGameOver()) {
    finishGame()
    return
  }

  requestAnimationFrame(animate)
  const secondsSinceLastRender = (currentTime - lastRenderTime) / 1000
  if (secondsSinceLastRender < 1 / SNAKE_SPEED) return

  lastRenderTime = currentTime

  CTX.clearRect(0, 0, CANVAS.width, CANVAS.height)
  snake.update()
  food.update()
}

animate()

function isGameOver() {
  return isOnSnake({ position: getSnakeHead(), ignoreHead: true, ignoreTailPosition: true }) || isSnakeOutsideCanvas()
}

function finishGame() {
  const gameOverScreen = document.getElementById('game-over')

  const gameOverText = document.createElement('h2')
  gameOverText.innerText = 'Game over...'
  gameOverText.classList.add('game-over')
  gameOverScreen.appendChild(gameOverText)

  const restartButton = document.createElement('button')
  restartButton.innerText = 'Restart'
  restartButton.classList.add('game-over-button')
  restartButton.onclick = () => location.reload()
  gameOverScreen.appendChild(restartButton)
}
