$(document).ready(function () {

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
            Descricao: {
                required: true,
                maxlength: 30,
                textoApenas: true 
            }
        },
        messages: {
            Descricao: {
                required: "O campo Descrição é obrigatório.",
                maxlength: "O campo Descrição não pode ter mais de 30 caracteres.",
                textoApenas: "Digite somente letras e espaços e caracteres especiais."
            }
        }
    });

    $('#btnSalvar').click(function () {

        if ($('#form').valid() != true) {
            FormularioInvalidoAlert();
            return;
        }

        let departamento = SerielizeForm($('#form'));
        let url = $('#form').attr('action');

        $.ajax({
            type: "POST",
            url: url,
            data: departamento,
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
    });

});
