"use strict";
import "jquery-validation";
import "jquery-validation-unobtrusive";
import "datatables.net";
import "datatables.net-bs4";
import "bootstrap/js/dist/modal";
import 'bootstrap-datepicker';
import 'bootstrap-datepicker/dist/locales/bootstrap-datepicker.pt-br.min';
import 'select2';
import 'tempusdominus-bootstrap-4';
import "datatables.net-buttons-bs4/js/buttons.bootstrap4.min.js";
import "datatables.net-buttons/js/buttons.html5.min.js";

export default (function () {

    global.setupValidator($.validator);

    $(function () {
        initPage();
    });

    function initPage() {
        let presenceTable = $("#presenceTable").DataTable({
            dom:
                "<'row'<'col-sm-12 col-md-6'l><'col-sm-12 col-md-6'B>>" +
                "<'row'<'col-sm-12'tr>>" +
                "<'row'<'col-sm-12 col-md-5'i><'col-sm-12 col-md-7'p>>",
            lengthMenu: [
                [10, 25, 50, 99999999],
                ['10', '25', '50', 'Tudo']
            ],
            buttons: [
                {
                    extend: "csv",
                    exportOptions: {
                        columns: [1, 2, 3]
                    }
                }
            ],
            processing: true,
            serverSide: true,
            language: global.datatablesLanguage,
            filter: false,
            ajax: {
                url: "/api/presence/search",
                type: "POST",
                data: function (d) {
                    d.name = $("#Name").val();
                    d.dateFrom = $("#DateFrom").val();
                    d.dateTo = $("#DateTo").val();
                },
                datatype: "json",
                error: function () {
                    global.swalWithBootstrapButtons.fire("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
                }
            },
            order: [2, "desc"],
            columns: [
                { data: "actions", title: "Ações", name: "Actions", orderable: false },
                { data: "patient", title: "Nome do Paciente", name: "Patient" },
                { data: "date", title: "Data da presença", name: "Date" },
                { data: "hour", title: "Hora da presença", name: "Hour" }
            ],
            drawCallback: function (settings) {
                $(".editPresenceButton").click(function () {
                    global.openModal($(this).attr("href"), $(this).data("title"), initEditForm);
                });

                $(".deletePresenceButton").click(function (e) {
                    initDelete($(this).data("url"), $(this).data("id"), $(this).data("relation") === "True");
                });
            }
        });

        $('#presenceTable').attr('style', 'border-collapse: collapse !important');

        $('#DateTo, #DateFrom').datepicker({
            clearBtn: true,
            format: "dd/mm/yyyy",
            language: "pt-BR",
            templates: {
                leftArrow: '<span class="fas fa-chevron-left"></span>',
                rightArrow: '<span class="fas fa-chevron-right"></span>'
            }
        });

        $("#searchForm").off("submit").submit(function (e) {
            e.preventDefault();
            presenceTable.search("").draw("");
        });

        $("#addPresenceButton").click(function () { 
            global.openModal($(this).attr("href"), $(this).data("title"), initAddForm);
        });
    }

    function initAddForm() {
        $.validator.unobtrusive.parse("#addPresenceForm");

        $(".select2").select2();

        $('#timePicker').datetimepicker({
            format: 'H:mm'
        });

        $('#Date').datepicker({
            clearBtn: true,
            format: "dd/mm/yyyy",
            language: "pt-BR",
            templates: {
                leftArrow: '<span class="fas fa-chevron-left"></span>',
                rightArrow: '<span class="fas fa-chevron-right"></span>'
            }
        });

        $("#addPresenceForm").off("submit").submit(function (e) {
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
                            $("#presenceTable").DataTable().ajax.reload(null, false);
                            global.swalWithBootstrapButtons.fire("Sucesso", "Presença registrado com sucesso.", "success");
                        } else {
                            $("#modalBody").html(data);
                            initAddForm();
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

    function initEditForm() {
        $.validator.unobtrusive.parse("#editPresenceForm");

        $('#timePicker').datetimepicker({
            format: 'H:mm'
        });

        $('#Date').datepicker({
            clearBtn: true,
            format: "dd/mm/yyyy",
            language: "pt-BR",
            templates: {
                leftArrow: '<span class="fas fa-chevron-left"></span>',
                rightArrow: '<span class="fas fa-chevron-right"></span>'
            }
        });

        $("#editPresenceForm").off("submit").submit(function (e) {
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
                            $("#presenceTable").DataTable().ajax.reload(null, false);
                            global.swalWithBootstrapButtons.fire("Sucesso", "Presença atualizado com sucesso.", "success");
                        } else {
                            $("#modalBody").html(data);
                            initEditForm();
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

    function initDelete(url, id, relation) {
        global.swalWithBootstrapButtons.fire({
            title: 'Você têm certeza?',
            text: "Você não poderá reverter isso!",
            type: 'warning',
            showCancelButton: true,
            showLoaderOnConfirm: true,
            preConfirm: () => {
                $.post(url, { id: id, __RequestVerificationToken: $("input[name=__RequestVerificationToken").val()  })
                    .done(function (data, textStatus) {
                        $("#presenceTable").DataTable().ajax.reload(null, false);
                        global.swalWithBootstrapButtons.fire("Removido!", "A presença foi removida com sucesso.", "success");
                    }).fail(function (error) {
                        global.swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                    });
            }
        });
    }

}());