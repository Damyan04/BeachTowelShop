// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(window).on("load", function () {
    $(".loader-wrapper").fadeOut("slow");
});
//$("[type='number']").keypress(function (evt) {
//   // evt.preventDefault();
//});
$(document).ready(function () {
    $('#reviews').slick({
        slidesToShow: 3,
        slidesToScroll: 1,
        autoplay: true,
        autoplaySpeed: 10000,
        prevArrow: $('.fa-chevron-left'),
        nextArrow: $('.fa-chevron-right'),

    });

    let modal = document.getElementById("myModal");
    var img = document.getElementsByTagName("img")

    // Get the button that opens the modal
    let btn = document.getElementById("myBtnMod");

    // Get the <span> element that closes the modal
    let span = document.getElementsByClassName("close")[0];

    // When the user clicks the button, open the modal 
    btn.onclick = function () {
        modal.style.display = "block";
    }

    // When the user clicks on <span> (x), close the modal
    span.onclick = function () {
        modal.style.display = "none";

       
    }
    
    // When the user clicks anywhere outside of the modal, close it
    window.onclick = function (event) {
        if (event.target == modal) {
            modal.style.display = "none";
        }
      
        if (event.target.localName == 'img') {
            modal.style.display = "none";
        }
    }

    
       

});

