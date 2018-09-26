﻿(function ($) {
    function doctor() {
        var $this = this;

        function initilizeModel() {
            $("#modal-action-doctor").on('loaded.bs.modal', function (e) {

            }).on('hidden.bs.modal', function (e) {
                $(this).removeData('bs.modal');
            });
        }
        $this.init = function () {
            initilizeModel();
        };
    }
    $(function () {
        var self = new doctor();
        self.init();
    });
}(jQuery));


$("#modal-action-doctor").on("show.bs.modal", function (e) {
    var link = $(e.relatedTarget);
    $(this).find(".modal-content").load(link.attr("href"), function (e) {
        $.validator.unobtrusive.parse('form');
    });

});

function AjaxError($xhr) {
    swal("Oops...", "Something went wrong!\nThe server response was:\n\n'" + $xhr.statusText + "'", "error");
}

function AjaxSuccess(data) {
    if (data === "200") {
        location.reload();
    }
    else {
        $("#modal-content").html(data);
    }
}