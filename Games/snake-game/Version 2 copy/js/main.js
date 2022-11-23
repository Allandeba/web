import {} from "./input.js"
import { getSnakeDirection } from "./snakeDirection.js"
import { snake } from "./snake.js"

function animate() {
  setTimeout(() => {
    requestAnimationFrame(animate)

    snake.update()
  }, 1000)
}

animate()
