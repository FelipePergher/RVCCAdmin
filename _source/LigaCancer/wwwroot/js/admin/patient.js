let patientTable;
let phoneTable;
let addressTable;
let familyMemberTable;
let attachmentsTable;

let dropzoneConfiguration = {
    maxFilesize: 20,
    acceptedFiles: "image/*,application/pdf",
    dictDefaultMessage: "Arraste seus arquivos aqui para upload.",
    dictFallbackMessage: "Seu navegador não tem suporte para arrastar e upload.",
    dictFileTooBig: "O arquivo é muito grande ({{filesize}}MB). Tamanho máximo: {{maxFilesize}}MB.",
    dictInvalidFileType: "Você não pode fazer upload de arquivos deste tipo.",
    dictResponseError: "Servidor respondeu com status de {{statusCode}}.",
    dictCancelUpload: "Cancelar upload",
    dictCancelUploadConfirmation: "Você têm certeza que quer cancelar este upload?",
    dictRemoveFile: "Remover arquivo",
    dictMaxFilesExceeded: "Você não pode fazer upload de mais arquivos."
};

$(function () {
    initPage();
});

function initPage() {
    $("#Cpf").mask(masks.Cpf);

    patientTable = $("#patientTable").DataTable({
        //dom: "l<'export-buttons'B>frtip",
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
                d.name = $("#Name").val();
                d.surname = $("#Surname").val();
                d.rg = $("#Rg").val();
                d.cpf = $("#Cpf").val();
                d.civilState = $("#CivilState").val();
                d.Sex = $("#Sex").val();
                d.cancerTypes = $("#CancerTypes").val();
                d.medicines = $("#Medicines").val();
                d.doctors = $("#Doctors").val();
                d.treatmentPlaces = $("#TreatmentPlaces").val();
                d.familiarityGroup = $("#FamiliarityGroup").val();
                d.death = $("#Death").is(":checked");
                d.discharge = $("#Discharge").is(":checked");
            },
            datatype: "json",
            error: function () {
                swalWithBootstrapButtons.fire("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
            }
        },
        order: [1, "asc"],
        columns: [
            { data: "actions", title: "Ações", name: "Actions", width: "20px", orderable: false },
            { data: "firstName", title: "Nome", name: "FirstName" },
            { data: "lastName", title: "Sobrenome", name: "LastName" },
            { data: "rg", title: "RG", name: "RG" },
            { data: "cpf", title: "CPF", name: "CPF" },
            { data: "dateOfBirth", title: "Data de nascimento", name: "DateOfBirth", render: function (data, type, row, meta) { return dateFormat(row.dateOfBirth); } },
            { data: "sex", title: "Gênero", name: "Sex" },
            { data: "phone", title: "Telefone", name: "Phone" },
            { data: "address", title: "Endereço", name: "Address" },
            { data: "civilState", title: "Estado Civil", name: "CivilState" },
            { data: "familiarityGroup", title: "Grupo de Convivência", name: "FamiliarityGroup" },
            { data: "profession", title: "Profissão", name: "Profession" },
            { data: "perCapitaIncome", title: "Renda Per Capita", name: "PerCapitaIncome" },
            { data: "medicines", title: "Remédios", name: "Medicines", orderable: false },
            { data: "canceres", title: "Cânceres", name: "Canceres", orderable: false },
            { data: "doctors", title: "Médicos", name: "Doctors", orderable: false },
            { data: "treatmentPlaces", title: "Locais de Tratamento", name: "TreatmentPlaces", orderable: false },
            { data: "status", title: "Status", name: "status", orderable: false }
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

            $(".fileUploadPatientButton").click(function () {
                $("#modal-dialog").addClass("modal-lg");
                openModal($(this).attr("href"), $(this).data("title"), initFileUpload);
            });

            //Fix dropdown with scrool
            $('.dropdown').on('shown.bs.dropdown', function () {
                let $menu = $(this).children(".dropdown-menu");
                offset = $menu.offset();
                $('body').append($menu);
                $menu.show().css('position', 'absolute').css('top', offset.top + 'px').css('left', offset.left + 'px');
                $(this).data("myDropdownMenu", $menu);
            });
            $('.dropdown').on('hide.bs.dropdown', function () {
                $(this).append($(this).data("myDropdownMenu"));
                $(this).data("myDropdownMenu").removeAttr('style');
            });

        }
    });
    $('#patientTable').attr('style', 'border-collapse: collapse !important');

    $(".filterSelect").select2();

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
    $("#CPF").mask(masks.Cpf);
    $("#MonthlyIncome").mask(masks.Price, { reverse: true });

    calendar("DateOfBirth");
    $(".patientProfileSelect2").select2();
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
            patientTable.ajax.reload(null, false);
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
    $(".patientInformationSelect").select2();
}

function addPatientInformationSuccess(data, textStatus) {
    if (data.ok) {
        patientTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire({
            title: 'Sucesso',
            text: "Informação do paciente adicionada com sucesso.",
            type: 'success'
        }).then((result) => {
            $("#modal-action").modal("hide");
        });
    }
    else {
        $("#modalBody").html(data);
        initAddPatientInformationForm();
    }
}

//Edit Functions
function initEditProfileForm() {
    $("#CPF").mask(masks.Cpf);
    $("#MonthlyIncome").mask(masks.Price, { reverse: true });

    calendar("DateOfBirth");
    $(".patientProfileSelect2").select2();
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
    $(".patientInformationSelect").select2();
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
        //dom: "l<'export-buttons'B>frtip",
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
        autoWidth: false,
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
    $('#Number').mask(SPMaskBehavior, spOptions);
    $(".phoneSelect2").select2();
    $.validator.unobtrusive.parse("#addPhoneForm");
}

function addPhoneSuccess(data, textStatus) {
    if (!data && textStatus === "success") {
        $("#modal-action-secondary").modal("hide");
        phoneTable.ajax.reload(null, false);
        patientTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire("Sucesso", "Telefone adicionado com sucesso.", "success");
    }
    else {
        $("#modalBodySecondary").html(data);
        initAddPhoneForm();
    }
}

function initEditPhoneForm() {
    $('#Number').mask(SPMaskBehavior, spOptions);
    $(".phoneSelect2").select2();
    $.validator.unobtrusive.parse("#editPhoneForm");
}

function editPhoneSuccess(data, textStatus) {
    if (!data && textStatus === "success") {
        $("#modal-action-secondary").modal("hide");
        phoneTable.ajax.reload(null, false);
        patientTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire("Sucesso", "Telefone atualizado com sucesso.", "success");
    }
    else {
        $('.phone').mask('(00) 9999-9999?9');
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
                    patientTable.ajax.reload(null, false);
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
        //dom: "l<'export-buttons'B>frtip",
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
        autoWidth: false,
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
    $(".addressSelect2").select2();

    $.validator.unobtrusive.parse("#addAddressForm");

    $("#ResidenceType").change(function (e) {
        !!$(this).val() ? $("#monthlyResidence").show() : $("#monthlyResidence").hide();
    });
}

function addAddressSuccess(data, textStatus) {
    if (!data && textStatus === "success") {
        $("#modal-action-secondary").modal("hide");
        addressTable.ajax.reload(null, false);
        patientTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire("Sucesso", "Endereço adicionado com sucesso.", "success");
    }
    else {
        $("#modalBodySecondary").html(data);
        initAddAddressForm();
    }
}

function initEditAddressForm() {
    $(".addressSelect2").select2();

    $.validator.unobtrusive.parse("#editAddressForm");

    $("#ResidenceType").change(function (e) {
        !!$(this).val() ? $("#monthlyResidence").show() : $("#monthlyResidence").hide();
    });
}

function editAddressSuccess(data, textStatus) {
    if (!data && textStatus === "success") {
        $("#modal-action-secondary").modal("hide");
        addressTable.ajax.reload(null, false);
        patientTable.ajax.reload(null, false);
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
                    patientTable.ajax.reload(null, false);
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
        //dom: "l<'export-buttons'B>frtip",
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
        autoWidth: false,
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
            { data: "dateOfBirth", title: "Data de Nascimento", name: "DateOfBirth", render: function (data, type, row, meta) { return dateFormat(row.dateOfBirth); } },
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
    $("#MonthlyIncome").mask(masks.Price, { reverse: true });
    $(".familyMemberSelect2").select2();
    calendar("dateOfBirth");
    $.validator.unobtrusive.parse("#addFamilyMemberForm");
}

function addFamilyMemberSuccess(data, textStatus) {
    if (!data && textStatus === "success") {
        $("#modal-action-secondary").modal("hide");
        familyMemberTable.ajax.reload(null, false);
        patientTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire("Sucesso", "Endereço adicionado com sucesso.", "success");
    }
    else {
        $("#modalBodySecondary").html(data);
        initAddFamilyMemberForm();
    }
}

function initEditFamilyMemberForm() {
    $("#MonthlyIncome").mask(masks.Price, { reverse: true });
    $(".familyMemberSelect2").select2();
    calendar("dateOfBirth");
    $.validator.unobtrusive.parse("#editFamilyMemberForm");
}

function editFamilyMemberSuccess(data, textStatus) {
    if (!data && textStatus === "success") {
        $("#modal-action-secondary").modal("hide");
        familyMemberTable.ajax.reload(null, false);
        patientTable.ajax.reload(null, false);
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

//Files
function initFileUpload() {
    var myDropzone = new Dropzone("#dropzoneForm", dropzoneConfiguration);

    attachmentsTable = $("#attachmentsTable").DataTable({
        //dom: "l<'export-buttons'B>frtip",
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
        language: language,
        filter: false,
        autoWidth: false,
        ajax: {
            url: "/api/FileAttachment/search",
            type: "POST",
            data: function (d) {
                d.patientId = $("#PatientId").val();
            },
            datatype: "json",
            error: function () {
                swalWithBootstrapButtons.fire("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
            }
        },
        order: [1, "asc"],
        columns: [
            { data: "actions", title: "Ações", name: "actions", width: "20px", orderable: false },
            {
                data: "size", title: "Tamanho", name: "Size",
                render: function (data, type, row, meta) {
                    return row.size + " Mb";
                }
            },
            {
                data: "name", title: "Arquivo", name: "Name",
                render: function (data, type, row, meta) {
                    let html =
                        '   <span class="editable" data-fileAttachmentId="' + row.fileAttachmentId + '">' + row.name + '</span>' +
                        '   <a class="float-right fa fa-download mt-1 ml-2" class="fa fa-download" href=" / ' + row.filePath + '" download="' + row.name + row.extension + '"></a>';
                    return html;
                }}
        ],
        drawCallback: function (settings) {
            $(".deleteFileAttachmentButton").click(function (e) {
                initDeleteFileAttachment($(this).data("url"), $(this).data("id"));
            });

            $('.editable').editable(function (value, settings) {
                let fileAttachmentId = $(this).data("fileattachmentid");

                $.post("/Admin/FileAttachment/UpdateNameFile", { fileAttachmentId: fileAttachmentId, name: value })
                    .done(function (data, textStatus) {
                        attachmentsTable.ajax.reload(null, false);
                    }).fail(function (error) {
                        swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                    });
            }, {
                    indicator: 'salvando…',
                    cssclass: "form-row",
                    submit: 'Salvar',
                    submitcssclass: 'btn btn-primary ml-2',
                    inputcssclass: "form-control",
                    placeholder: "Clique para editar",
                    tooltip: "Clique para editar"
                });
        }
    });
    $('#attachmentsTable').attr('style', 'border-collapse: collapse !important');

    myDropzone.on("success", function (file) {
        attachmentsTable.ajax.reload(null, false);
    });
}

function initDeleteFileAttachment(url, id) {
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
                    attachmentsTable.ajax.reload(null, false);
                    swalWithBootstrapButtons.fire("Removido!", "O arquivo foi removido com sucesso.", "success");
                }).fail(function (error) {
                    swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                });
        }
    });
}

//Control Enable/Disable Functions
function initArchivePatient() {
    calendar("DateTime");
    $(".archiveSelect2").select2();
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
