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

    $.getScript('/js/Mask.js', function () {
        console.log("Arquivo de funções personalizadas carregado com sucesso.");

        // Adicionando regra de validação para permitir somente texto
        $.validator.addMethod("textoApenas", function (value, element) {
            return /^[a-zA-Z\s'´`^,ãõáíéúêôç]*$/.test(value);
        }, "Digite somente letras e espaços.");

        $.validator.addMethod("numericoApenas", function (value, element) {
            return /^[0-9\s.-]*$/.test(value);
        }, "Digite somente números.");

        $.validator.addMethod("validaCPF", function (value, element) {
            return validarCPF(value);
        }, "Informe um CPF válido.");

        // Restante do seu código...

        $("#form").validate({
            rules: {
                Solicitante: {
                    required: true,
                    maxlength: 30,
                    textoApenas: true
                },
                CPF: {
                    required: true,
                    numericoApenas: true,
                    minlength: 14,
                    maxlength: 14,
                    validaCPF: true
                },
                DataCriacao: {
                    required: true
                }
            },
            messages: {
                Solicitante: {
                    required: "O campo Solicitante é obrigatório.",
                    maxlength: "O campo Solicitante não pode ter mais de 30 caracteres.",
                    textoApenas: "Digite somente letras e espaços e caracteres especiais."
                },
                CPF: {
                    required: "O campo CPF é obrigatório.",
                    minlength: "O campo CPF deve ter exatamente 14 caracteres.",
                    maxlength: "O campo CPF deve ter exatamente 14 caracteres.",
                    numericoApenas: "Digite somente números."
                },
                DataCriacao: {
                    required: "O campo Data de Criação é obrigatório."
                }
            }
        });

        $('#CPF').mask('000.000.000-00');

        // Desabilitar a edição direta do campo DataCriacao
        $("input[name='DataCriacao']").attr("readonly", "readonly");

        $('#btnSalvar').click(function () {
            if ($('#form').valid()) {
                let solicitante = SerielizeForm($('#form'));
                let url = $('#form').attr('action');

                $.ajax({
                    type: "POST",
                    url: url,
                    data: solicitante,
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
});
