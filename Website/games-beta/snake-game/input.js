const gameBoard = document.getElementById('game-board')

let inputDirection = { x: 0, y: 0 }
let lastDirection = { x: 0, y: 0 }

addEventListener('keydown', event => {
  switch (event.key) {
    case 'ArrowUp':
      if (lastDirection.y !== 0) break
      inputDirection = { x: 0, y: -1 }
      break

    case 'ArrowDown':
      if (lastDirection.y !== 0) break    
      inputDirection = { x: 0, y: 1 }
      break

    case 'ArrowLeft':
      if (lastDirection.x !== 0) break  
      inputDirection = { x: -1, y: 0 }
      break

    case 'ArrowRight':
      if (lastDirection.x !== 0) break
      inputDirection = { x: 1, y: 0 }
      break
  }  
})

// touch devices
let touchStartX = 0
let touchStartY = 0

function touchDirectionUp(touchEndEvent) {
  return touchEndEvent.changedTouches[0].screenY < touchStartY   
}

function touchDirectionDown(touchEndEvent) {
  return touchEndEvent.changedTouches[0].screenY > touchStartY   
}

function touchDirectionLeft(touchEndEvent) {
  return touchEndEvent.changedTouches[0].screenX < touchStartX   
}

function touchDirectionRight(touchEndEvent) {
  return touchEndEvent.changedTouches[0].screenX > touchStartX   
}

function touchDirection(event) {
  if (touchDirectionUp(event)) {
    if (lastDirection.y === 0) {
      inputDirection = { x: 0, y: -1 }
      return
    }  
  } 
   
   if (touchDirectionDown(event)) {
      if (lastDirection.y === 0) {
        inputDirection = { x: 0, y: 1 }
        return
      }  
  } 
  
  if (touchDirectionLeft(event)) {
    if (lastDirection.x === 0) {
      inputDirection = { x: -1, y: 0 }
      return
    }  
  } 
  
  if (touchDirectionRight(event)) {
    if (lastDirection.x === 0) {
      inputDirection = { x: 1, y: 0 }
      return
    }  
  }
}

gameBoard.addEventListener('touchstart', event => {
  event.preventDefault()
  touchStartX = event.changedTouches[0].screenX 
  touchStartY = event.changedTouches[0].screenY

  console.log('X: ' + touchStartX)
  console.log('Y: ' + touchStartY)
})

gameBoard.addEventListener('touchend', event => {
  touchDirection(event)
})

gameBoard.addEventListener('touchcancel', event => {
  touchDirection(event)
})

export function getInputDirection() {
  lastDirection = inputDirection
  return inputDirection  
}