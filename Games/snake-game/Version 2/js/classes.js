export class Sprite {
  constructor({ position, imageSrc, scale = 1, framesMax = 0, frameColumns = 1, frameRows = 1 }) {
    this.position = position
    this.image = new Image()
    this.image.src = imageSrc
    this.scale = scale
    this.framesMax = framesMax
    this.frameColumns = frameColumns
    this.frameRows = frameRows
    this.currentFrame = 1
  }

  draw() {}
  update() {
    if (this.currentFrame < this.framesMax) this.currentFrame++
    else this.currentFrame = 1

    this.draw()
  }
}
