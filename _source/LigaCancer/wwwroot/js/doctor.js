let doctorTable = $("#doctorTable").DataTable({
    processing: true,
    serverSide: true,
    language: language,
    filter: false,
    ajax: {
        url: "/api/doctor/search",
        type: "POST",
        datatype: "json",
        error: function() {
            swal("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
        }
    },
    order: [[0, "asc"]],
    columns: [
        { data: "actions", title: "Ações" },
        { data: "name", title: "Nome" },
        { data: "crm", title: "CRM" }
    ]
});

$("#modal-action").on("show.bs.modal", function (e) {
    var link = $(e.relatedTarget);
    $(this).find(".modal-content").load(link.attr("href"), function (e) {
        $.validator.unobtrusive.parse("form");
    });
});

function Error(error) {
    swal("Oops...", "Alguma coisa deu errado!\n", "error");
}

function Success(data, textStatus) {
    if (data === "" && textStatus === "success") {
        $("#modal-action").modal("hide");

        swal("Sucesso...", "Registro salvo com sucesso", "success").then((result) => {
            doctorTable.ajax.reload(null, false);
        });
    }
    else {
        $("#modal-content").html(data);
    }
}
