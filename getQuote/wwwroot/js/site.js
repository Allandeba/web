// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


import * as uploadImageUtils from "./uploadImageUtils.js";

const uploadFiles = document.getElementById('upload');
uploadFiles.addEventListener('change', (event) => uploadImageUtils.processNewImages(event));