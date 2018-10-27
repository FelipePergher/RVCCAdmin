$(function () {
    BuildDataTable();
});

function BuildDataTable() {
    return $("#userTable").DataTable({
        pageLength: 10,
        language: language,
        order: [[0, "asc"]],
        columnDefs: [
            { "orderable": false, "targets": [-1] },
            { "searchable": false, "targets": [-1] },
            { "orderable": true, "targets": [0, 1, 2] },
            { "searchable": true, "targets": [0, 1, 2] }
        ]
    });
}