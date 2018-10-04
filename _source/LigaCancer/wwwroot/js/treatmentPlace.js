﻿$(function () {
    $("#modal-action-treatmentPlace").on('loaded.bs.modal', function (e) {
    }).on('hidden.bs.modal', function (e) {
        $(this).removeData('bs.modal');
    });
});

$("#modal-action-treatmentPlace").on("show.bs.modal", function (e) {
    var link = $(e.relatedTarget);
    $(this).find(".modal-content").load(link.attr("href"), function () {
        $.validator.unobtrusive.parse('form');
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