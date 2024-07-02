var enableNotice = document.getElementById('enable-2fa-notice');
var disableNotice = document.getElementById('disable-2fa-notice');

//Handle Two factor enabled
function onModalTwoFactorBegin() {
    disableSubmitButton($('#TwoFactorModal').find(':submit'));
}

function onModalTwoFactorSuccess(response) {
    isTwoFactorEnabled = response.isTwoFactorEnabled; // Update based on server response
    if (isTwoFactorEnabled) {
        enableNotice.classList.add('d-none');
        disableNotice.classList.remove('d-none');
    } else {
        disableNotice.classList.add('d-none');
        enableNotice.classList.remove('d-none');
    }
    showSuccessMessage();
    $('#TwoFactorModal').modal('hide');
}

function showErrorMessageTwoFactor(response) {
    // Extract the error message from the AJAX response object
    let messageText = response.responseJSON ? response.responseJSON.message : 'Something went wrong!';

    // Display the error message using SweetAlert
    Swal.fire({
        text: messageText,
        icon: "error",
        buttonsStyling: false,
        confirmButtonText: "Ok",
        customClass: {
            confirmButton: "btn btn-danger"
        }
    });
}


document.addEventListener('DOMContentLoaded', function () {

    if (enableNotice && disableNotice) {
        var isTwoFactorEnabled = @Html.Raw(JsonConvert.ToString(isTwoFactorEnabled).ToLower());

        if (isTwoFactorEnabled) {
            enableNotice.classList.add('d-none');
            disableNotice.classList.remove('d-none');
        } else {
            disableNotice.classList.add('d-none');
            enableNotice.classList.remove('d-none');
        }
    } else {
        console.error("Element with ID 'enable-2fa-notice' or 'disable-2fa-notice' not found.");
    }

    // Event delegation using on() method
    $('body').on('click', '.js-disable-two-factor', function () {
        var btn = $(this);

        bootbox.confirm({
            message: btn.data('message'),
            buttons: {
                confirm: {
                    label: 'Yes',
                    className: 'btn-success'
                },
                cancel: {
                    label: 'No',
                    className: 'btn-secondary'
                }
            },
            callback: function (result) {
                if (result) {
                    // Ajax Request 'Post'
                    $.post({
                        url: btn.data('url'),
                        data: {
                            '__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
                        },
                        success: function (response) {
                            isTwoFactorEnabled = response.isTwoFactorEnabled; // Update based on server response
                            if (isTwoFactorEnabled) {
                                enableNotice.classList.add('d-none');
                                disableNotice.classList.remove('d-none');
                            } else {
                                disableNotice.classList.add('d-none');
                                enableNotice.classList.remove('d-none');
                            }
                            showSuccessMessage();
                        },
                        error: function () {
                            showErrorMessage();
                        }
                    });
                }
            }
        });
    });

    $('body').delegate('.js-two-factor', 'click', function () {
        var btn = $(this);
        var modal = $('#TwoFactorModal');

        modal.find('#ModalTitle').text(btn.data('title'));

        $.get({
            url: btn.data('url'),
            success: function (form) {
                modal.find('.modal-body-two-factor').html(form);
                $.validator.unobtrusive.parse(form);
            },
            error: function () {
                showErrorMessage();
            }
        });
        modal.modal('show');
    });

});