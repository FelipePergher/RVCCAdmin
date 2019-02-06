let presenceTable = $("#presenceTable").DataTable({
    dom: "l<'export-buttons'B>frtip",
    buttons: [
        {
            extend: 'pdf',
            className: 'btn btn-info',
            exportOptions: {
                columns: 'th:not(:first-child)'
            },
            customize: function (doc) {
                doc.defaultStyle.alignment = 'center';
                doc.styles.tableHeader.alignment = 'center';
            }
        },
        {
            extend: 'excel',
            className: 'btn btn-info',
            exportOptions: {
                columns: 'th:not(:first-child)'
            }
        }
    ],
    serverSide: true,
    language: language,
    filter: false,
    ajax: {
        url: "/api/presence/search",
        type: "POST",
        data: function (d) {
            d.name = $("#Name").val();
            d.surname = $("#Surname").val();
            d.dateFrom = $("#DateFrom").val();
            d.dateTo = $("#DateTo").val();
        },
        datatype: "json",
        error: function () {
            swalWithBootstrapButtons.fire("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
        }
    },
    order: [1, "desc"],
    columns: [
        { data: "actions", title: "Ações", name: "actions", orderable: false},
        { data: "date", title: "Data da presença", name: "date" },
        { data: "patient", title: "Nome do Paciente", name: "patient" },
        { data: "hour", title: "Hora da presença", name: "hour" }
    ],
    preDrawCallback: function (settings) {
        showSpinner();
    },
    drawCallback: function (settings) {
        $(".editPresenceButton").click(function () {
            openModal($(this).attr("href"), initEditForm);
        });

        $(".deletePresenceButton").click(function (e) {
            initDelete($(this).data("url"));
        });

        hideSpinner();
    }
});


$(function () {
    initPage();
});

function initPage() {
    $('#presenceTable').attr('style', 'border-collapse: collapse !important');
    calendar("DateTo");
    calendar("DateFrom");

    $(".select2").select2({
        language: languageSelect2
    });

    $("#addPresenceButton").click(function () {
        openModal($(this).attr("href"), initAddForm);
    });

    $("#searchForm").submit(function (e) {
        e.preventDefault();
        presenceTable.search("").draw("");
    });

    $("#modal-action").on("hidden.bs.modal", function (e) {
        $("#modal-content").html("");
    });
}

function initAddForm() {
    $.validator.unobtrusive.parse("#addPresenceForm");
    $(".select2").select2({
        language: languageSelect2
    });
    time("Time");
    calendar("Date");
}

function AddSuccess(data, textStatus) {
    if (data === "" && textStatus === "success") {
        $("#modal-action").modal("hide");
        presenceTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire("Sucesso...", "Presença registrada com sucesso.", "success");
    }
    else {
        $("#modal-content").html(data);
        initAddForm();
    }
}

function initEditForm() {
    $.validator.unobtrusive.parse("#editPresenceForm");
    $(".select2").select2({
        language: languageSelect2
    });
    time("Time");
    calendar("Date");
}

function EditSuccess(data, textStatus) {
    if (data === "" && textStatus === "success") {
        $("#modal-action").modal("hide");
        presenceTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire("Sucesso...", "Presença atualiada com sucesso.", "success");
    }
    else {
        $("#modal-content").html(data);
        initEditForm();
    }
}

function initDelete(url) {
    swalWithBootstrapButtons.queue([{
        title: 'Você têm certeza?',
        text: "Você não poderá reverter isso!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sim',
        cancelButtonText: 'Não',
        showLoaderOnConfirm: true,
        reverseButtons: true,
        preConfirm: () => {
            return fetch(url)
                .then(response => {
                    if (response.status === 200) {
                        presenceTable.ajax.reload(null, false);
                        return swalWithBootstrapButtons.fire("Removido!", "A presença foi removida com sucesso.", "success");
                    }
                    return swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                })
                .catch(error => {
                    Swal.showValidationMessage(
                        `Alguma coisa deu errado: ${error}`
                    );
                });
        }
    }]);
}

function Error(error) {
    swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
}