import {} from './input.js'
import { snake } from './snake.js'
import { food } from './food.js'

const CANVAS = document.getElementById('canvas')
const CTX = CANVAS.getContext('2d')

function animate() {
  setTimeout(() => {
    requestAnimationFrame(animate)

    CTX.clearRect(0, 0, CANVAS.width, CANVAS.height)
    snake.update()
    food.update()
  }, 500)
}

animate()
