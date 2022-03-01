"use strict";
import "jquery-validation";
import "jquery-validation-unobtrusive";
import "jquery-mask-plugin";

export default (function () {

    global.setupValidator($.validator);

    $(function () {
        initPage();
    });

    function initPage() {
        initForm();
    }

    function initForm() {
        $("#MinSalary").mask(global.masks.Price, { reverse: true });

        $("#adminForm").off("submit").submit(function (e) {
            let form = $(this);
            if (form.valid()) {
                let submitButton = $(this).find("button[type='submit']");
                $(submitButton).prop("disabled", "disabled").addClass("disabled");
                $("#submitSpinner").show();
            }
        });
    }
}());