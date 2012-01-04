$(document).ready(function () {
    $(".btn.info").click(function () {
        $("form").get(0).submit();
        return false;
    });
});