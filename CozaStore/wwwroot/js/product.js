let productIdGlb = 0;
let defaultSizeList = [];
let defaultColorList = [];
function GetProductDetail(productId) {
    productIdGlb = productId;
    console.log(productIdGlb);
    $.ajax({
        url: '/shop/GetProductDetail',
        method: 'GET',
        data: { id: productId },
        success: function (data) {
            console.log(data);
            $('.js-name-detail').text(data.name);
            $('.product-price').text('$' + data.priceSell);
            $('.product-description').html(data.description); // important @Html.Raw

            // Populate size dropdown
            var sizeOptions = '<option value="" selected>Choose an option</option>';
            data.sizeList.forEach(function (size) {
                sizeOptions += `<option value="${size.id}">${size.name}</option>`;
            });
            defaultSizeList = data.sizeList;
            $('.size-select').html(sizeOptions);

            // Populate color dropdown
            var colorOptions = '<option value="" selected>Choose an option</option>';
            data.colorList.forEach(function (color) {
                colorOptions += `<option value="${color.id}">${color.name}</option>`;
            });
            defaultColorList = data.colorList;
            $('.color-select').html(colorOptions);

            $('.slick3').html('');
            data.imageList.forEach(function (image) {
                var imgHtml = `<div class="item-slick3" data-thumb="${image}">
                        <div class="wrap-pic-w pos-relative">
                                <img src="${image}" alt="IMG-PRODUCT">
                            <a class="flex-c-m size-108 how-pos1 bor0 fs-16 cl10 bg0 hov-btn3 trans-04" href="${image}">
                                <i class="fa fa-expand"></i>
                            </a>
                        </div>
                    </div>`;
                $('.slick3').append(imgHtml);
            });
        },
        error: function () {
            alert('Failed to load product details');
        }
    });
}

let selectedSize = '';
let selectedColor = '';

$(document).ready(function () {
    // handle size change
    $('.size-select').on('change', function () {
        selectedSize = $(this).val();
        console.log("click size day la colorId: " + selectedColor);

        if (!selectedSize) {
            refeshSelect();
            return;
        }

        if (selectedColor && selectedSize) {
            getPrice(selectedSize, selectedColor);
            return;
        }

        if (!selectedColor) {
            $.ajax({
                url: `/shop/GetAvailableColors`,
                method: 'GET',
                data: { productId: productIdGlb, sizeId: selectedSize },
                success: function (data) {
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
        } 
    });

    // handle color change
    $('.color-select').on('change', function () {
        selectedColor = $(this).val();
        console.log("click color day la sizeId: " + selectedSize);

        if (!selectedColor) {
            refeshSelect();
            return;
        }

        if (selectedColor && selectedSize) {
            getPrice(selectedSize, selectedColor);
            return;
        }

        if (!selectedSize) {
            $.ajax({
                url: `/shop/GetAvailableSizes`,
                method: 'GET',
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
        }
    });
});

function getPrice(sizeId, colorId) {
    console.log(productIdGlb, sizeId, colorId)
    $.ajax({
        url: `/shop/GetPrice`,
        method: 'GET',
        data: {
            sizeId: sizeId,
            colorId: colorId,
            productId: productIdGlb
        },
        success: function (data) {
            console.log(data);
            if (data.success === false) {
                toastr.error(data.message);
                return;
            }
            
            $('.product-price').text('$' + data);
        },
        error: function () {
            alert('Failed to load price');
        }
    });
}

function refeshSelect() {
    selectedColor = '';
    selectedSize = '';

    //size
    $('.size-select').html('');
    var sizeOptions = '<option value="" selected>Choose an option</option>';
    defaultSizeList.forEach(function (size) {
        sizeOptions += `<option value="${size.id}">${size.name}</option>`;
    });
    $('.size-select').html(sizeOptions);
    selectedColor = '';

    //color
    $('.color-select').html('');
    var colorOptions = '<option value="" selected>Choose an option</option>';
    defaultColorList.forEach(function (color) {
        colorOptions += `<option value="${color.id}">${color.name}</option>`;
    });
    $('.color-select').html(colorOptions);
}

