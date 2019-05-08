function FileAttachmentDataTable() {
    let columns = [
        { data: "archiveCategorie", title: "Categoria arquivo" },
        {
            title: "Categoria arquivo",
            render: function (data, type, row, meta) {
                let anchor = '<a class="fa fa-file" href="/' + row.filePath + '" download="' + row.fileName + '"> ' + row.fileName + "</a>";
                return anchor;
            }
        },
        {
            title: "Ações",
            width: "20%",
            render: function (data, type, row, meta) {
                let render = 
                    '<a href="/FileAttachment/DeleteFileAttachment/' + row.fileAttachmentId + '" data-toggle="modal" data-target="#modal-action' +
                    '" class="btn btn-danger ml-1"><i class="fas fa-trash-alt"></i> Excluir </a>';
                return render;
            }
        }
    ];

    let columnDefs = [
        { "orderable": false, "targets": [-1] }
    ];

    return CreateDataTable("attachmentsTable", "/api/GetFileAttachmentsAsync/" + patientId, columns, columnDefs);
}