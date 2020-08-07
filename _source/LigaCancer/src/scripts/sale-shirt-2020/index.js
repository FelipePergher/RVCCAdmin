"use strict";
import "jquery-validation";
import "jquery-validation-unobtrusive";
import "datatables.net";
import "datatables.net-bs4";
import "bootstrap/js/dist/modal";
import 'bootstrap-datepicker';
import 'bootstrap-datepicker/dist/locales/bootstrap-datepicker.pt-br.min';
import "jquery-mask-plugin";

export default (function () {

    //global.setupValidator($.validator);

    $(function () {
        initPage();
    });

    function initPage() {
        let saleShirt2020Table = $("#saleShirt2020Table").DataTable({
            processing: true,
            serverSide: true,
            language: global.datatablesLanguage,
            filter: false,
            ajax: {
                url: "/api/SaleShirt2020/search",
                type: "POST",
                data: function (d) {
                    d.code = $("#Code").val();
                },
                datatype: "json",
                error: function () {
                    global.swalWithBootstrapButtons.fire("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
                }
            },
            order: [1, "asc"],
            columns: [
                { data: "actions", title: "Ações", width: "20px", name: "Actions", orderable: false },
                { data: "code", title: "Código", name: "Code" },
                { data: "status", title: "Estado", name: "Status" },
                { data: "buyerName", title: "Nome", name: "BuyerName" },
                { data: "buyerPhone", title: "Telefone", name: "BuyerPhone", orderable: false },
                { data: "date", title: "Data", name: "Date" },
                { data: "maskQuantity", title: "Máscaras", name: "MaskQuantity" },
                { data: "shirtQuantityTotal", title: "Camisetas", name: "ShirtQuantityTotal" },
                { data: "priceTotal", title: "Valor", name: "PriceTotal" }
            ],
            drawCallback: function (settings) {
                $(".detailsSaleShirt2020Button").click(function () {
                    global.openModal($(this).attr("href"), $(this).data("title"), initDetails);
                });

                $(".updateStatusSaleShirt2020Button").click(function (e) {
                    initUpdateStatusSaleShirt2020($(this).data("url"), $(this).data("id"), $(this).data("status"));
                });
            }
        });

        $('#saleShirt2020Table').attr('style', 'border-collapse: collapse !important');

        $("#searchForm").submit(function (e) {
            e.preventDefault();
            saleShirt2020Table.search("").draw("");
        });

        $("#addSaleShirt2020Button").click(function () {
            global.openModal($(this).attr("href"), $(this).data("title"), initAddForm);
        });
    }

    function initAddForm() {
        $("#modal-dialog").addClass("modal-elg");
        $('#DateOrdered').datepicker({
            clearBtn: true,
            format: "dd/mm/yyyy",
            language: "pt-BR",
            templates: {
                leftArrow: '<i class="fas fa-chevron-left"></i>',
                rightArrow: '<i class="fas fa-chevron-right"></i>'
            }
        });
        $('#BuyerPhone').mask(global.SPMaskBehavior, global.spOptions);

        $.validator.unobtrusive.parse("#addSaleShirt2020Form");

        $(".shirtQuantity, #MaskQuantity").change(function () {
            var totalShirtQuantity = $(".shirtQuantity").toArray().reduce(function (acumulador, valorAtual, index, array) {
                let value = $(valorAtual).val();
                return parseInt(acumulador) + parseInt(value);
            }, 0);
            let maskQuantity = parseInt($("#MaskQuantity").val());
            $("#totalShirtQuantity").text(totalShirtQuantity);
            $("#totalMaskQuantity").text(maskQuantity);
            $("#totalQuantity").text(parseInt(totalShirtQuantity) + maskQuantity);

            let totalShirtValue = totalShirtQuantity * 20;
            let totalMaskValue = maskQuantity * 5;

            $("#totalShirtValue").text(totalShirtValue);
            $("#totalMaskValue").text(totalMaskValue);
            $("#totalValue").text(totalShirtValue + totalMaskValue);
        });

        $("#addSaleShirt2020Form").submit(function (e) {
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
                            $("#saleShirt2020Table").DataTable().ajax.reload(null, false);
                            global.swalWithBootstrapButtons.fire("Sucesso", "Pedido de camisetas registrado com sucesso.", "success");
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

    function initDetails() {
        $("#modal-dialog").addClass("modal-elg");
    }

    function initUpdateStatusSaleShirt2020(url, id, status) {
        let message = "Você não poderá reverter isso!";

        let dateInput =
                '<form id="dateForm">' +
                '   <div class="form-group">' +
                '       <label class="control-label label-required" for="DateOrdered">Data</label>' +
                '       <input type="text" class="form-control" data-val="true" data-val-required="Este campo é obrigatório!" id="Date" name="Date" data-date-end-date="0d" value="">' +
                '       <span class="text-danger field-validation-valid" data-valmsg-for="Date" data-valmsg-replace="true"></span>' +
                '   </div>' +
                '</form>';

        global.swalWithBootstrapButtons.fire({
            title: 'Você têm certeza?',
            text: message,
            html: dateInput,
            type: 'warning',
            showCancelButton: true,
            showLoaderOnConfirm: true,
            onOpen: (e) => {
                $.validator.unobtrusive.parse("#dateForm");
                $('#Date').datepicker({
                    clearBtn: true,
                    format: "dd/mm/yyyy",
                    language: "pt-BR",
                    templates: {
                        leftArrow: '<i class="fas fa-chevron-left"></i>',
                        rightArrow: '<i class="fas fa-chevron-right"></i>'
                    }
                });
            },
            preConfirm: () => {
                let form = $("#dateForm");
                if (form.valid()) {
                    let date = $("#Date").val();
                    $.post(url, { id: id, status: status, date: date })
                        .done(function (data, textStatus) {
                            $("#saleShirt2020Table").DataTable().ajax.reload(null, false);
                            global.swalWithBootstrapButtons.fire("Atualizado!", "O pedido foi atualizado com sucesso.", "success");
                        }).fail(function (error) {
                            global.swalWithBootstrapButtons.fire("Oops...", "Alguma coisa deu errado!\n", "error");
                        });
                }
                else {
                    return false;
                }
            }
        });
    }

}());