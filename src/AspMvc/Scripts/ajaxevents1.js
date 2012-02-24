(function() {

  define(['jquery', 'postal'], function($, postal) {
    var options;
    options = {
      errors: {}
    };
    $(document).ajaxError(function(error, xhr, settings, thrownError) {
      var json, status, _ref, _ref2, _ref3, _ref4;
      json = !xhr.getResponseHeader('content-type').indexOf('application/json');
      status = (_ref = options.errors[xhr.status]) != null ? _ref : {};
      return postal.channel("ajax.error." + ((_ref4 = status.alias) != null ? _ref4 : xhr.status)).publish({
        status: (_ref2 = status.alias) != null ? _ref2 : xhr.status,
        message: (_ref3 = status.message) != null ? _ref3 : thrownError,
        data: json ? $.parseJSON(xhr.responseText) : xhr.responseText
      });
    });
    $(document).ajaxStart(function() {
      return postal.channel("ajax.start").publish();
    });
    $(document).ajaxStop(function() {
      return postal.channel("ajax.stop").publish();
    });
    return options;
  });

}).call(this);
