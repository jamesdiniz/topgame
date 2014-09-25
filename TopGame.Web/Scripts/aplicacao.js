var loader;
var jogoInit = false;
var jogadorId;
var jogadorToken;

$(document).ajaxStart(showLoading);
$(document).ajaxStop(hideLoading);

$(function () {
    $("a.btn-iniciar").on('click', function (event) {
        event.preventDefault();
        $("#box-identificacao form").submit();
    });

    $("#box-identificacao form").on('submit', function(event) {
        event.preventDefault();

        $.post($(this).attr('action'), $(this).serialize(), function (data) {
            if (data.status == "OK") {
                jogoInit = true;
                jogadorId = data.jogadorId;
                jogadorToken = data.token;
                loadPergunta();
            } else {
                loader.hide();
            }
        }, "json");
    });

    $('.cpf').mask('000.000.000-00', {
        reverse: true,
        onComplete: function(cpf) {
            buscaCadastro(cpf);
        }
    });
});

$(document).on('click', '#box-respostas ul li', function () {
    marcaResposta($(this));
});

$(document).on('click', '.btn-enviar', function() {
    var answer;
    if ($("#TipoResposta:hidden").val() == 'ALT') {
        answer = $("[name^='RSP|']:hidden");
        var item = $("#box-respostas ul li.selected");

        if (item.length == 0) {
            alert("Selecione uma resposta.");
            return;
        }
        $(answer.get(0)).val(item.attr('id'));
    } else {
        //TODO: Esse bloco acabou se tornando específico somente para o jogo dos bilionários
        var pRanking = $("#box-respostas ul li input:eq(0)");
        var pFortuna = $("#box-respostas ul li input:eq(1)");

        if (pRanking.val() != '' && !(parseFloat(pRanking.val()) > 0 && parseFloat(pRanking.val()) <= 50) || pRanking.val() == '') {
            alert("O valor do ranking deve estar entre 1 e 50.");
            pRanking.focus();
            return;
        }

        if (pFortuna.val() != '' && !(parseFloat(pFortuna.val()) > 0 && parseFloat(pFortuna.val()) <= 100) || pFortuna.val() == '') {
            alert("O valor da fortuna deve estar entre 1 e 100.");
            pFortuna.focus();
            return;
        }

        var erro = false;
        answer = $("[name^='RSP|']");
        answer.each(function (index, element) {
            if ($(element).val() == '') {
                alert("Informe um valor para o campo.");
                marcaResposta($(element).parent());
                $(element).focus();
                erro = true;
                return false;
            }
            return true;
        });

        if (erro) return;
    }

    postPergunta();
});

$(window).on('beforeunload', function () {
    if (jogoInit) {
        return 'Você está com o jogo iniciado, todo o progresso não salvo será perdido.';
    }
});

function showLoading() {
    if (loader == undefined) {
        $("body").prepend("<div id=\"loader-container\"><div id=\"loader-bg\"></div><div id=\"loader\">Processando, por favor aguarde...</div></div>");
        loader = $("#loader-container");
    }
    
    if (loader.is(":hidden"))
        loader.show();
}

function hideLoading() {
    if (loader)
        loader.fadeOut("fast");
}

function loadPergunta() {
    $("#content").load(window.location.pathname + '/Pergunta/?jogadorId=' + jogadorId + '&token=' + jogadorToken);
}

function postPergunta() {
    $.post(window.location.pathname + '/Pergunta/?jogadorId=' + jogadorId + '&token=' + jogadorToken, $("form[name='formPergunta']").serialize(), function(resultado) {
        if (resultado.status == "OK") {
            console.log("Carregando próxima pergunta...");
            loadPergunta();
        } else {
            console.log("Erro: " + resultado.message);
        }
    });
}

function marcaResposta(obj) {
    $("#box-respostas ul li").removeClass('selected');
    if (!obj.hasClass('selected'))
        obj.addClass('selected');
    else
        obj.removeClass('selected');
}

function buscaCadastro(cpf) {
    $.post('/Conta/Busca', { documento: cpf, "__RequestVerificationToken": $('input[name="__RequestVerificationToken"]').val() }, function (resultado) {
        if (resultado.status == "OK") {
            $("#Nome").val(resultado.nome);
            $("#Email").val(resultado.email);
        }
    });
}