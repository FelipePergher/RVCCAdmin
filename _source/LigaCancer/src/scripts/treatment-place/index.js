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
    });

    function initPage() {
        let treatmentPlaceTable = $("#treatmentPlaceTable").DataTable({
            processing: true,
            serverSide: true,
            language: global.datatablesLanguage,
            filter: false,
            ajax: {
                url: "/api/TreatmentPlace/search",
                type: "POST",
                data: function (d) {
                    d.city = $("#City").val();
                },
                datatype: "json",
                error: function () {
                    swalWithBootstrapButtons.fire("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
                }
            },
            order: [1, "asc"],
            columns: [
                { data: "actions", title: "Ações", name: "Actions", width: "20px", orderable: false },
                { data: "city", title: "Cidade", name: "City" }
            ],
            drawCallback: function (settings) {
                $(".editTreatmentPlaceButton").click(function () {
                    global.openModal($(this).attr("href"), $(this).data("title"), initEditForm);
                });

                $(".deleteTreatmentPlaceButton").click(function (e) {
                    initDelete($(this).data("url"), $(this).data("id"), $(this).data("relation") === "True");
                });
            }
        });

        $('#treatmentPlaceTable').attr('style', 'border-collapse: collapse !important');

        $("#searchForm").submit(function (e) {
            e.preventDefault();
            treatmentPlaceTable.search("").draw("");
        });

        $("#addTreatmentPlaceButton").click(function () {
            global.openModal($(this).attr("href"), $(this).data("title"), initAddForm);
        });
    }

    function initAddForm() {
        $.validator.unobtrusive.parse("#addTreatmentPlaceForm");

        $("#addTreatmentPlaceForm").submit(function (e) {
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
                            $("#treatmentPlaceTable").DataTable().ajax.reload(null, false);
                            global.swalWithBootstrapButtons.fire("Sucesso", "Local de tratamento registrado com sucesso.", "success");
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
        $.validator.unobtrusive.parse("#editTreatmentPlaceForm");

        $("#editTreatmentPlaceForm").submit(function (e) {
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
                            $("#treatmentPlaceTable").DataTable().ajax.reload(null, false);
                            global.swalWithBootstrapButtons.fire("Sucesso", "Local de tratamento atualizado com sucesso.", "success");
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
            message = "Este local de tratamento está atribuído a pacientes, deseja prosseguir mesmo assim?";
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
                        $("#treatmentPlaceTable").DataTable().ajax.reload(null, false);
                        global.swalWithBootstrapButtons.fire("Removido!", "O local de tratamento foi removido com sucesso.", "success");
                    }).fail(function (error) {
                        global.swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                    });
            }
        });
    }

}());