//function changeStage() {
//    let stepOne = $('li#one>a:first-child').find("span").css("background-color", "black");
   
//    let stepTwo = $('#two');
//    let stepThree = $('#three');
//   console.log(stepOne);
 
//}

function accordeon() {
   let element=event.target;
     let allParent=element.parentNode.parentElement;
     let icon=$(allParent.childNodes[1]).find("i");
     let content=$(allParent.childNodes[3]);
     if ($(allParent).hasClass('active')) {
        icon.removeClass('fa-minus').addClass('fa-plus');
       content.slideUp(); 
        $(allParent).removeClass('active');
     }else{
        icon.removeClass('fa-plus').addClass('fa-minus');
        content.slideDown(); 
        $(allParent).addClass('active');
     }
}
function showFonts() {
   $('.dropdown-menu').css("display") == "block" ? $('.dropdown-menu').hide() : $('.dropdown-menu').show();
}
function check() {
    let speedy = $("#speedy");
    let econt = $("#econt");
    let office = $("#office");
    let invoice = $('#invoice');
    let invoicediv = $('#invoice-div');
    let divDanger = $('.invoice-danger');
    if (speedy.is(":checked") || econt.is(":checked")) {
        //TODO:make request to populate the tabs
        office.slideDown();
    } else {
        office.slideUp();
    }
   
    if (invoice.is(":checked")||invoice.value==="false") {
      
        invoicediv.slideDown();
        invoice.value === "true";
        
        divDanger.slideDown();
    
        
    } else {
 
        
        invoicediv.slideUp();
        divDanger.slideUp();
    }
}

function opennnn() {
        let div = $('.all-products');
        console.log(div);
        console.log(div.hasClass('opened'));
        if (div.hasClass('opened')) {
            div.removeClass('opened');
            div.slideUp();
        } else {
            div.addClass('opened');
            div.slideDown();
        }
}

function rotate() {
    let currentImg = $('.coveredImage');
    let hiddenImg = $('.coveredImage-hidden');

    currentImg.hide();
    hiddenImg.show();
    currentImg.removeClass('coveredImage');
    currentImg.addClass('coveredImage-hidden');
    hiddenImg.removeClass('coveredImage-hidden');
    hiddenImg.addClass('coveredImage');
}



jQuery(document).ready(function ($) {
   
    window.onscroll = function () { scrollFunction() };
 

    function scrollFunction() {
        if (document.body.scrollTop > 20 || document.documentElement.scrollTop > 20) {
            document.getElementById("myBtn").style.display = "block";
        } else {
            document.getElementById("myBtn").style.display = "none";
        }
    }
   
    $('.scrollup').click(function () {
        $("html, body").animate({ scrollTop: 0 }, 1000);

        return false;
    });
    $(document).mouseup(e => {
        const $menu = $('.dropdown-menu');
        if (!$menu.is(e.target) // if the target of the click isn't the container...
            && $menu.has(e.target).length === 0) // ... nor a descendant of the container
        {
            $menu.hide();
        }
    });
   
    
});

