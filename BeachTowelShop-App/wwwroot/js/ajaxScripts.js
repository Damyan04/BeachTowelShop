function addImgToDesigner() {
  
    let fileInput = document.getElementById("file-input");
    let files = fileInput.files;
    let fd = new FormData();
    fd.append('image', files[0]);
    $.ajax({
        url: 'https://localhost:44381/Order/CheckImg',
        type: 'post',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: fd,
        contentType: false,
        processData: false,
        success: function (response) {
            if (response != 0) {
                //TODO: use the old code somehow-this res is working
                //var img = new Image();
                //img.src = 'data:image/jpeg;base64,' + response;
                //var offset = 50;
                //var left = fabric.util.getRandomInt(0 + offset, 200 - offset);
                //var top = fabric.util.getRandomInt(0 + offset, 400 - offset);
                var opacity = 0.8;
           
                //fabric.Image.fromURL(img.src, function (image) {
                //    image.set({
                //        angle: 0,
                //        opacity: opacity,
                //        padding: 0,
                //        cornersize: 10,
                //        hasRotatingPoint: true,
                //        rotatingPointOffset: 40,
                //        cornerStyle: 'circle',
                //        originX: "center",
                //        originY: "center",

                //    });
                var img = document.createElement('img');
                img.onload = function () {
               
                    var oImg = new fabric.Image(img);
                    var oImg = oImg.set({

                        //left: 50,
                        //top: 100,
                        //angle: 00
                         angle: 0,
                       opacity: opacity,
                       padding: 0,
                       cornersize: 10,
                       hasRotatingPoint: true,
                       rotatingPointOffset: 40,
                      cornerStyle: 'circle',
                       originX: "center",
                       originY: "center"
                    }).scaleToWidth(parseInt(canvas.width)).scaleToHeight(parseInt(canvas.height));
                    if (img.width > img.height) {
                       oImg.set('angle', 90).setCoords();
                    }
                    canvas.centerObject(oImg);
                    canvas.add(oImg);
                    canvas.setActiveObject(oImg);
                };
              
                img.src = 'data:image/jpeg;base64,' + response;
           
                   
                    //image.scaleToWidth(parseInt(canvas.width));
                   // image.scaleToHeight(parseInt(canvas.height));
                
                    //image.setCoords();               
                    //canvas.add(image).setActiveObject(image);
                   // image.moveTo(0);
                };
            
        },
        error: function (response) {
            if (response != 0) {
                console.log(response);
                alert(response.responseText);
            }
        },
    });

}
function addToCart() {
    let count = $('#volume').val();
    let size = $('#item-size').val();
    let imgURL = $(".item-size").find("img")[0].src.split('/');
    const urlParams = new URLSearchParams(window.location.search);
   let itemId = urlParams.get('itemid');
    if (size === " ") {
        alert("Please select a size");
        return;
    }
    let data = JSON.stringify({
        'Count': count,
        'Size': size,
        'DesignFolderPath': imgURL[3]+"/"+imgURL[4],
        'DesignName': imgURL[5],
        'ProductId':itemId
    });
   
    $.ajax({
        url: 'https://localhost:44381/Products/AddToCart',
        type: 'post',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: data,
        contentType: 'application/json',
        processData: false,
        success: function (response) {
            if (response != 0) {
                $('#volume').val('1');
                onSelectChange();
                alert(response);
               
            }
        },
        error: function (response) {
            if (response != 0) {
                //console.log(response);
                alert(response.status + "\n"+ response.statusText);
            }
        },
    });
}
function createDesign() {
    if (!$('#save-selected').is(":hidden")) {
        $('#save-selected').click();
    };
    let count = $('#volume').val();
    let size = $('#size').children("option").filter(":selected")[0].attributes.sizeName.value;
   
    let objects = canvas.getObjects();
    let img = canvas.toDataURL({ multiplier: 10 });
   
   
  
    let objectsInCanvas = JSON.stringify({
        'Count': count,
        'Size': size,
        'Design': img,
        'Objects': JSON.stringify(objects)
     

    })
  
    $.ajax({
        url: 'https://localhost:44381/Order/CreateImg',
        type: 'post',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: objectsInCanvas,
        contentType: 'application/json',
        processData: false,
        success: function (response) {
            if (response != 0) {
                $('#volume').val('1');
                canvas.clear();
                canvas.setBackgroundImage('/pictures/towel1.png', canvas.renderAll.bind(canvas));
                canvas.discardActiveObject().renderAll();
                alert(response);
            }
        },
        error: function (response) {
            if (response != 0) {
               // console.log(response);
                alert(response.responseText);
            }
        },
    });
}
function checkCart(){
    $.ajax({
        url: 'https://localhost:44381/Order/CheckCart',
        type: 'GET',
         
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
               $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        error: function (response) {
            if (response != 0) {
                console.log(response.responseText);
                alert(response.responseText);
            }
        },
    });
}
function removeFromCart() {
    let size = $(event.currentTarget.parentNode.parentNode).find(".cart-size-td")[0].textContent;
    let productId = $(event.currentTarget.parentNode.parentNode).find('.img-holder').find('img')[0].id;
    let data = JSON.stringify({'productId':productId,"size":size});
   
    $.ajax({
        url: 'https://localhost:44381/Cart/DeleteFromCart',
        type: 'delete',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: data,
        contentType: 'application/json',
        processData: false,
        success: function (response) {
            if (response != 0) {
                console.log(response);
                // alert(response);
                setTimeout(
                    function () {
                        location.reload();
                    }, 0001);    
            }
        },
        error: function (response) {
            if (response != 0) {
                console.log(response);
                alert(response.responseText);
            }
        },
    });




    
}
function changeCart() {
    let count = $(event.currentTarget.parentNode.parentNode).find("input[name='volume']").val();
    let size = $(event.currentTarget.parentNode.parentNode).find(".cart-size-td")[0].textContent;
    let productId = $(event.currentTarget.parentNode.parentNode).find('.img-holder').find('img')[0].id;
    
    let data = JSON.stringify({ 'productid': productId,'count':count,'size':size });
    console.log(data);
    $.ajax({
        url: 'https://localhost:44381/Cart/ChangeInCart',
        type: 'put',
        beforeSend: function (xhr) {
            xhr.setRequestHeader("XSRF-TOKEN",
                $('input:hidden[name="__RequestVerificationToken"]').val());
        },
        data: data,
        contentType: 'application/json',
        processData: false,
        success: function (response) {
            if (response != 0) {
                console.log(response);
                // alert(response);
                setTimeout(
                    function () {
                        location.reload();
                    }, 0001);
            }
        },
        error: function (response) {
            if (response != 0) {
                console.log(response);
                alert(response.responseText);
            }
        },
    });
}
function getCookie(cname) {
    var name = cname + "=";
    var ca = document.cookie.split(';');
    for (var i = 0; i < ca.length; i++) {
        var c = ca[i];
        while (c.charAt(0) == ' ') {
            c = c.substring(1);
        }
        if (c.indexOf(name) == 0) {
            return c.substring(name.length, c.length);
        }
    }
    return "";
}