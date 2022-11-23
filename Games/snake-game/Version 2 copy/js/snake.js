import { Sprite } from "./classes.js"

const CANVAS = document.getElementById("canvas")
const SNAKE_SPRITE_SRC = "../img/snake_sprite.png"
const SNAKE_IMAGE_SCALE = 0.5
const SNAKE_SPRITE_MOVEMENT = 0
const SNAKE_SPRITE_COLUMNS = 7
const SNAKE_SPRITE_ROWS = 2
const SNAKE_SPEED = 15

const SNAKE_SPRITE_PICTURES = {
  headUp: { x: 3, y: 0 },
  headDown: { x: 3, y: 1 },
  headRight: { x: 4, y: 0 },
  headLeft: { x: 4, y: 1 },

  tailUp: { x: 0, y: 0 },
  tailDown: { x: 1, y: 1 },
  tailRight: { x: 1, y: 0 },
  tailLeft: { x: 0, y: 1 },

  bodyCenterVertical: { x: 2, y: 0 },
  bodyCenterHorizontal: { x: 2, y: 1 },

  curveLeftUp: { x: 5, y: 0 },
  curveLeftDown: { x: 5, y: 1 },
  curveRightUp: { x: 6, y: 1 },
  curveRightDown: { x: 6, y: 0 },
}

class Snake extends Sprite {
  constructor() {
    super({
      position: getInitialSnakePosition(),
      imageSrc: SNAKE_SPRITE_SRC,
      scale: SNAKE_IMAGE_SCALE,
      framesMax: SNAKE_SPRITE_MOVEMENT,
      frameColumns: SNAKE_SPRITE_COLUMNS,
      frameRows: SNAKE_SPRITE_ROWS,
    })
    function getInitialSnakePosition() {
      return { x: CANVAS.width / 2, y: CANVAS.height / 2 }
    }
  }

  draw() {
    super.draw()
  }

  update() {
    super.update(SNAKE_SPEED)

    super.currentFramePosition = SNAKE_SPRITE_PICTURES.headUp
  }
}

export const snake = new Snake()
