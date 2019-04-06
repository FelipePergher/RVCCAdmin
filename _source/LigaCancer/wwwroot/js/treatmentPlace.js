let treatmentPlaceTable = $("#treatmentPlaceTable").DataTable({
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
        url: "/api/TreatmentPlace/search",
        type: "POST",
        data: function (d) {
            d.city = $("#City").val();
        },
        datatype: "json",
        error: function () {
            swalWithBootstrapButtons.fire("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
        }
    },
    order: [1, "asc"],
    columns: [
        { data: "actions", title: "Ações", name: "actions", width: "20px", orderable: false },
        { data: "city", title: "Cidade", name: "city" }
    ],
    preDrawCallback: function (settings) {
        showSpinner();
    },
    drawCallback: function (settings) {
        $(".editTreatmentPlaceButton").click(function () {
            openModal($(this).attr("href"), $(this).data("title"), initEditForm);
        });

        $(".deleteTreatmentPlaceButton").click(function (e) {
            initDelete($(this).data("url"), $(this).data("id"), $(this).data("relation") === "True");
        });

        hideSpinner();
    }
});

$(function () {
    initPage();
});

function initPage() {
    $('#treatmentPlaceTable').attr('style', 'border-collapse: collapse !important');

    $("#addTreatmentPlaceButton").click(function () {
        openModal($(this).attr("href"), $(this).data("title"), initAddForm);
    });

    $("#searchForm").submit(function (e) {
        e.preventDefault();
        treatmentPlaceTable.search("").draw("");
    });
}

function initAddForm() {
    $.validator.unobtrusive.parse("#addTreatmentPlaceForm");
}

function AddSuccess(data, textStatus) {
    if (data === "" && textStatus === "success") {
        $("#modal-action").modal("hide");
        treatmentPlaceTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire("Sucesso...", "Local de tratamento registrado com sucesso.", "success");
    }
    else {
        $("#modal-content").html(data);
        initAddForm();
    }
}

function initEditForm() {
    $.validator.unobtrusive.parse("#editTreatmentPlaceForm");
}

function EditSuccess(data, textStatus) {
    if (data === "" && textStatus === "success") {
        $("#modal-action").modal("hide");
        treatmentPlaceTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire("Sucesso...", "Local de tratamento atualizado com sucesso.", "success");
    }
    else {
        $("#modal-content").html(data);
        initEditForm();
    }
}

function initDelete(url, id, relation) {
    let message = "Você não poderá reverter isso!";
    if (relation) message = "Este local de tratamento está atribuido a pacientes, deseja prosseguir mesmo assim?";

    swalWithBootstrapButtons({
        title: 'Você têm certeza?',
        text: message,
        type: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sim',
        cancelButtonText: 'Não',
        showLoaderOnConfirm: true,
        reverseButtons: true,
        preConfirm: () => {
            $.post(url, { id: id })
                .done(function (data, textStatus) {
                    treatmentPlaceTable.ajax.reload(null, false);
                    swalWithBootstrapButtons.fire("Removido!", "O local de tratamento foi removido com sucesso.", "success");
                }).fail(function (error) {
                    swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                });
        }
    });
}

function Error(error) {
    swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
}
