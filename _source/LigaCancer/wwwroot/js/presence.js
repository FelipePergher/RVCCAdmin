$(function () {
    $('.date').datepicker({
        uiLibrary: 'bootstrap',
        format: 'dd/mm/yyyy',
        iconsLibrary: 'fontawesome',
    });
});

$("#modal-action").on("show.bs.modal", function (e) {
    var link = $(e.relatedTarget);
    $(this).find(".modal-content").load(link.attr("href"), function (e) {
        $.validator.unobtrusive.parse("form");
        $(function () {
            $(".select2").select2({
                language: languageSelect2
            });
            $('.date').datepicker({
                uiLibrary: 'bootstrap',
                format: 'dd/mm/yyyy',
                iconsLibrary: 'fontawesome'
            });
        });
    });
});

$(function () {
    $(".select2").select2({
        language: languageSelect2
    });
});

function AjaxErrorPresence(error) {
    swal("Oops...", "Alguma coisa deu errado!\n", "error");
}

function AjaxSuccessPresence(data) {
    if (data === "200") {
        $("#modal-action").modal("hide");
        $("#modal-action").removeClass("fade");
        if (dataTable !== null) {
            dataTable.ajax.reload();
        }
        swal("Sucesso...", "Registro salvo com sucesso", "success").then((result) => {
            if (dataTable === null) {
                location.reload();
            }
        });
    }
    else {
        $("#modal-content").html(data);
    }
}