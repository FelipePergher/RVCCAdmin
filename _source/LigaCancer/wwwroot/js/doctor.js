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

function Error(error) {
    swal("Oops...", "Alguma coisa deu errado!\n", "error");
}

function Success(data, textStatus) {
    if (data === "" && textStatus === "success") {
        $("#modal-action").modal("hide");

        swal("Sucesso...", "Registro salvo com sucesso", "success").then((result) => {
            doctorTable.ajax.reload();
        });
    }
    else {
        $("#modal-content").html(data);
    }
}
