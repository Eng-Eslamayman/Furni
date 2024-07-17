function disableSubmitButton(btn) {
	$(btn).attr('disabled', 'disabled');
}

function onModalBegin() {
	disableSubmitButton($('#Modal').find(':submit'));
}



function showSuccessMessage(message = 'Saved successfully!') {
	Swal.fire({
		text: message,
		icon: "success",
		buttonsStyling: false,
		confirmButtonText: "Ok, got it!",
		customClass: {
			confirmButton: "btn btn-success"
		}
	});
}

function showErrorMessage(message = 'Something went wrong!') {
	Swal.fire({
		text: message,
		icon: "error",
		buttonsStyling: false,
		confirmButtonText: "Ok",
		customClass: {
			confirmButton: "btn btn-danger"
		}
	});
}


function onModalComplete() {
	$('body :submit').removeAttr('disabled');
}


function onModalSuccess() {
	//showSuccessMessage();
	//$('#Modal').modal('hide');
}

$(document).ready(function () {
	



	

	function loadCarts(){
		$.ajax({
			url: '/Customer/Carts/GetCards',
			type: 'GET',
			success: function (data) {
				$('#show-all-cards').html(data);
			},
			error: function () {
				alert('Error loading cards.');
			}
		});
	}

	$('body').on('click', '.js-show-cards', function () {
		loadCarts();
	});

	$('.js-signout').on('click', function () {
		$('#SignOut').submit();
		//$(this).parent().submit();
	});



	function removeCard(productId) {
		// User is authenticated, proceed with adding to cart
		$.ajax({
			url: '/Customer/Carts/RemoveFromCart',
			type: 'POST',
			data: {
				productId: productId,
				'__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
			},
			success: function (data) {
				// Handle success response
				$('#show-all-cards').html(data);
			},
			error: function () {
				showErrorMessage();
			}
		});
	}

	function increaseCard(productId) {
		// User is authenticated, proceed with adding to cart
		$.ajax({
			url: '/Customer/Carts/Increase',
			type: 'POST',
			data: {
				productId: productId,
				'__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
			},
			success: function (data) {
				// Handle success response
				$('#show-all-cards').html(data);
			},
			error: function () {
				showErrorMessage();
			}
		});
	}

	function decreaseCard(productId) {
		// User is authenticated, proceed with adding to cart
		$.ajax({
			url: '/Customer/Carts/Decrease',
			type: 'POST',
			data: {
				productId: productId,
				'__RequestVerificationToken': $('input[name="__RequestVerificationToken"]').val()
			},
			success: function (data) {
				// Handle success response
				$('#show-all-cards').html(data);
			},
			error: function () {
				showErrorMessage();
			}
		});
	}

	$('body').on('click', '.js-remove-item', function (e) {
		e.preventDefault();
		var item = $(this);
		var productId = item.data('remove-card');
		removeCard(productId);
	});

	$('body').on('click', '.js-increase-item', function (e) {
		e.preventDefault();
		var item = $(this);
		var productId = item.data('increase-card');
		increaseCard(productId);
	});

	$('body').on('click', '.js-decrease-item', function (e) {
		e.preventDefault();
		var item = $(this);
		var productId = item.data('decrease-card');
		decreaseCard(productId);
	});


});