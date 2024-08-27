$(document).ready(function () {
    // Get the current page from the URL
    const urlParams = new URLSearchParams(window.location.search);
    const currentPage = urlParams.get('PageNumber') || 1;

    // Remove 'active' class from all page items
    $('.pagination .page-item').removeClass('active');

    // Find the page item with the matching data-page attribute and add the 'active' class
    $('.pagination .page-link').each(function () {
        if ($(this).data('page') == currentPage) {
            $(this).closest('.page-item').addClass('active');
        }
    });
});