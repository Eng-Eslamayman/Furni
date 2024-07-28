$(document).ready(function () {



	// Use Typeahead
	// Initialize Bloodhound suggestion engine
	var products = new Bloodhound({
		datumTokenizer: Bloodhound.tokenizers.obj.whitespace('value'),
		queryTokenizer: Bloodhound.tokenizers.whitespace,
		remote: {
			url: '/Customer/Products/GetCategoriesAndProducts?query=%QUERY',
			wildcard: '%QUERY',
			filter: function (response) {
				return $.map(response, function (item) {
					return {
						value: item.value,
						type: item.type,
						id: item.id || null, // Ensure id is null if not provided
						thumbnailUrl: item.thumbnailUrl || null // Ensure thumbnailUrl is null if not provided
					};
				});
			}
		}
	});

	products.initialize();

	// Initialize Typeahead
	$('#searchTermInput').typeahead(
		{
			hint: true,
			highlight: true,
			minLength: 1
		},
		{
			name: 'categories-products',
			display: 'value',
			source: products.ttAdapter(),
			limit: 15,
			templates: {
				suggestion: function (data) {
					if (data.type === "category") {
						return '<div>' + data.value + '</div>';
					} else if (data.type === "product") {
						return '<div><img src="' + data.thumbnailUrl + '" style="max-width: 50px; max-height: 50px; border-radius: 10%;" > ' + data.value + '</div>';

					}
				}
			}
		}
	).on('typeahead:select', function (event, suggestion) {
		// Handle selection

		if (suggestion.type === "product" && suggestion.id) {
			var productId = suggestion.id;
			window.location.href = '/Customer/Products/Details/' + productId;
		} else {
			var searchTerm = suggestion.value;
			sessionStorage.setItem('searchTerm', searchTerm);
			sessionStorage.setItem('currentPageNumber', 1);
			sessionStorage.setItem('selectedCategoryId', null);
			window.location.href = '/Customer/Products'; // Redirect to search results page
		}
	});


	// Handle form submission
	$('#top-search-form').on('submit', function (event) {
		event.preventDefault(); // Prevent form from submitting the default way
		const searchTerm = $('#searchTermInput').val();
		sessionStorage.setItem('searchTerm', searchTerm);
		sessionStorage.setItem('currentPageNumber', 1);
		sessionStorage.setItem('selectedCategoryId', null);
		window.location.href = '/Customer/Products';
	});

	$('.js-category-Id').on('click', function (event) {
		event.preventDefault();
		const categoryId = $(this).data('category-id');
		sessionStorage.setItem('selectedCategoryId', categoryId);
		sessionStorage.removeItem('searchTerm'); // Clear search term when selecting a category

		window.location.href = '/Customer/Products';
	});

	$('.js-header-all-products').on('click', function (event) {
		event.preventDefault();
		sessionStorage.setItem('selectedCategoryId', null);
		sessionStorage.setItem('currentPageNumber', 1);
		sessionStorage.removeItem('searchTerm'); // Clear search term when viewing all products

		window.location.href = '/Customer/Products';
	});


	// $('.top-search-form').on('submit', function (event) {
	// 	sessionStorage.setItem('selectedCategoryId', null);
	// 	const searchTerm = $('#searchTermInput').val();
	// 	sessionStorage.setItem('searchTerm', searchTerm);
	// 	sessionStorage.setItem('currentPageNumber', 1);
	// 	window.location.href = '/Customer/Products';
	// });


	$('body').on('click', '.js-render-modal', function (event) {
		event.preventDefault(); // Prevent default link behavior

		var btn = $(this);
		var modal = $('#Modal'); // Assuming you have a modal element with this ID

		// Set modal title using data-title
		modal.find('#ModalLabel').text(btn.data('title'));

		// Fetch content using data-url
		$.get(btn.data('url'), function (form) {
			// Update modal body with fetched content
			modal.find('.modal-body').html(form);

			// Show the modal
			modal.modal('show');

			// Setup form submission handler
			modal.find('form').submit(function (e) {
				e.preventDefault(); // Prevent default form submission

				var form = $(this);

				if (form.valid()) { // Check if the form is valid using jQuery Validate
					$.ajax({
						url: form.attr('action'),
						type: 'POST',
						data: form.serialize(),
						success: function (response) {
							if (response.success) {
								// If successful, close modal and show success message
								modal.modal('hide');
								showSuccessMessage(response.message); // Implement this function to show success message
							} else {
								// If not successful, update modal with error messages
								modal.find('.modal-body').html(response);
								$.validator.unobtrusive.parse('#changePasswordForm'); // Re-setup form submission handler if needed
							}
						},
						error: function () {
							showErrorMessage(); // Implement this function to handle generic error
						}
					});
				}
			});

			// Initialize jQuery Unobtrusive Validation
			$.validator.unobtrusive.parse('#changePasswordForm');
		}).fail(function () {
			showErrorMessage(); // Implement your error handling function for modal content retrieval
		});
	});

	$('.js-hide-modal').on('click', function () {
		var modal = $(this).closest('.modal');
		modal.modal('hide');
	});
});