




$(document).ready(function () {

    // Disabled Submit button
    $('form').not('#SignOut').not('.js-excluded-validation').on('submit', function () {
        //disabledSubmitButton($('#Modal').find(':submit'));

        if ($('.js-tinymce').length > 0) {
            $('.js-tinymce').each(function () {
                var input = $(this);

                var content = tinyMCE.get(input.attr('id')).getContent();
                input.val(content);
            });
        }

        var isValid = $(this).valid();
        if (isValid) disabledSubmitButton($(this).find(':submit'));
    });


    // Datepicker
    $('.js-datepicker').daterangepicker({
        singleDatePicker: true,
        autoApply: true,
        drops: 'up',
        showDropdowns: true,
        minYear: 1901,
        locale: {
            format: 'DD/MM/YYYY'
        },
        maxDate: new Date()
    });


    // Tinymce
    if ($('.js-tinymce').length > 0) {
        var options = {
            selector: ".js-tinymce",
            height: "430",
            toolbar: ["styleselect fontselect fontsizeselect",
                "undo redo | cut copy paste | bold italic | link image | alignleft aligncenter alignright alignjustify",
                "bullist numlist | outdent indent | blockquote subscript superscript | advlist | autolink | lists charmap | print preview |  code | forecolor backcolor emoticons"],
            plugins: "advlist autolink link image lists charmap print preview code"
        };

        if (KTThemeMode.getMode() === "dark") {
            options["skin"] = "oxide-dark";
            options["content_css"] = "dark";
        }


        tinymce.init(options);
    }

    // Handle SignOut
    $('.js-signout').on('click', function () {
        $('#SignOut').submit();
        //$(this).parent().submit();
    });
});