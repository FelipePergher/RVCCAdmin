let doctorTable = $("#doctorTable").DataTable({
    dom: "l<'export-buttons'B>frtip",
    buttons: [
        {
            extend: 'pdf',
            orientation: 'landscape',
            pageSize: 'LEGAL',
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
            exportOptions: {
                columns: 'th:not(:first-child)'
            }
        }
    ],
    processing: true,
    serverSide: true,
    language: language,
    filter: false,
    ajax: {
        url: "/api/doctor/search",
        type: "POST",
        data: function (d) {
            d.name = $("#Name").val();
            d.CRM = $("#CRM").val();
        },
        datatype: "json",
        error: function() {
            swalWithBootstrapButtons.fire("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
        }
    },
    order: [1, "asc"],
    columns: [
        { data: "actions", title: "Ações", name: "actions", width:"20px", orderable: false },
        { data: "name", title: "Nome", name: "Name" },
        { data: "crm", title: "CRM", name: "CRM" }
    ],
    drawCallback: function (settings) {
        $(".editDoctorButton").click(function () {
            openModal($(this).attr("href"), $(this).data("title"), initEditForm);
        });

        $(".deleteDoctorButton").click(function (e) {
            initDelete($(this).data("url"), $(this).data("id"), $(this).data("relation") === "True");
        });
    }
});

$(function () {
    initPage();
});

function initPage() {
    $('#doctorTable').attr('style', 'border-collapse: collapse !important');
    setupValidation();

    $("#searchForm").submit(function (e) {
        e.preventDefault();
        doctorTable.search("").draw("");
    });

    $("#addDoctorButton").click(function () {
        openModal($(this).attr("href"), $(this).data("title"), initAddForm);
    });
}

function initAddForm() {
    $.validator.unobtrusive.parse("#addDoctorForm");
}

function addSuccess(data, textStatus) {
    if (!data && textStatus === "success") {
        $("#modal-action").modal("hide");
        doctorTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire("Sucesso", "Médico registrado com sucesso.", "success");
    }
    else {
        $("#modalBody").html(data);
        initAddForm();
    }
}

function initEditForm() {
    $.validator.unobtrusive.parse("#editDoctorForm");
}

function editSuccess(data, textStatus) {
    if (!data && textStatus === "success") {
        $("#modal-action").modal("hide");
        doctorTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire("Sucesso", "Médico atualizado com sucesso.", "success");
    }
    else {
        $("#modalBody").html(data);
        initEditForm();
    }
}

function initDelete(url, id, relation) {
    let message = "Você não poderá reverter isso!";
    if (relation) message = "Este médico está atribuido a pacientes, deseja prosseguir mesmo assim?";

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
                    doctorTable.ajax.reload(null, false);
                    swalWithBootstrapButtons.fire("Removido!", "O médico foi removido com sucesso.", "success");
                }).fail(function (error) {
                    swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                });
        }
    });
}

function error(error) {
    swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
}