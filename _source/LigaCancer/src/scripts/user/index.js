"use strict";
import "jquery-validation";
import "jquery-validation-unobtrusive";
import "datatables.net";
import "datatables.net-bs4";
import "bootstrap/js/dist/modal";
import "datatables.net-buttons-bs4/js/buttons.bootstrap4.min.js";
import "datatables.net-buttons/js/buttons.html5.min.js";

export default (function () {

    global.setupValidator($.validator);

    $(function () {
        initPage();
    });

    function initPage() {
        let userTable = $("#userTable").DataTable({
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
                        columns: [1, 2, 5]
                    }
                }
            ],
            processing: true,
            serverSide: true,
            language: global.datatablesLanguage,
            filter: false,
            ajax: {
                url: "/api/user/search",
                type: "POST",
                data: function (d) {
                    d.name = $("#Name").val();
                },
                datatype: "json",
                error: function () {
                    global.swalWithBootstrapButtons.fire("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
                }
            },
            order: [1, "asc"],
            columns: [
                { data: "actions", title: "Ações", name: "Actions", width: "20px", orderable: false },
                { data: "name", title: "Nome", name: "Name" },
                { data: "email", title: "Email", name: "Email" },
                { data: "confirmedEmail", title: "Email confirmado", name: "ConfirmedEmail" },
                { data: "lockout", title: "Conta bloqueada", name: "Lockout" },
                { data: "role", title: "Regra", name: "Role" }
            ],
            drawCallback: function (settings) {
                $(".editUserButton").click(function () {
                    global.openModal($(this).attr("href"), $(this).data("title"), initEditForm);
                });

                $(".deleteUserButton").click(function (e) {
                    initDelete($(this).data("url"), $(this).data("id"));
                });

                $(".unlockUserButton").click(function (e) {
                    initUnlock($(this).data("url"), $(this).data("id"));
                });
            }
        });

        $('#userTable').attr('style', 'border-collapse: collapse !important');

        $("#searchForm").off("submit").submit(function (e) {
            e.preventDefault();
            userTable.search("").draw("");
        });

        $("#addUserButton").click(function () {
            global.openModal($(this).attr("href"), $(this).data("title"), initAddForm);
        });
    }

    function initAddForm() {
        $.validator.unobtrusive.parse("#addUserForm");

        $("#addUserForm").off("submit").submit(function (e) {
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
                            $("#userTable").DataTable().ajax.reload(null, false);
                            global.swalWithBootstrapButtons.fire("Sucesso", "Usuário registrado com sucesso.", "success");
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
        $.validator.unobtrusive.parse("#editUserForm");

        $("#editUserForm").off("submit").submit(function (e) {
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
                            $("#userTable").DataTable().ajax.reload(null, false);
                            global.swalWithBootstrapButtons.fire("Sucesso", "Usuário atualizado com sucesso.", "success");
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
                        $("#userTable").DataTable().ajax.reload(null, false);
                        global.swalWithBootstrapButtons.fire("Removido!", "O usuário foi removido com sucesso.", "success");
                    }).fail(function (error) {
                        global.swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                    });
            }
        });
    }

    function initUnlock(url, id) {
        global.swalWithBootstrapButtons.fire({
            title: 'Você têm certeza?',
            text: "A conta será desbloqueada!",
            type: 'warning',
            showCancelButton: true,
            confirmButtonText: 'Sim',
            cancelButtonText: 'Não',
            showLoaderOnConfirm: true,
            reverseButtons: true,
            preConfirm: () => {
                $.post(url, { id: id, __RequestVerificationToken: $("input[name=__RequestVerificationToken").val() })
                    .done(function (data, textStatus) {
                        $("#userTable").DataTable().ajax.reload(null, false);
                        global.swalWithBootstrapButtons.fire("Desbloqueado!", "Usuário desbloqueado com sucesso.", "success");
                    }).fail(function (error) {
                        global.swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                    });
            }
        });
    }
}());