"use strict";
import "jquery-validation";
import "jquery-validation-unobtrusive";
import "datatables.net";
import "datatables.net-bs4";
import "bootstrap/js/dist/modal";
import "select2";
import 'bootstrap-datepicker';
import 'bootstrap-datepicker/dist/locales/bootstrap-datepicker.pt-br.min';
import "jquery-mask-plugin";

export default (function () {

    global.setupValidator($.validator);

    $(function () {
        initPage();
    });

    function initPage() {
        $("#Cpf").mask(global.masks.Cpf);

        $('#BirthdayDateFrom, #BirthdayDateTo, #JoinDateFrom, #JoinDateTo').datepicker({
            clearBtn: true,
            format: "dd/mm/yyyy",
            language: "pt-BR",
            templates: {
                leftArrow: '<i class="fas fa-chevron-left"></i>',
                rightArrow: '<i class="fas fa-chevron-right"></i>'
            }
        });

        let patientTable = $("#patientTable").DataTable({
            scrollX: true,
            processing: true,
            serverSide: true,
            language: global.datatablesLanguage,
            filter: false,
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
                    d.BirthdayDateFrom = $("#BirthdayDateFrom").val();
                    d.BirthdayDateTo = $("#BirthdayDateTo").val();
                    d.joinDateFrom = $("#JoinDateFrom").val();
                    d.joinDateTo = $("#JoinDateTo").val();
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
                { data: "dateOfBirth", title: "Data de nascimento", name: "DateOfBirth" },
                { data: "joinDate", title: "Data de ingresso", name: "JoinDate" },
                { data: "sex", title: "Gênero", name: "Sex" },
                { data: "phone", title: "Telefone", name: "Phone" },
                { data: "address", title: "Endereço", name: "Address" },
                { data: "civilState", title: "Estado Civil", name: "CivilState" },
                { data: "familiarityGroup", title: "Grupo de Convivência", name: "FamiliarityGroup" },
                { data: "profession", title: "Profissão", name: "Profession" },
                { data: "perCapitaIncome", title: "Renda Per Capita", name: "PerCapitaIncome" },
                { data: "treatmentBeginDate", title: "Início Tratamento", name: "TreatmentBeginDate" },
                { data: "medicines", title: "Remédios", name: "Medicines", orderable: false },
                { data: "canceres", title: "Cânceres", name: "Canceres", orderable: false },
                { data: "doctors", title: "Médicos", name: "Doctors", orderable: false },
                { data: "treatmentPlaces", title: "Locais de Tratamento", name: "TreatmentPlaces", orderable: false },
                { data: "status", title: "Status", name: "status", orderable: false }
            ],
            drawCallback: function (settings) {
                $(".editDoctorButton").click(function () {
                    global.openModal($(this).attr("href"), $(this).data("title"), initEditForm);
                });

                $(".deleteDoctorButton").click(function (e) {
                    initDelete($(this).data("url"), $(this).data("id"), $(this).data("relation") === "True");
                });

                $(".editPatientButton").click(function () {
                    $("#modal-dialog").addClass("modal-lg");
                    global.openModal($(this).attr("href"), $(this).data("title"), initEditProfileForm);
                });

                $(".editNaturalityButton").click(function () {
                    global.openModal($(this).attr("href"), $(this).data("title"), initEditNaturalityForm);
                });

                $(".editPatientInformationButton").click(function () {
                    global.openModal($(this).attr("href"), $(this).data("title"), initEditPatientInformationForm);
                });

                //$(".archivePatientButton").click(function (e) {
                //    openModal($(this).attr("href"), $(this).data("title"), initArchivePatient);
                //});

                //$(".deletePatientButton").click(function (e) {
                //    initDelete($(this).data("url"), $(this).data("id"));
                //});

                //$(".activePatientButton").click(function (e) {
                //    initActivePatient($(this).data("url"), $(this).data("id"));
                //});

                //$(".phonesButton").click(function () {
                //    $("#modal-dialog").addClass("modal-lg");
                //    openModal($(this).attr("href"), $(this).data("title"), initPhoneIndex);
                //});

                //$(".addressesButton").click(function () {
                //    $("#modal-dialog").addClass("modal-elg");
                //    openModal($(this).attr("href"), $(this).data("title"), initAddressIndex);
                //});

                //$(".familyMembersButton").click(function () {
                //    $("#modal-dialog").addClass("modal-elg");
                //    openModal($(this).attr("href"), $(this).data("title"), initFamilyMemberIndex);
                //});

                //$(".fileUploadPatientButton").click(function () {
                //    $("#modal-dialog").addClass("modal-lg");
                //    openModal($(this).attr("href"), $(this).data("title"), initFileUpload);
                //});

                ////Fix dropdown with scrool
                //$('.dropdown').on('shown.bs.dropdown', function () {
                //    let $menu = $(this).children(".dropdown-menu");
                //    offset = $menu.offset();
                //    $('body').append($menu);
                //    $menu.show().css('position', 'absolute').css('top', offset.top + 'px').css('left', offset.left + 'px');
                //    $(this).data("myDropdownMenu", $menu);
                //});
                //$('.dropdown').on('hide.bs.dropdown', function () {
                //    $(this).append($(this).data("myDropdownMenu"));
                //    $(this).data("myDropdownMenu").removeAttr('style');
                //});
            }
        });

        $('#patientTable').attr('style', 'border-collapse: collapse !important');

        $(".filterSelect").select2();

        $('#searchForm').on('reset', function (e) {
            $(".filterSelect").val("").trigger("change");
        });

        $("#searchForm").submit(function (e) {
            e.preventDefault();
            patientTable.search("").draw("");
        });

        $("#addPatientButton").click(function () {
            $("#modal-dialog").addClass("modal-lg");
            global.openModal($(this).attr("href"), $(this).data("title"), initAddProfileForm);
        });
    }
    
    // Add Functions
    function initAddProfileForm() {
        $("#CPF").mask(global.masks.Cpf);
        $("#MonthlyIncome").mask(global.masks.Price, { reverse: true });

        $('#DateOfBirth, #JoinDate').datepicker({
            clearBtn: true,
            format: "dd/mm/yyyy",
            language: "pt-BR",
            templates: {
                leftArrow: '<i class="fas fa-chevron-left"></i>',
                rightArrow: '<i class="fas fa-chevron-right"></i>'
            }
        });

        $(".patientProfileSelect2").select2();
        $.validator.unobtrusive.parse("#addPatientProfileForm");

        $("#addPatientProfileForm").submit(function (e) {
            e.preventDefault();

            let form = $(this);
            if (form.valid()) {
                let submitButton = $(this).find("button[type='submit']");
                $(submitButton).prop("disabled", "disabled").addClass("disabled");
                $("#submitSpinner").show();

                $.post($(form).attr("action"), form.serialize())
                    .done(function (data, textStatus) {
                        if (data.ok) {
                            $("#patientTable").DataTable().ajax.reload(null, false);
                            global.swalWithBootstrapButtons.fire({
                                title: 'Sucesso',
                                text: "Paciente registrado com sucesso.",
                                type: 'success'
                            }).then((result) => {
                                global.cleanModal();
                                global.openModal(data.url, data.title, initAddNaturalityForm);
                            });
                        }
                        else {
                            $("#modalBody").html(data);
                            initAddProfileForm();
                        }
                    }).fail(function (error) {
                        global.swalWithBootstrapButtons.fire('Ops...', 'Alguma coisa deu errado!', 'error');
                    })
                    .always(function () {
                        $(submitButton).removeProp("disabled").removeClass("disabled");
                        $("#submitSpinner").hide();
                    });
            }
        });

    }

    function initAddNaturalityForm() {
        $.validator.unobtrusive.parse("#addPatientNaturalityForm");

        $("#addPatientNaturalityForm").submit(function (e) {
            e.preventDefault();

            let form = $(this);
            if (form.valid()) {
                let submitButton = $(this).find("button[type='submit']");
                $(submitButton).prop("disabled", "disabled").addClass("disabled");
                $("#submitSpinner").show();

                $.post($(form).attr("action"), form.serialize())
                    .done(function (data, textStatus) {
                        if (data.ok) {
                            $("#patientTable").DataTable().ajax.reload(null, false);
                            global.swalWithBootstrapButtons.fire({
                                title: 'Sucesso',
                                text: "Naturalidade registrado com sucesso.",
                                type: 'success'
                            }).then((result) => {
                                global.cleanModal();
                                global.openModal(data.url, data.title, initAddPatientInformationForm);
                            });
                        }
                        else {
                            $("#modalBody").html(data);
                            initAddNaturalityForm();
                        }
                    }).fail(function (error) {
                        global.swalWithBootstrapButtons.fire('Ops...', 'Alguma coisa deu errado!', 'error');
                    })
                    .always(function () {
                        $(submitButton).removeProp("disabled").removeClass("disabled");
                        $("#submitSpinner").hide();
                    });
            }
        });
    }

    function initAddPatientInformationForm() {
        $.validator.unobtrusive.parse("#addPatientInformationForm");
        $(".patientInformationSelect").select2();
        $('#TreatmentBeginDate').datepicker({
            clearBtn: true,
            format: "dd/mm/yyyy",
            language: "pt-BR",
            templates: {
                leftArrow: '<i class="fas fa-chevron-left"></i>',
                rightArrow: '<i class="fas fa-chevron-right"></i>'
            }
        });

        $("#addPatientInformationForm").submit(function (e) {
            e.preventDefault();

            let form = $(this);
            if (form.valid()) {
                let submitButton = $(this).find("button[type='submit']");
                $(submitButton).prop("disabled", "disabled").addClass("disabled");
                $("#submitSpinner").show();

                $.post($(form).attr("action"), form.serialize())
                    .done(function (data, textStatus) {
                        if (!data && textStatus === "success") {
                            $("#patientTable").DataTable().ajax.reload(null, false);
                            global.swalWithBootstrapButtons.fire({
                                title: 'Sucesso',
                                text: "Informação do paciente adicionada com sucesso.",
                                type: 'success'
                            }).then((result) => {
                                $("#modal-action").modal("hide");
                                $('.modal-backdrop').remove();
                            });
                        }
                        else {
                            $("#modalBody").html(data);
                            initAddPatientInformationForm();
                        }
                    }).fail(function (error) {
                        global.swalWithBootstrapButtons.fire('Ops...', 'Alguma coisa deu errado!', 'error');
                    })
                    .always(function () {
                        $(submitButton).removeProp("disabled").removeClass("disabled");
                        $("#submitSpinner").hide();
                    });
            }
        });
    }

    // Edit Functions
    function initEditProfileForm() {
        $.validator.unobtrusive.parse("#editPatientProfileForm");
        $("#CPF").mask(global.masks.Cpf);
        $("#MonthlyIncome").mask(global.masks.Price, { reverse: true });

        $('#DateOfBirth, #JoinDate').datepicker({
            clearBtn: true,
            format: "dd/mm/yyyy",
            language: "pt-BR",
            templates: {
                leftArrow: '<i class="fas fa-chevron-left"></i>',
                rightArrow: '<i class="fas fa-chevron-right"></i>'
            }
        });
        $(".patientProfileSelect2").select2();

        $("#editPatientProfileForm").submit(function (e) {
            e.preventDefault();

            let form = $(this);
            if (form.valid()) {
                let submitButton = $(this).find("button[type='submit']");
                $(submitButton).prop("disabled", "disabled").addClass("disabled");
                $("#submitSpinner").show();

                $.post($(form).attr("action"), form.serialize())
                    .done(function (data, textStatus) {
                        if (!data && textStatus === "success") {
                            $("#modal-action").modal("hide");
                            $('.modal-backdrop').remove();
                            $("#patientTable").DataTable().ajax.reload(null, false);
                            global.swalWithBootstrapButtons.fire("Sucesso", "Paciente atualizado com sucesso.", "success");
                            global.cleanModal();
                        } else {
                            $("#modalBody").html(data);
                            initEditProfileForm();
                        }
                    }).fail(function (error) {
                        global.swalWithBootstrapButtons.fire('Ops...', 'Alguma coisa deu errado!', 'error');
                    })
                    .always(function () {
                        $(submitButton).removeProp("disabled").removeClass("disabled");
                        $("#submitSpinner").hide();
                    });
            }
        });
    }

    function initEditNaturalityForm() {
        $.validator.unobtrusive.parse("#editPatientNaturalityForm");

        $("#editPatientNaturalityForm").submit(function (e) {
            e.preventDefault();

            let form = $(this);
            if (form.valid()) {
                let submitButton = $(this).find("button[type='submit']");
                $(submitButton).prop("disabled", "disabled").addClass("disabled");
                $("#submitSpinner").show();

                $.post($(form).attr("action"), form.serialize())
                    .done(function (data, textStatus) {
                        if (!data && textStatus === "success") {
                            $("#modal-action").modal("hide");
                            $('.modal-backdrop').remove();
                            $("#patientTable").DataTable().ajax.reload(null, false);
                            global.swalWithBootstrapButtons.fire("Sucesso", "Naturalidade atualizada com sucesso.", "success");
                            global.cleanModal();
                        } else {
                            $("#modalBody").html(data);
                            initEditNaturalityForm();
                        }
                    }).fail(function (error) {
                        global.swalWithBootstrapButtons.fire('Ops...', 'Alguma coisa deu errado!', 'error');
                    })
                    .always(function () {
                        $(submitButton).removeProp("disabled").removeClass("disabled");
                        $("#submitSpinner").hide();
                    });
            }
        });
    }

    function initEditPatientInformationForm() {
        $.validator.unobtrusive.parse("#editPatientInformationForm");

        $('#TreatmentBeginDate').datepicker({
            clearBtn: true,
            format: "dd/mm/yyyy",
            language: "pt-BR",
            templates: {
                leftArrow: '<i class="fas fa-chevron-left"></i>',
                rightArrow: '<i class="fas fa-chevron-right"></i>'
            }
        });
        $(".patientInformationSelect").select2();

        $("#editPatientInformationForm").submit(function (e) {
            e.preventDefault();

            let form = $(this);
            if (form.valid()) {
                let submitButton = $(this).find("button[type='submit']");
                $(submitButton).prop("disabled", "disabled").addClass("disabled");
                $("#submitSpinner").show();

                $.post($(form).attr("action"), form.serialize())
                    .done(function (data, textStatus) {
                        if (!data && textStatus === "success") {
                            $("#modal-action").modal("hide");
                            $('.modal-backdrop').remove();
                            $("#patientTable").DataTable().ajax.reload(null, false);
                            global.swalWithBootstrapButtons.fire("Sucesso", "Informação do paciente atualizada com sucesso.", "success");
                            global.cleanModal();
                        } else {
                            $("#modalBody").html(data);
                            initEditPatientInformationForm();
                        }
                    }).fail(function (error) {
                        global.swalWithBootstrapButtons.fire('Ops...', 'Alguma coisa deu errado!', 'error');
                    })
                    .always(function () {
                        $(submitButton).removeProp("disabled").removeClass("disabled");
                        $("#submitSpinner").hide();
                    });
            }
        });
    }
    


    // todo update

    function initDelete(url, id, relation) {
        let message = "Você não poderá reverter isso!";
        if (relation) {
            message = "Este médico está atribuído a pacientes, deseja prosseguir mesmo assim?";
        }

        global.swalWithBootstrapButtons.fire({
            title: 'Você têm certeza?',
            text: message,
            type: 'warning',
            showCancelButton: true,
            showLoaderOnConfirm: true,
            preConfirm: () => {
                $.post(url, { id: id })
                    .done(function (data, textStatus) {
                        $("#patientTable").DataTable().ajax.reload(null, false);
                        global.swalWithBootstrapButtons.fire("Removido!", "O médico foi removido com sucesso.", "success");
                    }).fail(function (error) {
                        global.swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                    });
            }
        });
    }

}());