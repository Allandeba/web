export let imageClient = document.querySelector('#images__client');

const MAX_HEIGHT = 250;
const MAX_WIDTH = 250;

const canvas = document.createElement('canvas');
const ctx = canvas.getContext('2d');
canvas.width = Math.floor(MAX_WIDTH);
canvas.height = Math.floor(MAX_HEIGHT);

export function processNewImages(_event) {
    const selectedFiles = _event.target.files;
    if (!selectedFiles) return;

    deleteAllChildren();

    for (let i = 0; i < selectedFiles.length; i++) {
        const selectedFile = selectedFiles[i];

        let newImage = getNewImage();
        newImage.name = selectedFile.name;

        const reader = new FileReader();
        reader.addEventListener('load', () => {
            newImage.src = reader.result;
        });

        reader.readAsDataURL(selectedFile);
    }
}

function deleteAllChildren() {
    Array.from(imageClient.getElementsByClassName('image__client')).forEach(function (element, index) {
        const parent = element.parentNode;
        imageClient.removeChild(parent);
    });
}

function getNewImage() {
    let image = new Image();
    image.addEventListener('load', (event) => {
        drawImage(event.target);
        drawElements();
    });

    return image;
}

function drawImage(_newImage) {
    ctx.drawImage(_newImage, 0, 0, MAX_WIDTH, MAX_HEIGHT);
}

function drawElements() {
    let span = getSpan();
    let deleteLink = getDeleteLinkElement();
    let spanTimes = getSpanTimesElement();

    deleteLink.appendChild(spanTimes);
    span.appendChild(deleteLink);

    let img = getImageElement();
    span.appendChild(img);

    imageClient.appendChild(span);
}

function getSpan() {
    let span = document.createElement('span');
    span.setAttribute('class', 'col-6 col-md-4');

    return span;
}

function getDeleteLinkElement() {
    let deleteLink = document.createElement('a');
    deleteLink.setAttribute('id', new Date().getTime());
    deleteLink.setAttribute('onmouseover', "this.style.opacity = '1'");
    deleteLink.setAttribute('onmouseout', "this.style.opacity = '0.75'");
    deleteLink.setAttribute(
        'class',
        'close position-absolute translate-middle badge border border-light rounded-circle bg-danger p-2 opacity-75 text-decoration-none'
    );
    deleteLink.setAttribute('arial-label', 'Close');

    deleteLink.addEventListener("click", (event) => {
        clearUploadImage(event.target.id)
    });

    return deleteLink;
}

function getSpanTimesElement() {
    let spanTimes = document.createElement('span');
    spanTimes.setAttribute('aria-hidden', 'true');
    spanTimes.innerHTML = "&times;";

    return spanTimes;
}

function getImageElement() {
    let img = document.createElement('img');
    img.setAttribute('width', MAX_WIDTH);
    img.setAttribute('height', MAX_HEIGHT);
    img.setAttribute('class', 'image__client');
    img.src = canvas.toDataURL('image/jpeg');

    return img;
}

//function deleteClientImage(_id) {
//    Array.from(imageClient.getElementsByClassName('close')).forEach(function (element, index) {
//        if (element.id == _id) {

//            const parent = element.parentNode;
//            //let img = parent.getElementsByClassName("image__client")[0];
//            //deleteFileFromUploadImage(img.name);

//            imageClient.removeChild(parent);
//            return;
//        }
//    });
//}

function clearUploadImage(_id) {
    if (!_id) return;

    let uploadFiles = document.getElementById('upload');
    uploadFiles.value = "";

    deleteAllChildren();

    //let pattern = "//[^\s] +?" + _name
    //console.log("pattern = ", pattern)
    //console.log("uploadFiles.value = ", uploadFiles.value)
    //uploadFiles.value.Replace(pattern, "");
    //console.log("uploadFiles.value = ", uploadFiles.value)
}