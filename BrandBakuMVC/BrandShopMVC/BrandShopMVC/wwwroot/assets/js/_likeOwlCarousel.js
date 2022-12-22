$('.owl-carousel-like').owlCarousel({
    center:true,
    loop:true,
    margin: 10,
    dots: false,
    nav:true,
    loop:true,
    autoplay:true,
    autoplayTimeout:30000,
    responsive:{
        0:{
            items:1
        },
        600:{
            items:2
        },
        1000:{
            items:4
        }
    }
})