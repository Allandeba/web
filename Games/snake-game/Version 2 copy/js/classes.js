const CANVAS = document.getElementById("canvas")
const CTX = CANVAS.getContext("2d")

export class Sprite {
  constructor({
    position,
    imageSrc,
    scale = 1,
    framesMax = 0,
    frameColumns = 1,
    frameRows: frameRows = 1,
  }) {
    this.position = position
    this.image = new Image()
    this.image.src = imageSrc
    this.scale = scale
    this.framesMax = framesMax
    this.frameColumns = frameColumns
    this.frameRows = frameRows
    this.currentFramePosition = { x: 0, y: 0 }
    this.frameLoopingPosition = 1
  }

  draw() {
    CTX.clearRect(0, 0, canvas.width, canvas.height)

    const SINGLE_IMAGE_HEIGHT = this.image.height / this.frameRows
    const SINGLE_IMAGE_WIDTH = this.image.width / this.frameColumns

    // console.log("IMAGE_HEIGHT_CROPPED: " + SINGLE_IMAGE_HEIGHT)
    // console.log("IMAGE_WIDTH_CROPPED: " + SINGLE_IMAGE_WIDTH)
    // console.log(
    //   "Position.x: " + this.position.x + " Position.y: " + this.position.y
    // )
    // console.log("image: " + this.image.src)
    // console.log("scale: " + this.scale)
    // console.log("framesMax: " + this.framesMax)
    // console.log("frameColumn: " + this.frameColumns)
    // console.log("frameRow: " + this.frameRows)
    // console.log(
    //   "currentFramePosition.x: " +
    //     this.currentFramePosition.x +
    //     " currentFramePosition.y: " +
    //     this.currentFramePosition.y
    // )
    // console.log("frameLoopingPosition: " + this.frameLoopingPosition)

    CTX.drawImage(
      this.image,
      SINGLE_IMAGE_WIDTH *
        this.currentFramePosition.x *
        this.frameLoopingPosition,
      SINGLE_IMAGE_HEIGHT *
        this.currentFramePosition.y *
        this.frameLoopingPosition,
      SINGLE_IMAGE_WIDTH,
      SINGLE_IMAGE_HEIGHT,
      this.position.x, // - SINGLE_IMAGE_WIDTH,
      this.position.y, // - SINGLE_IMAGE_HEIGHT,
      SINGLE_IMAGE_WIDTH * this.scale,
      SINGLE_IMAGE_HEIGHT * this.scale
    )
  }

  update() {
    if (this.frameLoopingPosition < this.framesMax - 1)
      this.frameLoopingPosition++
    else this.frameLoopingPosition = 1

    this.draw()
  }
}
