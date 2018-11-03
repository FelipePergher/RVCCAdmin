var dataTablePhone;

$(function () {
    $("#modal-action-patient-details").on('loaded.bs.modal', function (e) {
    }).on('hidden.bs.modal', function (e) {
        $(this).removeData('bs.modal');
    });

    
    dataTablePhone = PhoneDataTable();
    //dataTablePhone = CreateDataTable("phoneTable", "/api/GetPhoneDataTableResponseAsync");
    //dataTablePhone = PhoneTable();
    //Tables("familyMemberTable", 5);
    //Tables("addressTable", 6);
    //Tables("attachmentsTable", 2);
});

$("#modal-action-patient-details").on("show.bs.modal", function (e) {
    var link = $(e.relatedTarget);
    $(this).find(".modal-content").load(link.attr("href"), function () {
        $.validator.unobtrusive.parse('form');

        $("#ResidenceType").select2({
            theme: "bootstrap",
            placeholder: "Selecione o tipo de residência",
            allowClear: true
        }).on('select2:close', function (e) {
            let selected = $("#ResidenceType").val();
            if (selected !== "") {
                $("#monthlyResidence").show();
            } else {
                $("#monthlyResidence").hide();
            }
        });

        $("#formAddFile").submit(function (e) {
            e.preventDefault();
            let form = $(this);

            if (form.valid()) {
                var formData = new FormData();
                formData.append("FileCategory", $("#FileCategory").val());
                formData.append("File", document.getElementById("File").files[0]);
                formData.append("__RequestVerificationToken", $("input[name=__RequestVerificationToken]").val());
                formData.append("FileName", $("#FileName").val());
                formData.append("PatientId", $("#PatientId").val());

                $("#submitSpinner").show();

                $.ajax({
                    type: "POST",
                    url: form.attr("action"),
                    data: formData,
                    contentType: false,
                    processData: false,
                    success: function (response) {
                        $("#submitSpinner").hide();
                        if (response) {
                            location.reload();
                        } else {
                            swal("Oops...", "Algo deu errado!\n", "Erro");
                        }
                    },
                    error: function (error) {
                        $("#submitSpinner").hide();
                        swal("Oops...", "Algo deu errado!\nO servidor respondeu com:\n\n'" + error.statusText + "'", "Erro");
                    },
                    done: function () {
                    }
                });

            }
        });

    });
});

function AjaxSuccessPatient(data, result) {
    if (result === "success") {
        $("#modal-action").modal('hide');
        $('#modal-action').removeClass("fade");

        ReloadTables(data);

        swal("Sucesso...", "Registro salvo com sucesso", "success");
    }
    else {
        $("#modal-content").html(data);
    }
}

function ReloadTables(tableName) {
    let dataTableReload;

    if (tableName === "phone") dataTableReload = dataTablePhone;



    
    if (dataTableReload !== undefined && dataTableReload !== null) {
        dataTableReload.ajax.reload();
    } else {
        swal("Oops...", "Alguma coisa aconteceu errado!\n Se o problema persistir contate o administrador!", "error");
    }
}

function PhoneDataTable() {
    let columns = [
        { data: "number", title: "Numero", width: "25%" },
        { data: "phoneType", title: "Tipo", width: "25%" },
        { data: "observationNote", title: "Observação", width: "25%" },
        {
            title: "Ações",
            width: "25%",
            render: function (data, type, row, meta) {
                let options = '<a href="/Phone/EditPhone/' + row.phoneId + '" data-toggle="modal" data-target="#modal-action' +
                    '" class="btn btn-secondary"><i class="fas fa-edit"></i> Editar</a>';

                options = options.concat(
                    '<a href="/Phone/DeletePhone/' + row.phoneId + '" data-toggle="modal" data-target="#modal-action' +
                    '" class="btn btn-danger ml-1"><i class="fas fa-trash-alt"></i> Deletar</a>'
                );
                return options;
            }
        }
    ];

    let columnDefs = [
        { "orderable": false, "targets": [-1] }
    ];

    return CreateDataTable("phoneTable", "/api/GetPhoneDataTableResponseAsync", columns, columnDefs);
}

function CreateDataTable(tableId, url, columns, columnDefs) {
    return $("#" + tableId).DataTable({
        paginate: false,
        filter: false,
        info: false,
        language: language,
        ajax: {
            url: url,
            type: "GET",
            error: function () {
                swal("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
            }
        },
        order: [[0, "asc"]],
        columns: columns,
        columnDefs: columnDefs
    });
}