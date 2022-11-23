import { updateKeyboardDirections, updateTouchVariables, updateTouchDirections } from './snakeInputDirection.js'

const CANVAS = document.getElementById('canvas')

addEventListener('keydown', (event) => {
  updateKeyboardDirections(event)
})

CANVAS.addEventListener('touchstart', (event) => {
  event.preventDefault()
  updateTouchVariables(event)
})

CANVAS.addEventListener('touchend', (event) => {
  updateTouchDirections(event)
})

CANVAS.addEventListener('touchcancel', (event) => {
  updateTouchDirections(event)
})
