import { getInputDirection } from "./input.js"
import { incrementLevel } from "./informations.js"

const UPGRADE_SPEED_BY_FRAME = 0.1
const UPGRADE_SPEED_BY_FRAME_LEVEL = 4
export let snakeSpeed = 3
let snakeBody = [{ x: 11, y: 11 }]
let newSegments = 0
let canIncrementSnakeSpeed = false

export function update() {
  addSegments()
  addSnakeSpeed()
  const inputDirection = getInputDirection()

  for (let i = snakeBody.length - 2; i >= 0; i--) {
    snakeBody[i + 1] = { ...snakeBody[i] }
  }

  snakeBody[0].x += inputDirection.x
  snakeBody[0].y += inputDirection.y
}

export function draw(gameBoard) {
  snakeBody.forEach((segment) => {
    const snakeElement = document.createElement("div")
    snakeElement.style.gridColumnStart = segment.x
    snakeElement.style.gridRowStart = segment.y
    snakeElement.classList.add("snake")
    gameBoard.appendChild(snakeElement)
  })
}

export function expandSnake(amount) {
  newSegments += amount
}

export function incrementSnakeSpeed() {
  if (snakeBody.length % UPGRADE_SPEED_BY_FRAME_LEVEL === 0) {
    canIncrementSnakeSpeed = true
    incrementLevel(1)
  }
}

export function onSnake(position, { ignoreHead = false } = {}) {
  return snakeBody.some((segment, index) => {
    if (ignoreHead === true && index === 0) return false
    return equalPositions(segment, position)
  })
}

function equalPositions(position1, position2) {
  return position1.x === position2.x && position1.y === position2.y
}

function addSegments() {
  for (let i = 0; i < newSegments; i++) {
    snakeBody.push({ ...snakeBody[snakeBody.length - 1] })
  }

  newSegments = 0
}

export function getSnakeHead() {
  return snakeBody[0]
}

export function snakeIntersection() {
  return onSnake(snakeBody[0], { ignoreHead: true })
}

function addSnakeSpeed() {
  if (canIncrementSnakeSpeed) {
    snakeSpeed += UPGRADE_SPEED_BY_FRAME
    if (snakeSpeed.toFixed(2) % 1 === 0) canIncrementSnakeSpeed = false
  }
}
