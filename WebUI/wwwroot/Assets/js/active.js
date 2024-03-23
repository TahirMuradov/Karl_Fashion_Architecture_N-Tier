(function ($) {
    'use strict';

    // :: 1.0 Fullscreen Active Code
    // :: 2.0 Welcome Slider Active Code
    // :: 3.0 Related Product Active Code
    // :: 4.0 Testimonials Slider Active Code
    // :: 5.0 Gallery Menu Style Active Code
    // :: 6.0 Masonary Gallery Active Code
    // :: 7.0 Header Cart btn Active Code
    // :: 8.0 Side Menu Active Code
    // :: 9.0 Magnific-popup Video Active Code
    // :: 10.0 ScrollUp Active Code
    // :: 11.0 Slider Range Price Active Code
    // :: 12.0 PreventDefault a Click
    // :: 13.0 wow Active Code
    // :: 14.0 custom js in backend 
    var $window = $(window);

    // :: 1.0 Fullscreen Active Code
    $window.on('resizeEnd', function () {
        $(".full_height").height($window.height());
    });

    $window.on('resize', function () {
        if (this.resizeTO) clearTimeout(this.resizeTO);
        this.resizeTO = setTimeout(function () {
            $(this).trigger('resizeEnd');
        }, 300);
    }).trigger("resize");

    var welcomeSlide = $('.welcome_slides');

    // :: 2.0 Welcome Slider Active Code
    if ($.fn.owlCarousel) {
        welcomeSlide.owlCarousel({
            items: 1,
            margin: 0,
            loop: true,
            nav: false,
            dots: true,
            autoplay: true,
            autoplayTimeout: 7000,
            smartSpeed: 1000
        });
    }

    // :: 3.0 Related Product Active Code
    if ($.fn.owlCarousel) {
        $('.you_make_like_slider').owlCarousel({
            items: 3,
            margin: 30,
            loop: true,
            nav: false,
            dots: true,
            autoplay: true,
            autoplayTimeout: 7000,
            smartSpeed: 1000,
            responsive: {
                0: {
                    items: 1
                },
                576: {
                    items: 2
                },
                768: {
                    items: 3
                }
            }
        });
    }

    welcomeSlide.on('translate.owl.carousel', function () {
        var slideLayer = $("[data-animation]");
        slideLayer.each(function () {
            var anim_name = $(this).data('animation');
            $(this).removeClass('animated ' + anim_name).css('opacity', '0');
        });
    });

    welcomeSlide.on('translated.owl.carousel', function () {
        var slideLayer = welcomeSlide.find('.owl-item.active').find("[data-animation]");
        slideLayer.each(function () {
            var anim_name = $(this).data('animation');
            $(this).addClass('animated ' + anim_name).css('opacity', '1');
        });
    });

    $("[data-delay]").each(function () {
        var anim_del = $(this).data('delay');
        $(this).css('animation-delay', anim_del);
    });

    $("[data-duration]").each(function () {
        var anim_dur = $(this).data('duration');
        $(this).css('animation-duration', anim_dur);
    });

    // :: 4.0 Testimonials Slider Active Code
    if ($.fn.owlCarousel) {
        $(".karl-testimonials-slides").owlCarousel({
            items: 1,
            margin: 0,
            loop: true,
            dots: true,
            autoplay: true,
            smartSpeed: 1200
        });
    }

    // :: 5.0 Gallery Menu Style Active Code
    $('.portfolio-menu button.btn').on('click', function () {
        $('.portfolio-menu button.btn').removeClass('active');
        $(this).addClass('active');
    })

    // :: 6.0 Masonary Gallery Active Code
    if ($.fn.imagesLoaded) {
        $('.karl-new-arrivals').imagesLoaded(function () {
            // filter items on button click
            $('.portfolio-menu').on('click', 'button', function () {
                var filterValue = $(this).attr('data-filter');
                $grid.isotope({
                    filter: filterValue
                });
            });
            // init Isotope
            var $grid = $('.karl-new-arrivals').isotope({
                itemSelector: '.single_gallery_item',
                percentPosition: true,
                masonry: {
                    columnWidth: '.single_gallery_item'
                }
            });
        });
    }

    // :: 7.0 Header Cart btn Active Code
    $('#header-cart-btn').on('click', function () {
        $('body').toggleClass('cart-data-open');
    })

    // :: 8.0 Side Menu Active Code
    $('#sideMenuBtn').on('click', function () {
        $('#wrapper').toggleClass('karl-side-menu-open');
    })
    $('#sideMenuClose').on('click', function () {
        $('#wrapper').removeClass('karl-side-menu-open');
    })

    // :: 9.0 Magnific-popup Video Active Code
    if ($.fn.magnificPopup) {
        $('.video_btn').magnificPopup({
            disableOn: 0,
            type: 'iframe',
            mainClass: 'mfp-fade',
            removalDelay: 160,
            preloader: true,
            fixedContentPos: false
        });
        $('.gallery_img').magnificPopup({
            type: 'image',
            gallery: {
                enabled: true
            }
        });
    }

    // :: 10.0 ScrollUp Active Code
    if ($.fn.scrollUp) {
        $.scrollUp({
            scrollSpeed: 1000,
            easingType: 'easeInOutQuart',
            scrollText: '<i class="ti-angle-up" aria-hidden="true"></i>'
        });
    }

    // :: 11.0 Slider Range Price Active Code
    $('.slider-range-price').each(function () {
        var min = jQuery(this).data('min');
        var max = jQuery(this).data('max');
        var unit = jQuery(this).data('unit');
        var value_min = jQuery(this).data('value-min');
        var value_max = jQuery(this).data('value-max');
        var label_result = jQuery(this).data('label-result');
        var t = $(this);
        $(this).slider({
            range: true,
            min: min,
            max: max,
            values: [value_min, value_max],
            slide: function (event, ui) {
                var result = label_result + " " + unit + ui.values[0] + ' - ' + unit + ui.values[1];
             
                t.closest('.slider-range').find('.range-price').html(result);
              
            },
            stop: function (event, ui) {


                t.closest('.slider-range').find('.range-price').attr("data-filter", [ui.values[0], ui.values[1]])
                FilteredGetParametrs()
            
            }

        });
    })

    // :: 12.0 PreventDefault a Click
    $("a[href='#']").on('click', function ($) {
        $.preventDefault();
    });

    // :: 13.0 wow Active Code
    if ($window.width() > 767) {
        new WOW().init();
    }

})(jQuery);
// :: 14.0 Custom Js in Backend shop
$("a.color-link").on("click", function () {
    var allLinks = $("a.color-link"); 

    allLinks.removeClass("active"); 
    
    $(this).addClass("active");
    //$(".range-price").removeAttr("data-filter")
    FilteredGetParametrs()
})
$("a.filter-size").on("click", function () {
    var allLinks = $("a.filter-size");
  
    allLinks.removeClass("a_active")
    $(this).addClass("a_active")
    allLinks.removeClass("active");
    $(this).addClass("active");
    //$(".range-price").removeAttr("data-filter")

    FilteredGetParametrs()
})
$("button.filter-category").on("click", function () {
    var allLinks = $("button.filter-category");

    allLinks.removeClass("active");

    $(this).addClass("active");
  /*  $(".range-price").removeAttr("data-filter")*/

    FilteredGetParametrs()
})
function FilteredGetParametrs() {

    debugger;
    var category = $(".portfolio-menu ul li button.active").attr("data-filter")
    var color = $("a.color-link.active").attr("data-filter")
    var size = $("a.filter-size.active").attr("data-filter")
    var Price = $(".slider-range .range-price").attr("data-filter")
    var page = $(".shop_pagination_area nav ul.pagination li.active a").attr("data-filter")
    var maxPrice = null
    var minPrice=null
    if (Price) {
       
        Price = Price.split(",")
        maxPrice = Price[1]
        minPrice = Price[0]
    }
    else {
        Price = null
    }
    if (category) {
        if (category.includes('*')) {
            category = null
        }
        else {

        category = category
        }
    }
    else {
        category = null
    }
    if (color) {
        if (color.includes('*')) {
            color = null
        } else {

        //color = color.split("-")[1]
        }
    }
    else {
        color = null
    }
    if (size) {
        if (size.includes('*')) {
            size = null
        } else {

        //size = size.split("-")[1]
        }
    }
    else {
        size = null
    }

    if (!page) {
        page=null
    }
    FilterGetdata(category, size, color, minPrice, maxPrice, page)
    //var a = parametrArry[1].split("-")[1].split(",")[0]

   

  

}
function FilterGetdata(category, size, color, minPrice, maxPrice,page) {
        var url = `/shop/FilteredData?`;

    if (category) url += `category=${category}&`;
    if (color) url += `color='${color}'&`;
    if (size) url += `size=${size}&`;
    if (minPrice) url += `minPrice=${minPrice}&`;
    if (maxPrice) url += `maxPrice=${maxPrice}&`;
    if (page) url += `page=${page}&`;
    if (url.endsWith('&')) {
        url = url.slice(0, -1);
    } 
    

    $.ajax({
        url: url,
        type: 'GET',
        success: function (jqxhr, txt_status, code) {
            debugger;
            console.log();
            console.log(txt_status)
            console.log(code.status);
            //var datas = JSON.parse(jqxhr)
            console.log(jqxhr)
            console.log(jqxhr.resultProducts)
            console.log(jqxhr.allDataCount)
            $(".shop_grid_product_area div").html("");

            jqxhr.resultProducts.forEach((item, index) => {
                debugger;
                var sizeModalHtml = ``
                for (var [key, value] of Object.entries(item.product_Size) ) {
                    
                    sizeModalHtml += value > 0 ?
                    `
                    <li>
                    <a id="sizeSelected_${item.productID}" onclick="SelectSize(this,${key})" href='#'>${key}</a>
                     <div id="quantity" class="quantity">
                     <input size="${key}" onchange="SizeCountSelect(this,'${key}','${item.productID}')" type="number" class="qty-text d-none" id="qty" step="1" min="0" max="${value}" name="quantity" value="0">
                     </div>
                       </li>`: `
                         <li>
                        <s>${key}</s>
                        <div id="quantity" class="quantity">
                        <input size="${key}" disabled type="number" class="qty-text d-none" id="qty" step="1" min="0" max="${value}" name="quantity" value="0">
                        </div>
                         </li>
                             `




                }
            
                var discountHtml = item.disCount != 0 ? 

                    `<span>${item.price} &#x20BC</span>
                    <s>${item.disCount} &#x20BC </s>`
                    :
                `
                      <span>

                        ${item.price} &#x20BC
                    </span>
                
                `
                var productHtml = `
                
                <div class="col-12 col-sm-6 col-lg-4 single_gallery_item  wow fadeInUpBig" data-wow-delay="0.2s">
                            <!-- Product Image -->
                            <div class="product-img">
                                <img src="${item.picturesUrls[0]}" alt="">
                                <div class="product-quicview">
                                    <a href="#" data-toggle="modal" data-target="#${item.productID}"><i class="ti-plus"></i></a>
                                </div>
                            </div>
                            <!-- Product Description -->
                            <div class="product-description">
                                    <h4 class="product-price">
                                      ${discountHtml}
                                </h4>
                                <p>${item.productDescription}</p>
                                <!-- Add to Cart -->
                               
                            </div>
                        </div>
                
                
                
                `
                var productModal = `
                 <!-- ****** Quick View Modal Area Start ****** -->
                            <div class="modal fade" id="${item.productID}" tabindex="-1" role="dialog" aria-labelledby="quickview" aria-hidden="true">
                                <div class="modal-dialog modal-lg modal-dialog-centered" role="document">
                                    <div class="modal-content">
                                        <button type="button" class="close btn" data-dismiss="modal" aria-label="Close">
                                            <span aria-hidden="true">&times;</span>
                                        </button>

                                        <div class="modal-body">
                                            <div class="quickview_body">
                                                <div class="container">
                                                    <div class="row">
                                                        <div class="col-12 col-lg-5">
                                                            <div class="quickview_pro_img">
                                                                <img src="${item.picturesUrls[0]}" alt="">
                                                            </div>
                                                        </div>
                                                        <div class="col-12 col-lg-7">
                                                            <div class="quickview_pro_des">
                                                                <h4 class="title">${item.productDescription}</h4>
                                                                <div class="top_seller_product_rating mb-15">
                                                                    <i class="fa fa-star" aria-hidden="true"></i>
                                                                    <i class="fa fa-star" aria-hidden="true"></i>
                                                                    <i class="fa fa-star" aria-hidden="true"></i>
                                                                    <i class="fa fa-star" aria-hidden="true"></i>
                                                                    <i class="fa fa-star" aria-hidden="true"></i>
                                                                </div>

                 ${discountHtml}

                                                                <p>${item.productDescription}</p>
                                                                <a href="#">View Full Product Details</a>
                                                            </div>

                                                            <!-- Add to Cart Form -->
                                                            <form class="cart row" method="post" id="form_${item.productID}">

                                                                <div class="widget size mb-50 col-12">
                                                                    <h6 class="widget-title">Size</h6>
                                                                    <div class="widget-desc">
                                                                        <ul>
                                                                        ${sizeModalHtml}

                                                                        </ul>
                                                                    </div>
                                                                </div>
                                                                <a href="#" data-dismiss="modal" onclick="AddToCartProduct('${item.productID}')" name="addtocart" value="5" class="cart-submit" style="display: flex;justify-content: center; align-items: center;">Add to cart</a>

                                                                <!-- Wishlist -->

                                                                <div class="modal_pro_wishlist">
                                                                    <a href="wishlist.html" target="_blank"><i class="ti-heart"></i></a>
                                                                </div>

                                                            </form>

                                                            <div class="share_wf mt-30">
                                                                <p>Share With Friend</p>
                                                                <div class="_icon">
                                                                    <a href="#"><i class="fa fa-facebook" aria-hidden="true"></i></a>
                                                                    <a href="#"><i class="fa fa-twitter" aria-hidden="true"></i></a>
                                                                    <a href="#"><i class="fa fa-pinterest" aria-hidden="true"></i></a>
                                                                    <a href="#"><i class="fa fa-google-plus" aria-hidden="true"></i></a>
                                                                </div>
                                                            </div>
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <!-- ****** Quick View Modal Area End ****** -->
                        

                
                `
                $(".shop_grid_product_area .products").append(productHtml)
                $(".shop_grid_product_area .products").append(productModal)
            }
            )
            
            //pagenations algorithm start

        },
        
        error: function (jqxhr, txt_status, code) {
            console.log("ajaxda error isledi")
            console.log(jqxhr);
            console.log(txt_status);
            console.log(code.status);
        }
    });
}

//14.1 Custom js in backend CartDetail

//inspect blocked btn
$(document).keydown(function (e) {
    if (e.which === 123) {

        return false;

    }

});
//14.2 CartDetail Js
function updateCart() {
   
    var updates = [];
 
    $("table tbody tr").each(function () {
  
        var id = $(this).find("input[name='Id']").val();
        var size = $(this).find("input[name='Size']").val();
        var quantity = $(this).find(`#qty_${id}-${size}`).val();

        updates.push({ 'Id': id, 'Quantity': quantity, 'Size': size });
    });

    console.log(updates);


    




$.ajax({
        type: "PUT",
        url: "/cart/updatecart",
        contentType: "application/json",
    data: JSON.stringify(updates),
       
        success: function (result) {

            console.log(result)
            window.location.reload();
          
        },
        error: function (error) {
            console.error("Cart update failed:", error);
           
        }
    });
}


$(".update-cart-btn").on("click", function () {
    updateCart();
    UpdateCartItem()
});

//14.3 Check out in backen js



function getCookie(cname) {
    let name = cname + "=";
    let decodedCookie = decodeURIComponent(document.cookie);
    let ca = decodedCookie.split(';');
    for (let i = 0; i < ca.length; i++) {
        let c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}
function CheckedCokkieItem(callback) {
    $.ajax({
        url: "addtocart/checkedcartiteminfo",
        type: "GET",
    }).done(function (data) {

        callback(null, data);
    }).fail(function (jqXHR, textStatus, errorThrown) {
        
        callback(textStatus, null);
    });
}




$("input[name='shippingMethod']").on("click", function (e) {
    var value = $(e.target).val();
    CheckedCokkieItem(function (error, data) {
        if (error) {
            console.error("Ajax Call Failed with Status:", error);
            window.location.reload();
        } else {
            $.ajax({
                url: `/cart/ShippingMethodChecked?id=${value}`,
                type: "GET",
                contentType: "application/json; charset=utf-8",
                success: function (response) {


                    var cookieItems = JSON.parse(getCookie("cart")) || [];
                    var shippingPrice = response.price;
                    var totalItemPrice = 0;

                    cookieItems.map((item) => {
                        totalItemPrice += (item.Quantity * item.Price);
                    });


                    var total = shippingPrice + data.totalCartPrice;

                    $("strong[id='deliveryPrice']").html("");
                    $("strong[id='cartTotalPrice']").html("");
                    $("strong[id='deliveryPrice']").html(`${shippingPrice}&#x20BC`);
                    $("strong[id='cartTotalPrice']").html(`${total}&#x20BC`);
                },
                error: function (error) {
                    window.location.reload();
                }
            });
        }
    });
    if (false) {
       
    }

     
          
     
 
  
});
//14.4 modal js
function SizeCountSelect(e, size, id) {

    var SizeTagValue = document.querySelectorAll(`#sizeSelected_${id}`);

    SizeTagValue.forEach((item) => {
        if (item.innerHTML == size) {
         
            if (item.getAttribute("data-value") != null) {

                item.setAttribute("size-count", `${e.value}`)
            }
            if (item.getAttribute("size-count") == "0") {
                item.removeAttribute("size-count")
            }



        }
    })

}
function SelectSize(e, size) {
   
    if (e.getAttribute("data-value") == null) {
     
        e.setAttribute("data-value", `${size}`)
    }
    else {
        e.removeAttribute("data-value")
    }
    e.classList.toggle("a_onclikc")
    var input = document.querySelectorAll(".qty-text")

    input.forEach((tag) => {

        if (tag.getAttribute("size") == size) {
            tag.classList.toggle("d-none")
        }
    })


}
function ProductModal(id) {

    var size = document.querySelectorAll(`#sizeSelected_${id}`)
    var sizeArry = [];
    var input = document.querySelectorAll(".qty-text")
    var a_onclick = document.querySelectorAll(".widget-desc ul li a.a_onclikc")
    if (a_onclick) {
        a_onclick.forEach((item) => {
            item.classList.remove("a_onclikc")
            // item.removeAttribute("data-filter")
        })
    }

    input.forEach((tag) => {

        if (!tag.classList.contains("d-none")) {
            tag.classList.toggle("d-none")

        }

    })
    size.forEach((tag) => {
        if (tag.getAttribute("data-value") != null && tag.getAttribute("size-count") != 0) {

            sizeArry.push(tag.getAttribute("data-value"));
            sizeArry.push(tag.getAttribute("size-count"))
        }
    })





    $.ajax({

        url: `/addtocart/addtocart/${id}?size=${sizeArry}`,

        success: function (jqxhr, txt_status, code) {
            console.log(jqxhr)
            console.log(txt_status)
            console.log(code.status)
            UpdateCartItem()
        },
        error: function (jqxhr, txt_status, code) {
            console.log(jqxhr)
            console.log(txt_status)
            console.log(code.status)

        },


    });



}

