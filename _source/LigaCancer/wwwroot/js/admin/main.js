const swalWithBootstrapButtons = Swal.mixin({
    confirmButtonClass: 'btn btn-success ml-2',
    cancelButtonClass: 'btn btn-danger',
    buttonsStyling: false
});

let language = {
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

function setupPhoneMaskOnField(selector) {
    var inputElement = $(selector);

    setCorrectPhoneMask(inputElement);
    inputElement.on('input, keyup', function () {
        setCorrectPhoneMask(inputElement);
    });
}

var SPMaskBehavior = function (val) {
    return val.replace(/\D/g, '').length === 11 ? '(00) 00000-0000' : '(00) 0000-00009';
}, spOptions = {
        onKeyPress: function (val, e, field, options) {
            field.mask(SPMaskBehavior.apply({}, arguments), options);
        }
    };

let masks = {
    Cpf: '000.000.000-00',
    Price: '000.000.000.000.000,00',
    date: '99/99/9999'
};

function calendar(id) {
    $("#" + id).datepicker({
        format: "dd/mm/yyyy",
        iconsLibrary: "fontawesome",
        locale: "pt-br",
        uiLibrary: "bootstrap4"
    });
    $('#' + id).mask(masks.date, { placeholder: "dd/mm/yyyy" });
}

function time(id) {
    let timepicker = $("#" + id).timepicker({
        uiLibrary: "bootstrap4",
        modal: false,
        header: false,
        footer: false,
        mode: '24hr'
    });
    $("#" + id).siblings(".input-group-append").find("i").removeClass("gj-icon").removeClass("clock").addClass("fa").addClass("fa-clock");

    $("#" + id).click(function () {
        $(".timepicker").is(":visible") ? timepicker.close() : timepicker.open();
    });
}

//Modal Primary functions
function openModal(url, title, callback = null) {
    $("#modal-title").text(title);

    $("#modalBody").load(url, function () {
        if (callback !== null) callback();
        hideSpinnerModal();
    });

    $("#modal-action").on("hidden.bs.modal", function (e) {
        cleanModal();
    });
}

function cleanModal() {
    $("#modalBody").html("");
    $("#modal-title").text("");
    $("#modal-dialog").removeClass("modal-lg");
    $("#modal-dialog").removeClass("modal-elg");
    showSpinnerModal();
    setTimeout(function () {
        $(".swal2-confirm").focus();
    }, 10);
}

function showSpinnerModal() {
    $("#modalSpinner").show();
}

function hideSpinnerModal() {
    $("#modalSpinner").hide();
}

//Secondary Modal Functions
function openModalSecondary(url, title, callback = null) {
    setTimeout(function () {
        let overlaySecondModal = $(".modal-backdrop.fade.show");
        if (overlaySecondModal.length === 2) $(overlaySecondModal[1]).css("z-index", 1050);
    }, 10);

    $("#modal-title-secondary").text(title);

    $("#modalBodySecondary").load(url, function () {
        if (callback !== null) callback();
        hideSpinnerModalSecondary();
    });

    $("#modal-action-secondary").on("hidden.bs.modal", function (e) {
        cleanModalSecondary();
    });
}

function cleanModalSecondary() {
    $("#modalBodySecondary").html("");
    $("#modal-title-secondary").text("");
    showSpinnerModalSecondary();
    setTimeout(function () {
        $(".swal2-confirm").focus();
    }, 10);
}

function showSpinnerModalSecondary() {
    $("#modalSpinnerSecondary").show();
}

function hideSpinnerModalSecondary() {
    $("#modalSpinnerSecondary").hide();
}

$(function () {
    if (!!$.validator) {
        $.validator.setDefaults({
            highlight: function highlight(element) {
                $(element).addClass('is-invalid').removeClass('is-valid');
            },
            // eslint-disable-next-line object-shorthand
            unhighlight: function unhighlight(element) {
                $(element).addClass('is-valid').removeClass('is-invalid');
            },
            errorElement: 'span',
            errorPlacement: function errorPlacement(error, element) {
                error.addClass('invalid-feedback');
                element.prop('type') === 'checkbox' ? error.insertAfter(element.parent('label')) : error.insertAfter(element);
            }
        });
    }
});
