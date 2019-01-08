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
                width: "180px",
                render: function (data, type, row, meta) {
                    let link = $("#linkEdit");
                    let render = '<a href="/Doctor/EditDoctor/' + row.doctorId + '" data-toggle="modal" data-target="#modal-action"'+
                        ' class="btn btn-secondary"><i class="fas fa-edit"></i> Editar </a>';

                    if (row.patientInformationDoctors.length === 0) {
                        link = $("#linkDelete");

                        render = render.concat(
                            '<a href="/Doctor/DeleteDoctor/' + row.doctorId + '" data-toggle="modal" data-target="#modal-action"' +
                            ' class="btn btn-danger ml-1"><i class="fas fa-trash-alt"></i> Excluir </a>'
                        );
                    } else {
                        render = render.concat(
                            '<a class="btn btn-danger ml-1 disabled"><i class="fas fa-trash-alt"></i> Excluir </a>'
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
