import { Sprite } from './classes.js'
import { getSnakeDirection as getSnakeLastInputDirection } from './snakeInputDirection.js'
import { getSpriteImagePosition, equalPositions, SNAKE_DISTANCE_MOVEMENT } from './snakeImageDirection.js'

const CANVAS = document.getElementById('canvas')
const CTX = CANVAS.getContext('2d')
const CANVAS_CENTER = {
  x: CANVAS.width / 2,
  y: CANVAS.height / 2,
}
export const SNAKE_SPEED = 3

export class Snake extends Sprite {
  constructor() {
    super({
      position: CANVAS_CENTER,
      imageSrc: '../img/snake_sprite.png',
      scale: 0.5,
      framesMax: 0,
      frameColumns: 7,
      frameRows: 2,
    })
    this.singleImageHeight = this.image.height / this.frameRows
    this.singleImageWidth = this.image.width / this.frameColumns
    this.snakeBody = [CANVAS_CENTER]
  }

  draw() {
    super.draw()
    this.snakeBody.forEach((snakeElement, index) => {
      const CURRENT_FRAME_POSITION = getSpriteImagePosition(snakeElement, index)
      if (CURRENT_FRAME_POSITION == null || CURRENT_FRAME_POSITION == undefined) return

      CTX.drawImage(
        this.image,
        this.singleImageWidth * CURRENT_FRAME_POSITION.x,
        this.singleImageHeight * CURRENT_FRAME_POSITION.y,
        this.singleImageWidth,
        this.singleImageHeight,
        snakeElement.x,
        snakeElement.y,
        this.singleImageWidth * this.scale,
        this.singleImageHeight * this.scale
      )
    })
  }

  updateSnakePosition() {
    for (let i = this.snakeBody.length - 2; i >= 0; i--) this.snakeBody[i + 1] = { ...this.snakeBody[i] }
  }

  updateSnakeDirection() {
    const SNAKE_HEAD_DIRECTION = getSnakeLastInputDirection()
    this.snakeBody[0].y += SNAKE_HEAD_DIRECTION.y
    this.snakeBody[0].x += SNAKE_HEAD_DIRECTION.x
  }

  update() {
    this.updateSnakePosition()
    this.updateSnakeDirection()
    super.update()
  }
}

export const snake = new Snake()

export function isOnSnake({ position, height, width, ignoreHead = false, ignoreTailPosition = false } = {}) {
  return snake.snakeBody.some((snakeElement, index) => {
    if (ignoreHead === true && index === 0) return false
    if (ignoreTailPosition === true && index === snake.snakeBody.length - 1) return false
    return equalPositions({ snakeElement: snakeElement, position: position, height: height, width: width })
  })
}

export function incrementSnakeBody() {
  let lastPosition = snake.snakeBody.length - 1
  let snakeTailElement = snake.snakeBody[lastPosition]
  snake.snakeBody.push({ ...snakeTailElement })
}

export function getSnakeHead() {
  return snake.snakeBody[0]
}

export function isSnakeOutsideCanvas() {
  let snakeHead = getSnakeHead()
  let isCrossingOverRightSide = snakeHead.x > CANVAS.width
  let isCrossingOverLeftSide = snakeHead.x + SNAKE_DISTANCE_MOVEMENT < 0
  let isSnakeOutsideCanvasWidth = isCrossingOverRightSide || isCrossingOverLeftSide

  let isCrossingOverBottomSide = snakeHead.y > CANVAS.height
  let isCrossingOverTopSide = snakeHead.y + SNAKE_DISTANCE_MOVEMENT < 0
  let isSnakeOutsideCanvasHeight = isCrossingOverBottomSide || isCrossingOverTopSide

  return isSnakeOutsideCanvasWidth || isSnakeOutsideCanvasHeight
}
