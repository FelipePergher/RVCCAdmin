$(function () {
    dataTable = BuildDataTable();
});

function BuildDataTable() {
    return $("#doctorTable").DataTable({
        pageLength: 10,
        processing: true,
        serverSide: true,
        language: language,
        ajax: {
            url: "/api/GetDoctorDataTableResponseAsync",
            type: "POST",
            error: errorDataTable
        },
        order: [[0, "asc"]],
        columns: [
            { data: "name", title: "Nome" },
            { data: "crm", title: "CRM" },
            {
                title: "Ações",
                width: "20%",
                render: function (data, type, row, meta) {
                    let link = $("#linkEdit");
                    let options = '<a href="/Doctor/EditDoctor/' + row.doctorId + '" data-toggle="modal" data-target="#modal-action"'+
                        ' class="btn btn-secondary"><i class="fas fa-edit"></i> Editar</a>';

                    if (row.patientInformationDoctors.length === 0) {
                        link = $("#linkDelete");

                        options = options.concat(
                            '<a href="/Doctor/DeleteDoctor/' + row.doctorId + '" data-toggle="modal" data-target="#modal-action"' +
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
