(function() {

  require.config({
    paths: {
      text: '/content/scripts/require/text',
      order: '/content/scripts/require/order',
      jquery: '/content/scripts/jquery',
      underscore: '/content/scripts/underscore',
      backbone: '/content/scripts/backbone',
      postal: '/content/scripts/postal',
      ajaxevents: '/content/scripts/ajaxevents',
      scrollevents: '/content/scripts/scrollevents'
    }
  });

  require(['underscore', 'backbone', 'ajaxevents', 'scrollevents', 'app', 'entries'], function(_, Backbone, ajaxEvents, scrollEvents, app, entries) {
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
