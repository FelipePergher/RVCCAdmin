$(function () {
   dataTable = BuildDataTable();
});

function BuildDataTable() {
    return $("#userTable").DataTable({
        language: language,
        ajax: {
            url: "/api/GetUsersDataTableResponseAsync/",
            type: "GET",
            error: errorDataTable
        },
        order: [[0, "asc"]],
        columns: [
            { data: "firstName", title: "Nome" },
            { data: "lastName", title: "Sobrenome" },
            { data: "email", title: "Email" },
            { data: "role", title: "regra" },
            {
                title: "Ações",
                width: "20%",
                render: function (data, type, row, meta) {
                    if (row.email !== "felipepergher_10@hotmail.com") {
                        let render = '<a href="/User/EditUser/' + row.userId + '" data-toggle="modal" data-target="#modal-action"' +
                            ' class="btn btn-secondary"><i class="fas fa-edit"></i> Editar</a>';
                        render = render.concat(
                            '<a href="/User/DeleteUser/' + row.userId + '" data-toggle="modal" data-target="#modal-action"' +
                            ' class="btn btn-danger ml-1"><i class="fas fa-trash-alt"></i> Deletar</a>'
                        );
                        return render;
                    }
                    return "";
                }
            }
        ],
        columnDefs: [
            { "orderable": false, "targets": [-1] },
            { "searchable": false, "targets": [-1] }
        ]
    });
}