"use strict";
import "jquery-validation";
import "jquery-validation-unobtrusive";
import "datatables.net";
import "datatables.net-bs4";
import "datatables.net-buttons-bs4/js/buttons.bootstrap4.min.js";
import "datatables.net-buttons/js/buttons.html5.min.js";
import "bootstrap/js/dist/modal";

export default (function () {

    global.setupValidator($.validator);

    $(function () {
        initPage();
    });

    function initPage() {
        let visitorTable = $("#visitorTable").DataTable({
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
                        columns: [1, 2, 3]
                    }
                }
            ],
            processing: true,
            serverSide: true,
            language: global.datatablesLanguage,
            filter: false,
            ajax: {
                url: "/api/visitor/search",
                type: "POST",
                data: function (d) {
                    d.name = $("#Name").val();
                    d.CPF = $("#CPF").val();
                    d.Phone = $("#Phone").val();
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
                { data: "cpf", title: "CPF", name: "CPF" },
                { data: "phone", title: "Phone", name: "Phone" },
            ],
            drawCallback: function () {
                $(".editVisitorButton").click(function () {
                    global.openModal($(this).attr("href"), $(this).data("title"), initEditForm);
                });

                $(".deleteVisitorButton").click(function () {
                    initDelete($(this).data("url"), $(this).data("id"), $(this).data("relation") === "True");
                });
            }
        });

        $("#visitorTable").attr("style", "border-collapse: collapse !important");

        $("#searchForm").off("submit").submit(function (e) {
            e.preventDefault();
            visitorTable.search("").draw("");
        });

        $("#addVisitorButton").click(function () {
            global.openModal($(this).attr("href"), $(this).data("title"), initAddForm);
        });
    }

    function initAddForm() {
        $.validator.unobtrusive.parse("#addVisitorForm");

        $("#addVisitorForm").off("submit").submit(function (e) {
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
                            $(".modal-backdrop").remove();
                            $("#visitorTable").DataTable().ajax.reload(null, false);
                            global.swalWithBootstrapButtons.fire("Sucesso", "Visitante registrado com sucesso.", "success");
                        } else {
                            $("#modalBody").html(data);
                            initAddForm();
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

    function initEditForm() {
        $.validator.unobtrusive.parse("#editVisitorForm");

        $("#editVisitorForm").off("submit").submit(function (e) {
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
                            $(".modal-backdrop").remove();
                            $("#visitorTable").DataTable().ajax.reload(null, false);
                            global.swalWithBootstrapButtons.fire("Sucesso", "Visitante atualizado com sucesso.", "success");
                        } else {
                            $("#modalBody").html(data);
                            initEditForm();
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

    function initDelete(url, id) {
        let message = "Você não poderá reverter isso!";

        global.swalWithBootstrapButtons.fire({
            title: "Você têm certeza?",
            text: message,
            type: "warning",
            showCancelButton: true,
            showLoaderOnConfirm: true,
            preConfirm: () => {
                $.post(url, { id: id, __RequestVerificationToken: $("input[name=__RequestVerificationToken").val() })
                    .done(function () {
                        $("#visitorTable").DataTable().ajax.reload(null, false);
                        global.swalWithBootstrapButtons.fire("Removido!", "O visitante foi removido com sucesso.", "success");
                    }).fail(function () {
                        global.swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                    });
            }
        });
    }

}());