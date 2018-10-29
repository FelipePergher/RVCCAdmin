$(function () {
    $("#modal-action-patient-details").on('loaded.bs.modal', function (e) {
    }).on('hidden.bs.modal', function (e) {
        $(this).removeData('bs.modal');
    });

    $("#familyMemberTable").DataTable({
        paginate: false,
        filter: false,
        info: false,
        order: [[0, "asc"]],
        columnDefs: [{
            targets: 5,
            orderable: false
        }],
        language: {
            emptyTable: "Nenhuma informação cadastrada!"
        }
    });

    $("#addressTable").DataTable({
        paginate: false,
        filter: false,
        info: false,
        order: [[0, "asc"]],
        columnDefs: [{
            targets: 6,
            orderable: false
        }],
        language: {
            emptyTable: "Nenhuma informação cadastrada!"
        }
    });

    $("#phoneTable").DataTable({
        paginate: false,
        filter: false,
        info: false,
        order: [[0, "asc"]],
        columnDefs: [{
            targets: 3,
            orderable: false
        }],
        language: {
            emptyTable: "Nenhuma informação cadastrada!"
        }
    });

    $("#attachmentsTable").DataTable({
        paginate: false,
        filter: false,
        info: false,
        order: [[0, "asc"]],
        columnDefs: [{
            targets: 2,
            orderable: false
        }],
        language: {
            emptyTable: "Nenhuma informação cadastrada!"
        }
    });
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
