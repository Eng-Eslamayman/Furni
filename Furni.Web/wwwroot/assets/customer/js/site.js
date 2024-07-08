﻿function showSuccessMessage(message = 'Saved successfully!') {
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

$(document).ready(function () {
	$('.js-category-Id').on('click', function (event) {
		event.preventDefault();
		const categoryId = $(this).data('category-id');
		sessionStorage.setItem('selectedCategoryId', categoryId);
		window.location.href = '/Customer/Products';
	});

	$('.js-header-all-products').on('click', function (event) {
		event.preventDefault();
		sessionStorage.setItem('selectedCategoryId', null);
		sessionStorage.setItem('currentPageNumber', 1);
		window.location.href = '/Customer/Products';
	});


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

	$('body').on('click', '.remove-item', function (e) {
		e.preventDefault();
		var item = $(this);
		var productId = item.data('remove-card');
		removeCard(productId);
	});


});