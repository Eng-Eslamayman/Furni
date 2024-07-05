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
});