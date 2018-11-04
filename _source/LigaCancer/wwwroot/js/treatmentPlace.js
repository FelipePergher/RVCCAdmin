$(function () {
    dataTable = BuildDataTable();
});

function BuildDataTable() {
    return $("#treatmentPlaceTable").DataTable({
        pageLength: 10,
        processing: true,
        serverSide: true,
        language: language,
        ajax: {
            url: "api/GetTreatmentPlaceDataTableResponseAsync",
            type: "POST",
            error: errorDataTable
        },
        order: [[0, "asc"]],
        columns: [
            { data: "city", title: "Cidade"},
            {
                title: "Ações",
                width: "20%",
                render: function (data, type, row, meta) {
                    let options = '<a href="/TreatmentPlace/EditTreatmentPlace/' + row.treatmentPlaceId + '" data-toggle="modal" data-target="#modal-action"' +
                        ' class="btn btn-secondary"><i class="fas fa-edit"></i> Editar</a>';

                    if (row.patientInformationTreatmentPlaces.length === 0) {
                        options = options.concat(
                            '<a href="/TreatmentPlace/DeleteTreatmentPlace/' + row.treatmentPlaceId + '" data-toggle="modal" data-target="#modal-action"' +
                                ' class="btn btn-danger ml-1"><i class="fas fa-trash-alt"></i> Deletar</a>'
                        );
                    } else {
                        options = options.concat(
                            '<a class="btn btn-danger ml-1 disabled"><i class="fas fa-trash-alt"></i> Deletar</a>'
                        );
                    }
                    return options;
                }
            }
        ],
        columnDefs: [
            { "orderable": false, "targets": [-1] },
            { "searchable": false, "targets": [-1] },
            { "orderable": true, "targets": [0] },
            { "searchable": true, "targets": [0] }
        ]
    });
}
