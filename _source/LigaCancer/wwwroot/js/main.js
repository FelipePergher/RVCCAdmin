var dataTable = null;
var language = {
    emptyTable: "Nenhuma informação cadastrada!",
    processing: '<div id="loaderSpan" class="loading" ></div>',
    info: "Mostrando _START_ até _END_ de _TOTAL_ registros - Página _PAGE_ de _PAGES_",
    infoEmpty: "",
    search: "Procurar",
    lengthMenu: "Mostrar _MENU_ registros por página",
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

function AjaxError(error = null) {
    console.log(error);
    swal("Oops...", "Alguma coisa deu errado!\n", "error");
}

function AjaxSuccess(data) {
    if (data === "200") {
        $("#modal-action").modal('hide');
        $('#modal-action').removeClass("fade");
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

$("#modal-action").on("show.bs.modal", function (e) {
    var link = $(e.relatedTarget);
    $(this).find(".modal-content").load(link.attr("href"), function (e) {
        $.validator.unobtrusive.parse('form');
    });
});