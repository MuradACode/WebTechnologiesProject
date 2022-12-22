window.addEventListener("load", function () {
    var loader = document.querySelector(".loader");
    loader.classList.add("hidden");

    let $heroHeader = document.querySelector('#hero .header');
    let $heroDesc = document.querySelector('#hero .desc');
    let $heroBtn = document.querySelector('#hero .linkBtn');
    let $heroImage = document.querySelector('#hero .hero-image');

    $heroHeader.style.marginLeft = "0px";
    $heroHeader.style.marginRight = "0px";
    $heroHeader.style.visibility = "visible"
    $heroHeader.style.opacity = "1"
    $heroDesc.style.marginLeft = "0px";
    $heroDesc.style.marginRight = "0px";
    $heroDesc.style.visibility = "visible"
    $heroDesc.style.opacity = "1"
    $heroBtn.style.marginLeft = "0px";
    $heroBtn.style.marginRight = "0px";
    $heroBtn.style.visibility = "visible"
    $heroBtn.style.opacity = "1"
    $heroImage.style.marginTop = "0px";
    $heroImage.style.visibility = "visible"
    $heroImage.style.opacity = "1"
});

let thumbnails = document.getElementsByClassName('thumbnail')

let activeImages = document.getElementsByClassName('active')

for (var i = 0; i < thumbnails.length; i++) {

    thumbnails[i].addEventListener('click', function () {
        console.log(activeImages)

        if (activeImages.length > 0) {
            activeImages[0].classList.remove('active')
        }


        this.classList.add('active')
        document.getElementById('featured').src = this.src
    })
}


// let buttonRight = document.getElementById('slideRight');
// let buttonLeft = document.getElementById('slideLeft');

// buttonLeft.addEventListener('click', function () {
//     document.getElementById('slider').scrollLeft -= 180
// })

// buttonRight.addEventListener('click', function () {
//     document.getElementById('slider').scrollLeft += 180
// })



$(document).ready(function () {
    // $(".testimonial .indicators li").click(function () {
    //     var i = $(this).index();
    //     var targetElement = $(".testimonial .tabs li");
    //     targetElement.eq(i).addClass('active');
    //     targetElement.not(targetElement[i]).removeClass('active');
    // });
    // $(".testimonial .tabs li").click(function () {
    //     var targetElement = $(".testimonial .tabs li");
    //     targetElement.addClass('active');
    //     targetElement.not($(this)).removeClass('active');
    // });

    $(".qtyminus").on("click", function () {
        var now = $(".qty").val();
        if ($.isNumeric(now)) {
            if (parseInt(now) - 1 > 0) { now--; }
            $(".qty").val(now);
        }
    })
    $(".qtyplus").on("click", function () {
        var now = $(".qty").val();
        if ($.isNumeric(now)) {
            $(".qty").val(parseInt(now) + 1);
        }
    });
});

$(document).ready(function () {
    $(".slider .swiper-pagination span").each(function (i) {
        $(this).text(i + 1).prepend("0");
    });
});


let $tryAgain = document.querySelector('.upPageBox');

let $chatBtn = document.querySelector('.questionBox');

let $searchForm = document.querySelector('.search-form');

window.addEventListener('scroll', (e) => {
    let $pageHeight = window.pageYOffset;

    if ($pageHeight > 100) {
        $tryAgain.style.opacity = '1';
        $tryAgain.style.visibility = 'visible';
        $chatBtn.style.opacity = '1';
        $chatBtn.style.visibility = 'visible';
        $searchForm.style.display = 'none';
    }

    if ($pageHeight < 100) {
        $tryAgain.style.opacity = '0';
        $tryAgain.style.visibility = 'hidden';
        $chatBtn.style.opacity = '0';
        $chatBtn.style.visibility = 'hidden';
        $searchForm.style.display = 'flex';
    }
})



$tryAgain.addEventListener('click', function (e) {
    e.preventDefault();

    window.scrollTo({
        top: 0,
        behavior: 'smooth'
    });
})



window.addEventListener('resize', (e) => {
    e.preventDefault();

    let $pageWidth = document.body.clientWidth;

    let $filterSmall = document.querySelector('.filter-small');

    let $filterBig = document.querySelector('.filter-big');

    if ($pageWidth < 992) {
        $filterSmall.classList.remove('filter-none');
        $filterBig.classList.add('filter-none');
    }

    if ($pageWidth > 992) {
        $filterSmall.classList.add('filter-none');
        $filterBig.classList.remove('filter-none');
    }
})

