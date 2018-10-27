$(function () {
    $("#modal-action-user").on('loaded.bs.modal', function (e) {
    }).on('hidden.bs.modal', function (e) {
        $(this).removeData('bs.modal');
    });

    $("#userTable").DataTable({
        paginate: false,
        filter: false,
        order: [[0, "asc"]],
        columnDefs: [{
            targets: 3,
            orderable: false
        }],
        language: {
            emptyTable: "Nenhuma informação cadastrada!"
        }
    });
});

$("#modal-action-user").on("show.bs.modal", function (e) {
    var link = $(e.relatedTarget);
    $(this).find(".modal-content").load(link.attr("href"), function (e) {
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