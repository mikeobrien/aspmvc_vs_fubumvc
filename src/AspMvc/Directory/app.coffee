define ['jquery', 'underscore', 'backbone', 'postal',
		'text!error-template.html',
		'text!menu-template.html', 
		'text!about-template.html', 
		'text!search-template.html', 
		'text!search-result-template.html'
		], ($, _, Backbone, postal, errorTemplate, menuTemplate, aboutTemplate, searchTemplate, searchResultTemplate) ->

	class MenuView extends Backbone.View
		initialize: (options) ->
			_.bindAll @, 'render'
			options.router.on 'all', @render, @
			@template = options.template
		render: (route) ->
			@$el.html @template(route: route.split(':')[1])

	class ErrorView extends Backbone.View
		initialize: (options) ->
			_.bindAll @, 'render'
			options.errorChannel.subscribe @render
			@template = options.template
		render: (message) -> 
			error = $ @template { message: message }
			@$el.append error
			error.fadeIn 'slow'
			error.delay(3000).fadeOut('slow').hide
	
	class AboutView extends Backbone.View
		initialize: (options) ->
			_.bindAll @, 'render'
			@template = options.template
		render: ->
			@$el.html @template()

	class Entry extends Backbone.Model

	class LazyCollection extends Backbone.Collection
		indexQuerystring: 'index'
		index: 1
		lastLength: 0
		fetch: (options) ->
			options or= {}
			if options.reset
				@index = 1 
				@lastLength = 0
			else 
				if @lastLength == @length then return
				@lastLength = @length
				@index++
				options.add = true
			options.data or= {}
			options.data[@indexQuerystring] = @index
			success = options.success
			options.success = (model, resp) => 
				@trigger 'fetch:end'
				if success then success model, resp
			error = options.error
			options.error = (originalModel, resp, options) => 
				@trigger 'fetch:end'
				if error then error originalModel, resp, options
			@trigger 'fetch:start'
			Backbone.Collection.prototype.fetch.call @, options

	class SearchResults extends LazyCollection
		initialize: ->
			_.bindAll @, 'search'
			@query = ''
		model: Entry
		url: -> "/directory/entries"
		search: (query) -> 
			@query = query ? @query
			@fetch { reset: query?, data: { query: @query } }

	class SearchResultView extends Backbone.View
		tagName: 'tr'
		events:
			'click .delete': 'delete'
		initialize: (options) ->
			_.bindAll @, 'render', 'delete'
			@model.on 'destroy', @remove, @
			@template = options.template
		render: -> 
			@$el.html @template(@model.toJSON())
			@
		delete: ->
			@model.destroy wait: true

	class SearchResultsView extends Backbone.View
		initialize: (options) ->
			_.bindAll @, 'render', 'renderResult'
			@template = options.template
			@collection.on 'reset', @render, @
			@collection.on 'add', @renderResult, @
		render: ->
			@$el.empty()
			@collection.each @renderResult
		renderResult: (result) ->
			view = new SearchResultView 
				template: @template
				model: result
			@$el.append view.render().el

	class SearchView extends Backbone.View
		events:
			'click .search': 'search'
		initialize: (options) ->
			_.bindAll @, 'render', 'search', 'toggleSpinner'
			@template = options.template
			@resultTemplate = options.resultTemplate
			options.scrollChannel.subscribe @collection.search
			@collection.on 'fetch:start', @toggleSpinner, @
			@collection.on 'fetch:end', @toggleSpinner, @
		render: ->
			@$el.html @template()
			@resultsView = new SearchResultsView
				el: $ '.search-results'
				template: @resultTemplate
				collection: @collection
		search: (event) ->
			@collection.search $('.search-text').val()
			event.preventDefault()
		toggleSpinner: ->
			@$('.spinner').toggle()

	class Router extends Backbone.Router
		initialize: (options) ->
			@searchView = options.searchView
			@aboutView = options.aboutView
		routes:	
			'': 'search'
			'about': 'about'
		search: ->
			@searchView.render()
		about: ->
			@aboutView.render()

	start: (results) ->

		container = $ '#container'

		errorChannel = postal.channel 'error'
		$(document).ajaxError (error, xhr, settings, thrownError) ->
			message = thrownError unless xhr.status == 0
			message = 'Unable to communicate with the server. ' + 
					  'Make sure you are connected to the internet and try again.' unless xhr.status > 0
			errorChannel.publish message

		scrollChannel = postal.channel 'scroll.bottom'

		# TODO: Make this a jQuery plugin
		$window = $ window
		$document = $ document
		$window.scroll ->
			scrollTop = $window.scrollTop()
			scrollChannel.publish() unless scrollTop == 0 or scrollTop < ($document.height() - $window.height()) - 100

		@errorView = new ErrorView
			el: $ '#messages'
			errorChannel: errorChannel
			template: _.template errorTemplate

		@aboutView = new AboutView
			el: container
			template: _.template aboutTemplate

		@searchResults = new SearchResults

		@searchView = new SearchView
			el: container
			collection: @searchResults
			template: _.template searchTemplate
			resultTemplate: _.template searchResultTemplate
			scrollChannel: scrollChannel

		@router = new Router 
			searchView: @searchView
			aboutView: @aboutView

		@menuView = new MenuView 
			el: $ '#menu'
			router: @router
			template: _.template menuTemplate

		Backbone.history.start()

		@searchResults.reset results