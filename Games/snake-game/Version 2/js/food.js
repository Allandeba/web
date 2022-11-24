import { Sprite } from './classes.js'
import { isOnSnake } from './snake.js'

const CANVAS = document.getElementById('canvas')
const CTX = CANVAS.getContext('2d')

let foodPosition = getRandomFoodPosition()

class Food extends Sprite {
  constructor() {
    super({
      position: foodPosition,
      imageSrc: '../img/food.png',
      scale: 1.2,
      framesMax: 1,
      frameColumns: 0,
      frameRows: 0,
    })
  }

  draw() {
    super.draw()

    CTX.drawImage(
      this.image,
      this.image.width * this.frameColumns * this.currentFrame,
      this.image.height * this.frameRows * this.currentFrame,
      this.image.width,
      this.image.height,
      this.position.x,
      this.position.y,
      this.image.width * this.scale,
      this.image.height * this.scale
    )
  }

  update() {
    super.update()
    if (isOnSnake(foodPosition)) {
      foodPosition = getRandomFoodPosition()
    }
  }
}

function getRandomFoodPosition() {
  let newFoodPosition

  while (newFoodPosition == null || isOnSnake(newFoodPosition)) newFoodPosition = getRandomCanvasPosition()
  return newFoodPosition
}

function getRandomCanvasPosition() {
  const CANVAS_MARGIN = 0.1
  return {
    x: Math.floor(Math.random() * CANVAS.width) - CANVAS.width * CANVAS_MARGIN,
    y: Math.floor(Math.random() * CANVAS.height) - CANVAS.height * CANVAS_MARGIN,
  }
}

export const food = new Food()
