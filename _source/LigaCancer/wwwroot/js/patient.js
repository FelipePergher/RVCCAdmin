﻿$(function () {
    $("#modal-action-patient").on('loaded.bs.modal', function (e) {
    }).on('hidden.bs.modal', function (e) {
        $(this).removeData('bs.modal');
    });
});

$("#modal-action-patient").on("show.bs.modal", function (e) {
    var link = $(e.relatedTarget);
    $(this).find(".modal-content").load(link.attr("href"), function () {
        $.validator.unobtrusive.parse('form');

        $("select").select2({
            theme: "bootstrap"
        });

    });
});

function AjaxError($xhr) {
    console.log($xhr);
    swal("Oops...", "Alguma coisa deu errado!\n", "error");
}

function AjaxSuccess(data) {
    if (data === "200") {
        location.reload();
    }
    else {
        $("#modal-content").html(data);
    }
}