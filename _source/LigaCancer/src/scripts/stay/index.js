"use strict";
import "jquery-validation";
import "jquery-validation-unobtrusive";
import "datatables.net";
import "datatables.net-bs4";
import "bootstrap/js/dist/modal";
import 'bootstrap-datepicker';
import 'bootstrap-datepicker/dist/locales/bootstrap-datepicker.pt-br.min';
import 'select2';

export default (function () {

    global.setupValidator($.validator);

    $(function () {
        initPage();
    });

    function initPage() {
        let stayTable = $("#stayTable").DataTable({
            processing: true,
            serverSide: true,
            language: global.datatablesLanguage,
            filter: false,
            ajax: {
                url: "/api/stay/search",
                type: "POST",
                data: function (d) {
                    d.name = $("#Name").val();
                    d.city = $("#City").val();
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
                { data: "city", title: "Cidade", name: "City" },
                { data: "note", title: "Notas", name: "Note" }
            ],
            drawCallback: function (settings) {
                $(".editStayButton").click(function () {
                    global.openModal($(this).attr("href"), $(this).data("title"), initEditForm);
                });

                $(".deleteStayButton").click(function (e) {
                    initDelete($(this).data("url"), $(this).data("id"));
                });
            }
        });

        $('#stayTable').attr('style', 'border-collapse: collapse !important');

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
            stayTable.search("").draw("");
        });

        $("#addStayButton").click(function () { 
            global.openModal($(this).attr("href"), $(this).data("title"), initAddForm);
        });
    }

    function initAddForm() {
        $.validator.unobtrusive.parse("#addStayForm");

        $(".select2").select2();

        $('#Date').datepicker({
            clearBtn: true,
            format: "dd/mm/yyyy",
            language: "pt-BR",
            templates: {
                leftArrow: '<span class="fas fa-chevron-left"></span>',
                rightArrow: '<span class="fas fa-chevron-right"></span>'
            }
        });

        $("#addStayForm").off("submit").submit(function (e) {
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
                            $("#stayTable").DataTable().ajax.reload(null, false);
                            global.swalWithBootstrapButtons.fire("Sucesso", "Estadia registrado com sucesso.", "success");
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
        $.validator.unobtrusive.parse("#editStayForm");

        $('#Date').datepicker({
            clearBtn: true,
            format: "dd/mm/yyyy",
            language: "pt-BR",
            templates: {
                leftArrow: '<span class="fas fa-chevron-left"></span>',
                rightArrow: '<span class="fas fa-chevron-right"></span>'
            }
        });

        $("#editStayForm").off("submit").submit(function (e) {
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
                            $("#stayTable").DataTable().ajax.reload(null, false);
                            global.swalWithBootstrapButtons.fire("Sucesso", "Estadia atualizado com sucesso.", "success");
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

    function initDelete(url, id) {
        global.swalWithBootstrapButtons.fire({
            title: 'Você têm certeza?',
            text: "Você não poderá reverter isso!",
            type: 'warning',
            showCancelButton: true,
            showLoaderOnConfirm: true,
            preConfirm: () => {
                $.post(url, { id: id, __RequestVerificationToken: $("input[name=__RequestVerificationToken").val()  })
                    .done(function (data, textStatus) {
                        $("#stayTable").DataTable().ajax.reload(null, false);
                        global.swalWithBootstrapButtons.fire("Removido!", "A estadia foi removida com sucesso.", "success");
                    }).fail(function (error) {
                        global.swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                    });
            }
        });
    }

}());