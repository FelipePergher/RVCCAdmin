function setupPhoneMaskOnField(selector) {
    var inputElement = $(selector);

    setCorrectPhoneMask(inputElement);
    inputElement.on('input, keyup', function () {
        setCorrectPhoneMask(inputElement);
    });
}

var SPMaskBehavior = function (val) {
    return val.replace(/\D/g, '').length === 11 ? '(00) 00000-0000' : '(00) 0000-00009';
}, spOptions = {
        onKeyPress: function (val, e, field, options) {
            field.mask(SPMaskBehavior.apply({}, arguments), options);
        }
    };

//Secondary Modal Functions
function openModalSecondary(url, title, callback = null) {
    setTimeout(function () {
        let overlaySecondModal = $(".modal-backdrop.fade.show");
        if (overlaySecondModal.length === 2) $(overlaySecondModal[1]).css("z-index", 1050);
    }, 10);

    $("#modal-title-secondary").text(title);

    $("#modalBodySecondary").load(url, function () {
        if (callback !== null) callback();
        hideSpinnerModalSecondary();
    });

    $("#modal-action-secondary").on("hidden.bs.modal", function (e) {
        cleanModalSecondary();
    });
}

function cleanModalSecondary() {
    $("#modalBodySecondary").html("");
    $("#modal-title-secondary").text("");
    showSpinnerModalSecondary();
    setTimeout(function () {
        $(".swal2-confirm").focus();
    }, 10);
}

function showSpinnerModalSecondary() {
    $("#modalSpinnerSecondary").show();
}

function hideSpinnerModalSecondary() {
    $("#modalSpinnerSecondary").hide();
}
