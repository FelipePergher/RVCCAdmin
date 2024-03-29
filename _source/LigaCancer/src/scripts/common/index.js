﻿"use strict";
import "bootstrap";
import "@coreui/coreui";
import Swal from "sweetalert2";

export default (function () {

    global.datatablesLanguage = {
        "sEmptyTable": "Nenhum registro encontrado",
        "sInfo": "Mostrando de _START_ até _END_ de _TOTAL_ registros",
        "sInfoEmpty": "Mostrando 0 até 0 de 0 registros",
        "sInfoFiltered": "(Filtrados de _MAX_ registros)",
        "sInfoPostFix": "",
        "sInfoThousands": ".",
        "sLengthMenu": "_MENU_ resultados por página",
        "sLoadingRecords": "Carregando...",
        "sProcessing": "Processando...",
        "sZeroRecords": "Nenhum registro encontrado",
        "sSearch": "Pesquisar",
        "oPaginate": {
            "sNext": "Próximo",
            "sPrevious": "Anterior",
            "sFirst": "Primeiro",
            "sLast": "Último"
        },
        "oAria": {
            "sSortAscending": ": Ordenar colunas de forma ascendente",
            "sSortDescending": ": Ordenar colunas de forma descendente"
        }
    };

    global.select2Language = {
        errorLoading: function () {
            return "Os resultados não puderam ser carregados.";
        },
        inputTooLong: function (args) {
            var overChars = args.input.length - args.maximum;

            var message = "Apague " + overChars + " caracter";

            if (overChars != 1) {
                message += "es";
            }

            return message;
        },
        inputTooShort: function (args) {
            var remainingChars = args.minimum - args.input.length;

            return "Digite " + remainingChars + " ou mais caracteres";
        },
        loadingMore: function () {
            return "Carregando mais resultados…";
        },
        maximumSelected: function (args) {
            var message = "Você só pode selecionar " + args.maximum + " ite";

            if (args.maximum == 1) {
                message += "m";
            } else {
                message += "ns";
            }

            return message;
        },
        noResults: function () {
            return "Nenhum resultado encontrado";
        },
        searching: function () {
            return "Buscando…";
        },
        removeAllItems: function () {
            return "Remover todos os itens";
        }
    };

    global.setupValidator = function (validator) {
        validator.setDefaults({
            highlight: function highlight(element) {
                $(element).addClass("is-invalid").removeClass("is-valid");
            },
            // eslint-disable-next-line object-shorthand
            unhighlight: function unhighlight(element) {
                $(element).addClass("is-valid").removeClass("is-invalid");
            },
            errorElement: "span",
            errorPlacement: function errorPlacement(error, element) {
                error.addClass("invalid-feedback");
                element.prop("type") === "checkbox" ? error.insertAfter(element.parent("label")) : error.insertAfter(element);
            }
        });
    };

    global.openModal = function (url, title, callback = null, largeModal = false) {
        global.cleanModal();
        largeModal ? $("#modal-dialog").addClass("modal-lg") : $("#modal-dialog").removeClass("modal-lg");

        $("#modal-title").html(title);

        $("#modalBody").load(url, function () {
            if (callback !== null) {
                callback();
            }
            hideSpinnerModal();
        });

        $("#modal-action").off("shown.bs.modal").on("shown.bs.modal", function () {
            $(this).find("input,select").filter(":visible:first").trigger("focus");
        });
    };

    global.swalWithBootstrapButtons = Swal.mixin({
        confirmButtonClass: "btn btn-success ml-2",
        cancelButtonClass: "btn btn-danger",
        buttonsStyling: false,
        confirmButtonText: "Sim",
        cancelButtonText: "Não",
        reverseButtons: true
    });

    global.cleanModal = function () {
        $("#modalBody").html("");
        $("#modal-title").text("");
        showSpinnerModal();
    };

    global.eyePassword = function () {
        $(".eyePassword").click(function () {
            let icon = $(this);
            let inputPassword = icon.siblings("input");
            if (icon.hasClass("fa-eye")) {
                icon.removeClass("fa-eye").addClass("fa-eye-slash");
                inputPassword.attr("type", "password");
            } else {
                icon.addClass("fa-eye").removeClass("fa-eye-slash");
                inputPassword.attr("type", "text");
            }
        });
    };

    global.masks = {
        Cpf: "000.000.000-00",
        Price: "000.000.000.000.000,00",
        date: "99/99/9999"
    };

    global.setupPhoneMaskOnField = function (selector) {
        var inputElement = $(selector);

        setCorrectPhoneMask(inputElement);
        inputElement.on("input, keyup", function () {
            setCorrectPhoneMask(inputElement);
        });
    };

    global.SPMaskBehavior = function (val) {
        return val.replace(/\D/g, "").length === 11 ? "(00) 00000-0000" : "(00) 0000-00009";
    };

    global.spOptions = {
        onKeyPress: function (val, e, field, options) {
            field.mask(SPMaskBehavior.apply({}, arguments), options);
        }
    };

    function showSpinnerModal() {
        $("#modalSpinner").show();
    }

    function hideSpinnerModal() {
        $("#modalSpinner").hide();
    }

}());