window.addEventListener("scroll", function() {
    var scrollHeight = document.documentElement.scrollHeight;
    var scrollTop = document.documentElement.scrollTop;
    var clientHeight = document.documentElement.clientHeight;
    
    if (scrollTop + clientHeight >= scrollHeight) {
      window.scrollTo(0, 0);
    }
  });