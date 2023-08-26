$(document).ready(function () {
    $('.glyphicon-calendar').closest("div.date").datepicker({
        todayBtn: "linked",
        keyboardNavigation: false,
        forceParse: false,
        calendarWeeks: false,
        format: 'dd/mm/yyyy',
        autoclose: true,
        language: 'pt-BR',
        startDate: new Date() // Definir data mínima como hoje
    });

    $('#btnCancelar').click(function () {
        Swal.fire({
            html: "Deseja cancelar essa operação? O registro não será salvo.",
            type: "warning",
            showCancelButton: true,
        }).then(function (result) {
            if (result.value) {
                history.back();
            } else {
                console.log("Cancelou a inclusão.");
            }
        });
    });

    // Adicionando regra de validação para permitir somente texto
    $.validator.addMethod("textoApenas", function (value, element) {
        return /^[a-zA-Z\s'´`^,ãõáíéúêôç]*$/.test(value);
    }, "Digite somente letras e espaços.");

    $("#form").validate({
        rules: {
            Assunto: {
                required: true,
                maxlength: 30,
                textoApenas: true
            },
            Solicitante: {
                required: true,
                maxlength: 30,
                textoApenas: true
            },
            DataAbertura: {
                required: true
            },
            IdDepartamento: {
                required: true
            }
        },
        messages: {
            Assunto: {
                required: "O campo Assunto é obrigatório.",
                maxlength: "O campo Assunto não pode ter mais de 30 caracteres.",
                textoApenas: "Digite somente letras e espaços e caracteres especiais."
            },
            Solicitante: {
                required: "O campo Solicitante é obrigatório.",
                maxlength: "O campo Solicitante não pode ter mais de 30 caracteres.",
                textoApenas: "Digite somente letras e espaços e caracteres especiais."
            },
            DataAbertura: {
                required: "O campo Data de Abertura é obrigatório."
            },
            IdDepartamento: {
                required: "Selecione um Departamento."
            }
        }
    });

    // Desabilitar a edição direta do campo DataAbertura
    $("input[name='DataAbertura']").attr("readonly", "readonly");

    $('#btnSalvar').click(function () {
        if ($('#form').valid()) {
            let chamado = SerielizeForm($('#form'));
            let url = $('#form').attr('action');

            $.ajax({
                type: "POST",
                url: url,
                data: chamado,
                success: function (result) {
                    Swal.fire({
                        type: result.Type,
                        title: result.Title,
                        text: result.Message,
                    }).then(function () {
                        window.location.href = config.contextPath + result.Controller + '/' + result.Action;
                    });
                },
                error: function (result) {
                    Swal.fire({
                        text: result,
                        confirmButtonText: 'OK',
                        icon: 'error'
                    });
                },
            });
        }
    });
});
