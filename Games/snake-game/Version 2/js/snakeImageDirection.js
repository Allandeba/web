import { snake } from './snake.js'
import { getSnakeDirection as getSnakeLastInputDirection } from './snakeInputDirection.js'
import { food } from './food.js'

const SNAKE_HEAD_MOVEMENT_DIRECTION = {
  none: 0,
  up: 1,
  down: 2,
  left: 3,
  right: 4,
  curveRightUp: 5,
  curveRightDown: 6,
  curveLeftUp: 7,
  curveLeftDown: 8,
}
export const SNAKE_DISTANCE_MOVEMENT = 20
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

  curveLeftUp: { x: 6, y: 0 },
  curveRightUp: { x: 5, y: 1 },
  curveLeftDown: { x: 6, y: 1 },
  curveRightDown: { x: 5, y: 0 },
}

export function getSpriteImagePosition(snakeElement, index) {
  const IS_SNAKE_HEAD = index === 0
  const IS_SNAKE_TAIL = index === snake.snakeBody.length - 1
  const IS_ANY_BODY_PART = !IS_SNAKE_HEAD && !IS_SNAKE_TAIL

  if (IS_SNAKE_HEAD) return getSnakeHeadImage()
  if (IS_SNAKE_TAIL) return getSnakeTailImage(snakeElement, index)
  if (IS_ANY_BODY_PART) return getSnakeBodyPart(snakeElement, index)
}

function getSnakeHeadImage() {
  const DIRECTION = getSnakeHeadMovementDirection()
  switch (DIRECTION) {
    case SNAKE_HEAD_MOVEMENT_DIRECTION.up:
      return SNAKE_SPRITE_PICTURES.headUp

    case SNAKE_HEAD_MOVEMENT_DIRECTION.down:
      return SNAKE_SPRITE_PICTURES.headDown

    case SNAKE_HEAD_MOVEMENT_DIRECTION.left:
      return SNAKE_SPRITE_PICTURES.headLeft

    case SNAKE_HEAD_MOVEMENT_DIRECTION.right:
      return SNAKE_SPRITE_PICTURES.headRight
  }

  return getDefaultSnakePosition()
}

function getSnakeHeadMovementDirection() {
  const SNAKE_HEAD_DIRECTION = getSnakeLastInputDirection()

  if (isNewGame(SNAKE_HEAD_DIRECTION)) return getDefaultSnakePosition()
  if (SNAKE_HEAD_DIRECTION.y < 0) return SNAKE_HEAD_MOVEMENT_DIRECTION.up
  if (SNAKE_HEAD_DIRECTION.y > 0) return SNAKE_HEAD_MOVEMENT_DIRECTION.down
  if (SNAKE_HEAD_DIRECTION.x < 0) return SNAKE_HEAD_MOVEMENT_DIRECTION.left
  if (SNAKE_HEAD_DIRECTION.x > 0) return SNAKE_HEAD_MOVEMENT_DIRECTION.right
}

function getSnakeTailImage(snakeElement, index) {
  const DIRECTION = getPriorSnakeTailPosition(snakeElement, index)
  switch (DIRECTION) {
    case SNAKE_HEAD_MOVEMENT_DIRECTION.up:
      return SNAKE_SPRITE_PICTURES.tailUp

    case SNAKE_HEAD_MOVEMENT_DIRECTION.down:
      return SNAKE_SPRITE_PICTURES.tailDown

    case SNAKE_HEAD_MOVEMENT_DIRECTION.left:
      return SNAKE_SPRITE_PICTURES.tailLeft

    case SNAKE_HEAD_MOVEMENT_DIRECTION.right:
      return SNAKE_SPRITE_PICTURES.tailRight

    case SNAKE_HEAD_MOVEMENT_DIRECTION.none:
      return
  }

  console.log('DIRECTION not expected for tails.')
}

function getPriorSnakeTailPosition(snakeElement, index) {
  const PRIOR_SNAKE_ELEMENT = snake.snakeBody[index - 1]
  if (snakeElement.y - SNAKE_DISTANCE_MOVEMENT === PRIOR_SNAKE_ELEMENT.y) return SNAKE_HEAD_MOVEMENT_DIRECTION.up
  if (snakeElement.y + SNAKE_DISTANCE_MOVEMENT === PRIOR_SNAKE_ELEMENT.y) return SNAKE_HEAD_MOVEMENT_DIRECTION.down
  if (snakeElement.x - SNAKE_DISTANCE_MOVEMENT === PRIOR_SNAKE_ELEMENT.x) return SNAKE_HEAD_MOVEMENT_DIRECTION.left
  if (snakeElement.x + SNAKE_DISTANCE_MOVEMENT === PRIOR_SNAKE_ELEMENT.x) return SNAKE_HEAD_MOVEMENT_DIRECTION.right

  if (snakeElement.y === PRIOR_SNAKE_ELEMENT.y && snakeElement.x === PRIOR_SNAKE_ELEMENT.x) return SNAKE_HEAD_MOVEMENT_DIRECTION.none

  return getDefaultSnakeDirection()
}

function getSnakeBodyPart(snakeElement, index) {
  const DIRECTION = getPriorSnakeBodyPosition(snakeElement, index)
  switch (DIRECTION) {
    case SNAKE_HEAD_MOVEMENT_DIRECTION.up:
    case SNAKE_HEAD_MOVEMENT_DIRECTION.down:
      return SNAKE_SPRITE_PICTURES.bodyCenterVertical

    case SNAKE_HEAD_MOVEMENT_DIRECTION.left:
    case SNAKE_HEAD_MOVEMENT_DIRECTION.right:
      return SNAKE_SPRITE_PICTURES.bodyCenterHorizontal

    case SNAKE_HEAD_MOVEMENT_DIRECTION.curveLeftDown:
      return SNAKE_SPRITE_PICTURES.curveLeftDown

    case SNAKE_HEAD_MOVEMENT_DIRECTION.curveLeftUp:
      return SNAKE_SPRITE_PICTURES.curveLeftUp

    case SNAKE_HEAD_MOVEMENT_DIRECTION.curveRightDown:
      return SNAKE_SPRITE_PICTURES.curveRightDown

    case SNAKE_HEAD_MOVEMENT_DIRECTION.curveRightUp:
      return SNAKE_SPRITE_PICTURES.curveRightUp
  }
}

function getPriorSnakeBodyPosition(snakeElement, index) {
  const PRIOR_SNAKE_ELEMENT = snake.snakeBody[index - 1]
  const NEXT_SNAKE_ELEMENT = snake.snakeBody[index + 1]

  const IS_WALKING_VERTICAL = snakeElement.x === NEXT_SNAKE_ELEMENT.x && snakeElement.x === PRIOR_SNAKE_ELEMENT.x
  const IS_WALKING_HORIZONTAL = snakeElement.y === NEXT_SNAKE_ELEMENT.y && snakeElement.y === PRIOR_SNAKE_ELEMENT.y
  const IS_PRIOR_SNAKE_ELEMENT_GOING_UP = IS_WALKING_VERTICAL
  const IS_PRIOR_SNAKE_ELEMENT_GOING_DOWN = IS_WALKING_VERTICAL
  const IS_PRIOR_SNAKE_ELEMENT_GOING_LEFT = IS_WALKING_HORIZONTAL
  const IS_PRIOR_SNAKE_ELEMENT_GOING_RIGHT = IS_WALKING_HORIZONTAL

  const IS_PRIOR_ELEMENT_WALKING_VERTICALLY_EQUAL = snakeElement.x === PRIOR_SNAKE_ELEMENT.x
  const IS_PRIOR_ELEMENT_WALKING_HORIZONTALLY_EQUAL = snakeElement.y === PRIOR_SNAKE_ELEMENT.y

  const IS_NEXT_BODY_WALKING_LEFT = snakeElement.x - SNAKE_DISTANCE_MOVEMENT === NEXT_SNAKE_ELEMENT.x
  const IS_NEXT_BODY_WALKING_RIGHT = snakeElement.x + SNAKE_DISTANCE_MOVEMENT === NEXT_SNAKE_ELEMENT.x
  const IS_NEXT_BODY_WALKING_DOWN = snakeElement.y + SNAKE_DISTANCE_MOVEMENT === NEXT_SNAKE_ELEMENT.y
  const IS_NEXT_BODY_WALKING_UP = snakeElement.y - SNAKE_DISTANCE_MOVEMENT === NEXT_SNAKE_ELEMENT.y
  const IS_PRIOR_BODY_WALKING_LEFT = snakeElement.x - SNAKE_DISTANCE_MOVEMENT === PRIOR_SNAKE_ELEMENT.x
  const IS_PRIOR_BODY_WALKING_RIGHT = snakeElement.x + SNAKE_DISTANCE_MOVEMENT === PRIOR_SNAKE_ELEMENT.x

  const IS_PRIOR_BODY_WALKING_DOWN = snakeElement.y + SNAKE_DISTANCE_MOVEMENT === PRIOR_SNAKE_ELEMENT.y
  const IS_PRIOR_BODY_WALKING_UP = snakeElement.y - SNAKE_DISTANCE_MOVEMENT === PRIOR_SNAKE_ELEMENT.y

  const IS_PRIOR_ELEMENT_WALKING_SAME_DIRECTION = IS_PRIOR_ELEMENT_WALKING_VERTICALLY_EQUAL || IS_PRIOR_ELEMENT_WALKING_HORIZONTALLY_EQUAL
  const IS_NEXT_SNAKE_ELEMENT_GOING_LEFT_UP =
    IS_PRIOR_ELEMENT_WALKING_SAME_DIRECTION &&
    (IS_NEXT_BODY_WALKING_LEFT || IS_NEXT_BODY_WALKING_DOWN) &&
    (IS_PRIOR_BODY_WALKING_LEFT || IS_PRIOR_BODY_WALKING_DOWN)

  const IS_NEXT_SNAKE_ELEMENT_GOING_LEFT_DOWN =
    IS_PRIOR_ELEMENT_WALKING_SAME_DIRECTION &&
    (IS_NEXT_BODY_WALKING_LEFT || IS_NEXT_BODY_WALKING_UP) &&
    (IS_PRIOR_BODY_WALKING_LEFT || IS_PRIOR_BODY_WALKING_UP)

  const IS_NEXT_SNAKE_ELEMENT_GOING_RIGHT_UP =
    IS_PRIOR_ELEMENT_WALKING_SAME_DIRECTION &&
    (IS_NEXT_BODY_WALKING_RIGHT || IS_NEXT_BODY_WALKING_DOWN) &&
    (IS_PRIOR_BODY_WALKING_RIGHT || IS_PRIOR_BODY_WALKING_DOWN)

  const IS_NEXT_SNAKE_ELEMENT_GOING_RIGHT_DOWN =
    IS_PRIOR_ELEMENT_WALKING_SAME_DIRECTION &&
    (IS_NEXT_BODY_WALKING_RIGHT || IS_NEXT_BODY_WALKING_UP) &&
    (IS_PRIOR_BODY_WALKING_RIGHT || IS_PRIOR_BODY_WALKING_UP)

  if (IS_PRIOR_SNAKE_ELEMENT_GOING_UP) return SNAKE_HEAD_MOVEMENT_DIRECTION.up
  if (IS_PRIOR_SNAKE_ELEMENT_GOING_DOWN) return SNAKE_HEAD_MOVEMENT_DIRECTION.down
  if (IS_PRIOR_SNAKE_ELEMENT_GOING_LEFT) return SNAKE_HEAD_MOVEMENT_DIRECTION.left
  if (IS_PRIOR_SNAKE_ELEMENT_GOING_RIGHT) return SNAKE_HEAD_MOVEMENT_DIRECTION.right
  if (IS_NEXT_SNAKE_ELEMENT_GOING_LEFT_UP) return SNAKE_HEAD_MOVEMENT_DIRECTION.curveLeftUp
  if (IS_NEXT_SNAKE_ELEMENT_GOING_LEFT_DOWN) return SNAKE_HEAD_MOVEMENT_DIRECTION.curveLeftDown
  if (IS_NEXT_SNAKE_ELEMENT_GOING_RIGHT_UP) return SNAKE_HEAD_MOVEMENT_DIRECTION.curveRightUp
  if (IS_NEXT_SNAKE_ELEMENT_GOING_RIGHT_DOWN) return SNAKE_HEAD_MOVEMENT_DIRECTION.curveRightDown

  return getDefaultSnakeDirection()
}

function isNewGame(snakeDirection) {
  return snakeDirection.x === 0 && snakeDirection.y === 0
}

function getDefaultSnakePosition() {
  return SNAKE_SPRITE_PICTURES.headUp
}

function getDefaultSnakeDirection() {
  return SNAKE_HEAD_MOVEMENT_DIRECTION.up
}

export function equalPositions({ snakeElement = {}, position = {}, height = 0, width = 0 }) {
  let snakeElementTop = snakeElement.y
  let snakeElementBottom = snakeElement.y + SNAKE_DISTANCE_MOVEMENT / 2
  let snakeElementLeft = snakeElement.x
  let snakeElementRight = snakeElement.x + SNAKE_DISTANCE_MOVEMENT / 2

  let objectTop = position.y
  let objectBottom = position.y + height
  let objectLeft = position.x
  let objectRight = position.x + width

  let aLeftOfB = snakeElementRight < objectLeft
  let aRightOfB = snakeElementLeft > objectRight
  let aAboveB = snakeElementBottom < objectTop
  let aBelowB = snakeElementTop > objectBottom

  return !(aLeftOfB || aRightOfB || aAboveB || aBelowB)
}
