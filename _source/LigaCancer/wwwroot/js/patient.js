$(function () {
    dataTable = BuildDataTable();

    $('#CivilState, #Sex, #CancerType, #Medicines, #Doctors, #TreatmentPlaces').change(function () {
        dataTable.draw();
    });

    BuildSelect2("CancerTypes", "Cânceres");
    BuildSelect2("Medicines", "Remédios");
    BuildSelect2("Doctors", "Médicos");
    BuildSelect2("TreatmentPlaces", "Locais de Tratamento");
    BuildSelect2("CivilState", "Estado Civil");
    BuildSelect2("Sex", "Gênero");
});

function BuildSelect2(elementId, placeholder) {
    $("#" + elementId).select2({
        theme: "bootstrap",
        placeholder: placeholder,
        allowClear: true
    }).on("change", function (e) {
        dataTable.draw();
    });
}

function BuildDataTable() {
    return $("#patientTable").DataTable({
        pageLength: 10,
        processing: true,
        serverSide: true,
        scrollX: true,
        language: language,
        ajax: {
            url: "/api/GetPatientDataTableResponseAsync",
            type: "POST",
            data: function (d) {
                return $.extend({}, d, {
                    civilState: $('#CivilState').val(),
                    Sex: $('#Sex').val(),
                    cancerTypes: $("#CancerTypes").val(),
                    medicines: $("#Medicines").val(),
                    doctors: $("#Doctors").val(),
                    treatmentPlaces: $("#TreatmentPlaces").val()
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
            {
                title: "Nascimento",
                render: function(data, type, row, meta) {
                    let render = row.dateOfBirth !== null ? DateFormat(row.dateOfBirth) : "";
                    return render;
                }
            },
            { data: "sex", title: "Gênero" },
            { data: "civilState", title: "Estado Civil" },
            {
                title: "Grupo de convivência",
                render: function (data, type, row, meta) {
                    let render = row.familiarityGroup ? "Participa" : "Não Participa";
                    return render;
                }
            },
            {
                title: "Profissão",
                render: function (data, type, row, meta) {
                    let render = row.profession !== null ? row.profession.name : "";
                    return render;
                }
            },
            {
                title: "Renda Familiar",
                render: function (data, type, row, meta) {
                    //todo problem here
                    //console.log(row.family.familyIncome);
                    let render = row.family.familyIncome !== 0 ? row.family.familyIncome : "";
                    return render;
                }
            },
            {
                title: "Renda Percapita",
                render: function (data, type, row, meta) {
                    let render = row.family.perCapitaIncome !== 0 ? row.family.perCapitaIncome : "";
                    return render;
                }
            },
            {
                title: "Remédios",
                render: function (data, type, row, meta) {
                    let render = null;
                    jQuery.each(row.patientInformation.patientInformationMedicines, function (index, value) {
                        render = render !== null ? render.concat(", " + value.medicine.name) : value.medicine.name;
                    });
                    return render;
                }
            },
            {
                title: "Cancêres",
                render: function (data, type, row, meta) {
                    let render = null;
                    jQuery.each(row.patientInformation.patientInformationCancerTypes, function (index, value) {
                        render = render !== null ? render.concat(", " + value.cancerType.name) : value.cancerType.name;
                    });
                    return render;
                }
            },
            {
                title: "Locais de Tratamento",
                render: function (data, type, row, meta) {
                    let render = null;
                    jQuery.each(row.patientInformation.patientInformationTreatmentPlaces, function (index, value) {
                        render = render !== null ? render.concat(", " + value.treatmentPlace.city) : value.treatmentPlace.city;
                    });
                    return render;
                }
            },
            {
                title: "Médicos",
                render: function (data, type, row, meta) {
                    let render = null;
                    jQuery.each(row.patientInformation.patientInformationDoctors, function (index, value) {
                        render = render !== null ? render.concat(", " + value.doctor.name) : value.doctor.name;
                    });
                    return render;
                }
            },
            {
                title: "Ações",
                width: "180px",
                render: function (data, type, row, meta) {
                    let render = '<a href="/Patient/DetailsPatient/' + row.patientId + '" class="btn btn-info w-25"><i class="fas fa-info"></i></a>';

                    link = $("#linkEdit");
                    render = render.concat(
                        '<a href="/Patient/EditPatient/' + row.patientId + '" data-toggle="modal" data-target="#modal-action"' +
                        ' class="btn btn-secondary w-25 ml-1"><i class="fas fa-edit"></i></a>'
                    );

                    link = $("#linkDelete");
                    render = render.concat(
                        '<a href="/Patient/DisablePatient/' + row.patientId + '" data-toggle="modal" data-target="#modal-action"' +
                        ' class="btn btn-danger w-25 ml-1"><i class="fas fa-trash-alt"></i></a>'
                    );
                    return render;
                }
            }
        ],
        columnDefs: [
            { "orderable": false, "targets": [4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15] },
            { "searchable": false, "targets": [4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15] }
        ]
    });
}
