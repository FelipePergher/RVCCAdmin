$(function () {
    $('.date').datepicker({
        format: 'dd/mm/yyyy',
        iconsLibrary: 'fontawesome',
    });
    $(".select2").select2({
        language: languageSelect2
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
                format: 'dd/mm/yyyy',
                iconsLibrary: 'fontawesome'
            });
            $('#Time').timepicker({
                iconsLibrary: 'fontawesome'
            });

        });
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

let presenceTable = $("#presenceTable").DataTable({
    processing: true,
    serverSide: true,
    language: language,
    filter: false,
    ajax: {
        url: "/api/presence/search",
        type: "POST",
        datatype: "json",
        error: function () {
            swal("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
        }
    },
    order: [[0, "asc"]],
    columns: [
        { data: "actions", title: "Ações" },
        { data: "patient", title: "Nome do Paciente" },
        { data: "date", title: "Data da presença" },
        { data: "hour", title: "Hora da presença" },
        
    ]
});