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
import Dropzone from 'dropzone';

export default (function () {

    global.setupValidator($.validator);

    $(function () {
        initPage();
    });

    function initPage() {
        initEvents();
        initPhoneTable();
        initAddressTable();
        initFamilyMemberTable();
        initFilesTable();
        initPatientBenefitTable();
        initPresenceTable();
        initStayTable();
    }

    function initEvents() {

        $("#editPatientButton").click(function () {
            global.openModal($(this).attr("href"), $(this).data("title"), initEditProfileForm, true);
        });

        $("#editPatientInformationButton").click(function () {
            global.openModal($(this).attr("href"), $(this).data("title"), initEditPatientInformationForm);
        });

        $("#editNaturalityButton").click(function () {
            global.openModal($(this).attr("href"), $(this).data("title"), initEditNaturalityForm);
        });

    }

    // Edit Functions
    function initEditProfileForm() {
        $.validator.unobtrusive.parse("#editPatientProfileForm");
        $("#CPF").mask(global.masks.Cpf);
        $("#MonthlyIncomeMinSalary").mask(global.masks.Price, { reverse: true });

        $('#DateOfBirth, #JoinDate').datepicker({
            clearBtn: true,
            format: "dd/mm/yyyy",
            language: "pt-BR",
            templates: {
                leftArrow: '<span class="fas fa-chevron-left"></span>',
                rightArrow: '<span class="fas fa-chevron-right"></span>'
            }
        });
        $(".patientProfileSelect2").select2();

        $("#editPatientProfileForm").off("submit").submit(function (e) {
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

                            global.swalWithBootstrapButtons.fire({
                                title: 'Sucesso',
                                text: "Paciente atualizado com sucesso.",
                                type: 'success'
                            }).then((result) => {
                                location.reload();
                            });
                        } else {
                            $("#modalBody").html(data);
                            initEditProfileForm();
                        }
                    }).fail(function (error) {
                        global.swalWithBootstrapButtons.fire('Ops...', 'Alguma coisa deu errado!', 'error');
                    })
                    .always(function () {
                        $(submitButton).removeAttr("disabled").removeClass("disabled");
                        $("#submitSpinner").hide();
                    });
            }
        });
    }

    function initEditNaturalityForm() {
        $.validator.unobtrusive.parse("#editPatientNaturalityForm");

        $("#editPatientNaturalityForm").off("submit").submit(function (e) {
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

                            global.swalWithBootstrapButtons.fire({
                                title: 'Sucesso',
                                text: "Naturalidade atualizada com sucesso.",
                                type: 'success'
                            }).then((result) => {
                                location.reload();
                            });
                        } else {
                            $("#modalBody").html(data);
                            initEditNaturalityForm();
                        }
                    }).fail(function (error) {
                        global.swalWithBootstrapButtons.fire('Ops...', 'Alguma coisa deu errado!', 'error');
                    })
                    .always(function () {
                        $(submitButton).removeAttr("disabled").removeClass("disabled");
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
                leftArrow: '<span class="fas fa-chevron-left"></span>',
                rightArrow: '<span class="fas fa-chevron-right"></span>'
            }
        });
        $(".patientInformationSelect").select2();

        $("#editPatientInformationForm").off("submit").submit(function (e) {
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
                            global.swalWithBootstrapButtons.fire({
                                title: 'Sucesso',
                                text: "Informação do paciente atualizadas com sucesso.",
                                type: 'success'
                            }).then((result) => {
                                location.reload();
                            });
                        } else {
                            $("#modalBody").html(data);
                            initEditPatientInformationForm();
                        }
                    }).fail(function (error) {
                        global.swalWithBootstrapButtons.fire('Ops...', 'Alguma coisa deu errado!', 'error');
                    })
                    .always(function () {
                        $(submitButton).removeAttr("disabled").removeClass("disabled");
                        $("#submitSpinner").hide();
                    });
            }
        });
    }

    //Phone Functions
    function initPhoneTable() {
        $("#phoneTable").DataTable({
            autoWidth: false,
            processing: true,
            serverSide: true,
            language: global.datatablesLanguage,
            filter: false,
            ajax: {
                url: "/api/phone/search",
                type: "POST",
                data: function (d) {
                    d.patientId = $("#patientId").val();
                },
                datatype: "json",
                error: function () {
                    global.swalWithBootstrapButtons.fire("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
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
                    global.openModal($(this).attr("href"), $(this).data("title"), initEditPhoneForm);
                });

                $(".deletePhoneButton").click(function (e) {
                    initDeletePhone($(this).data("url"), $(this).data("id"));
                });
            }
        });
        $('#phoneTable').attr('style', 'border-collapse: collapse !important');

        $("#addPhoneButton").click(function () {
            global.openModal($(this).attr("href"), $(this).data("title"), initAddPhoneForm);
        });
    }

    function initAddPhoneForm() {
        $('#Number').mask(global.SPMaskBehavior, global.spOptions);
        $.validator.unobtrusive.parse("#addPhoneForm");

        $("#addPhoneForm").off("submit").submit(function (e) {
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

                            $("#phoneTable").DataTable().ajax.reload(null, false);
                            reloadIframe();
                            global.swalWithBootstrapButtons.fire("Sucesso!", "Telefone adicionado com sucesso.", "success");
                        } else {
                            $("#modalBody").html(data);
                            initAddPhoneForm();
                        }
                    }).fail(function (error) {
                        global.swalWithBootstrapButtons.fire('Ops...', 'Alguma coisa deu errado!', 'error');
                    })
                    .always(function () {
                        $(submitButton).removeAttr("disabled").removeClass("disabled");
                        $("#submitSpinner").hide();
                    });
            }
        });
    }

    function initEditPhoneForm() {
        $('#Number').mask(global.SPMaskBehavior, global.spOptions);
        $.validator.unobtrusive.parse("#editPhoneForm");

        $("#editPhoneForm").off("submit").submit(function (e) {
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

                            $("#phoneTable").DataTable().ajax.reload(null, false);
                            reloadIframe();
                            global.swalWithBootstrapButtons.fire("Sucesso!", "Telefone atualizado com sucesso.", "success");
                        } else {
                            $("#modalBody").html(data);
                            initEditPhoneForm();
                        }
                    }).fail(function (error) {
                        global.swalWithBootstrapButtons.fire('Ops...', 'Alguma coisa deu errado!', 'error');
                    })
                    .always(function () {
                        $(submitButton).removeAttr("disabled").removeClass("disabled");
                        $("#submitSpinner").hide();
                    });
            }
        });
    }

    function initDeletePhone(url, id) {
        global.swalWithBootstrapButtons.fire({
            title: 'Você têm certeza?',
            text: "Você não poderá reverter isso!",
            type: 'warning',
            showCancelButton: true,
            showLoaderOnConfirm: true,
            preConfirm: () => {
                $.post(url, { id: id, __RequestVerificationToken: $("input[name=__RequestVerificationToken").val()  })
                    .done(function (data, textStatus) {
                        $("#phoneTable").DataTable().ajax.reload(null, false);
                        reloadIframe();
                        global.swalWithBootstrapButtons.fire("Removido!", "O telefone foi removido com sucesso.", "success");
                    }).fail(function (error) {
                        global.swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                    });
            }
        });
    }

    //Address Functions
    function initAddressTable() {
        $("#addressTable").DataTable({
            autoWidth: false,
            processing: true,
            serverSide: true,
            language: global.datatablesLanguage,
            filter: false,
            ajax: {
                url: "/api/address/search",
                type: "POST",
                data: function (d) {
                    d.patientId = $("#patientId").val();
                },
                datatype: "json",
                error: function () {
                    global.swalWithBootstrapButtons.fire("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
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
                    global.openModal($(this).attr("href"), $(this).data("title"), initEditAddressForm);
                });
                $(".deleteAddressButton").click(function (e) {
                    initDeleteAddress($(this).data("url"), $(this).data("id"));
                });
            }
        });
        $('#addressTable').attr('style', 'border-collapse: collapse !important');

        $("#addAddressButton").click(function () {
            global.openModal($(this).attr("href"), $(this).data("title"), initAddAddressForm);
        });
    }

    function initAddAddressForm() {
        $("#MonthlyAmmountResidence").mask(global.masks.Price, { reverse: true });

        $.validator.unobtrusive.parse("#addAddressForm");

        $("#ResidenceType").change(function (e) {
            !!$(this).val() ? $("#monthlyResidence").show() : $("#monthlyResidence").hide();
        });

        $("#addAddressForm").off("submit").submit(function (e) {
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

                            $("#addressTable").DataTable().ajax.reload(null, false);
                            reloadIframe();
                            global.swalWithBootstrapButtons.fire("Sucesso!", "Endereço adicionado com sucesso.", "success");
                        } else {
                            $("#modalBody").html(data);
                            initAddAddressForm();
                        }
                    }).fail(function (error) {
                        global.swalWithBootstrapButtons.fire('Ops...', 'Alguma coisa deu errado!', 'error');
                    })
                    .always(function () {
                        $(submitButton).removeAttr("disabled").removeClass("disabled");
                        $("#submitSpinner").hide();
                    });
            }
        });
    }

    function initEditAddressForm() {
        $("#MonthlyAmmountResidence").mask(global.masks.Price, { reverse: true });

        $.validator.unobtrusive.parse("#editAddressForm");

        $("#ResidenceType").change(function (e) {
            !!$(this).val() ? $("#monthlyResidence").show() : $("#monthlyResidence").hide();
        });

        $("#editAddressForm").off("submit").submit(function (e) {
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

                            $("#addressTable").DataTable().ajax.reload(null, false);
                            reloadIframe();
                            global.swalWithBootstrapButtons.fire("Sucesso!", "Endereço atualizado com sucesso.", "success");
                        } else {
                            $("#modalBody").html(data);
                            initEditAddressForm();
                        }
                    }).fail(function (error) {
                        global.swalWithBootstrapButtons.fire('Ops...', 'Alguma coisa deu errado!', 'error');
                    })
                    .always(function () {
                        $(submitButton).removeAttr("disabled").removeClass("disabled");
                        $("#submitSpinner").hide();
                    });
            }
        });
    }

    function initDeleteAddress(url, id) {
        global.swalWithBootstrapButtons.fire({
            title: 'Você têm certeza?',
            text: "Você não poderá reverter isso!",
            type: 'warning',
            showCancelButton: true,
            showLoaderOnConfirm: true,
            preConfirm: () => {
                $.post(url, { id: id, __RequestVerificationToken: $("input[name=__RequestVerificationToken").val() })
                    .done(function (data, textStatus) {
                        $("#addressTable").DataTable().ajax.reload(null, false);
                        reloadIframe();
                        global.swalWithBootstrapButtons.fire("Removido!", "O endereço foi removido com sucesso.", "success");
                    }).fail(function (error) {
                        global.swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                    });
            }
        });
    }

    //Patient Benefits Functions
    function initPatientBenefitTable() {
        $("#patientBenefitTable").DataTable({
            autoWidth: false,
            processing: true,
            serverSide: true,
            language: global.datatablesLanguage,
            filter: false,
            ajax: {
                url: "/api/patientBenefit/search",
                type: "POST",
                data: function (d) {
                    d.patientId = $("#patientId").val();
                },
                datatype: "json",
                error: function () {
                    global.swalWithBootstrapButtons.fire("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
                }
            },
            order: [1, "asc"],
            columns: [
                { data: "benefit", title: "Benefício", name: "Benefit" },
                { data: "date", title: "Data do benefício", name: "Date" },
                { data: "quantity", title: "Quantidade", name: "Quantity" }
            ]
        });
        $('#patientBenefitTable').attr('style', 'border-collapse: collapse !important');
    }

    //Presence Functions
    function initPresenceTable() {
        $("#presenceTable").DataTable({
            autoWidth: false,
            processing: true,
            serverSide: true,
            language: global.datatablesLanguage,
            filter: false,
            ajax: {
                url: "/api/presence/search",
                type: "POST",
                data: function (d) {
                    d.patientId = $("#patientId").val();
                },
                datatype: "json",
                error: function () {
                    global.swalWithBootstrapButtons.fire("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
                }
            },
            columns: [
                { data: "date", title: "Data da presença", name: "Date" },
                { data: "hour", title: "Hora da presença", name: "Hour" }
            ],
        });
        $('#presenceTable').attr('style', 'border-collapse: collapse !important');
    }

    //Stay Functions
    function initStayTable() {
        $("#stayTable").DataTable({
            autoWidth: false,
            processing: true,
            serverSide: true,
            language: global.datatablesLanguage,
            filter: false,
            ajax: {
                url: "/api/stay/search",
                type: "POST",
                data: function (d) {
                    d.patientId = $("#patientId").val();
                },
                datatype: "json",
                error: function () {
                    global.swalWithBootstrapButtons.fire("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
                }
            },
            order: [1, "asc"],
            columns: [
                { data: "date", title: "Data da estadia", name: "Date" },
                { data: "city", title: "Cidade", name: "City" },
                { data: "note", title: "Notas", name: "Note" }
            ],
        });
        $('#stayTable').attr('style', 'border-collapse: collapse !important');
    }

    //Family Member Functions
    function initFamilyMemberTable() {
        $("#familyMemberTable").DataTable({
            autoWidth: false,
            processing: true,
            serverSide: true,
            language: global.datatablesLanguage,
            filter: false,
            ajax: {
                url: "/api/familyMember/search",
                type: "POST",
                data: function (d) {
                    d.patientId = $("#patientId").val();
                },
                dataSrc: function (data) {
                    $("#familyIncome").text(data.familyIncome);
                    $("#perCapitaIncome").text(data.perCapitaIncome);
                    $("#monthlyIncome").text(data.monthlyIncome);
                    return data.data;
                },
                datatype: "json",
                error: function () {
                    global.swalWithBootstrapButtons.fire("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
                }
            },
            order: [1, "asc"],
            columns: [
                { data: "actions", title: "Ações", name: "actions", width: "20px", orderable: false },
                { data: "name", title: "Nome", name: "Name" },
                { data: "kinship", title: "Parentesco", name: "Kinship" },
                { data: "dateOfBirth", title: "Data de Nascimento", name: "DateOfBirth" },
                { data: "sex", title: "Gênero", name: "Sex" },
                { data: "monthlyIncome", title: "Renda", name: "MonthlyIncome" }
            ],
            drawCallback: function (settings) {
                $(".editFamilyMemberButton").click(function () {
                    global.openModal($(this).attr("href"), $(this).data("title"), initEditFamilyMemberForm);
                });
                $(".deleteFamilyMemberButton").click(function (e) {
                    initDeleteFamilyMember($(this).data("url"), $(this).data("id"));
                });
            }
        });
        $('#familyMemberTable').attr('style', 'border-collapse: collapse !important');

        $("#addFamilyMemberButton").click(function () {
            global.openModal($(this).attr("href"), $(this).data("title"), initAddFamilyMemberForm);
        });
    }

    function initAddFamilyMemberForm() {
        $("#MonthlyIncomeMinSalary").mask(global.masks.Price, { reverse: true });
        $('#dateOfBirth').datepicker({
            clearBtn: true,
            format: "dd/mm/yyyy",
            language: "pt-BR",
            templates: {
                leftArrow: '<span class="fas fa-chevron-left"></span>',
                rightArrow: '<span class="fas fa-chevron-right"></span>'
            }
        });
        $.validator.unobtrusive.parse("#addFamilyMemberForm");

        $("#addFamilyMemberForm").off("submit").submit(function (e) {
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

                            $("#familyMemberTable").DataTable().ajax.reload(null, false);
                            reloadIframe();
                            global.swalWithBootstrapButtons.fire("Sucesso!", "Membro familiar adicionado com sucesso.", "success");
                        } else {
                            $("#modalBody").html(data);
                            initAddFamilyMemberForm();
                        }
                    }).fail(function (error) {
                        global.swalWithBootstrapButtons.fire('Ops...', 'Alguma coisa deu errado!', 'error');
                    })
                    .always(function () {
                        $(submitButton).removeAttr("disabled").removeClass("disabled");
                        $("#submitSpinner").hide();
                    });
            }
        });
    }

    function initEditFamilyMemberForm() {
        $("#MonthlyIncomeMinSalary").mask(masks.Price, { reverse: true });
        $(".familyMemberSelect2").select2();
        $('#dateOfBirth').datepicker({
            clearBtn: true,
            format: "dd/mm/yyyy",
            language: "pt-BR",
            templates: {
                leftArrow: '<span class="fas fa-chevron-left"></span>',
                rightArrow: '<span class="fas fa-chevron-right"></span>'
            }
        });
        $.validator.unobtrusive.parse("#editFamilyMemberForm");

        $("#editFamilyMemberForm").off("submit").submit(function (e) {
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

                            $("#familyMemberTable").DataTable().ajax.reload(null, false);
                            reloadIframe();
                            global.swalWithBootstrapButtons.fire("Sucesso!", "Membro familiar atualizado com sucesso.", "success");
                        } else {
                            $("#modalBody").html(data);
                            initEditFamilyMemberForm();
                        }
                    }).fail(function (error) {
                        global.swalWithBootstrapButtons.fire('Ops...', 'Alguma coisa deu errado!', 'error');
                    })
                    .always(function () {
                        $(submitButton).removeAttr("disabled").removeClass("disabled");
                        $("#submitSpinner").hide();
                    });
            }
        });
    }

    function initDeleteFamilyMember(url, id) {
        global.swalWithBootstrapButtons.fire({
            title: 'Você têm certeza?',
            text: "Você não poderá reverter isso!",
            type: 'warning',
            showCancelButton: true,
            showLoaderOnConfirm: true,
            preConfirm: () => {
                $.post(url, { id: id, __RequestVerificationToken: $("input[name=__RequestVerificationToken").val() })
                    .done(function (data, textStatus) {
                        $("#familyMemberTable").DataTable().ajax.reload(null, false);
                        reloadIframe();
                        global.swalWithBootstrapButtons.fire("Removido!", "Membro familiar removido com sucesso.", "success");
                    }).fail(function (error) {
                        global.swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                    });
            }
        });
    }

    //Files

    function initFilesTable() {
        $("#attachmentsTable").DataTable({
            autoWidth: false,
            processing: true,
            serverSide: true,
            language: global.datatablesLanguage,
            filter: false,
            ajax: {
                url: "/api/FileAttachment/search",
                type: "POST",
                data: function (d) {
                    d.patientId = $("#patientId").val();
                },
                datatype: "json",
                error: function () {
                    global.swalWithBootstrapButtons.fire("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
                }
            },
            order: [1, "asc"],
            columns: [
                { data: "actions", title: "Ações", name: "actions", width: "20px", orderable: false },
                {
                    render: function (data, type, row, meta) {
                        return row.size + " Mb";
                    },
                    data: "size", title: "Tamanho", name: "Size"
                },
                {
                    data: "name", title: "Arquivo", name: "Name",
                    render: function (data, type, row, meta) {
                        return '   <span class="editable" data-fileAttachmentId="' + row.fileAttachmentId + '">' + row.name + '</span>' +
                            '   <a class="float-right fa fa-download mt-1 ml-2" class="fa fa-download" href="' + row.filePath + '" download="' + row.name + row.extension + '"></a>';
                    }
                }
            ],
            drawCallback: function (settings) {
                $(".deleteFileAttachmentButton").click(function (e) {
                    initDeleteFileAttachment($(this).data("url"), $(this).data("id"));
                });

                // Todo
                //$('.editable').editable(function (value, settings) {
                //    let fileAttachmentId = $(this).data("fileattachmentid");

                //    $.post("/FileAttachment/UpdateNameFile", { fileAttachmentId: fileAttachmentId, name: value })
                //        .done(function (data, textStatus) {
                //            attachmentsTable.ajax.reload(null, false);
                //        }).fail(function (error) {
                //            global.swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                //        });
                //}, {
                //    indicator: 'salvando…',
                //    cssclass: "form-row",
                //    submit: 'Salvar',
                //    submitcssclass: 'btn btn-primary ml-2',
                //    inputcssclass: "form-control",
                //    placeholder: "Clique para editar",
                //    tooltip: "Clique para editar"
                //});
            }
        });
        $('#attachmentsTable').attr('style', 'border-collapse: collapse !important');

        $("#addFileButton").click(function () {
            global.openModal($(this).attr("href"), $(this).data("title"), initFileUpload, true);
        });
    }

    function initFileUpload() {
        let dropzoneConfiguration = {
            maxFilesize: 20,
            acceptedFiles: "image/*,application/pdf",
            dictDefaultMessage: "Arraste seus arquivos aqui ou clique para upload.",
            dictFallbackMessage: "Seu navegador não tem suporte para arrastar e upload.",
            dictFileTooBig: "O arquivo é muito grande ({{filesize}}MB). Tamanho máximo: {{maxFilesize}}MB.",
            dictInvalidFileType: "Você não pode fazer upload de arquivos deste tipo.",
            dictResponseError: "Servidor respondeu com status de {{statusCode}}.",
            dictCancelUpload: "Cancelar upload",
            dictCancelUploadConfirmation: "Você têm certeza que quer cancelar este upload?",
            dictRemoveFile: "Remover arquivo",
            dictMaxFilesExceeded: "Você não pode fazer upload de mais arquivos."
        };

        var myDropzone = new Dropzone("#dropzoneForm", dropzoneConfiguration);

        myDropzone.on("success", function (file) {
            $("#attachmentsTable").DataTable().ajax.reload(null, false);
            reloadIframe();
        });
    }

    function initDeleteFileAttachment(url, id) {
        global.swalWithBootstrapButtons.fire({
            title: 'Você têm certeza?',
            text: "Você não poderá reverter isso!",
            type: 'warning',
            showCancelButton: true,
            showLoaderOnConfirm: true,
            preConfirm: () => {
                $.post(url, { id: id, __RequestVerificationToken: $("input[name=__RequestVerificationToken").val()  })
                    .done(function (data, textStatus) {
                        $("#attachmentsTable").DataTable().ajax.reload(null, false);
                        reloadIframe();
                        global.swalWithBootstrapButtons.fire("Removido!", "O arquivo foi removido com sucesso.", "success");
                    }).fail(function (error) {
                        globa.swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                    });
            }
        });
    }

    // Reload Iframe
    function reloadIframe() {
        document.getElementById('printPatient').contentDocument.location.reload(true);
    }
}());