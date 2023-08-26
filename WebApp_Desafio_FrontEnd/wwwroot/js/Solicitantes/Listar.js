﻿$(document).ready(function () {

    var table = $('#dataTables-Solicitantes').DataTable({
        paging: true,
        processing: true,
        serverSide: true,
        searching: false,
        ordering: true,
        ajax: {
            url: config.contextPath + 'Solicitantes/Datatable',
            type: 'GET',
            data: function (d) {
                d.search = $('#PesquisarSolicitante').val();
            }
        },
        columns: [
            { data: 'ID' },
            { data: 'Solicitante' },
            { data: 'CPF' },
            { data: 'DataCriacaoWrapper', title: 'Data Criacao' },
        ],
        language: {
            "sEmptyTable": "Nenhum registro encontrado",
            "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
            "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
            "sInfoFiltered": "(Filtrados de _MAX_ registros)",
            "sInfoPostFix": "",
            "sInfoThousands": ".",
            "sLengthMenu": "_MENU_ resultados por página",
            "sLoadingRecords": "Carregando...",
            "sProcessing": "Processando...",
            "sSearch": "Pesquisar:",
            "sZeroRecords": "Nenhum registro encontrado",
            "oPaginate": {
                "sFirst": "Primeiro",
                "sLast": "Último",
                "sNext": "Próximo",
                "sPrevious": "Anterior"
            },
            "oAria": {
                "sSortAscending": ": Ordenar colunas de forma ascendente",
                "sSortDescending": ": Ordenar colunas de forma descendente"
            }
        }
    });

    $('#dataTables-Solicitantes tbody').on('click', 'tr', function () {
        if ($(this).hasClass('selected')) {
            $(this).removeClass('selected');
        } else {
            table.$('tr.selected').removeClass('selected');
            $(this).addClass('selected');
        }
    });

    $('#btnRelatorio').click(function () {
        window.location.href = config.contextPath + 'Solicitantes/Report';
    });

    $('#btnAdicionar').click(function () {
        window.location.href = config.contextPath + 'Solicitantes/Cadastrar';
    });

    var editDoubleClickTimeout;
    $('#btnEditar').on('click', function () {
    }).on('dblclick', function () {
        clearTimeout(editDoubleClickTimeout);
        var data = table.row('.selected').data();
        if (data) {
            window.location.href = config.contextPath + 'Solicitantes/Editar/' + data.ID;
        }
    });

    $('#btnExcluir').click(function () {

        let data = table.row('.selected').data();
        let idRegistro = data.ID;
        if (!idRegistro || idRegistro <= 0) {
            return;
        }

        if (idRegistro) {
            Swal.fire({
                text: "Tem certeza de que deseja excluir " + data.Solicitante + " ?",
                type: "warning",
                showCancelButton: true,
            }).then(function (result) {

                if (result.value) {
                    $.ajax({
                        url: config.contextPath + 'Solicitantes/Excluir/' + idRegistro,
                        type: 'DELETE',
                        contentType: 'application/json',
                        error: function (result) {

                            Swal.fire({
                                text: result,
                                confirmButtonText: 'OK',
                                icon: 'error'
                            });

                        },
                        success: function (result) {

                            Swal.fire({
                                type: result.Type,
                                title: result.Title,
                                text: result.Message,
                            }).then(function () {
                                table.draw();
                            });
                        }
                    });
                } else {
                    console.log("Cancelou a exclusão.");
                }

            });
        }
    });

    $('#PesquisarSolicitante').on('keyup', function () {
        var inputText = $(this).val();
        $.ajax({
            url: config.contextPath + 'Solicitantes/PesquisarSolicitantes',
            type: 'POST',
            data: { solicitante: inputText },
            success: function (data) {
                table.clear().rows.add(data.data).draw();
            },
            error: function (error) {
                console.error(error);
            }
        });
    });

});
