$(document).ready(function () {
    $("form").ajaxForm({
        beforeSubmit: function () {
            $(".alert-message").addClass("hide");
            return $("form").valid();
        },
        success: function (res) {
            if (res.success) {
                $("form").load("/products/success/" + res.id + "/add");
            }
            else {
                var errors = res.errors;
                var groups = {};
                for (var i = 0; i < errors.length; i++) {
                    var error = errors[i];
                    var field = error.field;
                    if (!groups[field]) {
                        groups[field] = {
                            items: []
                        };
                    }
                    groups[field].items.push(error.message);
                }
                $(".alert-message dl").html("");
                for (var g in groups) {
                    var group = groups[g];
                    var items = group.items;
                    var dt = $("<dt></dt>");
                    dt.html(g);
                    $(".alert-message dl").append(dt);
                    for (var i = 0; i < items.length; i++) {
                        var dd = $("<dd></dd>");
                        dd.html(items[i]);
                        $(".alert-message dl").append(dd);
                    }
                }
                $(".alert-message").removeClass("hide");
            }
        }
    });
});