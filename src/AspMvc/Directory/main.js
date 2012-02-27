(function() {

  require.config({
    paths: {
      text: '/scripts/require/text',
      order: '/scripts/require/order',
      jquery: '/scripts/jquery',
      underscore: '/scripts/underscore',
      backbone: '/scripts/backbone',
      postal: '/scripts/postal',
      ajaxevents: '/scripts/ajaxevents',
      scrollevents: '/scripts/scrollevents'
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
