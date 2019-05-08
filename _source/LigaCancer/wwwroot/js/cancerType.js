let cancerTypeTable = $("#cancerTypeTable").DataTable({
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
        url: "/api/CancerType/search",
        type: "POST",
        data: function (d) {
            d.name = $("#Name").val();
        },
        datatype: "json",
        error: function () {
            swalWithBootstrapButtons.fire("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
        }
    },
    order: [1, "asc"],
    columns: [
        { data: "actions", title: "Ações", name: "actions", width: "20px", orderable: false },
        { data: "name", title: "Nome", name: "name" }
    ],
    drawCallback: function (settings) {
        $(".editCancerTypeButton").click(function () {
            openModal($(this).attr("href"), $(this).data("title"), initEditForm);
        });

        $(".deleteCancerTypeButton").click(function (e) {
            initDelete($(this).data("url"), $(this).data("id"), $(this).data("relation") === "True");
        });
    }
});

$(function () {
    initPage();
});

function initPage() {
    $('#cancerTypeTable').attr('style', 'border-collapse: collapse !important');

    $("#addCancerTypeButton").click(function () {
        openModal($(this).attr("href"), $(this).data("title"), initAddForm);
    });

    $("#searchForm").submit(function (e) {
        e.preventDefault();
        cancerTypeTable.search("").draw("");
    });
}

function initAddForm() {
    $.validator.unobtrusive.parse("#addCancerTypeForm");
}

function addSuccess(data, textStatus) {
    if (!data && textStatus === "success") {
        $("#modal-action").modal("hide");
        cancerTypeTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire("Sucesso...", "Tipo de câncer registrado com sucesso.", "success");
    }
    else {
        $("#modalBody").html(data);
        initAddForm();
    }
}

function initEditForm() {
    $.validator.unobtrusive.parse("#editCancerTypeForm");
}

function editSuccess(data, textStatus) {
    if (!data && textStatus === "success") {
        $("#modal-action").modal("hide");
        cancerTypeTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire("Sucesso...", "Tipo de câncer  atualizado com sucesso.", "success");
    }
    else {
        $("#modalBody").html(data);
        initEditForm();
    }
}

function initDelete(url, id, relation) {
    let message = "Você não poderá reverter isso!";
    if (relation) message = "Este tipo de câncer está atribuido a pacientes, deseja prosseguir mesmo assim?";

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
                    cancerTypeTable.ajax.reload(null, false);
                    swalWithBootstrapButtons.fire("Removido!", "O local de tratamento foi removido com sucesso.", "success");
                }).fail(function (error) {
                    swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                });
        }
    });
}

function error(error) {
    swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
}