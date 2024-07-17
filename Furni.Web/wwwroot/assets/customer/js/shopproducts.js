$(document).ready(function () {
    // Initialize variables to store current state
    let selectedCategoryId = sessionStorage.getItem('selectedCategoryId');
    //if (selectedCategoryId === 'null') selectedCategoryId = null; // Ensure it's null if set to 'null'
    let currentPageNumber = sessionStorage.getItem('currentPageNumber') || 1;
    let searchTerm = sessionStorage.getItem('searchTerm') || '';

    // Function to load products based on category, page number, and search term
    function loadProducts(categoryId, pageNumber, searchTerm) {
        // Show loading indicator
        $('#productsListContainer').html('<div class="loader-container"><div class="loader"></div></div>');

        $.ajax({
            url: '/Customer/Products/GetProducts',
            type: 'GET',
            data: { categoryId: categoryId, pageNumber: pageNumber, searchTerm: searchTerm },
            success: function (data) {
                $('#productsListContainer').html(data);
                // Update current state in sessionStorage
                sessionStorage.setItem('selectedCategoryId', categoryId);
                sessionStorage.setItem('currentPageNumber', pageNumber);
                sessionStorage.setItem('searchTerm', searchTerm);

                // Debug statements
                console.log('After searching and result selectedCategoryId:', sessionStorage.getItem('selectedCategoryId'));
                console.log('After searching and result searchTerm:', sessionStorage.getItem('searchTerm'));
            },
            error: function (xhr, status, error) {
                $('#productsListContainer').html('<p>Error loading products. Please try again later.</p>');
            }
        });
    }

    // Event delegation for category selection
    $('body').on('click', '.js-category-value', function () {
        selectedCategoryId = $(this).data('category-id');
        currentPageNumber = 1; // Reset to first page when selecting a new category
        searchTerm = ''; // Clear search term when selecting a category
        loadProducts(selectedCategoryId, currentPageNumber, searchTerm);
    });

    // Event delegation for pagination
    $('body').on('click', '.js-pagination-number', function () {
        var pageNumber = $(this).data('page-number');
        loadProducts(selectedCategoryId, pageNumber, searchTerm);
    });

    // Event delegation for "All Products" link
    $('body').on('click', '.js-all-products', function () {
        selectedCategoryId = null; // Set categoryId to null for all products
        currentPageNumber = 1; // Reset to first page when viewing all products
        searchTerm = ''; // Clear search term when viewing all products
        loadProducts(selectedCategoryId, currentPageNumber, searchTerm);
    });

     //Event delegation for search form submission
    $('.top-search-form').on('submit', function (event) {
        event.preventDefault();
        searchTerm = $('#searchTermInput').val();
        currentPageNumber = 1; // Reset to first page when performing a new search
        selectedCategoryId = null; // Clear category when searching
        sessionStorage.setItem('searchTerm', searchTerm);
        sessionStorage.setItem('selectedCategoryId', selectedCategoryId);
        sessionStorage.setItem('currentPageNumber', currentPageNumber);
        window.location.href = '/Customer/Products';
    });

    // Initial load of products when page loads
    if (selectedCategoryId || currentPageNumber || searchTerm) {
        loadProducts(selectedCategoryId, currentPageNumber, searchTerm);
    }
});
