$(function () {
    dataTable = BuildDataTable();
});

function BuildDataTable() {
    return $("#patientTable").DataTable({
        pageLength: 10,
        processing: true,
        serverSide: true,
        language: language,
        ajax: {
            url: $("#linkAjaxDT").attr("href"),
            type: "POST",
            error: errorDataTable
        },
        order: [[0, "asc"]],
        columns: [
            { data: "firstName", title: "Nome" },
            { data: "surname", title: "Sobrenome"},
            { data: "rg", title: "RG"},
            { data: "cpf", title: "CPF"},
            {
                title: "Ações",
                render: function (data, type, row, meta) {
                    let link = $("#linkShow");
                    let options = '<a href="' + link.attr("href") + '/' + row.patientId + '" class="btn btn-info"><i class="fas fa-info"></i> Detalhes</a>';

                    link = $("#linkEdit");
                    options = options.concat(
                        '<a href="' + link.attr("href") + '/' + row.patientId + '" data-toggle="' + $(link).data("toggle") + '" data-target="' + $(link).data("target") +
                        '" class="btn btn-secondary ml-1"><i class="fas fa-edit"></i> Editar</a>'
                    );

                    link = $("#linkDelete");
                    options = options.concat(
                        '<a href="' + link.attr("href") + '/' + row.patientId + '" data-toggle="' + $(link).data("toggle") + '" data-target="' + $(link).data("target") +
                        '" class="btn btn-danger ml-1"><i class="fas fa-trash-alt"></i> Deletar</a>'
                    );
                    return options;
                }
            }
        ],
        columnDefs: [
            { "orderable": false, "targets": [-1] },
            { "searchable": false, "targets": [-1] },
            { "orderable": true, "targets": [0, 1] },
            { "searchable": true, "targets": [0, 1, 2, 3] }
        ]
    });
}
