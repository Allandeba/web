import {} from './input.js'
import { snake } from './snake.js'

function animate() {
  setTimeout(() => {
    requestAnimationFrame(animate)

    snake.update()
  }, 500)
}

animate()
