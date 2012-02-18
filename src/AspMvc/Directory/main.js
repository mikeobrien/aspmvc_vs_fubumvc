(function() {

  require.config({
    paths: {
      text: '/scripts/require/text',
      order: '/scripts/require/order',
      jquery: '/scripts/jquery/jquery-1.7.1'
    }
  });

  define('underscore', ['/scripts/underscore/underscore.js'], function() {
    return _;
  });

  define('backbone', ['underscore', 'jquery', 'order!/scripts/backbone/backbone.js'], function() {
    return Backbone;
  });

  require(['underscore', 'app', 'entries'], function(_, app, entries) {
    _.templateSettings = {
      evaluate: /\{\{([\s\S]+?)\}\}/g,
      interpolate: /\{\{=([\s\S]+?)\}\}/g,
      escape: /\{\{-([\s\S]+?)\}\}/g
    };
    return app.start(entries);
  });

}).call(this);
