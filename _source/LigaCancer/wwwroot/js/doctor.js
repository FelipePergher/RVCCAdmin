let doctorTable = $("#doctorTable").DataTable({
    dom: "l<'export-buttons'B>frtip",
    buttons: [
        {
            extend: 'pdf',
            orientation: 'landscape',
            pageSize: 'LEGAL',
            exportOptions: {
                columns: 'th:not(:first-child)',
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
        { data: "name", title: "Nome", name: "name", },
        { data: "crm", title: "CRM", name: "crm" }
    ],
    preDrawCallback: function (settings) {
        showSpinner();
    },
    drawCallback: function (settings) {
        $(".editDoctorButton").click(function () {
            openModal($(this).attr("href"), initEditForm);
        });

        $(".deleteDoctorButton").click(function (e) {
            initDelete($(this).data("url"), $(this).data("relation") === "True");
        });

        hideSpinner();
    }
});

$(function () {
    initPage();
});

function initPage() {
    $('#doctorTable').attr('style', 'border-collapse: collapse !important');

    $("#searchForm").submit(function (e) {
        e.preventDefault();
        doctorTable.search("").draw("");
    });

    $("#addDoctorButton").click(function () {
        openModal($(this).attr("href"), initAddForm);
    });

    $("#modal-action").on("hidden.bs.modal", function (e) {
        $("#modal-content").html("");
    });
}

function initAddForm() {
    $.validator.unobtrusive.parse("#addDoctorForm");
}

function AddSuccess(data, textStatus) {
    if (data === "" && textStatus === "success") {
        $("#modal-action").modal("hide");
        doctorTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire("Sucesso", "Médico registrado com sucesso.", "success");
    }
    else {
        $("#modal-content").html(data);
        initAddForm();
    }
}

function initEditForm() {
    $.validator.unobtrusive.parse("#editDoctorForm");
}

function EditSuccess(data, textStatus) {
    if (data === "" && textStatus === "success") {
        $("#modal-action").modal("hide");
        doctorTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire("Sucesso", "Médico atualizado com sucesso.", "success");
    }
    else {
        $("#modal-content").html(data);
        initEditForm();
    }
}

function initDelete(url, relation) {
    let message = "Você não poderá reverter isso!";
    if (relation) message = "Este médico está atribuido a pacientes, deseja prosseguir mesmo assim?";

    swalWithBootstrapButtons.queue([{
        title: 'Você tem certeza?',
        text: message,
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
                        doctorTable.ajax.reload(null, false);
                        return swalWithBootstrapButtons.fire("Removido", "O médico foi removido com sucesso.", "success");
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