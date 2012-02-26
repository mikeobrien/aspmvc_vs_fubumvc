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

  define('underscore', ['order!/scripts/underscore.js'], function() {
    return _;
  });

  define('backbone', ['order!/scripts/backbone.js'], function() {
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
