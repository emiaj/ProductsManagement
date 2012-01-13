$(document).ready(function () {
    $("form").validate({
        meta: "rules",
//        showErrors: function (errorMap, errorList) {
//            for (var i = 0; errorList[i]; i++) {
//                var element = this.errorList[i].element;
//                var idOrName = this.idOrName(element);
//                var form = this.currentForm;
//                var errorsFor = $("*", form).filter(function () {
//                    return $(this).attr("for") == idOrName;
//                });
//                errorsFor.each(function() { $(this).remove(); });
//            }
//            this.defaultShowErrors();
//        },
        errorContainer: ".alert-message.error",
        errorElement: "dd",
        errorPlacement: function (error, element) {
            var localizedName = element.metadata().localizedName;
            var dt = $(".alert-message.error dl dt[data-target='" + localizedName + "']");
            if (dt.size() == 0) {
                var idOrName = $("form").data("validator").idOrName($(element).get(0));
                dt = $("<dt></dt>");
                dt.attr("data-target", localizedName);
                dt.html(localizedName);
                dt.attr("for", idOrName);
                $(".alert-message.error dl").append(dt);
            }
            var next = dt.nextUntil("dt");
            if (next.size() == 0) {
                $(".alert-message.error dl").append(error);
            }
            else {
                next.after(error);
            }
        }
    });
});