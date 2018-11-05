$(function () {
    dataTable = BuildDataTable();

    $('#CivilState, #Sex').change(function (e) {
        dataTable.draw();
    });
});

function BuildDataTable() {
    return $("#patientTable").DataTable({
        pageLength: 10,
        processing: true,
        serverSide: true,
        language: language,
        ajax: {
            url: "/api/GetPatientDataTableResponseAsync",
            type: "POST",
            data: function (d) {
                return $.extend({}, d, {
                    civilState: $('#CivilState').val(),
                    Sex: $('#Sex').val()
                });
            },
            error: errorDataTable
        },
        order: [[0, "asc"]],
        columns: [
            { data: "firstName", title: "Nome" },
            { data: "surname", title: "Sobrenome" },
            { data: "rg", title: "RG" },
            { data: "cpf", title: "CPF" },
            { data: "sex", title: "Gênero" },
            { data: "civilState", title: "Estado Civil" },
            { data: "dateOfBirth", title: "Nascimento" },
            {
                title: "Ações",
                width: "30%",
                render: function (data, type, row, meta) {
                    console.log(row);
                    let render = '<a href="/Patient/DetailsPatient/' + row.patientId + '" class="btn btn-info"><i class="fas fa-info"></i> Detalhes</a>';

                    link = $("#linkEdit");
                    render = render.concat(
                        '<a href="/Patient/EditPatient/' + row.patientId + '" data-toggle="modal" data-target="#modal-action"' +
                        ' class="btn btn-secondary ml-1"><i class="fas fa-edit"></i> Editar</a>'
                    );

                    link = $("#linkDelete");
                    render = render.concat(
                        '<a href="/Patient/DisablePatient/' + row.patientId + '" data-toggle="modal" data-target="#modal-action"' +
                        ' class="btn btn-danger ml-1"><i class="fas fa-trash-alt"></i> Deletar</a>'
                    );
                    return render;
                }
            }
        ],
        columnDefs: [
            { "orderable": false, "targets": [5, 6, 7, 8] },
            { "searchable": false, "targets": [5, 6, 7, 8] }
        ]
    });
}
