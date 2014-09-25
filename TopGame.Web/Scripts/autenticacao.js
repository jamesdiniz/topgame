var video = document.querySelector("#video");
var canvasCaptura = document.querySelector('#canvas-captura');
var canvasTratado = document.querySelector('#canvas-tratado');
var ctxCaptura = canvasCaptura.getContext('2d');
var ctxTratado = canvasTratado.getContext('2d');
var mediaStream = null;

$(function () {
    // limpa configuração que ignora campos ocultos
    $.data($('form')[0], 'validator').settings.ignore = "";
});

function getImage(canvas) {
    return canvas.toDataURL("image/png");
}

function snapshot() {
    if (mediaStream) {
        ctxCaptura.drawImage(video, 0, 0, 333, 250);

        var imagePreview = new Image();
        imagePreview.src = getImage(canvasCaptura);

        ctxTratado.drawImage(imagePreview, 40, 0, 250, 250, 0, 0, 250, 250);
        var imageResultado = $("#image-resultado")[0];
        imageResultado.src = getImage(canvasTratado);
        $(imageResultado).show();

        $("#Foto").val(imageResultado.src.replace('data:image/png;base64,', ''));
    }
}

navigator.getUserMedia = navigator.getUserMedia || navigator.webkitGetUserMedia || navigator.mozGetUserMedia || navigator.msGetUserMedia;
window.URL = window.URL || window.webkitURL;

navigator.getUserMedia({ video: true, }, function (stream) {
    video.src = window.URL.createObjectURL(stream);
    mediaStream = stream;
}, function (e) {
    alert('A camera não está funcionando.', e);
});

$.validator.addMethod("customvalidationcpf", function (value, element, params) {
    var valid = validaCPF(value);
    console.log(valid);
    return valid;
});

$.validator.unobtrusive.adapters.add("customvalidationcpf", [], function (options) {
    options.rules["customvalidationcpf"] = true;
    options.messages["customvalidationcpf"] = options.message;
});