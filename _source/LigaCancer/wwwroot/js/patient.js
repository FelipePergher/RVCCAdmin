$(function () {
    dataTable = BuildDataTable();

    $("#CivilState, #Sex, #CancerType, #Medicines, #Doctors, #TreatmentPlaces, #Discharge, #Death").change(function () {
        dataTable.draw();
    });

    BuildSelect2("CancerTypes", "Cânceres");
    BuildSelect2("Medicines", "Remédios");
    BuildSelect2("Doctors", "Médicos");
    BuildSelect2("TreatmentPlaces", "Locais de Tratamento");
    BuildSelect2("CivilState", "Estado Civil", true);
    BuildSelect2("Sex", "Gênero", true);
    BuildSelect2("FamiliarityGroup", "Grupo de convivência", true);
});

function BuildSelect2(elementId, placeholder, allowClear = false) {
    $("#" + elementId).select2({
        theme: "bootstrap",
        placeholder: placeholder,
        allowClear: allowClear,
        language: languageSelect2
    }).on("change", function (e) {
        dataTable.draw();
    });
}

function BuildDataTable() {
    return $("#patientTable").DataTable({
        pageLength: 10,
        processing: true,
        searching:false,
        serverSide: true,
        scrollX: true,
        language: language,
        ajax: {
            url: "/api/patient/GetPatientDataTableResponseAsync",
            type: "POST",
            data: function (d) {
                return $.extend({}, d, {
                    civilState: $("#CivilState").val(),
                    Sex: $("#Sex").val(),
                    cancerTypes: $("#CancerTypes").val(),
                    medicines: $("#Medicines").val(),
                    doctors: $("#Doctors").val(),
                    treatmentPlaces: $("#TreatmentPlaces").val(),
                    familiarityGroup: $("#FamiliarityGroup").val(),
                    death: $("#Death").is(":checked"), 
                    discharge: $("#Discharge").is(":checked")
                });
            },
            error: errorDataTable
        },
        order: [[1, "asc"]],
        columns: [
            {
                title: "Ações",
                width: "300px",
                render: function (data, type, row, meta) {
                    if ($("#Discharge").is(":checked")) {
                        //Todo if discharge appear to reativate
                        return '<a href="/Patient/ActivePatient/' + row.patientId + '" class="btn btn-primary w-100" data-toggle="modal" data-target="#modal-action">Reativar</a> ';
                    }
                    if ($("#Death").is(":checked")) {
                        return "";
                    }
                    let render = '<a href="/Patient/DetailsPatient/' + row.patientId + '" class="btn btn-info w-40"><i class="fas fa-info"></i> Detalhes </a>';

                    link = $("#linkEdit");
                    render = render.concat(
                        '<a href="/Patient/EditPatient/' + row.patientId + '" data-toggle="modal" data-target="#modal-action"' +
                        ' class="btn btn-secondary w-40 ml-1"><i class="fas fa-user-edit"></i> Editar </a>'
                    );

                    //link = $("#linkDelete");
                    //render = render.concat(
                    //    '<a href="/Patient/DisablePatient/' + row.patientId + '" data-toggle="modal" data-target="#modal-action"' +
                    //    ' class="btn btn-danger w-40 ml-1"><i class="fas fa-user-times"></i>  Desabilitar </a>'
                    //);
                    return render;
                }
            },
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
            { title: "Profissão", data: "profession" },
            {
                title: "Renda Familiar",
                render: function (data, type, row, meta) {
                    let render = row.family.familyIncome !== 0 ? "$" + row.family.familyIncome.toFixed(2) : "";
                    return render;
                }
            },
            {
                title: "Renda Percapita",
                render: function (data, type, row, meta) {
                    let render = row.family.perCapitaIncome !== 0 ? "$" +  row.family.perCapitaIncome.toFixed(2) : "";
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
                title: "Cânceres",
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
            }
            
        ],
        columnDefs: [
            { "orderable": false, "targets": [4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15] },
            { "searchable": false, "targets": [4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15] }
        ],
        dom: "l<'mr-3'B>frtip",
        buttons:
            [
                {
                    extend: 'pdf',
                    orientation: 'landscape',
                    pageSize: 'LEGAL',
                    className: 'btn btn-info',
                    exportOptions: {
                        columns: 'th:not(:first-child)',
                    },
                    customize: function (doc) {
                        doc.defaultStyle.alignment = 'center';
                        doc.styles.tableHeader.alignment = 'center';
                    }  
                },
                {
                    extend: 'excel',
                    className: 'btn btn-info',
                    exportOptions: {
                        columns: 'th:not(:first-child)'
                    }
                },
            ],
    });
}
