"use strict";
import "jquery-validation";
import "jquery-validation-unobtrusive";
import "datatables.net";
import "datatables.net-bs4";
import 'bootstrap-datepicker';
import 'bootstrap-datepicker/dist/locales/bootstrap-datepicker.pt-br.min';

export default (function () {

    global.setupValidator($.validator);

    $(function () {
        initPage();
    });

    function initPage() {
        $('#Month').datepicker({
            clearBtn: true,
            language: "pt-BR",
            format: "MM",
            viewMode: "months",
            minViewMode: "months",
            maxViewMode: "months",
            immediateUpdates: true,
            templates: {
                leftArrow: '<span class="fas fa-chevron-left"></span>',
                rightArrow: '<span class="fas fa-chevron-right"></span>'
            }
        });

        let birthdayTable = $("#birthdayTable").DataTable({
            processing: true,
            serverSide: true,
            language: global.datatablesLanguage,
            filter: false,
            ajax: {
                url: "/api/birthday/search",
                type: "POST",
                data: function (d) {
                    d.name = $("#Name").val();
                    d.month = $("#Month").val();

                    var monthDatepicker = $("#Month").data('datepicker');
                    if (monthDatepicker) {
                        d.month = monthDatepicker.getFormattedDate('mm');
                    }
                },
                datatype: "json",
                error: function () {
                    global.swalWithBootstrapButtons.fire("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
                }
            },
            order: [1, "desc"],
            columns: [
                { data: "name", title: "Nome do Paciente", name: "Name" },
                { data: "dateOfBirth", title: "Data do aniversário", name: "DateOfBirth" },
                { data: "phone", title: "Telefone", name: "Phone" }
            ]
        });

        $('#birthdayTable').attr('style', 'border-collapse: collapse !important');

        $("#searchForm").off("submit").submit(function (e) {
            e.preventDefault();
            birthdayTable.search("").draw("");
        });
    }
}());