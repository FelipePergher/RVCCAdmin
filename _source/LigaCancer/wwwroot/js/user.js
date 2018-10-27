$(function () {
    //Todo criar a api para o user
    //BuildDataTable();
});

function BuildDataTable() {
    return $("#userTable").DataTable({
        pageLength: 10,
        processing: true,
        serverSide: true,
        language: language,
        ajax: {
            url: "/User/GetDTResponseAsync",
            type: "POST",
            error: function (ex) {
            }
        },
        order: [[0, "asc"]],
        columns: [
            { data: "name", width: "25%" },
            { data: "username", width: "25%" },
            { data: "role", width: "25%" }
            //{
            //    "render": function (data, type, row, meta) {
            //        let link = $("#linkEdit");
            //        let options = '<a href="' + link.attr("href") + '/' + row.doctorId + '" data-toggle="' + $(link).data("toggle") + '" data-target="' + $(link).data("target") + '" class="btn btn-secondary">Editar</a>';

            //        if (row.patientInformationDoctors.length === 0) {
            //            link = $("#linkDelete");

            //            options = options.concat(
            //                '<a href="' + link.attr("href") + '/' + row.doctorId + '" data-toggle="' + $(link).data("toggle") + '" data-target="' + $(link).data("target") + '" class="btn btn-danger ml-1">Deletar</a>'
            //            );
            //        }
            //        return options;
            //    }
            //}
        ],
        columnDefs: [
            { "orderable": false, "targets": [-1] },
            { "searchable": false, "targets": [-1] },
            { "orderable": true, "targets": [0, 1, 2] },
            { "searchable": true, "targets": [0, 1, 2] }
        ]
    });
}