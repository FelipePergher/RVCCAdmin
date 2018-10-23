$(function () {
    $("#modal-action-patient-details").on('loaded.bs.modal', function (e) {
    }).on('hidden.bs.modal', function (e) {
        $(this).removeData('bs.modal');
    });

    $("#familyMemberTable").DataTable({
        paginate: false,
        filter: false,
        info: false,
        order: [[0, "asc"]],
        columnDefs: [{
            targets: 5,
            orderable: false
        }],
        language: {
            emptyTable: "Nenhuma informação adicionada ainda!"
        }
    });

    $("#addressTable").DataTable({
        paginate: false,
        filter: false,
        info: false,
        order: [[0, "asc"]],
        columnDefs: [{
            targets: 6,
            orderable: false
        }],
        language: {
            emptyTable: "Nenhuma informação adicionada ainda!"
        }
    });

    $("#phoneTable").DataTable({
        paginate: false,
        filter: false,
        info: false,
        order: [[0, "asc"]],
        columnDefs: [{
            targets: 3,
            orderable: false
        }],
        language: {
            emptyTable: "Nenhuma informação adicionada ainda!"
        }
    });

    $("#attachmentsTable").DataTable({
        paginate: false,
        filter: false,
        info: false,
        order: [[0, "asc"]],
        columnDefs: [{
            targets: 2,
            orderable: false
        }],
        language: {
            emptyTable: "Nenhuma informação adicionada ainda!"
        }
    });
});

$("#modal-action-patient-details").on("show.bs.modal", function (e) {
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