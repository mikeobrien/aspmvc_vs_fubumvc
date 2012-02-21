(function() {

  require.config({
    paths: {
      text: '/content/scripts/require/text',
      order: '/content/scripts/require/order',
      jquery: '/content/scripts/jquery/jquery-1.7.1',
      postal: '/content/scripts/postal/postal'
    }
  });

  define('underscore', ['/content/scripts/underscore/underscore.js'], function() {
    _.templateSettings = {
      evaluate: /\{\{([\s\S]+?)\}\}/g,
      interpolate: /\{\{=([\s\S]+?)\}\}/g,
      escape: /\{\{-([\s\S]+?)\}\}/g
    };
    return _;
  });

  define('backbone', ['order!jquery', 'order!underscore', 'order!/content/scripts/backbone/backbone.js'], function() {
    return Backbone;
  });

  require(['app', 'entries'], function(app, entries) {
    return app.start(entries);
  });

}).call(this);
