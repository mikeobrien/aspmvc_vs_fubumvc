(function() {

  require.config({
    paths: {
      text: '/content/scripts/require/text',
      order: '/content/scripts/require/order',
      jquery: '/content/scripts/jquery',
      underscore: '/content/scripts/underscore',
      backbone: '/content/scripts/backbone',
      postal: '/content/scripts/postal/postal',
      postalajax: '/content/scripts/postal/postal.ajax',
      postalscroll: '/content/scripts/postal/postal.scroll'
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
