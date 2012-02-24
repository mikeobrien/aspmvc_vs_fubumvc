(function() {

  define(['jquery', 'postal'], function($, postal) {
    var $document, $window, bottom, options, top;
    top = true;
    bottom = false;
    options = {
      topOffset: 0,
      bottomOffset: 0
    };
    $window = $(window);
    $document = $(document);
    $window.scroll(function() {
      var bottomPosition, topPosition;
      topPosition = $window.scrollTop();
      bottomPosition = topPosition + $window.height();
      if (topPosition <= options.topOffset) {
        if (top) return;
        postal.channel('scroll.top').publish();
        top = true;
        return bottom = false;
      } else if (bottomPosition >= $document.height() - options.bottomOffset) {
        if (bottom) return;
        postal.channel('scroll.bottom').publish();
        top = false;
        return bottom = true;
      } else {
        top = false;
        return bottom = false;
      }
    });
    return options;
  });

}).call(this);
