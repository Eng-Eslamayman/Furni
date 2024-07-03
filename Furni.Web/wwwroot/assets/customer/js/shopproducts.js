$(document).ready(function () {
    $('.js-pagination').on('click', function () {
        var btn = $(this);
        var pageNumber = btn.data('page-number');

        if (btn.parent().hasClass('active')) return;

        $('#PageNumber').val(pageNumber);
        $('#Filters').submit();
    });
});