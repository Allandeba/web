import { Sprite } from './classes.js'
import { getSnakeDirection as getSnakeLastInputDirection } from './snakeInputDirection.js'
import { getSpriteImagePosition } from './snakeImageDirection.js'

const CANVAS = document.getElementById('canvas')
const CTX = CANVAS.getContext('2d')
const SNAKE_INITIAL_POSITION = {
  x: CANVAS.width / 2,
  y: CANVAS.height / 2,
}
const SNAKE_SPRITE_SRC = '../img/snake_sprite.png'
const SNAKE_IMAGE_SCALE = 0.5
const SNAKE_SPRITE_MOVEMENT = 0
const SNAKE_SPRITE_COLUMNS = 7
const SNAKE_SPRITE_ROWS = 2

export let snakeBody = [SNAKE_INITIAL_POSITION]

class Snake extends Sprite {
  constructor() {
    super({
      position: SNAKE_INITIAL_POSITION,
      imageSrc: SNAKE_SPRITE_SRC,
      scale: SNAKE_IMAGE_SCALE,
      framesMax: SNAKE_SPRITE_MOVEMENT,
      frameColumns: SNAKE_SPRITE_COLUMNS,
      frameRows: SNAKE_SPRITE_ROWS,
    })
  }

  draw() {
    super.draw()
    const SINGLE_IMAGE_HEIGHT = this.image.height / this.frameRows
    const SINGLE_IMAGE_WIDTH = this.image.width / this.frameColumns

    snakeBody.forEach((snakeElement, index) => {
      const CURRENT_FRAME_POSITION = getSpriteImagePosition(snakeElement, index)
      if (CURRENT_FRAME_POSITION === null || CURRENT_FRAME_POSITION === undefined) return

      CTX.drawImage(
        this.image,
        SINGLE_IMAGE_WIDTH * CURRENT_FRAME_POSITION.x,
        SINGLE_IMAGE_HEIGHT * CURRENT_FRAME_POSITION.y,
        SINGLE_IMAGE_WIDTH,
        SINGLE_IMAGE_HEIGHT,
        snakeElement.x,
        snakeElement.y,
        SINGLE_IMAGE_WIDTH * this.scale,
        SINGLE_IMAGE_HEIGHT * this.scale
      )
    })
  }

  update() {
    const SNAKE_HEAD_DIRECTION = getSnakeLastInputDirection()
    for (let i = snakeBody.length - 2; i >= 0; i--) snakeBody[i + 1] = { ...snakeBody[i] }

    snakeBody[0].y += SNAKE_HEAD_DIRECTION.y
    snakeBody[0].x += SNAKE_HEAD_DIRECTION.x

    super.update()
  }
}

export const snake = new Snake()
