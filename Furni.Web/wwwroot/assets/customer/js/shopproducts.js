$(document).ready(function () {
    // Initialize variables to store current state
    let selectedCategoryId = sessionStorage.getItem('selectedCategoryId');
    //if (selectedCategoryId === 'null') selectedCategoryId = null; // Ensure it's null if set to 'null'
    let currentPageNumber = sessionStorage.getItem('currentPageNumber') || 1;
    let searchTerm = sessionStorage.getItem('searchTerm') || '';
    let sortCriteria = sessionStorage.getItem('sortCriteria') || 'default';

    // Function to load products based on category, page number, and search term
    function loadProducts(categoryId, pageNumber, searchTerm, sortCriteria) {
        // Show loading indicator
        $('#productsListContainer').html('<div class="loader-container"><div class="loader"></div></div>');

        $.ajax({
            url: '/Customer/Products/GetProducts',
            type: 'GET',
            data: { categoryId: categoryId, pageNumber: pageNumber, searchTerm: searchTerm, sortCriteria: sortCriteria },
            success: function (data) {
                $('#productsListContainer').html(data);
                // Update current state in sessionStorage
                sessionStorage.setItem('selectedCategoryId', categoryId);
                sessionStorage.setItem('currentPageNumber', pageNumber);
                sessionStorage.setItem('searchTerm', searchTerm);
                sessionStorage.setItem('sortCriteria', sortCriteria);
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
        sortCriteria = 'default'; // Reset sort criteria when selecting a category
        loadProducts(selectedCategoryId, currentPageNumber, searchTerm, sortCriteria);
    });

    // Event delegation for pagination
    $('body').on('click', '.js-pagination-number', function () {
        var pageNumber = $(this).data('page-number');
        loadProducts(selectedCategoryId, pageNumber, searchTerm, sortCriteria);
    });

    // Event delegation for "All Products" link
    $('body').on('click', '.js-all-products', function () {
        selectedCategoryId = null; // Set categoryId to null for all products
        currentPageNumber = 1; // Reset to first page when viewing all products
        searchTerm = ''; // Clear search term when viewing all products
        sortCriteria = 'default'; // Reset sort criteria when viewing all products
        loadProducts(selectedCategoryId, currentPageNumber, searchTerm, sortCriteria);
    });

     //Event delegation for search form submission
    $('.top-search-form').on('submit', function (event) {
        event.preventDefault();
        searchTerm = $('#searchTermInput').val();
        currentPageNumber = 1; // Reset to first page when performing a new search
        selectedCategoryId = null; // Clear category when searching
        sortCriteria = 'default'; // Reset sort criteria when performing a new search
        sessionStorage.setItem('searchTerm', searchTerm);
        sessionStorage.setItem('selectedCategoryId', selectedCategoryId);
        sessionStorage.setItem('currentPageNumber', currentPageNumber);
        window.location.href = '/Customer/Products';
    });

    // Event delegation for sorting options
    $('body').on('click', '.dropdown-item[data-sort]', function () {
        sortCriteria = $(this).data('sort');
        loadProducts(selectedCategoryId, currentPageNumber, searchTerm, sortCriteria);
    });

    // Initial load of products when page loads
    if (selectedCategoryId || currentPageNumber || searchTerm || sortCriteria) {
        loadProducts(selectedCategoryId, currentPageNumber, searchTerm, sortCriteria);
    }
});
