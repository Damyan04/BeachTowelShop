var canvas;
//var tshirts = new Array(); //prototype: [{style:'x',color:'white',front:'a',back:'b',price:{tshirt:'12.95',frontPrint:'4.99',backPrint:'4.99',total:'22.47'}}]
var a;
var b;
var line1;
var line2;
var line3;
var line4;

 	$(document).ready(function() {
		//setup front side canvas 
		  canvas = new fabric.Canvas('coveringCanvas', {
		  hoverCursor: 'pointer',
		  selection: true,
			  selectionBorderColor: 'blue',

			 
		  });
		  let selectSize = document.getElementById('size');
		  let wh = selectSize.value.split(':');
		  console.log(selectSize);
		  canvas.setHeight(wh[1]);
		  canvas.setWidth(wh[0]);
		  canvas.setBackgroundImage('/pictures/towel1.png', canvas.renderAll.bind(canvas));
		
		  canvas.on({
			  'object:moving': function (e) {
				  e.target.opacity = 0.5;
			  }, 'object:moving': onObjectSelected,
			  'object:modified': function (e) {
				  e.target.opacity = 0.6;
			  }, 'object:modified':onObjectSelected,
			  'object:selected': onObjectSelected,
			  'selection:cleared': onSelectedCleared,
			  'selection:updated': onObjectSelected
			  
		 });
		// piggyback on `canvas.findTarget`, to fire "object:over" and "object:out" events
 		canvas.findTarget = (function(originalFn) {
		  return function() {
		    var target = originalFn.apply(this, arguments);
		    if (target) {
		      if (this._hoveredTarget !== target) {
		    	  canvas.fire('object:over', { target: target });
		        if (this._hoveredTarget) {
		        	canvas.fire('object:out', { target: this._hoveredTarget });
		        }
		        this._hoveredTarget = target;
		      }
		    }
		    else if (this._hoveredTarget) {
		    	canvas.fire('object:out', { target: this._hoveredTarget });
		      this._hoveredTarget = null;
		    }
		    return target;
		  };
		})(canvas.findTarget);

 		canvas.on('object:over', function(e) {		
		 
				e.target.opacity = 1;
		});
		
 		canvas.on('object:out', function(e) {		
		  
				e.target.opacity = 1;
		});
		document.getElementById('add-text').onclick = function() {
			var text = $("#text-string").val();
			
			if (text != "") {
				createText(text);
				$("#texteditor").css('display', 'flex');
				$("#imageeditor").css('display', 'flex');
				
				$("#all-types").css('display', 'flex');
			}
			//console.log(canvas);
            //canvas.item(canvas.item.length-1).hasRotatingPoint = true;    
			
         
			//$('.miniColors-trigger').css("background-color", " rgba(255, 255, 255, 0.8)");
	  	};
	  	$("#text-string").keyup(function(){	  		
	  		var activeObject = canvas.getActiveObject();
		      if (activeObject && activeObject.type === 'text') {
		    	  activeObject.text = this.value;
		    	  canvas.renderAll();
		      }
	  	});
	  	$(".img-polaroid").click(function(e){
	  		var el = e.target;
	  		/*temp code*/
	  		var offset = 50;
	        var left = fabric.util.getRandomInt(0 + offset, 200 - offset);
	        var top = fabric.util.getRandomInt(0 + offset, 400 - offset);
	        var angle = fabric.util.getRandomInt(-20, 40);
	        var width = fabric.util.getRandomInt(30, 50);
	        var opacity = 0.8//(function(min, max){ return Math.random() * (max - min) + min; })(0.5, 1);
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

				img.src = el.src;
	  		//fabric.Image.fromURL(el.src, function(image) {
					//image.set({
					//	left: left,
					//	top: top,
					//	angle: 0,
					//	opacity: opacity,
					//	padding: 0,
					//	cornersize: 12,
					//	hasRotatingPoint: true,
					//	rotatingPointOffset: 40,
					//	cornerStyle: 'circle',
					//	originX: "center",
					//	originY: "center"
					//});
					
					
					//image.scaleToWidth(parseInt(canvas.width));
					//image.scaleToHeight(parseInt(canvas.height));
					//canvas.centerObject(image);
					//image.setCoords();
					//canvas.add(image).setActiveObject(image);
					//image.moveTo(0);
                  });
		         // image.scale(0.1).setCoords();
                  
		
		  document.getElementById('resize-selected').onclick = function () {
			  var activeObject = canvas.getActiveObject();
			  if (activeObject && activeObject.type === 'image') {
				 
				  //if (activeObject.width > activeObject.height) {
					 // let diff = activeObject.width - activeObject.height
					 // if ((activeObject.angle <= 90 || activeObject.angle >= 270) && activeObject.angle != 360 && activeObject.angle != 0 && diff >= 500) { 
						//  activeObject.scaleToHeight(parseInt(canvas.height));
						//  activeObject.scaleToWidth(parseInt(canvas.width));
					 // } else {
						//  activeObject.scaleToWidth(parseInt(canvas.width));
						//  activeObject.scaleToHeight(parseInt(canvas.height)); 
					 // }
				  //} else {
					 // if ((activeObject.angle <= 90 || activeObject.angle >= 270) && activeObject.angle != 360) {
						//  activeObject.scaleToHeight(parseInt(canvas.height));
						//  activeObject.scaleToWidth(parseInt(canvas.width));
						 
					 // }
					 // activeObject.scaleToWidth(parseInt(canvas.width));
					 // activeObject.scaleToHeight(parseInt(canvas.height));
				  //}
				
				  activeObject.scaleToWidth(parseInt(canvas.width));
				activeObject.scaleToHeight(parseInt(canvas.height));
				  canvas.centerObject(activeObject);
				  activeObject.setCoords();
			  }
			  $('#scale-control').val(((activeObject.scaleX).toFixed(1)));
			  activeObject.opacity = 1;
			  $('.text3').text(((activeObject.scaleX).toFixed(1)) + '\u{0025}');
			  onSelectedCleared();
			  canvas.discardActiveObject().renderAll();
		  };
	  document.getElementById('remove-selected').onclick = function() {		  
		  var activeObject = canvas.getActiveObject();

		    if (activeObject) {
		      canvas.remove(activeObject);
		      $("#text-string").val("");
		    }
		    
		  };
		  $('#clear-selected').click(
			  function () {
				  var activeObject = canvas.getActiveObject();
				 
				  if (activeObject && activeObject.type === 'text') {
					  let text = activeObject.text;
					  canvas.remove(activeObject);
					  createText(text);
					  onSelectedCleared();
					 
				  } else {
					  //
				  }
				  activeObject.opacity = 1;
				  onSelectedCleared();
				  canvas.discardActiveObject().renderAll();

			  });
		
		  $('#center-selected').click(
			  function () {
				  var activeObject = canvas.getActiveObject();
				 
				  canvas.centerObject(activeObject);
				  activeObject.setCoords();
				  activeObject.opacity = 1;
				  onSelectedCleared();
				  canvas.discardActiveObject().renderAll();
			  });
		  $('#save-selected').click(
			  function () {
				  var activeObject = canvas.getActiveObject();
				  activeObject.opacity = 1;
				  onSelectedCleared();
				 
				  canvas.discardActiveObject().renderAll();
			  });
		
		  selectSize.onchange = function () {
			
			  let selectSize = document.getElementById('size');
			  let wh = selectSize.value.split(':');
			  let widht = wh[0];
			  let height = wh[1];
			  canvas.setWidth(widht);
			  canvas.setHeight(height);
			  //canvas.clear();
			  //canvas.setBackgroundImage('/pictures/towel1.png', canvas.renderAll.bind(canvas));
			  canvas.discardActiveObject().renderAll();
		  };
		  var scaleControl = document.getElementById('scale-control');
		  
		  scaleControl.onchange = function () {
			  var activeObject = canvas.getActiveObject();
			
			  activeObject.scale(parseFloat(this.value)).setCoords();
			  let b = '\u{0025}';
			  $('span[class="text3"]').text(parseFloat(this.value) + b);
			  canvas.renderAll();
		  };
		  var angleControl = document.getElementById('angle-control');
		  angleControl.oninput = function () {
			  var activeObject = canvas.getActiveObject();
			 
			  activeObject.set('angle', parseInt(this.value)).setCoords();
			  var a = '\u{00B0}';
			  $('span[class="text"]').text(parseInt(this.value) +a);
			  canvas.renderAll();
		  };
		  var sizeControl = document.getElementById('size-control');
		  sizeControl.oninput = function () {
			  var activeObject = canvas.getActiveObject();
			  
			  activeObject.set('fontSize', parseFloat(this.value));
			 
			  $('span[class="text2"]').text(parseFloat(this.value) + 'px');
			  canvas.renderAll();
		  };
	  $("#text-bold").click(function() {		  
		  var activeObject = canvas.getActiveObject();
		  if (activeObject && activeObject.type === 'text') {
		    activeObject.fontWeight = (activeObject.fontWeight == 'bold' ? '' : 'bold');		    
		    canvas.renderAll();
		  }
		});
	  $("#text-italic").click(function() {		 
		  var activeObject = canvas.getActiveObject();
		  if (activeObject && activeObject.type === 'text') {
			  activeObject.fontStyle = (activeObject.fontStyle == 'italic' ? '' : 'italic');		    
		    canvas.renderAll();
		  }
		});
		  $("#text-strike").click(function () {	
			
		  var activeObject = canvas.getActiveObject();
			  if (activeObject && activeObject.type === 'text') {	
				  var isStrike = dtGetStyle(activeObject, 'linethrough') === true;
				  dtSetStyle(activeObject, 'linethrough', isStrike ? '' : true);
				  canvas.renderAll();
		  }
		});
	  $("#text-underline").click(function() {		  
		  var activeObject = canvas.getActiveObject();
		  if (activeObject && activeObject.type === 'text') {
			  var isUnderline = dtGetStyle(activeObject, 'underline') === true;
			  dtSetStyle(activeObject, 'underline', isUnderline ? '' : true);
			  canvas.renderAll();
		  }
		});
	  $("#font-family").change(function() {
	      var activeObject = canvas.getActiveObject();
	      if (activeObject && activeObject.type === 'text') {
			  activeObject.set( 'fontFamily', this.value);
	        canvas.renderAll();
	      }
	    });	  
		$('#text-bgcolor').miniColors({
			change: function(hex, rgb) {
			  var activeObject = canvas.getActiveObject();
		      if (activeObject && activeObject.type === 'text') {
				  activeObject.set('backgroundColor', this.value);
		        canvas.renderAll();
		      }
			},
			open: function(hex, rgb) {
				//
			},
			close: function(hex, rgb) {
				//
			}
		});		
		$('#text-fontcolor').miniColors({
			change: function(hex, rgb) {
			  var activeObject = canvas.getActiveObject();
		      if (activeObject && activeObject.type === 'text') {
				  activeObject.set('fill',this.value);
		    	  canvas.renderAll();
		      }
			},
			open: function(hex, rgb) {
				//
			},
			close: function(hex, rgb) {
				
			}
		});
		
		  $('#text-resetBackground').click(function () {
			  var activeObject = canvas.getActiveObject();
			  if (activeObject && activeObject.type === 'text') {
				  activeObject.set('backgroundColor', '');
				 
				  $('#text-backgroundColor').val(activeObject.backgroundColor);
				  $('#text-backgroundColor')[0].nextElementSibling.firstChild.style.backgroundColor = "#ffffff";
			
				  canvas.renderAll();
			  }
		  });	 
		

		  $('#text-resetFill').click(function () {
			  var activeObject = canvas.getActiveObject();
			  if (activeObject && activeObject.type === 'text') {
				  activeObject.set('stroke', '');
				  activeObject.set('strokeWidth', 0);
				  $('#text-stroke').val(activeObject.stroke);
				  $('#text-stroke')[0].nextElementSibling.firstChild.style.backgroundColor = "#ffffff";
				  canvas.renderAll();
			  }
		  });	
		  $('#text-stroke').miniColors({
			  change: function (hex, rgb) {
				  var activeObject = canvas.getActiveObject();
				  if (activeObject && activeObject.type === 'text') {
					  activeObject.set('stroke', this.value);
					  activeObject.set('strokeWidth', 1);
					  canvas.renderAll();
				  }
			  },
			  open: function (hex, rgb) {
				  //
			  },
			  close: function (hex, rgb) {
				
				  
			  }
		  });
		  $('#text-backgroundColor').miniColors({
			  change: function (hex, rgb) {
				  var activeObject = canvas.getActiveObject();
				  if (activeObject && activeObject.type === 'text') {
					  activeObject.set('backgroundColor', this.value);
					  canvas.renderAll();
				  }
			  },
			  open: function (hex, rgb) {
				  //
			  },
			  close: function (hex, rgb) {

				  
			  }
		  });
		
	   $("#drawingArea").hover(
	        function() { 	        	
	        	 canvas.add(line1);
		         canvas.add(line2);
		         canvas.add(line3);
		         canvas.add(line4); 
		         canvas.renderAll();
	        },
	        function() {	        	
	        	 canvas.remove(line1);
		         canvas.remove(line2);
		         canvas.remove(line3);
		         canvas.remove(line4);
		         canvas.renderAll();
	        }
	    );
		  
	 
	     
	
	   line1 = new fabric.Line([0,0,200,0], {"stroke":"#000000", "strokeWidth":1,hasBorders:false,hasControls:false,hasRotatingPoint:false,selectable:false});
	   line2 = new fabric.Line([199,0,200,399], {"stroke":"#000000", "strokeWidth":1,hasBorders:false,hasControls:false,hasRotatingPoint:false,selectable:false});
	   line3 = new fabric.Line([0,0,0,400], {"stroke":"#000000", "strokeWidth":1,hasBorders:false,hasControls:false,hasRotatingPoint:false,selectable:false});
	   line4 = new fabric.Line([0,400,200,399], {"stroke":"#000000", "strokeWidth":1,hasBorders:false,hasControls:false,hasRotatingPoint:false,selectable:false});
	 });//doc ready

function createText(text) {
	var textSample = new fabric.Text(text, {
		left: fabric.util.getRandomInt(0, 200),
		top: fabric.util.getRandomInt(0, 400),
		fontFamily: 'helvetica',
		angle: 0,
		fill: '#000000',
		scaleX: 1,
		scaleY: 1,
		fontWeight: '',
		padding: 0,
		cornersize: 12,
		hasRotatingPoint: true,
		rotatingPointOffset: 40,
		strokeWidth: 4,
		cornerStyle: 'circle',
		originX: "center",
		originY: "center",
		fontSize:"16"
	});
	canvas.add(textSample).setActiveObject(textSample);
}
	 function getRandomNum(min, max) {
	    return Math.random() * (max - min) + min;
}
	 
	 function onObjectSelected(e) {	 
		 var selectedObject = e.target;
		 e.target.opacity = 0.6;
	    $("#text-string").val('');
		 selectedObject.hasRotatingPoint = true
		
		
	    if (selectedObject && selectedObject.type === 'text') {
	    	//display text editor	    	
			$("#all-types").css('display', 'flex');
	    	$("#texteditor").css('display', 'flex');
	    	$("#text-string").val(selectedObject.text);	    	
	    	$('#text-fontcolor').miniColors('value',selectedObject.fill);
			$('#text-backgroundColor').miniColors('value', selectedObject.backgroundColor);
			$('#text-stroke').miniColors('value', selectedObject.stroke);
			$("#imageeditor").css('display', 'flex');
			//$("#text-fontSize").css('display', 'flex');
			
			$('#size-control').val(selectedObject.fontSize);
			$('.text2').text(selectedObject.fontSize + 'px');
			$('#scale-control').val(((selectedObject.scaleX).toFixed(1)));
			$('.text3').text(((selectedObject.scaleX).toFixed(1)) + '\u{0025}');
			$('#angle-control').val(selectedObject.angle);
			$('span[class="text"]').text(((selectedObject.angle).toFixed(0)) + '\u{00B0}');
			$('#text-stroke').val(selectedObject.stroke);
			$('#text-backgroundColor').val(selectedObject.backgroundColor);
			$('#text-fontColor').val(selectedObject.fill);
			$('#text-backgroundColor')[0].nextElementSibling.firstChild.style.backgroundColor = selectedObject.backgroundColor;
			$('#text-stroke')[0].nextElementSibling.firstChild.style.backgroundColor = selectedObject.stroke;
			$('#resize-selected').hide();
			$('#clear-selected').show();
	    }
	    else if (selectedObject && selectedObject.type === 'image'){
	    	//display image editor
			
	    	$("#texteditor").css('display', 'none');	
			$("#imageeditor").css('display', 'flex');
			$("#all-types").css('display', 'flex');
			$('#scale-control').val(((selectedObject.scaleX).toFixed(1)));
			$('.text3').text(((selectedObject.scaleX).toFixed(1)) + '\u{0025}');
			$('#angle-control').val((selectedObject.angle).toFixed(0));
			$('span[class="text"]').text(((selectedObject.angle).toFixed(0)) + '\u{00B0}');
			$('#width-control').val((selectedObject.width).toFixed(0));
			$('#height-control').val((selectedObject.height).toFixed(0));
			$('#clear-selected').hide();
			$('#resize-selected').show();
		 }
		 
		
		
}

// Get the style
function dtGetStyle(object, styleName) {
	return object[styleName];
}

// Set the style
function dtSetStyle(object, styleName, value) {
	object[styleName] = value;
	object.set({ dirty: true });
	canvas.renderAll();
}   
	 function onSelectedCleared(){
		 $("#texteditor").css('display', 'none');
		 $("#text-string").val("");
		 $("#imageeditor").css('display', 'none');
		 $("#all-types").css('display', 'none');
		
	 }
	 function setFont(font){
		 var activeObject = canvas.getActiveObject();
		
		
	      if (activeObject && activeObject.type === 'text') {
			  activeObject.fontFamily = font;
			
	        canvas.renderAll();
	      }
	  }
	 function removeWhite(){
		  var activeObject = canvas.getActiveObject();
		  if (activeObject && activeObject.type === 'image') {			  
			  activeObject.filters[2] =  new fabric.Image.filters.RemoveWhite({hreshold: 100, distance: 10});//0-255, 0-255
			  activeObject.applyFilters(canvas.renderAll.bind(canvas));
		  }	        
	 }