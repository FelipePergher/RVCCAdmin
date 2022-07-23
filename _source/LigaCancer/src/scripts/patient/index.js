"use strict";
import "jquery-validation";
import "jquery-validation-unobtrusive";
import "datatables.net";
import "datatables.net-bs4";
import "bootstrap/js/dist/modal";
import "select2";
import "bootstrap-datepicker";
import "bootstrap-datepicker/dist/locales/bootstrap-datepicker.pt-br.min";
import "jquery-mask-plugin";
import "datatables.net-buttons-bs4/js/buttons.bootstrap4.min.js";
import "datatables.net-buttons/js/buttons.html5.min.js";

export default (function () {

    global.setupValidator($.validator);

    $(function () {
        initPage();
    });

    function initPage() {
        $("#Cpf").mask(global.masks.Cpf);

        $("#BirthdayDateFrom, #BirthdayDateTo, #JoinDateFrom, #JoinDateTo").datepicker({
            clearBtn: true,
            format: "dd/mm/yyyy",
            language: "pt-BR",
            templates: {
                leftArrow: "<span class=\"fas fa-chevron-left\"></span>",
                rightArrow: "<span class=\"fas fa-chevron-right\"></span>"
            }
        });

        let patientTable = $("#patientTable").DataTable({
            dom:
                "<'row'<'col-sm-12 col-md-6'l><'col-sm-12 col-md-6'B>>" +
                "<'row'<'col-sm-12'tr>>" +
                "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
            lengthMenu: [
                [10, 25, 50, 99999999],
                ["10", "25", "50", "Tudo"]
            ],
            buttons: [
                {
                    extend: "csv",
                    exportOptions: {
                        columns: [1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12]
                    }
                }
            ],
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
                    d.serviceTypes = $("#ServiceTypes").val();
                    d.familiarityGroup = $("#FamiliarityGroup").val();
                    d.forwardedToSupportHouse = $("#ForwardedToSupportHouse").val();
                    d.archivePatientType = $("#ArchivePatientType").val();
                    d.BirthdayDateFrom = $("#BirthdayDateFrom").val();
                    d.BirthdayDateTo = $("#BirthdayDateTo").val();
                    d.joinDateFrom = $("#JoinDateFrom").val();
                    d.joinDateTo = $("#JoinDateTo").val();
                },
                datatype: "json",
                error: function () {
                    global.swalWithBootstrapButtons.fire("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
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
            drawCallback: function () {
                $(".archivePatientButton").click(function () {
                    global.openModal($(this).attr("href"), $(this).data("title"), initArchivePatient);
                });

                $(".deletePatientButton").click(function () {
                    initDelete($(this).data("url"), $(this).data("id"));
                });

                $(".activePatientButton").click(function () {
                    initActivePatient($(this).data("url"), $(this).data("id"));
                });

                $(".socialObservationButton").click(function () {
                    global.openModal($(this).attr("href"), $(this).data("title"), initAddSocialObservationForm);
                });

            }
        });

        $("#patientTable").attr("style", "border-collapse: collapse !important");

        $(".filterSelect").select2();

        $("#searchForm").on("reset", function () {
            $(".filterSelect").val("").trigger("change");
        });

        $("#searchForm").off("submit").submit(function (e) {
            e.preventDefault();
            patientTable.search("").draw("");
        });

        $("#addPatientButton").click(function () {
            global.openModal($(this).attr("href"), $(this).data("title"), initAddProfileForm, true);
        });
    }

    // Add Functions
    function initAddProfileForm() {
        $("#CPF").mask(global.masks.Cpf);
        $("#MonthlyIncome").mask(global.masks.Price, { reverse: true });

        $("#DateOfBirth, #JoinDate").datepicker({
            clearBtn: true,
            format: "dd/mm/yyyy",
            language: "pt-BR",
            templates: {
                leftArrow: "<span class=\"fas fa-chevron-left\"></span>",
                rightArrow: "<span class=\"fas fa-chevron-right\"></span>"
            }
        });

        $.validator.unobtrusive.parse("#addPatientProfileForm");

        $("#addPatientProfileForm").off("submit").submit(function (e) {
            e.preventDefault();

            let form = $(this);
            if (form.valid()) {
                let submitButton = $(this).find("button[type='submit']");
                $(submitButton).prop("disabled", "disabled").addClass("disabled");
                $("#submitSpinner").show();

                $.post($(form).attr("action"), form.serialize())
                    .done(function (data) {
                        if (data.ok) {
                            $("#patientTable").DataTable().ajax.reload(null, false);
                            global.swalWithBootstrapButtons.fire({
                                title: "Sucesso",
                                text: "Paciente registrado com sucesso.",
                                type: "success"
                            }).then(() => {
                                global.openModal(data.url, data.title, initAddNaturalityForm);
                            });
                        }
                        else {
                            $("#modalBody").html(data);
                            initAddProfileForm();
                        }
                    }).fail(function () {
                        global.swalWithBootstrapButtons.fire("Ops...", "Alguma coisa deu errado!", "error");
                    })
                    .always(function () {
                        $(submitButton).removeAttr("disabled").removeClass("disabled");
                        $("#submitSpinner").hide();
                    });
            }
        });

    }

    function initAddNaturalityForm() {
        $.validator.unobtrusive.parse("#addPatientNaturalityForm");

        $("#addPatientNaturalityForm").off("submit").submit(function (e) {
            e.preventDefault();

            let form = $(this);
            if (form.valid()) {
                let submitButton = $(this).find("button[type='submit']");
                $(submitButton).prop("disabled", "disabled").addClass("disabled");
                $("#submitSpinner").show();

                $.post($(form).attr("action"), form.serialize())
                    .done(function (data) {
                        if (data.ok) {
                            $("#patientTable").DataTable().ajax.reload(null, false);
                            global.swalWithBootstrapButtons.fire({
                                title: "Sucesso",
                                text: "Naturalidade registrado com sucesso.",
                                type: "success"
                            }).then(() => {
                                global.openModal(data.url, data.title, initAddPatientInformationForm);
                            });
                        }
                        else {
                            $("#modalBody").html(data);
                            initAddNaturalityForm();
                        }
                    }).fail(function () {
                        global.swalWithBootstrapButtons.fire("Ops...", "Alguma coisa deu errado!", "error");
                    })
                    .always(function () {
                        $(submitButton).removeAttr("disabled").removeClass("disabled");
                        $("#submitSpinner").hide();
                    });
            }
        });
    }

    function initAddPatientInformationForm() {
        $.validator.unobtrusive.parse("#addPatientInformationForm");
        $(".patientInformationSelect").select2();
        $("#TreatmentBeginDate").datepicker({
            clearBtn: true,
            format: "dd/mm/yyyy",
            language: "pt-BR",
            templates: {
                leftArrow: "<span class=\"fas fa-chevron-left\"></span>",
                rightArrow: "<span class=\"fas fa-chevron-right\"></span>"
            }
        });

        $("#addPatientInformationForm").off("submit").submit(function (e) {
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
                                title: "Sucesso",
                                text: "Informação do paciente adicionada com sucesso.",
                                type: "success"
                            }).then(() => {
                                $("#modal-action").modal("hide");
                                $(".modal-backdrop").remove();
                            });
                        }
                        else {
                            $("#modalBody").html(data);
                            initAddPatientInformationForm();
                        }
                    }).fail(function () {
                        global.swalWithBootstrapButtons.fire("Ops...", "Alguma coisa deu errado!", "error");
                    })
                    .always(function () {
                        $(submitButton).removeAttr("disabled").removeClass("disabled");
                        $("#submitSpinner").hide();
                    });
            }
        });
    }

    function initAddSocialObservationForm() {
        $.validator.unobtrusive.parse("#addSocialObservationForm");

        $("#addSocialObservationForm").off("submit").submit(function (e) {
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
                                title: "Sucesso",
                                text: "Observações salvas com sucesso.",
                                type: "success"
                            }).then(() => {
                                $("#modal-action").modal("hide");
                                $(".modal-backdrop").remove();
                            });
                        }
                        else {
                            $("#modalBody").html(data);
                            initAddSocialObservationForm();
                        }
                    }).fail(function () {
                        global.swalWithBootstrapButtons.fire("Ops...", "Alguma coisa deu errado!", "error");
                    })
                    .always(function () {
                        $(submitButton).removeAttr("disabled").removeClass("disabled");
                        $("#submitSpinner").hide();
                    });
            }
        });
    }

    //Control Enable/Disable Functions
    function initArchivePatient() {
        $("#DateTime").datepicker({
            clearBtn: true,
            format: "dd/mm/yyyy",
            language: "pt-BR",
            templates: {
                leftArrow: "<span class=\"fas fa-chevron-left\"></span>",
                rightArrow: "<span class=\"fas fa-chevron-right\"></span>"
            }
        });
        $.validator.unobtrusive.parse("#archivePatientForm");

        $("#archivePatientForm").off("submit").submit(function (e) {
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
                                title: "Sucesso",
                                text: "O paciente foi arquivado com sucesso.",
                                type: "success"
                            }).then(() => {
                                $("#modal-action").modal("hide");
                                $(".modal-backdrop").remove();
                            });
                        }
                        else {
                            $("#modalBody").html(data);
                            initArchivePatient();
                        }
                    }).fail(function () {
                        global.swalWithBootstrapButtons.fire("Ops...", "Alguma coisa deu errado!", "error");
                    })
                    .always(function () {
                        $(submitButton).removeAttr("disabled").removeClass("disabled");
                        $("#submitSpinner").hide();
                    });
            }
        });
    }

    function initActivePatient(url, id) {
        global.swalWithBootstrapButtons.fire({
            title: "Você têm certeza?",
            text: "Você quer reativar este paciente?",
            type: "warning",
            showCancelButton: true,
            showLoaderOnConfirm: true,
            preConfirm: () => {
                $.post(url, { id: id, __RequestVerificationToken: $("input[name=__RequestVerificationToken").val()  })
                    .done(function () {
                        $("#patientTable").DataTable().ajax.reload(null, false);
                        global.swalWithBootstrapButtons.fire("Ativo!", "O paciente foi reativado com sucesso.", "success");
                    }).fail(function () {
                        global.swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                    });
            }
        });
    }

    function initDelete(url, id) {
        global.swalWithBootstrapButtons.fire({
            title: "Você têm certeza?",
            text: "Você não poderá reverter isso!",
            type: "warning",
            showCancelButton: true,
            showLoaderOnConfirm: true,
            preConfirm: () => {
                $.post(url, { id: id, __RequestVerificationToken: $("input[name=__RequestVerificationToken").val()  })
                    .done(function () {
                        $("#patientTable").DataTable().ajax.reload(null, false);
                        global.swalWithBootstrapButtons.fire("Removido!", "O paciente foi removido com sucesso.", "success");
                    }).fail(function () {
                        global.swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                    });
            }
        });
    }

}());