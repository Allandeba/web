let scoreCounter = 1
let levelCounter = 1

export function update() {
  updateMaxScore()
}

export function draw() {
  updateScore()
  updateLevel()
}

function getMaxSavedScore() {
  let maxSavedScore = JSON.parse(localStorage.getItem('max-score'))
  maxSavedScore = (maxSavedScore === null ? 1 : maxSavedScore)
  return maxSavedScore
}

function updateMaxScore() {
  document.querySelector('#max-score').innerHTML = 'max score: ' + getMaxSavedScore()
}

function incrementMaxScore() {
  if (getMaxSavedScore() < scoreCounter)
    localStorage.setItem('max-score', scoreCounter)
}

export function incrementScore(increment) {
  scoreCounter += increment
  incrementMaxScore()
}

function updateScore(gameBoard) {
  document.querySelector('#score').innerHTML = 'score: ' + scoreCounter
}

export function incrementLevel(increment) {
  levelCounter += increment;
}

function updateLevel() {
  document.querySelector('#level').innerHTML = 'level: ' + levelCounter
}