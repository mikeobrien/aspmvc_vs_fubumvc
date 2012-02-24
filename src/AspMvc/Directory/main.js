(function() {

  require.config({
    paths: {
      text: '/scripts/require/text',
      order: '/scripts/require/order',
      jquery: '/scripts/jquery',
      postal: '/scripts/postal',
      ajaxevents: '/scripts/ajaxevents',
      scrollevents: '/scripts/scrollevents'
    }
  });

  define('underscore', ['/scripts/underscore.js'], function() {
    return _;
  });

  define('backbone', ['order!jquery', 'order!underscore', 'order!/scripts/backbone.js'], function() {
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
