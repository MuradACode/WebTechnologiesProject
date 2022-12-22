$('.owl-carousel-brand').owlCarousel({
    center:false,
    loop:true,
    margin: 20,
    dots: false,
    nav:false,
    loop:true,
    autoplay:true,
    autoplayTimeout:30000,
    responsive:{
        0:{
            items:1
        },
        600:{
            items:3
        },
        1000:{
            items:4
        }
    }
})