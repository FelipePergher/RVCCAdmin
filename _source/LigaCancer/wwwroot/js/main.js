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

function openModal(url, title, callback) {
    $("#modal-title").text(title);

    $("#modalBody").load(url, function () {
        if (callback !== null) callback();
        hideSpinnerModal();
    });

    $("#modal-action").on("hidden.bs.modal", function (e) {
        $("#modalBody").html("");
        $("#modal-title").text("");
        showSpinnerModal();
    });
}

function showSpinner() {
    $("#spinner-container").show();
}

function hideSpinner() {
    $("#spinner-container").hide();
}

function showSpinnerModal() {
    $("#modalSpinner").show();
}

function hideSpinnerModal() {
    $("#modalSpinner").hide();
}

//function AjaxError(error) {
//    swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
//}

//function AjaxSuccess(data) {
//    if (data === "200") {
//        $("#modal-action").modal("hide");
//        $("#modal-action").removeClass("fade");
//        if (dataTable !== null) {
//            dataTable.ajax.reload();
//        }
//        swalWithBootstrapButtons.fire("Sucesso...", "Registro salvo com sucesso", "success").then((result) => {
//            if (dataTable === null) {
//                location.reload();
//            }
//        });
//    }
//    else {
//        $("#modal-content").html(data);
//    }
//}

//$("#modal-action").on("show.bs.modal", function (e) {
//    var link = $(e.relatedTarget);
//    $(this).find(".modal-content").load(link.attr("href"), function (e) {
//        $.validator.unobtrusive.parse("form");

//        if ($(".customSelect2").length > 0) {
//            $(".customSelect2").select2({
//                theme: "bootstrap",
//                language: languageSelect2
//            });
//        }
//    });
//});

//function errorDataTable() {
//    swalWithBootstrapButtons.fire("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
//}

//function DateFormat(dateOfBirth) {
//    let date = new Date(dateOfBirth);
//    return date.toLocaleDateString("pt-BR");
//}