$(function () {
    dataTable = BuildDataTable();
});

function BuildDataTable() {
    return $("#cancerTypeTable").DataTable({
        pageLength: 10,
        processing: true,
        serverSide: true,
        language: language,
        ajax: {
            url: "/api/GetCancerTypeDataTableResponseAsync",
            type: "POST",
            error: errorDataTable
        },
        order: [[0, "asc"]],
        columns: [
            { data: "name", title: "Nome" },
            {
                title: "Ações",
                width: "20%",
                render: function (data, type, row, meta) {
                    let options = '<a href="/CancerType/EditCancerType/' + row.cancerTypeId + '" data-toggle="modal" data-target="#modal-action"' +
                        ' class="btn btn-secondary"><i class="fas fa-edit"></i> Editar</a>';

                    if (row.patientInformationCancerTypes.length === 0) {
                        options = options.concat(
                            '<a href="/CancerType/DeleteCancerType/' + row.cancerTypeId + '" data-toggle="modal" data-target="#modal-action"' +
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
            { "searchable": false, "targets": [-1] }
        ]
    });
}
