

let userTable = $("#userTable").DataTable({
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
    serverSide: true,
    processing: true,
    language: language,
    filter: false,
    ajax: {
        url: "/api/user/search",
        type: "POST",
        data: function (d) {
            d.name = $("#Name").val();
        },
        datatype: "json",
        error: function () {
            swalWithBootstrapButtons.fire("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
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
            openModal($(this).attr("href"), $(this).data("title"), initEditForm);
        });

        $(".deleteUserButton").click(function (e) {
            initDelete($(this).data("url"), $(this).data("id"));
        });

        $(".unlockUserButton").click(function (e) {
            initUnlock($(this).data("url"), $(this).data("id"));
        });
    }
});

$(function () {
    initPage();
});

function initPage() {
    $('#userTable').attr('style', 'border-collapse: collapse !important');

    $("#searchForm").submit(function (e) {
        e.preventDefault();
        console.log("aaa");
        userTable.search("").draw();
    });

    $("#addUserButton").click(function () {
        openModal($(this).attr("href"), $(this).data("title"), initAddForm);
    });
}

function initAddForm() {
    $.validator.unobtrusive.parse("#addUserForm");
    $(".userSelect2").select2();

    $(".eyePassword").click(function () {
        let icon = $(this);
        let inputPassword = icon.siblings("input");
        if (icon.hasClass("fa-eye")) {
            icon.removeClass("fa-eye").addClass("fa-eye-slash");
            inputPassword.attr("type", "password");
        } else {
            icon.addClass("fa-eye").removeClass("fa-eye-slash");
            inputPassword.attr("type", "text");
        }
    });
}

function addSuccess(data, textStatus) {
    if (!data && textStatus === "success") {
        $("#modal-action").modal("hide");
        userTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire("Sucesso...", "Usuário registrado com sucesso.", "success");
    }
    else {
        $("#modalBody").html(data);
        initAddForm();
    }
}

function initEditForm() {
    $.validator.unobtrusive.parse("#editUserForm");
    $(".userSelect2").select2();
}

function editSuccess(data, textStatus) {
    if (!data && textStatus === "success") {
        $("#modal-action").modal("hide");
        userTable.ajax.reload(null, false);
        swalWithBootstrapButtons.fire("Sucesso...", "Usuário atualizado com sucesso.", "success");
    }
    else {
        $("#modalBody").html(data);
        initEditForm();
    }
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
                    userTable.ajax.reload(null, false);
                    swalWithBootstrapButtons.fire("Removido!", "Usuário removido com sucesso.", "success");
                }).fail(function (error) {
                    swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                });
        }
    });
}

function initUnlock(url, id) {
    swalWithBootstrapButtons({
        title: 'Você têm certeza?',
        text: "A conta será desbloqueada!",
        type: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Sim',
        cancelButtonText: 'Não',
        showLoaderOnConfirm: true,
        reverseButtons: true,
        preConfirm: () => {
            $.post(url, { id: id })
                .done(function (data, textStatus) {
                    userTable.ajax.reload(null, false);
                    swalWithBootstrapButtons.fire("Desbloqueado!", "Usuário desbloqueado com sucesso.", "success");
                }).fail(function (error) {
                    swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                });
        }
    });
}

function error(error) {
    swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
}