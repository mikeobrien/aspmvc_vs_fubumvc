require.config
	paths:
		text: '/scripts/require/text'
		order: '/scripts/require/order'
		jquery: '/scripts/jquery/jquery-1.7.1'

define 'underscore', ['/scripts/underscore/underscore.js'], ->
	_.templateSettings =
		evaluate    : /\{\{([\s\S]+?)\}\}/g,
		interpolate : /\{\{=([\s\S]+?)\}\}/g
		escape      : /\{\{-([\s\S]+?)\}\}/g 
	_

define 'backbone', ['underscore', 'jquery', 'order!/scripts/backbone/backbone.js'], -> Backbone

require ['app', 'entries'], (app, entries) ->
	app.start(entries);