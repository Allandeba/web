import {} from "./input.js"
import { getSnakeDirection } from "./snakeDirection.js"

addEventListener("keydown", (event) => {
  console.log(getSnakeDirection())
})

addEventListener("touchend", (event) => {
  console.log(getSnakeDirection())
})
addEventListener("touchcancel", (event) => {
  console.log(getSnakeDirection())
})
