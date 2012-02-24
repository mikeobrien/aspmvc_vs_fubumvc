(function() {

  require.config({
    paths: {
      text: '/content/scripts/require/text',
      order: '/content/scripts/require/order',
      jquery: '/content/scripts/jquery',
      postal: '/content/scripts/postal',
      ajaxevents: '/content/scripts/ajaxevents',
      scrollevents: '/content/scripts/scrollevents'
    }
  });

  define('underscore', ['/content/scripts/underscore.js'], function() {
    return _;
  });

  define('backbone', ['order!jquery', 'order!underscore', 'order!/content/scripts/backbone.js'], function() {
    return Backbone;
  });

  require(['ajaxevents', 'scrollevents', 'underscore', 'app', 'entries'], function(ajaxEvents, scrollEvents, _, app, entries) {
    _.templateSettings = {
      evaluate: /\{\{([\s\S]+?)\}\}/g,
      interpolate: /\{\{=([\s\S]+?)\}\}/g,
      escape: /\{\{-([\s\S]+?)\}\}/g
    };
    ajaxEvents.errors[0] = {
      message: 'Unable to communicate with the server. Make sure you are connected to the internet and try again.'
    };
    scrollEvents.bottomOffset = 100;
    return app.start(entries);
  });

}).call(this);
