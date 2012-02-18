require.config
	paths:
		text: '/content/scripts/require/text'
		order: '/content/scripts/require/order'
		jquery: '/content/scripts/jquery/jquery-1.7.1'

define 'underscore', ['/content/scripts/underscore/underscore.js'], ->
	_.templateSettings =
		evaluate    : /\{\{([\s\S]+?)\}\}/g,
		interpolate : /\{\{=([\s\S]+?)\}\}/g
		escape      : /\{\{-([\s\S]+?)\}\}/g 
	_

define 'backbone', ['underscore', 'jquery', 'order!/content/scripts/backbone/backbone.js'], -> Backbone

require ['app', 'entries'], (app, entries) ->
	app.start(entries);