(function() {

  require.config({
    paths: {
      text: '/scripts/require/text',
      order: '/scripts/require/order',
      jquery: '/scripts/jquery/jquery-1.7.1',
      postal: '/scripts/postal/postal'
    }
  });

  define('underscore', ['/scripts/underscore/underscore.js'], function() {
    _.templateSettings = {
      evaluate: /\{\{([\s\S]+?)\}\}/g,
      interpolate: /\{\{=([\s\S]+?)\}\}/g,
      escape: /\{\{-([\s\S]+?)\}\}/g
    };
    return _;
  });

  define('backbone', ['order!jquery', 'order!underscore', 'order!/scripts/backbone/backbone.js'], function() {
    return Backbone;
  });

  require(['app', 'entries'], function(app, entries) {
    return app.start(entries);
  });

}).call(this);
