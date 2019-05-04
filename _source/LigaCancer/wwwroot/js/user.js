let userTable = $("#userTable").DataTable({
    dom: "l<'export-buttons'B>frtip",
    buttons: [
        {
            extend: 'pdf',
            orientation: 'landscape',
            pageSize: 'LEGAL',
            exportOptions: {
                columns: 'th:not(:first-child)'
            },
            customize: function (doc) {
                doc.defaultStyle.alignment = 'center';
                doc.styles.tableHeader.alignment = 'center';
            }
        },
        {
            extend: 'excel',
            exportOptions: {
                columns: 'th:not(:first-child)'
            }
        }
    ],
    processing: true,
    language: language,
    filter: false,
    ajax: {
        url: "/api/user/search",
        type: "GET",
        data: function (d) {
            //d.name = $("#Name").val();
        },
        datatype: "json",
        error: function () {
            swalWithBootstrapButtons.fire("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
        }
    },
    order: [1, "asc"],
    columns: [
        { data: "actions", title: "Ações", name: "actions", width: "20px", orderable: false },
        { data: "name", title: "Nome", name: "name" },
        { data: "email", title: "Email", name: "email" },
        { data: "role", title: "Regra", name: "role" }
    ],
    drawCallback: function (settings) {
        //$(".editUserButton").click(function () {
        //    openModal($(this).attr("href"), $(this).data("title"), initEditForm);
        //});

        //$(".deleteUserButton").click(function (e) {
        //    initDelete($(this).data("url"), $(this).data("id"), $(this).data("relation") === "True");
        //});
    }
});

$(function () {
    initPage();
});

function initPage() {
    $('#userTable').attr('style', 'border-collapse: collapse !important');

    $("#searchForm").submit(function (e) {
        e.preventDefault();
        userTable.search("").draw("");
    });

    //$("#addUserButton").click(function () {
    //    openModal($(this).attr("href"), $(this).data("title"), initAddForm);
    //});
}
