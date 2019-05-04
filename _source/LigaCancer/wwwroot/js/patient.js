﻿let patientTable;
let phoneTable;
let addressTable;
let familyMemberTable;

$(function () {
    initPage();
});

function initPage() {
    patientTable = $("#patientTable").DataTable({
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
        scrollY: '50vh',
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
            { data: "canceres", title: "Cânceres", name: "Canceres", orderable: false },
            { data: "doctors", title: "Doctors", name: "Doctors", orderable: false },
            { data: "treatmentPlaces", title: "TreatmentPlaces", name: "TreatmentPlaces", orderable: false }
        ],
        drawCallback: function (settings) {
            $(".editPatientButton").click(function () {
                $("#modal-dialog").addClass("modal-lg");
                openModal($(this).attr("href"), $(this).data("title"), initEditProfileForm);
            });

            $(".editNaturalityButton").click(function () {
                openModal($(this).attr("href"), $(this).data("title"), initEditNaturalityForm);
            });

            $(".editPatientInformationButton").click(function () {
                openModal($(this).attr("href"), $(this).data("title"), initEditPatientInformationForm);
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

            $(".phonesButton").click(function () {
                $("#modal-dialog").addClass("modal-lg");
                openModal($(this).attr("href"), $(this).data("title"), initPhoneIndex);
            });

            $(".addressesButton").click(function () {
                $("#modal-dialog").addClass("modal-elg");
                openModal($(this).attr("href"), $(this).data("title"), initAddressIndex);
            });

            $(".familyMembersButton").click(function () {
                $("#modal-dialog").addClass("modal-elg");
                openModal($(this).attr("href"), $(this).data("title"), initFamilyMemberIndex);
            });
        }
    });
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
        $("#modal-dialog").addClass("modal-lg");
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
            text: "Naturalidade adicionada com sucesso.",
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
            //Todo call Contact/address form
            $("#modal-action").modal("hide");
            patientTable.ajax.reload(null, false);
        });
    }
    else {
        $("#modalBody").html(data);
        initAddPatientInformationForm();
    }
}

//Edit Functions
function initEditProfileForm() {
    calendar("DateOfBirth");
    $.validator.unobtrusive.parse("#editPatientProfileForm");
}

function editProfileSuccess(data, textStatus) {
    if (!data && textStatus === "success") {
        $("#modal-action").modal("hide");
        patientTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire("Sucesso", "Paciente atualizado com sucesso.", "success");
    }
    else {
        $("#modalBody").html(data);
        initEditProfileForm();
    }
}

function initEditNaturalityForm() {
    $.validator.unobtrusive.parse("#editPatientNaturalityForm");
}

function editNaturalitySuccess(data, textStatus) {
    if (!data && textStatus === "success") {
        $("#modal-action").modal("hide");
        patientTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire("Sucesso", "Naturalidade atualizada com sucesso.", "success");
    }
    else {
        $("#modalBody").html(data);
        initEditNaturalityForm();
    }
}

function initEditPatientInformationForm() {
    $.validator.unobtrusive.parse("#editPatientInformationForm");
    $(".patientInformationSelect").select2({
        theme: "bootstrap",
        language: languageSelect2
    });
}

function editPatientInformationSuccess(data, textStatus) {
    if (!data && textStatus === "success") {
        $("#modal-action").modal("hide");
        patientTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire("Sucesso", "Informação do paciente atualizada com sucesso.", "success");
    }
    else {
        $("#modalBody").html(data);
        initEditPatientInformationForm();
    }
}

//Phone Functions
function initPhoneIndex() {
    phoneTable = $("#phoneTable").DataTable({
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
            url: "/api/phone/search",
            type: "POST",
            data: function (d) {
                d.patientId = $("#phonePatientId").val();
            },
            datatype: "json",
            error: function () {
                swalWithBootstrapButtons.fire("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
            }
        },
        order: [1, "asc"],
        columns: [
            { data: "actions", title: "Ações", name: "actions", width: "20px", orderable: false },
            { data: "number", title: "Número", name: "Number" },
            { data: "phoneType", title: "Tipo", name: "PhoneType" },
            { data: "observationNote", title: "Observação", name: "ObservationNote" }
        ],
        drawCallback: function (settings) {
            $(".editPhoneButton").click(function () {
                openModalSecondary($(this).attr("href"), $(this).data("title"), initEditPhoneForm);
            });
            $(".deletePhoneButton").click(function (e) {
                initDeletePhone($(this).data("url"), $(this).data("id"));
            });
        }
    });
    $('#phoneTable').attr('style', 'border-collapse: collapse !important');

    $("#addPhoneButton").click(function () {
        openModalSecondary($(this).attr("href"), $(this).data("title"), initAddPhoneForm);
    });
}

function initAddPhoneForm() {
    $.validator.unobtrusive.parse("#addPhoneForm");
}

function addPhoneSuccess(data, textStatus) {
    if (!data && textStatus === "success") {
        $("#modal-action-secondary").modal("hide");
        phoneTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire("Sucesso", "Telefone adicionado com sucesso.", "success");
    }
    else {
        $("#modalBodySecondary").html(data);
        initAddPhoneForm();
    }
}

function initEditPhoneForm() {
    $.validator.unobtrusive.parse("#editPhoneForm");
}

function editPhoneSuccess(data, textStatus) {
    if (!data && textStatus === "success") {
        $("#modal-action-secondary").modal("hide");
        phoneTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire("Sucesso", "Telefone atualizado com sucesso.", "success");
    }
    else {
        $("#modalBodySecondary").html(data);
        initEditPhoneForm();
    }
}

function initDeletePhone(url, id) {
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
                    phoneTable.ajax.reload(null, false);
                    swalWithBootstrapButtons.fire("Removido!", "O telefone foi removido com sucesso.", "success");
                }).fail(function (error) {
                    swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                });
        }
    });
}

//Address Functions
function initAddressIndex() {
    addressTable = $("#addressTable").DataTable({
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
            url: "/api/address/search",
            type: "POST",
            data: function (d) {
                d.patientId = $("#addressPatientId").val();
            },
            datatype: "json",
            error: function () {
                swalWithBootstrapButtons.fire("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
            }
        },
        order: [1, "asc"],
        columns: [
            { data: "actions", title: "Ações", name: "actions", width: "20px", orderable: false },
            { data: "street", title: "Rua", name: "Street" },
            { data: "neighborhood", title: "Bairro", name: "Neighborhood" },
            { data: "city", title: "Cidade", name: "City" },
            { data: "houseNumber", title: "Nº", name: "HouseNumber" },
            { data: "complement", title: "Complemento", name: "Complement" },
            { data: "residenceType", title: "Residência", name: "ResidenceType" },
            { data: "monthlyAmmountResidence", title: "Valor Mensal", name: "MonthlyAmmountResidence" },
            { data: "observationAddress", title: "Observação", name: "ObservationAddress" }
        ],
        drawCallback: function (settings) {
            $(".editAddressButton").click(function () {
                openModalSecondary($(this).attr("href"), $(this).data("title"), initEditAddressForm);
            });
            $(".deleteAddressButton").click(function (e) {
                initDeleteAddress($(this).data("url"), $(this).data("id"));
            });
        }
    });
    $('#addressTable').attr('style', 'border-collapse: collapse !important');

    $("#addAddressButton").click(function () {
        openModalSecondary($(this).attr("href"), $(this).data("title"), initAddAddressForm);
    });
}

function initAddAddressForm() {
    $.validator.unobtrusive.parse("#addAddressForm");

    $("#ResidenceType").change(function (e) {
        !!$(this).val() ? $("#monthlyResidence").show() : $("#monthlyResidence").hide();
    });
}

function addAddressSuccess(data, textStatus) {
    if (!data && textStatus === "success") {
        $("#modal-action-secondary").modal("hide");
        addressTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire("Sucesso", "Endereço adicionado com sucesso.", "success");
    }
    else {
        $("#modalBodySecondary").html(data);
        initAddAddressForm();
    }
}

function initEditAddressForm() {
    $.validator.unobtrusive.parse("#editAddressForm");

    $("#ResidenceType").change(function (e) {
        !!$(this).val() ? $("#monthlyResidence").show() : $("#monthlyResidence").hide();
    });
}

function editAddressSuccess(data, textStatus) {
    if (!data && textStatus === "success") {
        $("#modal-action-secondary").modal("hide");
        addressTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire("Sucesso", "Endereço atualizado com sucesso.", "success");
    }
    else {
        $("#modalBodySecondary").html(data);
        initEditAddressForm();
    }
}

function initDeleteAddress(url, id) {
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
                    addressTable.ajax.reload(null, false);
                    swalWithBootstrapButtons.fire("Removido!", "O endereço foi removido com sucesso.", "success");
                }).fail(function (error) {
                    swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                });
        }
    });
}

//Family Member Functions
function initFamilyMemberIndex() {
    familyMemberTable = $("#familyMemberTable").DataTable({
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
            url: "/api/familyMember/search",
            type: "POST",
            data: function (d) {
                d.patientId = $("#familyMemberPatientId").val();
            },
            dataSrc: function (data) {
                $("#familyIncome").text(data.familyIncome);
                $("#perCapitaIncome").text(data.perCapitaIncome);
                $("#monthlyIncome").text(data.monthlyIncome);
                return data.data;
            },
            datatype: "json",
            error: function () {
                swalWithBootstrapButtons.fire("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
            }
        },
        order: [1, "asc"],
        columns: [
            { data: "actions", title: "Ações", name: "actions", width: "20px", orderable: false },
            { data: "name", title: "Nome", name: "Name" },
            { data: "kinship", title: "Parentesco", name: "Kinship" },
            { data: "age", title: "Idade", name: "Age" },
            { data: "sex", title: "Gênero", name: "Sex" },
            { data: "monthlyIncome", title: "Renda", name: "MonthlyIncome" }
        ],
        drawCallback: function (settings) {
            $(".editFamilyMemberButton").click(function () {
                openModalSecondary($(this).attr("href"), $(this).data("title"), initEditFamilyMemberForm);
            });
            $(".deleteFamilyMemberButton").click(function (e) {
                initDeleteFamilyMember($(this).data("url"), $(this).data("id"));
            });
        }
    });
    $('#familyMemberTable').attr('style', 'border-collapse: collapse !important');

    $("#addFamilyMemberButton").click(function () {
        openModalSecondary($(this).attr("href"), $(this).data("title"), initAddFamilyMemberForm);
    });
}

function initAddFamilyMemberForm() {
    $.validator.unobtrusive.parse("#addFamilyMemberForm");
}

function addFamilyMemberSuccess(data, textStatus) {
    if (!data && textStatus === "success") {
        $("#modal-action-secondary").modal("hide");
        familyMemberTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire("Sucesso", "Endereço adicionado com sucesso.", "success");
    }
    else {
        $("#modalBodySecondary").html(data);
        initAddFamilyMemberForm();
    }
}

function initEditFamilyMemberForm() {
    $.validator.unobtrusive.parse("#editFamilyMemberForm");
}

function editFamilyMemberSuccess(data, textStatus) {
    if (!data && textStatus === "success") {
        $("#modal-action-secondary").modal("hide");
        familyMemberTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire("Sucesso", "Endereço atualizado com sucesso.", "success");
    }
    else {
        $("#modalBodySecondary").html(data);
        initEditFamilyMemberForm();
    }
}

function initDeleteFamilyMember(url, id) {
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
                    familyMemberTable.ajax.reload(null, false);
                    swalWithBootstrapButtons.fire("Removido!", "O endereço foi removido com sucesso.", "success");
                }).fail(function (error) {
                    swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                });
        }
    });
}

//Control Enable/Disable Functions

function initArchivePatient() {
    calendar("DateTime");
    $.validator.unobtrusive.parse("#archivePatientForm");
}

function archivePatientSuccess(data, textStatus) {
    if (!data && textStatus === "success") {
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
