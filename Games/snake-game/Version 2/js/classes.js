const CURRENT_FRAME_START = 0

export class Sprite {
  constructor({ position, imageSrc, scale = 1, framesMax = 0, frameColumns = 1, frameRows = 1, numOfImages = 1 }) {
    this.position = position
    this.image = new Image()
    this.image.src = imageSrc
    this.image.onload = this.IsImagesLoaded
    this.scale = scale
    this.framesMax = framesMax
    this.frameColumns = frameColumns
    this.frameRows = frameRows
    this.currentFrame = CURRENT_FRAME_START
    this.numOfImages = numOfImages
  }

  draw() {}

  update() {
    if (!this.IsImagesLoaded()) return

    if (this.currentFrame < this.framesMax) this.currentFrame++
    else this.currentFrame = CURRENT_FRAME_START

    this.draw()
  }

  IsImagesLoaded() {
    return !(--this.numOfImages > 0)
  }
}
