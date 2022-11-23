const POSITION = { up: 0, down: 1, left: 2, right: 3 }
const MIN_MOVEMENT = 20

let snakeDirection = { x: 0, y: 0 }
let touchStartX = { x: 0, y: 0 }
let touchStartY = { x: 0, y: 0 }

export function updateTouchVariables(event) {
  touchStartX = event.changedTouches[0].screenX
  touchStartY = event.changedTouches[0].screenY
}

export function getSnakeDirection() {
  return snakeDirection
}

function canSnakeMove(position) {
  switch (position) {
    case POSITION.up:
    case POSITION.down: {
      return snakeDirection.y === 0
    }

    case POSITION.left:
    case POSITION.right: {
      return snakeDirection.x === 0
    }
  }
}

export function updateKeyboardDirections(event) {
  switch (event.code) {
    case "ArrowUp":
    case "KeyW":
      updateSnakeDirection(POSITION.up)
      break

    case "ArrowDown":
    case "KeyS":
      updateSnakeDirection(POSITION.down)
      break

    case "ArrowLeft":
    case "KeyA":
      updateSnakeDirection(POSITION.left)
      break

    case "ArrowRight":
    case "KeyD":
      updateSnakeDirection(POSITION.right)
      break
  }
}

export function updateTouchDirections(touchEvent) {
  const TOUCH_END_POSITION = {
    x: touchEvent.changedTouches[0].screenX,
    y: touchEvent.changedTouches[0].screenY,
  }
  if (touchDirectionUp(TOUCH_END_POSITION) && canSnakeMove(POSITION.up)) {
    updateSnakeDirection(POSITION.up)
    return
  }

  if (touchDirectionDown(TOUCH_END_POSITION) && canSnakeMove(POSITION.down)) {
    updateSnakeDirection(POSITION.down)
    return
  }

  if (touchDirectionLeft(TOUCH_END_POSITION) && canSnakeMove(POSITION.left)) {
    updateSnakeDirection(POSITION.left)
    return
  }

  if (touchDirectionRight(TOUCH_END_POSITION) && canSnakeMove(POSITION.right)) {
    updateSnakeDirection(POSITION.right)
    return
  }
}

function touchDirectionUp(position) {
  return position.y < touchStartY && touchStartY - position.y > MIN_MOVEMENT
}

function touchDirectionDown(position) {
  return position.y > touchStartY && position.y - touchStartY > MIN_MOVEMENT
}

function touchDirectionLeft(position) {
  return position.x < touchStartX && touchStartX - position.x > MIN_MOVEMENT
}

function touchDirectionRight(position) {
  return position.x > touchStartX && position.x - touchStartX > MIN_MOVEMENT
}

function updateSnakeDirection(position) {
  switch (position) {
    case POSITION.up: {
      if (canSnakeMove(POSITION.up)) snakeDirection = { x: 0, y: -1 }
      break
    }

    case POSITION.down: {
      if (canSnakeMove(POSITION.down)) snakeDirection = { x: 0, y: 1 }
      break
    }

    case POSITION.left: {
      if (canSnakeMove(POSITION.left)) snakeDirection = { x: -1, y: 0 }
      break
    }

    case POSITION.right: {
      if (canSnakeMove(POSITION.right)) snakeDirection = { x: 1, y: 0 }
      break
    }
  }
}
