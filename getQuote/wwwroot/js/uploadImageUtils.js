let imageClient = document.querySelector("#images__client");
let defaultImageSelect = document.querySelector("#defaultImageSelect");

const MAX_HEIGHT = 250;
const MAX_WIDTH = 250;
const DEFAULT_SELECT_ENUM = "None";

const canvas = document.createElement("canvas");
const ctx = canvas.getContext("2d");
canvas.width = Math.floor(MAX_WIDTH);
canvas.height = Math.floor(MAX_HEIGHT);

const uploadFiles = document.getElementById("upload");
uploadFiles.addEventListener("change", (event) => processNewImages(event));

function processNewImages(_event) {
  const selectedFiles = _event.target.files;
  if (!selectedFiles) return;

  deleteAllChildren();

  for (let i = 0; i < selectedFiles.length; i++) {
    const selectedFile = selectedFiles[i];

    let newImage = getNewImage();
    newImage.name = selectedFile.name;

    const reader = new FileReader();
    reader.addEventListener("load", () => {
      newImage.src = reader.result;
    });

    reader.readAsDataURL(selectedFile);
  }
}

function deleteAllChildren() {
  Array.from(imageClient.getElementsByClassName("image__client")).forEach(
    function (element, index) {
      const parent = element.parentNode;
      imageClient.removeChild(parent);
    }
  );

  deleteNotExistentDefaultImage();
}

function deleteClientItemImage(itemImageId) {
  Array.from(imageClient.getElementsByTagName("img")).forEach(function (
    element,
    index
  ) {
    if (element.id == itemImageId) {
      const parent = element.parentNode;
      imageClient.removeChild(parent);

      return;
    }
  });
}

function getNewImage() {
  let image = new Image();
  image.addEventListener("load", (event) => {
    drawImage(event.target);
    drawElements(event.target);
    drawNewDefaultImageSelect(event.target);
  });

  return image;
}

function drawImage(_newImage) {
  ctx.drawImage(_newImage, 0, 0, MAX_WIDTH, MAX_HEIGHT);
}

function drawElements(_newImage) {
  let span = getSpan();
  let deleteLink = getDeleteLinkElement();
  let spanTimes = getSpanTimesElement();

  deleteLink.appendChild(spanTimes);
  span.appendChild(deleteLink);

  let img = getImageElement(_newImage);
  span.appendChild(img);

  imageClient.appendChild(span);
}

function getSpan() {
  let span = document.createElement("span");
  span.setAttribute("class", "col-6 col-md-4");

  return span;
}

function getDeleteLinkElement() {
  let deleteLink = document.createElement("a");
  deleteLink.setAttribute("id", 0);
  deleteLink.setAttribute("arial-label", "Close");
  deleteLink.setAttribute(
    "class",
    "close " +
      "position-absolute " +
      "translate-middle " +
      "d - block " +
      "badge " +
      "badge-danger " +
      "border border-light " +
      "rounded-circle " +
      "bg-danger " +
      "p-2 " +
      "opacity-75 " +
      "text-decoration-none"
  );

  deleteLink.addEventListener("click", (event) => {
    deleteItemImage(event.target.id);
  });

  return deleteLink;
}

function getSpanTimesElement() {
  let spanTimes = document.createElement("span");
  spanTimes.setAttribute("aria-hidden", "true");
  spanTimes.innerHTML = "&times;";

  return spanTimes;
}

function getImageElement(_image) {
  let img = document.createElement("img");
  img.setAttribute("width", MAX_WIDTH);
  img.setAttribute("height", MAX_HEIGHT);
  img.setAttribute("class", "image__client");
  img.setAttribute("alt", _image.name);
  img.src = canvas.toDataURL("image/jpeg");
  img.name = _image.name;

  return img;
}

function clearUploadImage(_id) {
  if (!_id) return;

  let uploadFiles = document.getElementById("upload");
  uploadFiles.value = "";

  deleteAllChildren();
}

function drawNewDefaultImageSelect(_newImage) {
  var option = document.createElement("option");
  option.value = _newImage.name;
  option.text = _newImage.name;
  defaultImageSelect.appendChild(option);
}

function canDeleteDefaultImageSelect(_name) {
  return _name != DEFAULT_SELECT_ENUM;
}

function deleteNotExistentDefaultImage() {
  let imageNameList = [];

  Array.from(imageClient.getElementsByTagName("img")).forEach(function (
    element,
    index
  ) {
    imageNameList.push(element.name);
  });

  Array.from(defaultImageSelect.getElementsByTagName("option")).forEach(
    function (element, index) {
      if (
        !imageNameList.includes(element.value) &&
        canDeleteDefaultImageSelect(element.value)
      ) {
        defaultImageSelect.removeChild(element);
      }
    }
  );
}

function deleteItemImageOnServer(itemImageId) {
  var xhr = new XMLHttpRequest();
  xhr.open("POST", "/Item/DeleteImage/" + itemImageId);
  xhr.setRequestHeader("Content-Type", "application/json");
  xhr.onload = function () {
    if (xhr.status === 200) {
      alert("Imagem removida com sucesso!");
    } else {
      alert("Ocorreu um erro ao deletar a imagem!");
    }
  };
  xhr.send();
}

function deleteItemImage(itemImageId) {
  if (itemImageId == 0) {
    if (
      confirm(
        "Tem certeza de que deseja excluir TODAS as imagens adicionadas agora?"
      )
    ) {
      // Não posso manipular o input do "type=file" para excluir somente a selecionada.
      clearUploadImage(itemImageId);
      deleteNotExistentDefaultImage();
    }
  } else {
    if (
      confirm(
        "Tem certeza de que deseja excluir a Item Image ID: " +
          itemImageId +
          " ?"
      )
    ) {
      deleteItemImageOnServer(itemImageId);
      deleteClientItemImage(itemImageId);
      deleteNotExistentDefaultImage();
    }
  }
}
