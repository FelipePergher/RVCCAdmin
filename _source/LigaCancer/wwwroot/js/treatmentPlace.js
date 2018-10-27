﻿$(function () {
    dataTable = BuildDataTable();
});

function BuildDataTable() {
    return $("#treatmentPlaceTable").DataTable({
        pageLength: 10,
        processing: true,
        serverSide: true,
        language: language,
        ajax: {
            url: $("#linkAjaxDT").attr("href"),
            type: "POST",
            error: function (ex) {
            }
        },
        order: [[0, "asc"]],
        columns: [
            { data: "city", width: "50%" },
            {
                "render": function (data, type, row, meta) {
                    let link = $("#linkEdit");
                    let options = '<a href="' + link.attr("href") + '/' + row.doctorId + '" data-toggle="' + $(link).data("toggle") + '" data-target="' + $(link).data("target") + '" class="btn btn-secondary">Editar</a>';

                    if (row.patientInformationTreatmentPlaces.length === 0) {
                        link = $("#linkDelete");

                        options = options.concat(
                            '<a href="' + link.attr("href") + '/' + row.doctorId + '" data-toggle="' + $(link).data("toggle") + '" data-target="' + $(link).data("target") + '" class="btn btn-danger ml-1">Deletar</a>'
                        );
                    } else {
                        options = options.concat(
                            '<a class="btn btn-danger ml-1 disabled">Deletar</a>'
                        );
                    }
                    return options;
                }
            }
        ],
        columnDefs: [
            { "orderable": false, "targets": [-1] },
            { "searchable": false, "targets": [-1] },
            { "orderable": true, "targets": [0, 1] },
            { "searchable": true, "targets": [0, 1] }
        ]
    });
}
