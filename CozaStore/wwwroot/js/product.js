let productIdGlb = 0;
let selectedSize = '';
let selectedColor = '';

function GetProductDetail(productId) {
    productIdGlb = productId;
    console.log(productIdGlb);
    $.ajax({
        url: '/shop/GetProductDetail',
        method: 'post',
        data: { id: productId },
        success: function (data) {
            $("#product-detail").html(data);
            $('#modal-product').addClass('show-modal1');
            initModalScripts();
        },
        error: function () {
            alert('Failed to load product details');
        }
    });
}

function initModalScripts() {
    $('.gallery-lb').each(function () { // the containers for all your galleries
        $(this).magnificPopup({
            delegate: 'a', // the selector for gallery item
            type: 'image',
            gallery: {
                enabled: true
            },
            mainClass: 'mfp-fade mfp-with-zoom'
        });
    });

    // Khởi tạo lại slick slider
    $('.wrap-slick3').each(function () {
        $(this).find('.slick3').slick({
            slidesToShow: 1,
            slidesToScroll: 1,
            fade: true,
            infinite: true,
            autoplay: false,
            autoplaySpeed: 6000,

            arrows: true,
            appendArrows: $(this).find('.wrap-slick3-arrows'),
            prevArrow: '<button class="arrow-slick3 prev-slick3"><i class="fa fa-angle-left" aria-hidden="true"></i></button>',
            nextArrow: '<button class="arrow-slick3 next-slick3"><i class="fa fa-angle-right" aria-hidden="true"></i></button>',

            dots: true,
            appendDots: $(this).find('.wrap-slick3-dots'),
            dotsClass: 'slick3-dots',
            customPaging: function (slick, index) {
                var portrait = $(slick.$slides[index]).data('thumb');
                return '<img src=" ' + portrait + ' "/><div class="slick3-dot-overlay"></div>';
            },
        });
    });
    initMainScripts();
    refeshSelect();
}
function initMainScripts() {
    $('.js-hide-modal1').on('click', function () {
        $('.js-modal1').removeClass('show-modal1');
    });

    $('.btn-num-product-down').on('click', function () {
        var numProduct = Number($(this).next().val());
        if (numProduct > 0) $(this).next().val(numProduct - 1);
    });

    $('.btn-num-product-up').on('click', function () {
        var numProduct = Number($(this).prev().val());
        $(this).prev().val(numProduct + 1);
    });
}



$(document).on('click', '.btn-add-cart', function () {
    //get value
    const sizeId = $('.size-select').val();
    const colorId = $('.color-select').val();
    const count = $('.num-product').val();


    $.ajax({
        url: '/shop/addtocart',
        method: 'post',
        data: {
            productId: productIdGlb,
            sizeId: sizeId,
            colorId: colorId,
            count: count,
        },
        success: function (response) {
            if (response.success === true) {
                if (response.add === true) {
                    const currentCount = parseInt($('.icon-shopping-cart.icon-header-noti').attr('data-notify')) || 0;
                    const newCount = currentCount + 1;
                    $('.icon-shopping-cart.icon-header-noti').attr('data-notify', newCount);
                }
                toastr.success(response.message);
            } else {
                toastr.error(response.message);
            }
        },
        error: function (xhr, status, error) {
            console.log("error: " + error.message);
            if (xhr.status === 401) {
                toastr.error("You are not logged in. Please log in to add items to your cart.");
            } else if (xhr.status === 403) {
                toastr.error("You do not have permission to add products to cart. Please contact admin for more details.");
            } else {
                toastr.error("Adding product to cart error please try again later");
            }
        }
    })
});

$(document).on('change', '.size-select', async function () {
    selectedSize = $(this).val();

    if (selectedColor && selectedSize) {
        const success = await getPrice(selectedSize, selectedColor, "size");
        if (success) {
            return; // Exit early if `getPrice` returns `true`
        }
    }

    $.ajax({
        url: `/shop/GetAvailableColors`,
        method: 'post',
        data: { productId: productIdGlb, sizeId: selectedSize },
        success: function (data) {
            console.log(data);
            $('.color-select').html('');
            var colorOptions = '<option value="" selected>Choose an option</option>';
            data.forEach(function (color) {
                colorOptions += `<option value="${color.id}">${color.name}</option>`;
            });
            $('.color-select').html(colorOptions);
        },
        error: function () {
            alert('Failed to GetAvailableColors');
        }
    });
});


$(document).on('change', '.color-select', async function () {
    selectedColor = $(this).val();

    if (selectedColor && selectedSize) {
        const success = await getPrice(selectedSize, selectedColor, "color");
        if (success) {
            return; // Stop further execution if success is true
        }
    }

    $.ajax({
        url: `/shop/GetAvailableSizes`,
        method: 'post',
        data: { productId: productIdGlb, colorId: selectedColor },
        success: function (data) {
            $('.size-select').html('');
            // Populate size dropdown
            var sizeOptions = '<option value="" selected>Choose an option</option>';
            data.forEach(function (size) {
                sizeOptions += `<option value="${size.id}">${size.name}</option>`;
            });
            $('.size-select').html(sizeOptions);
        },
        error: function () {
            alert('Failed to GetAvailableSizes');
        }
    });
});



function getPrice(sizeId, colorId, currentSelect) {
    console.log(productIdGlb, sizeId, colorId);
    return $.ajax({
        url: `/shop/GetPrice`,
        method: 'post',
        data: {
            sizeId: sizeId,
            colorId: colorId,
            productId: productIdGlb,
            currentSelect: currentSelect
        }
    }).then(function (data) {
        console.log(data);
        if (data.success === false) {
            if (data.select === "color") { // getsize (variant null)
                $('.size-select').html("");
            }

            if (data.select === "size") { //getcolor (variant null)
                $('.color-select').html("");
            }
            return false;
        }

        $('.product-price').text('$' + data);
        return true;
    }).catch(function () {
        alert('Failed to load price');
        return false;
    });
}

function refeshSelect() {
    selectedColor = '';
    selectedSize = '';
}
