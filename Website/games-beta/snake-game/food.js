import { onSnake, expandSnake, incrementSnakeSpeed } from "./snake.js"
import { randomGridPosition } from "./grid.js"
import { incrementScore } from "./informations.js"

const EXPANSION_RATE = 1
const SCORE_INCREMENT = 1
let foodPosition = getRandomFoodPosition()

export function update() {
  if (onSnake(foodPosition)) {
    expandSnake(EXPANSION_RATE)
    incrementSnakeSpeed();
    incrementScore(SCORE_INCREMENT)
    foodPosition = getRandomFoodPosition()
  }
}

export function draw(gameBoard) {
  const foodElement = document.createElement('div')
  foodElement.style.gridColumnStart = foodPosition.x
  foodElement.style.gridRowStart = foodPosition.y
  foodElement.classList.add('food')
  gameBoard.appendChild(foodElement)
}

function getRandomFoodPosition() {
  let newFoodPosition
  while (newFoodPosition == null || onSnake(newFoodPosition)) {
    newFoodPosition = randomGridPosition()
  }

  return newFoodPosition
}