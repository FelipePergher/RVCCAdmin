$(function () {
    dataTable = BuildDataTable();
});

function BuildDataTable() {
    return $("#medicineTable").DataTable({
        pageLength: 10,
        processing: true,
        serverSide: true,
        language: language,
        ajax: {
            url: "/api/GetMedicineDataTableResponseAsync",
            type: "POST",
            error: errorDataTable
        },
        order: [[0, "asc"]],
        columns: [
            { data: "name", title: "Nome"},
            {
                title: "Ações",
                width: "180px",
                render: function (data, type, row, meta) {
                    let render = '<a href="/Medicine/EditMedicine/' + row.medicineId + '" data-toggle="modal" data-target="#modal-action"' +
                        ' class="btn btn-secondary"><i class="fas fa-edit"></i> Editar </a>';

                    if (row.patientInformationMedicines.length === 0) {
                        render = render.concat(
                            '<a href="/Medicine/DeleteMedicine/' + row.medicineId + '" data-toggle="modal" data-target="#modal-action"' +
                            ' class="btn btn-danger ml-1"><i class="fas fa-trash-alt"></i> Excluir </a>'
                        );
                    } else {
                        render = render.concat(
                            '<a class="btn btn-danger ml-1 disabled"><i class="fas fa-trash-alt"></i>  Excluir </a>'
                        );
                    }
                    return render;
                }
            }
        ],
        columnDefs: [
            { "orderable": false, "targets": [-1] },
            { "searchable": false, "targets": [-1] }
        ]
    });
}
