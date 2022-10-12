// Self info
document.getElementById("Name").innerHTML = "Allan Debastiani";
document.getElementById("CenterText").innerHTML = "Get in touch";

// Copyright  
var Date = new Date();
document.getElementById('CurrentYear').innerHTML = Date.getFullYear();

(function ($) {
  "use strict";

    // PRE LOADER
    $(window).load(function(){
      $('.preloader').fadeOut(1000); // set duration in brackets    
    });
}
)(jQuery);
