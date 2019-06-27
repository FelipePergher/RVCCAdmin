let presenceTable = $("#presenceTable").DataTable({
    //dom: "l<'export-buttons'B>frtip",
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
            d.dateFrom = $("#DateFrom").val();
            d.dateTo = $("#DateTo").val();
        },
        datatype: "json",
        error: function (error) {
            console.log(error);
            swalWithBootstrapButtons.fire("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
        }
    },
    order: [2, "desc"],
    columns: [
        { data: "actions", title: "Ações", name: "Actions", orderable: false},
        { data: "patient", title: "Nome do Paciente", name: "Patient" },
        { data: "date", title: "Data da presença", name: "Date" },
        { data: "hour", title: "Hora da presença", name: "Hour" }
    ],
    drawCallback: function (settings) {
        $(".editPresenceButton").click(function () {
            openModal($(this).attr("href"), $(this).data("title"), initEditForm);
        });

        $(".deletePresenceButton").click(function (e) {
            initDelete($(this).data("url"), $(this).data("id"));
        });
    }
});


$(function () {
    initPage();
});

function initPage() {
    $('#presenceTable').attr('style', 'border-collapse: collapse !important');
    calendar("DateTo");
    calendar("DateFrom");

    $(".select2").select2();

    $("#addPresenceButton").click(function () {
        openModal($(this).attr("href"), "Adicionar Presença", initAddForm);
    });

    $("#searchForm").submit(function (e) {
        e.preventDefault();
        presenceTable.search("").draw("");
    });
}

function initAddForm() {
    $.validator.unobtrusive.parse("#addPresenceForm");
    $(".select2").select2();
    time("Time");
    calendar("Date");
}

function addSuccess(data, textStatus) {
    if (!data && textStatus === "success") {
        $("#modal-action").modal("hide");
        presenceTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire("Sucesso...", "Presença registrada com sucesso.", "success");
    }
    else {
        $("#modalBody").html(data);
        initAddForm();
    }
}

function initEditForm() {
    $.validator.unobtrusive.parse("#editPresenceForm");
    time("Time");
    calendar("Date");
}

function editSuccess(data, textStatus) {
    if (!data && textStatus === "success") {
        $("#modal-action").modal("hide");
        presenceTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire("Sucesso", "Presença atualizada com sucesso.", "success");
    }
    else {
        $("#modalBody").html(data);
        initEditForm();
    }
}

function initDelete(url, id) {
    swalWithBootstrapButtons({
        title: 'Você têm certeza?',
        text: "Você não poderá reverter isso!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sim',
        cancelButtonText: 'Não',
        showLoaderOnConfirm: true,
        reverseButtons: true,
        preConfirm: () => {
            $.post(url, { id: id })
                .done(function (data, textStatus) {
                    presenceTable.ajax.reload(null, false);
                    swalWithBootstrapButtons.fire("Removido", "A presença foi removida com sucesso.", "success");
                }).fail(function (error) {
                    swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                });
        }
    });
}

function error(error) {
    swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
}