import { Sprite } from './classes.js'
import { isOnSnake, incrementSnakeBody } from './snake.js'

const CANVAS = document.getElementById('canvas')
const CTX = CANVAS.getContext('2d')

class Food extends Sprite {
  constructor() {
    super({
      position: { x: 195, y: 80 },
      imageSrc: '../img/food.png',
      scale: 1,
      framesMax: 0,
      frameColumns: 5,
      frameRows: 1,
    })

    this.width = this.image.width / this.frameColumns
    this.height = this.image.height / this.frameRows
  }

  draw() {
    super.draw()

    CTX.drawImage(
      this.image,
      this.width * this.currentFrame,
      this.height * this.currentFrame,
      this.width,
      this.height,
      this.position.x,
      this.position.y,
      this.width * this.scale,
      this.height * this.scale
    )
  }

  update() {
    if (isOnSnake(this.position)) {
      food.position = getRandomFoodPosition()
      incrementSnakeBody()
    }
    super.update()
  }
}

export const food = new Food()

function getRandomFoodPosition() {
  let newFoodPosition = getRandomCanvasPosition()
  while (newFoodPosition == null || newFoodPosition == undefined || isOnSnake(newFoodPosition, true)) {
    newFoodPosition = getRandomCanvasPosition()
  }

  return newFoodPosition
}

function getRandomCanvasPosition() {
  const CANVAS_MARGIN = 0.3
  return {
    x: Math.floor(Math.random() * (CANVAS.width - CANVAS.width * CANVAS_MARGIN)),
    y: Math.floor(Math.random() * (CANVAS.height - CANVAS.height * CANVAS_MARGIN)),
  }
}
