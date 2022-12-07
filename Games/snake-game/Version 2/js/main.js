import {} from './input.js'
import { snake, SNAKE_SPEED } from './snake.js'
import { food } from './food.js'

const CANVAS = document.getElementById('canvas')
const CTX = CANVAS.getContext('2d')

let lastRenderTime = 0

function animate(currentTime) {
  requestAnimationFrame(animate)
  const secondsSinceLastRender = (currentTime - lastRenderTime) / 1000
  if (secondsSinceLastRender < 1 / SNAKE_SPEED) return

  lastRenderTime = currentTime

  CTX.clearRect(0, 0, CANVAS.width, CANVAS.height)
  snake.update()
  food.update()
}

animate()
