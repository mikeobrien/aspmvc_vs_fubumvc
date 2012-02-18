require.config
	paths:
		text: '/content/scripts/require/text'
		order: '/content/scripts/require/order'
		jquery: '/content/scripts/jquery/jquery-1.7.1'

define 'underscore', ['/content/scripts/underscore/underscore.js'], -> _
define 'backbone', ['underscore', 'jquery', 'order!/content/scripts/backbone/backbone.js'], -> Backbone

require ['underscore', 'app', 'entries'], (_, app, entries) ->
	_.templateSettings =
		evaluate    : /\{\{([\s\S]+?)\}\}/g,
		interpolate : /\{\{=([\s\S]+?)\}\}/g
		escape      : /\{\{-([\s\S]+?)\}\}/g
	app.start(entries);