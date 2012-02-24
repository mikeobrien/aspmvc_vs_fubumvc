define ['jquery', 'postal'], 
	($, postal) ->
		options = errors: {}
		$(document).ajaxError (error, xhr, settings, thrownError) ->
			json = !xhr.getResponseHeader('content-type').indexOf('application/json')
			status = options.errors[xhr.status] ? {}
			postal.channel("ajax.error.#{status.alias ? xhr.status}").publish
				status: status.alias ? xhr.status
				message: status.message ? thrownError
				data: if json then $.parseJSON(xhr.responseText) else xhr.responseText
		$(document).ajaxStart -> postal.channel("ajax.start").publish()
		$(document).ajaxStop -> postal.channel("ajax.stop").publish()
		options