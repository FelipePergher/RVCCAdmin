var dataTable = null;
var language = {
    emptyTable: "Nenhuma informação cadastrada!",
    info: "Mostrando _START_ até _END_ de _TOTAL_ registros - Página _PAGE_ de _PAGES_",
    infoEmpty: "",
    search: "Procurar",
    zeroRecords: "Não foi encontrado resultados",
    lengthMenu: "Mostrar _MENU_ registros por página",
    processing: '<span class="fa fa-spinner fa-pulse" style="font-size: 35px;" ></span>',
    loadingRecords: '<span class="fa fa-spinner fa-pulse" style="font-size: 35px; margin-left: -60px;" ></span>',
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

        if ($(".select2").length > 0) {
            $(".select2").select2({
                theme: "bootstrap"
            });
        }
    });
});

function errorDataTable() {
    swal("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
}

function DateFormat(dateOfBirth) {
    let date = new Date(dateOfBirth);
    return date.toLocaleDateString("pt-BR");
}