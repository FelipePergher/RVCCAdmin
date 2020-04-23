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
                { data: "treatmentBeginDate", title: "Início Tratamento", name: "TreatmentBeginDate" },
                { data: "phone", title: "Telefone", name: "Phone", orderable: false },
                { data: "address", title: "Endereço", name: "Address", orderable: false },
                { data: "medicines", title: "Remédios", name: "Medicines", orderable: false },
                { data: "canceres", title: "Cânceres", name: "Canceres", orderable: false },
                { data: "doctors", title: "Médicos", name: "Doctors", orderable: false }
            ],
            drawCallback: function (settings) {
                $(".archivePatientButton").click(function (e) {
                    global.openModal($(this).attr("href"), $(this).data("title"), initArchivePatient);
                });

                $(".deletePatientButton").click(function (e) {
                    initDelete($(this).data("url"), $(this).data("id"));
                });

                $(".activePatientButton").click(function (e) {
                    initActivePatient($(this).data("url"), $(this).data("id"));
                });

                $(".socialObservationButton").click(function () {
                    global.openModal($(this).attr("href"), $(this).data("title"), initAddSocialObservationForm);
                });

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

    function initAddSocialObservationForm() {
        $.validator.unobtrusive.parse("#addSocialObservationForm");

        $("#addSocialObservationForm").submit(function (e) {
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
                                text: "Observações salvas com sucesso.",
                                type: 'success'
                            }).then((result) => {
                                $("#modal-action").modal("hide");
                                $('.modal-backdrop').remove();
                            });
                        }
                        else {
                            $("#modalBody").html(data);
                            initAddSocialObservationForm();
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

    //Control Enable/Disable Functions
    function initArchivePatient() {
        $('#DateTime').datepicker({
            clearBtn: true,
            format: "dd/mm/yyyy",
            language: "pt-BR",
            templates: {
                leftArrow: '<i class="fas fa-chevron-left"></i>',
                rightArrow: '<i class="fas fa-chevron-right"></i>'
            }
        });
        $.validator.unobtrusive.parse("#archivePatientForm");

        $("#archivePatientForm").submit(function (e) {
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
                                text: "O paciente foi arquivado com sucesso.",
                                type: 'success'
                            }).then((result) => {
                                $("#modal-action").modal("hide");
                                $('.modal-backdrop').remove();
                            });
                        }
                        else {
                            $("#modalBody").html(data);
                            initArchivePatient();
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

    function initActivePatient(url, id) {
        global.swalWithBootstrapButtons.fire({
            title: 'Você têm certeza?',
            text: "Você quer reativar este paciente?",
            type: 'warning',
            showCancelButton: true,
            showLoaderOnConfirm: true,
            preConfirm: () => {
                $.post(url, { id: id })
                    .done(function (data, textStatus) {
                        $("#patientTable").DataTable().ajax.reload(null, false);
                        global.swalWithBootstrapButtons.fire("Ativo!", "O paciente foi reativado com sucesso.", "success");
                    }).fail(function (error) {
                        global.swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                    });
            }
        });
    }

    function initDelete(url, id) {
        global.swalWithBootstrapButtons.fire({
            title: 'Você têm certeza?',
            text: "Você não poderá reverter isso!",
            type: 'warning',
            showCancelButton: true,
            showLoaderOnConfirm: true,
            preConfirm: () => {
                $.post(url, { id: id })
                    .done(function (data, textStatus) {
                        $("#patientTable").DataTable().ajax.reload(null, false);
                        global.swalWithBootstrapButtons.fire("Removido!", "O paciente foi removido com sucesso.", "success");
                    }).fail(function (error) {
                        global.swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                    });
            }
        });
    }

}());