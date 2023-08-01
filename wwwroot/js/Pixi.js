/**
 * This function initializes the images onto the canvas
 * 
 * source ****  http://www.yeahbutisitflash.com/?p=5226
 **/
function init() {
    stage = new PIXI.Container();
    renderer = PIXI.autoDetectRenderer(
        512,
        384,
        { view: document.getElementById("game-canvas") }
    );

    // Puts the first image onto the canvas and stages onto the container
    var farTexture = PIXI.Texture.from("resources/bg-far.png");
    far = new PIXI.extras.TilingSprite(farTexture, 512, 256);
    far.position.x = 0;
    far.position.y = 0;
    far.tilePosition.x = 0;
    far.tilePosition.y = 0;
    stage.addChild(far);

    // Puts the 2nd image onto the canvas and stages onto the container
    var midTexture = PIXI.Texture.from("resources/bg-mid.png");
    mid = new PIXI.extras.TilingSprite(midTexture, 512, 256);
    mid.position.x = 0;
    mid.position.y = 128;
    mid.tilePosition.x = 0;
    mid.tilePosition.y = 0;
    stage.addChild(mid);

    // 60 fps
    requestAnimationFrame(update);
}

/**
 * This function updates and renders the two images
 **/
function update() {
    far.tilePosition.x -= 0.128;
    mid.tilePosition.x -= 0.64;

    renderer.render(stage);

    requestAnimationFrame(update);
}