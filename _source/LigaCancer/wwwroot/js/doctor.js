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
            swalWithBootstrapButtons.fire("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
        }
    },
    order: [1, "asc"],
    columns: [
        { data: "actions", title: "Ações", name: "actions", orderable: false },
        { data: "name", title: "Nome", name: "name", },
        { data: "crm", title: "CRM", name: "crm" }
    ]
});

$("#modal-action").on("show.bs.modal", function (e) {
    var link = $(e.relatedTarget);
    $(this).find(".modal-content").load(link.attr("href"), function (e) {
        $.validator.unobtrusive.parse("form");
    });
});

function Error(error) {
    swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
}

function Success(data, textStatus) {
    if (data === "" && textStatus === "success") {
        $("#modal-action").modal("hide");

        swalWithBootstrapButtons.fire("Sucesso...", "Registro salvo com sucesso", "success").then((result) => {
            doctorTable.ajax.reload(null, false);
        });
    }
    else {
        $("#modal-content").html(data);
    }
}
