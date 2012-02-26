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

  define('underscore', ['order!/content/scripts/underscore.js'], function() {
    return _;
  });

  define('backbone', ['order!/content/scripts/backbone.js'], function() {
    return Backbone;
  });

  require(['order!jquery', 'order!underscore', 'order!backbone', 'ajaxevents', 'scrollevents', 'app', 'entries'], function($, _, Backbone, ajaxEvents, scrollEvents, app, entries) {
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
