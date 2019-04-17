let patientTable = $("#patientTable").DataTable({
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
    scrollX: true,
    ajax: {
        url: "/api/patient/search",
        type: "POST",
        data: function (d) {
            //d.civilState $("#CivilState").val();
            //d.Sex = $("#Sex").val();
            //d.cancerTypes = $("#CancerTypes").val();
            //d.medicines = $("#Medicines").val();
            //d.doctors = $("#Doctors").val();
            //d.treatmentPlaces = $("#TreatmentPlaces").val();
            //d.familiarityGroup = $("#FamiliarityGroup").val();
            //d.death = $("#Death").is(":checked");
            //d.discharge = $("#Discharge").is(":checked");
        },
        datatype: "json",
        error: function () {
            swalWithBootstrapButtons.fire("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
        }
    },
    order: [1, "asc"],
    columns: [
        { data: "actions", title: "Ações", name: "actions", width: "20px", orderable: false },
        { data: "status", title: "Status", name: "status" },
        { data: "firstName", title: "Nome", name: "firstName" },
        { data: "lastName", title: "Sobrenome", name: "lastName" },
        { data: "rg", title: "RG", name: "rg" },
        { data: "cpf", title: "CPF", name: "cpf" },
        { data: "dateOfBirth", title: "DateOfBirth", name: "DateOfBirth" },
        { data: "sex", title: "Sex", name: "Sex" },
        { data: "civilState", title: "CivilState", name: "CivilState" },
        { data: "familiarityGroup", title: "FamiliarityGroup", name: "FamiliarityGroup" },
        { data: "profession", title: "Profession", name: "Profession" },
        { data: "familyIncome", title: "FamilyIncome", name: "FamilyIncome" },
        { data: "perCapitaIncome", title: "PerCapitaIncome", name: "PerCapitaIncome" },
        { data: "medicines", title: "Medicines", name: "Medicines", orderable: false },
        { data: "canceres", title: "Canceres", name: "Canceres", orderable: false },
        { data: "doctors", title: "Doctors", name: "Doctors", orderable: false },
        { data: "treatmentPlaces", title: "TreatmentPlaces", name: "TreatmentPlaces", orderable: false }
    ],
    preDrawCallback: function (settings) {
        showSpinner();
    },
    drawCallback: function (settings) {
        $(".editPatientButton").click(function () {
            $(".modal-dialog").addClass("modal-lg");
            openModal($(this).attr("href"), $(this).data("title"), initEditForm);
        });

        $(".archivePatientButton").click(function (e) {
            openModal($(this).attr("href"), $(this).data("title"), initArchivePatient);
        });

        $(".deletePatientButton").click(function (e) {
            initDelete($(this).data("url"), $(this).data("id"));
        });

        $(".activePatientButton").click(function (e) {
            initActivePatient($(this).data("url"), $(this).data("id"));
        });


        hideSpinner();
    }
});

$(function () {
    initPage();
});

function initPage() {
    $('#patientTable').attr('style', 'border-collapse: collapse !important');

    $(".filterSelect").select2({
        theme: "bootstrap",
        language: languageSelect2
    });

    $("#searchForm").submit(function (e) {
        e.preventDefault();
        patientTable.search("").draw("");
    });

    $("#addPatientButton").click(function () {
        $(".modal-dialog").addClass("modal-lg");
        openModal($(this).attr("href"), $(this).data("title"), initAddProfileForm);
    });
}

//Add Functions
function initAddProfileForm() {
    calendar("DateOfBirth");
    $.validator.unobtrusive.parse("#addPatientProfileForm");
}

function addProfileSuccess(data, textStatus) {
    if (data.ok) {
        patientTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire({
            title: 'Sucesso',
            text: "Paciente registrado com sucesso.",
            type: 'success'
        }).then((result) => {
            cleanModal();
            openModal(data.url, data.title, initAddNaturalityForm);
        });
    }
    else {
        $("#modalBody").html(data);
        initAddProfileForm();
    }
}

function initAddNaturalityForm() {
    $.validator.unobtrusive.parse("#addPatientNaturalityForm");
}

function addNaturalitySuccess(data, textStatus) {
    if (data.ok) {
        patientTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire({
            title: 'Sucesso',
            text: "naturalidade adicionada com sucesso.",
            type: 'success'
        }).then((result) => {
            openModal(data.url, data.title, initAddPatientInformationForm);
        });
    }
    else {
        $("#modalBody").html(data);
        initAddNaturalityForm();
    }
}

function initAddPatientInformationForm() {
    $.validator.unobtrusive.parse("#addPatientInformationForm");
    $(".patientInformationSelect").select2({
        theme: "bootstrap",
        language: languageSelect2
    });
}

function addPatientInformationSuccess(data, textStatus) {
    if (data.ok) {
        patientTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire({
            title: 'Sucesso',
            text: "Informação do paciente adicionada com sucesso.",
            type: 'success'
        }).then((result) => {
        });
    }
    else {
        $("#modalBody").html(data);
        initAddPatientInformationForm();
    }
}

//Edit functions
function initEditForm() {
    $.validator.unobtrusive.parse("#editPatientForm");

    $("#editPatientForm").submit(function () {
        if ($("#editPatientForm").valid()) {
            console.log("asdadsa");
            let nextTab = $('.nav-tabs > .nav-item > .active').parent().next('li').find('a').data("tab");
            $('#patientTabs a[href="#' + nextTab + '"]').tab('show');
            $(".btnPrevious").show();

            if (nextTab === "address") {
                $("#footerSubmit").show();
                $("#footerPreviousNext").hide();
            }
        }
        return false;
    });
}

function editSuccess(data, textStatus) {
    if (data === "" && textStatus === "success") {
        $("#modal-action").modal("hide");
        patientTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire("Sucesso", "Paciente atualizado com sucesso.", "success");
    }
    else {
        $("#modalBody").html(data);
        initEditForm();
    }
}

function initArchivePatient() {
    calendar("DateTime");
    $.validator.unobtrusive.parse("#archivePatientForm");
}

function archivePatientSuccess(data, textStatus) {
    if (data === "") {
        patientTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire({
            title: 'Sucesso',
            text: "O paciente foi arquivado com sucesso.",
            type: 'success'
        }).then((result) => {
            $("#modal-action").modal("hide");
        });
    }
    else {
        $("#modalBody").html(data);
        initArchivePatient();
    }
}

function initActivePatient(url, id) {
    swalWithBootstrapButtons({
        title: 'Você têm certeza?',
        text: "Você quer reativar este paciente?",
        type: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sim',
        cancelButtonText: 'Não',
        showLoaderOnConfirm: true,
        reverseButtons: true,
        preConfirm: () => {
            $.post(url, { id: id })
                .done(function (data, textStatus) {
                    patientTable.ajax.reload(null, false);
                    swalWithBootstrapButtons.fire("Ativo!", "O paciente foi reativado com sucesso.", "success");
                }).fail(function (error) {
                    swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                });
        }
    });
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
                    patientTable.ajax.reload(null, false);
                    swalWithBootstrapButtons.fire("Removido!", "O paciente foi removido com sucesso.", "success");
                }).fail(function (error) {
                    swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                });
        }
    });
}

function error(error) {
    swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
}
