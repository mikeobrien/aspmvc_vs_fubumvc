define(['jquery', 'postal'], function($, postal) {
    var options = { errors: {} };

    $(document).ajaxError(function(error, xhr, settings, thrownError) {
        var json = !xhr.getResponseHeader('content-type').indexOf('application/json');
        var status = options.errors[xhr.status] || {};
        return postal.channel("ajax.error." + (status.alias || xhr.status)).publish({
            status: status.alias || xhr.status,
            message: status.message || thrownError,
            data: json ? $.parseJSON(xhr.responseText) : xhr.responseText
        });
    });
    
    $(document).ajaxStart(function() { postal.channel("ajax.start").publish(); });
    $(document).ajaxStop(function() { postal.channel("ajax.stop").publish(); });
    
    return options;
});
