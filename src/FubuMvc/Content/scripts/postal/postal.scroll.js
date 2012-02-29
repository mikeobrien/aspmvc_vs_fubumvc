define(['jquery', 'postal'], function($, postal) {
    
    var top = true;
    var bottom = false;
    
    var options = {
        topOffset: 0,
        bottomOffset: 0
    };
    
    var $window = $(window);
    var $document = $(document);
    
    $window.scroll(function() {
        var topPosition = $window.scrollTop();
        var bottomPosition = topPosition + $window.height();
        
        if (topPosition <= options.topOffset) 
        {
            if (top) return;
            postal.channel('scroll.top').publish();
            top = true;
            bottom = false;
        } 
        else if (bottomPosition >= $document.height() - options.bottomOffset) 
        {
            if (bottom) return;
            postal.channel('scroll.bottom').publish();
            top = false;
            bottom = true;
        } 
        else 
        {
            top = false;
            bottom = false;
        }
    });
    return options;
});