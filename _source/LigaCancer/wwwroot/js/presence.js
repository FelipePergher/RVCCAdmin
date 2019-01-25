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
        { data: "hour", title: "Hora da presença" }
    ]
});

$(function () {
    calendar("DateTo");
    calendar("DateFrom");
    $(".select2").select2({
        language: languageSelect2
    });
});


$("#modal-action").on("show.bs.modal", function (e) {
    var link = $(e.relatedTarget);
    $(this).find(".modal-content").load(link.attr("href"), function (e) {
        $.validator.unobtrusive.parse("form");
        $(".select2").select2({
            language: languageSelect2
        });
        time("Time");
        calendar("Date");
    });
});

function Error(error) {
    swal("Oops...", "Alguma coisa deu errado!\n", "error");
}

function Success(data, textStatus) {
    if (data === "" && textStatus === "success") {
        $("#modal-action").modal("hide");

        swal("Sucesso...", "Registro salvo com sucesso", "success").then((result) => {
            presenceTable.ajax.reload(null, false);
        });
    }
    else {
        $("#modal-content").html(data);
    }
}

