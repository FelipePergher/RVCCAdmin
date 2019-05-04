const swalWithBootstrapButtons = Swal.mixin({
    confirmButtonClass: 'btn btn-success ml-2',
    cancelButtonClass: 'btn btn-danger',
    buttonsStyling: false
});

let language = {
    emptyTable: "Nenhum dado encontrado!",
    info: "Mostrando _START_ até _END_ de _TOTAL_ registros - Página _PAGE_ de _PAGES_",
    infoEmpty: "",
    zeroRecords: "Não foi encontrado resultados",
    lengthMenu: "Mostrar _MENU_ registros por página",
    loadingRecords: "<span class='fa fa-spinner fa-pulse' style='font-size: 35px; margin-left:-60px; ' ></span>",
    paginate: {
        first: "Primeiro",
        last: "Último",
        next: "Próximo",
        previous: "Anterior"
    },
    aria: {
        sortAscending: ": ativar ordenação ascendente",
            sortDescending: ": ativar ordenação descendente"
    }
};

let languageSelect2 = {
    noResults: function () {
        return "Não foi encontrado resultados";
    }
};

function calendar(id) {
    $("#" + id).datepicker({
        format: "dd/mm/yyyy",
        iconsLibrary: "fontawesome",
        uiLibrary: "bootstrap4",
        showRightIcon: false
    });
}

function time(id) {
    let timepicker = $("#" + id).timepicker({
        iconsLibrary: "fontawesome",
        uiLibrary: "bootstrap4",
        modal: false,
        header: false,
        footer: false,
        mode: '24hr'
    });
    $(".input-group-append").remove();

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
}

function showSpinnerModal() {
    $("#modalSpinner").show();
}

function hideSpinnerModal() {
    $("#modalSpinner").hide();
}

//Secondary Modal Functions
function openModalSecondary(url, title, callback = null) {
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
}

function showSpinnerModalSecondary() {
    $("#modalSpinnerSecondary").show();
}

function hideSpinnerModalSecondary() {
    $("#modalSpinnerSecondary").hide();
}
