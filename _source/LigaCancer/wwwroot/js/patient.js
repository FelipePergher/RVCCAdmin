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

        //$(".disablePatientButton").click(function (e) {
        //    initDelete($(this).data("url"), $(this).data("id"));
        //});

         //$(".deletePatientButton").click(function (e) {
        //    initDelete($(this).data("url"), $(this).data("id"));
        //});

        hideSpinner();
    }
});

$(function () {
    initPage();
});

function initPage() {
    $('#patientTable').attr('style', 'border-collapse: collapse !important');

    $("#searchForm").submit(function (e) {
        e.preventDefault();
        patientTable.search("").draw("");
    });

    $("#addPatientButton").click(function () {
        $(".modal-dialog").addClass("modal-lg");
        openModal($(this).attr("href"), $(this).data("title"), initAddProfileForm);
    });
}

function initAddProfileForm() {
    calendar("DateOfBirth");
    $.validator.unobtrusive.parse("#addPatientProfileForm");
}

function AddProfileSuccess(data, textStatus) {
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

function AddNaturalitySuccess(data, textStatus) {
    if (data.ok) {
        patientTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire({
            title: 'Sucesso',
            text: "naturalidade adicionada com sucesso.",
            type: 'success'
        }).then((result) => {
            openModal(data.url, data.title, initAddNaturalityForm);
        });
    }
    else {
        $("#modalBody").html(data);
        initAddNaturalityForm();
    }
}

function initAddPatientInformationForm() {
    $.validator.unobtrusive.parse("#addPatientInformationForm");
    $(".customSelect2").select2({
        theme: "bootstrap",
        language: languageSelect2
    });
}

function AddPatientInformationSuccess(data, textStatus) {
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

function initEditForm() {
    $.validator.unobtrusive.parse("#editPatientForm");

    //Tabs control
    //$('#patientTabs a[href="#profile"]').tab('show'); // Select tab by name
    //$('#patientTabs li:first-child a').tab('show'); // Select first tab
    //$('#patientTabs li:last-child a').tab('show'); // Select last tab
    //$('#patientTabs li:nth-child(3) a').tab('show'); // Select third tab
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

    $(".btnPrevious").click(function () {
        $("#footerSubmit").hide();
        $("#footerPreviousNext").show();

        let previousTab = $('.nav-tabs > .nav-item > .active').parent().prev('li').find('a').data("tab");
        $('#patientTabs a[href="#' + previousTab + '"]').tab('show');

        if (previousTab === "profile") $(".btnPrevious").hide();
        //Todo valid the for in each page before change
    });
}

function EditSuccess(data, textStatus) {
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

function Error(error) {
    swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
}

//$(function () {
    //BuildSelect2("CancerTypes", "Cânceres");
    //BuildSelect2("Medicines", "Remédios");
    //BuildSelect2("Doctors", "Médicos");
    //BuildSelect2("TreatmentPlaces", "Locais de Tratamento");
    //BuildSelect2("CivilState", "Estado Civil", true);
    //BuildSelect2("Sex", "Gênero", true);
    //BuildSelect2("FamiliarityGroup", "Grupo de convivência", true);
//});

//function BuildSelect2(elementId, placeholder, allowClear = false) {
//    $("#" + elementId).select2({
//        theme: "bootstrap",
//        placeholder: placeholder,
//        allowClear: allowClear,
//        language: languageSelect2
//    }).on("change", function (e) {
//        dataTable.draw();
//    });
//}
