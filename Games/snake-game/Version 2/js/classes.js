const CANVAS = document.getElementById('canvas')
const CTX = CANVAS.getContext('2d')

export class Sprite {
  constructor({ position, imageSrc, scale = 1, frameColumns = 1, frameRows: frameRows = 1 }) {
    this.position = position
    this.image = new Image()
    this.image.src = imageSrc
    this.scale = scale
    this.frameColumns = frameColumns
    this.frameRows = frameRows
  }

  draw() {
    CTX.clearRect(0, 0, canvas.width, canvas.height)
  }

  update() {
    this.draw()
  }
}
