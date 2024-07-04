$(document).ready(function () {
    // Initialize variables to store current state
    let selectedCategoryId = sessionStorage.getItem('selectedCategoryId');
    let currentPageNumber = sessionStorage.getItem('currentPageNumber') || 1;

    // Function to load products based on category and page number
    function loadProducts(categoryId, pageNumber) {
        // Show loading indicator
        $('#productsListContainer').html('<div class="loader-container"><div class="loader"></div></div>');

        $.ajax({
            url: '/Customer/Products/GetProducts',
            type: 'GET',
            data: { categoryId: categoryId, pageNumber: pageNumber },
            success: function (data) {
                $('#productsListContainer').html(data);
                // Update current state in sessionStorage
                sessionStorage.setItem('selectedCategoryId', categoryId);
                sessionStorage.setItem('currentPageNumber', pageNumber);
            },
            error: function () {
                alert('Error loading products.');
            }
        });
    }

    // Event delegation for category selection
    $('body').on('click', '.js-category-value', function () {
        selectedCategoryId = $(this).data('category-id');
        currentPageNumber = 1; // Reset to first page when selecting a new category
        loadProducts(selectedCategoryId, currentPageNumber);
    });

    // Event delegation for pagination
    $('body').on('click', '.js-pagination-number', function () {
        var pageNumber = $(this).data('page-number');
        loadProducts(selectedCategoryId, pageNumber);
    });

    // Event delegation for "All Products" link
    $('body').on('click', '.js-all-products', function () {
        selectedCategoryId = null; // Set categoryId to null for all products
        currentPageNumber = 1; // Reset to first page when viewing all products
        loadProducts(selectedCategoryId, currentPageNumber);
    });

    // Initial load of products when page loads
    if (selectedCategoryId) {
        loadProducts(selectedCategoryId, currentPageNumber);
    }
});
