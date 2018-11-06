var dataTablePhone;
var dataTableAddress;
var dataTableFamilyMember;
var dataTableAttachmentFile;

$(function () {
    $("#modal-action").on('loaded.bs.modal', function (e) {
    }).on('hidden.bs.modal', function (e) {
        $(this).removeData('bs.modal');
    });

    dataTablePhone = PhoneDataTable();
    dataTableAddress = AddressDataTable();
    dataTableFamilyMember = FamilyMemberDataTable();
    dataTableAttachmentFile = FileAttachmentDataTable();
});

$("#modal-action").on("show.bs.modal", function (e) {
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
                    success: AjaxSuccessPatient,
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
    if (tableName === "address") dataTableReload = dataTableAddress;
    if (tableName === "familyMember") {
        dataTableReload = dataTableFamilyMember;
        
        $.ajax({
            url: "/api/GetFamilyIncomeAsync/" + patientId,
            type: "GET",
            success: function (data) {
                $("#familyIncome").text("$" + parseFloat(data.familyIncome).toFixed(2));
                $("#perCapitaIncome").text("$" + parseFloat(data.perCapitaIncome).toFixed(2));
            },
            error: function (data) {
                swal("Oops...", "Alguma coisa aconteceu errado!\n Se o problema persistir contate o administrador!", "error");
            }
        });
    }
    if (tableName === "attachmentFile") dataTableReload = dataTableAttachmentFile;

    if (dataTableReload !== undefined && dataTableReload !== null) {
        dataTableReload.ajax.reload();
    } else {
        swal("Oops...", "Alguma coisa aconteceu errado!\n Se o problema persistir contate o administrador!", "error");
    }
}

function PhoneDataTable() {
    let columns = [
        { data: "number", title: "Numero" },
        { data: "phoneType", title: "Tipo" },
        { data: "observationNote", title: "Observação"},
        {
            title: "Ações",
            width: "180px",
            render: function (data, type, row, meta) {
                let render = '<a href="/Phone/EditPhone/' + row.phoneId + '" data-toggle="modal" data-target="#modal-action' +
                    '" class="btn btn-secondary"><i class="fas fa-edit"></i></a>';

                render = render.concat(
                    '<a href="/Phone/DeletePhone/' + row.phoneId + '" data-toggle="modal" data-target="#modal-action' +
                    '" class="btn btn-danger ml-1"><i class="fas fa-trash-alt"></i></a>'
                );
                return render;
            }
        }
    ];

    let columnDefs = [
        { "orderable": false, "targets": [-1] }
    ];

    return CreateDataTable("phoneTable", "/api/GetPhoneDataTableResponseAsync/" + patientId, columns, columnDefs);
}

function AddressDataTable() {
    let columns = [
        { data: "street", title: "Rua" },
        { data: "neighborhood", title: "Bairro" },
        { data: "city", title: "Cidade" },
        { data: "houseNumber", title: "Nº", },
        { data: "complement", title: "Complemento" },
        { data: "residenceType", title: "Residência" },
        { data: "monthlyAmmountResidence", title: "Valor Mensal"},
        { data: "observationAddress", title: "Observação" },
        {
            title: "Ações",
            width: "180px%",
            render: function (data, type, row, meta) {
                let render = '<a href="/Address/EditAddress/' + row.addressId + '" data-toggle="modal" data-target="#modal-action' +
                    '" class="btn btn-secondary"><i class="fas fa-edit"></i></a>';

                render = render.concat(
                    '<a href="/Address/DeleteAddress/' + row.addressId + '" data-toggle="modal" data-target="#modal-action' +
                    '" class="btn btn-danger ml-1"><i class="fas fa-trash-alt"></i></a>'
                );
                return render;
            }
        }
    ];

    let columnDefs = [
        { "orderable": false, "targets": [-1] }
    ];

    return CreateDataTable("addressTable", "/api/GetAddressDataTableResponseAsync/" + patientId, columns, columnDefs);
}

function FamilyMemberDataTable() {
    let columns = [
        { data: "name", title: "Nome" },
        { data: "kinship", title: "Parentesco" },
        { data: "age", title: "Idade" },
        { data: "sex", title: "Gênero" },
        { data: "monthlyIncome", title: "Renda" },
        {
            title: "Ações",
            width: "180px",
            render: function (data, type, row, meta) {
                let render = '<a href="/FamilyMember/EditFamilyMember/' + row.familyMemberId + '" data-toggle="modal" data-target="#modal-action' +
                    '" class="btn btn-secondary"><i class="fas fa-edit"></i></a>';

                render = render.concat(
                    '<a href="/FamilyMember/DeleteFamilyMember/' + row.familyMemberId + '" data-toggle="modal" data-target="#modal-action' +
                    '" class="btn btn-danger ml-1"><i class="fas fa-trash-alt"></i></a>'
                );
                return render;
            }
        }
    ];

    let columnDefs = [
        { "orderable": false, "targets": [-1] }
    ];

    return CreateDataTable("familyMemberTable", "/api/GetFamilyMemberDataTableResponseAsync/" + patientId, columns, columnDefs);
}

function FileAttachmentDataTable() {
    let columns = [
        { data: "archiveCategorie", title: "Categoria arquivo" },
        {
            title: "Categoria arquivo",
            render: function (data, type, row, meta) {
                let anchor = '<a class="fa fa-file" href="/' + row.filePath + '" download="' + row.fileName + '">' + row.fileName + '</a>';
                return anchor;
            }
        },
        {
            title: "Ações",
            width: "20%",
            render: function (data, type, row, meta) {
                let render = 
                    '<a href="/FileAttachment/DeleteFileAttachment/' + row.fileAttachmentId + '" data-toggle="modal" data-target="#modal-action' +
                    '" class="btn btn-danger ml-1"><i class="fas fa-trash-alt"></i></a>';
                return render;
            }
        }
    ];

    let columnDefs = [
        { "orderable": false, "targets": [-1] }
    ];

    return CreateDataTable("attachmentsTable", "/api/GetFileAttachmentsAsync/" + patientId, columns, columnDefs);
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