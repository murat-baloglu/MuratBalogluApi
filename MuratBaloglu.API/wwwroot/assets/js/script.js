$(".scrollTop").on("click", function () {
    $("html, body").animate({ "scrollTop": 0 }, 500);
});

$(".select-language .dropdown-item").on("click", function () {
    let clickedFlagAttr = $(this).find("img").attr("src");
    $(".currentFlag").attr("src", clickedFlagAttr)
})


$(".whatsapp-widget").on("click", function () {
    $(".whatsapp-widget-panel").show();
    $(".whatsapp-widget-panel").animate({ "opacity": "1", "z-index": "101", "bottom": "60" }, 50)
});
$(".wwp-close").on("click", function () {
    $(".whatsapp-widget-panel").animate({ "opacity": "0", "z-index": "99", "bottom": "50" }, 50)
    $(".whatsapp-widget-panel").hide();
});
function patientCommentsCarousel() {
    let items = document.querySelectorAll('#recipeCarousel .carousel-item');
    items.forEach((el) => {
        const minPerSlide = 3
        let next = el.nextElementSibling
        for (var i = 1; i < minPerSlide; i++) {
            if (!next) {
                // wrap carousel by using first child
                next = items[0]
            }
            let cloneChild = next.cloneNode(true)
            el.appendChild(cloneChild.children[0])
            next = next.nextElementSibling
        }
    })

}

function mainCarouselSetting() {
    $("#carouselExampleAutoplaying .carousel-item").each(function () {
        $(this).css({
            "background-image": "url(" + $(this).find("img").attr("src") + ")"
        })
    })
}


patientCommentsCarousel();

navigation.addEventListener('navigate', () => {
    window.scrollTo(0, 0);
});

window.onpopstate = function () {
    window.location.reload()
}
window.onclick = function (e) {
    if (e.target.classList.contains("home")) {
        patientCommentsCarousel();
        mainCarouselSetting()
    }
}