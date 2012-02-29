(function() {

  require.config({
    paths: {
      text: '/scripts/require/text',
      order: '/scripts/require/order',
      jquery: '/scripts/jquery',
      underscore: '/scripts/underscore',
      backbone: '/scripts/backbone',
      postal: '/scripts/postal/postal',
      postalajax: '/scripts/postal/postal.ajax',
      postalscroll: '/scripts/postal/postal.scroll'
    }
  });

  require(['underscore', 'backbone', 'postalajax', 'postalscroll', 'app', 'entries'], function(_, Backbone, postalAjax, postalScroll, app, entries) {
    _.templateSettings = {
      evaluate: /\{\{([\s\S]+?)\}\}/g,
      interpolate: /\{\{=([\s\S]+?)\}\}/g,
      escape: /\{\{-([\s\S]+?)\}\}/g
    };
    postalAjax.errors[0] = {
      message: 'Unable to communicate with the server. Make sure you are connected to the internet and try again.'
    };
    postalScroll.bottomOffset = 100;
    return app.start(entries);
  });

}).call(this);
