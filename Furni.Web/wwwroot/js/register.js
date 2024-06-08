$(document).ready(function () {
    $('#email').on('change', function () {
        var email = $(this).val();

        $.ajax({
            type: "POST",
            url: '/Identity/Account/Register?handler=AllowEmail&email=' + encodeURIComponent(email),
            headers: {
                'X-CSRF-TOKEN': $('input[name="__RequestVerificationToken"]').val()
            },
            success: function (data) {
                if (!data) {
                    // Email unavailable, update UI
                    $('#email-availability').text('Email is already in use.');
                    $('#submit-button').attr('disabled', true); // Optional: Disable submit button
                } else {
                    // Email available, update UI
                    $('#email-availability').text('Email is available.');
                    $('#submit-button').attr('disabled', false); // Optional: Enable submit button
                }
            },
            error: function (jqXHR, textStatus, errorThrown) {
                console.error('Error checking email:', textStatus, errorThrown);
                // Display a user-friendly error message
                $('#email-availability').text('An error occurred. Please try again later.');
            }
        });
    });
});