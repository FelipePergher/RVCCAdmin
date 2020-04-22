"use strict";
import "jquery-validation";
import "jquery-validation-unobtrusive";
import "datatables.net";
import "datatables.net-bs4";
import "bootstrap/js/dist/modal";

export default (function () {

    global.setupValidator($.validator);

    $(function () {
        initPage();
        global.cleanModalEvent();
    });

    function initPage() {
        let medicineTable = $("#medicineTable").DataTable({
            processing: true,
            serverSide: true,
            language: global.datatablesLanguage,
            filter: false,
            ajax: {
                url: "/api/Medicine/search",
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
                { data: "actions", title: "Ações", width: "20px", name: "Actions", orderable: false },
                { data: "name", title: "Nome", name: "Name" }
            ],
            drawCallback: function (settings) {
                $(".editMedicineButton").click(function () {
                    global.openModal($(this).attr("href"), $(this).data("title"), initEditForm);
                });

                $(".deleteMedicineButton").click(function (e) {
                    initDelete($(this).data("url"), $(this).data("id"), $(this).data("relation") === "True");
                });
            }
        });

        $('#medicineTable').attr('style', 'border-collapse: collapse !important');

        $("#searchForm").submit(function (e) {
            e.preventDefault();
            medicineTable.search("").draw("");
        });

        $("#addMedicineButton").click(function () { 
            global.openModal($(this).attr("href"), $(this).data("title"), initAddForm);
        });
    }

    function initAddForm() {
        $.validator.unobtrusive.parse("#addMedicineForm");

        $("#addMedicineForm").submit(function (e) {
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
                            $("#medicineTable").DataTable().ajax.reload(null, false);
                            global.swalWithBootstrapButtons.fire("Sucesso", "Remédio registrado com sucesso.", "success");
                            global.cleanModal();
                        } else {
                            $("#modalBody").html(data);
                            initAddForm();
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

    function initEditForm() {
        $.validator.unobtrusive.parse("#editMedicineForm");

        $("#editMedicineForm").submit(function (e) {
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
                            $("#medicineTable").DataTable().ajax.reload(null, false);
                            global.swalWithBootstrapButtons.fire("Sucesso", "Remédio atualizado com sucesso.", "success");
                            global.cleanModal();
                        } else {
                            $("#modalBody").html(data);
                            initEditForm();
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

    function initDelete(url, id, relation) {
        let message = "Você não poderá reverter isso!";
        if (relation) {
            message = "Este remédio está atribuído a pacientes, deseja prosseguir mesmo assim?";
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
                        $("#medicineTable").DataTable().ajax.reload(null, false);
                        global.swalWithBootstrapButtons.fire("Removido!", "O remédio foi removido com sucesso.", "success");
                    }).fail(function (error) {
                        global.swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                    });
            }
        });
    }

}());