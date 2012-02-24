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
			postal.channel('ajax.error.*').subscribe @render
			@template = options.template
		render: (error) -> 
			message = $ @template { message: error.message }
			@$el.append message
			message.fadeIn 'slow'
			message.delay(3000).fadeOut('slow').hide
	
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
		window: 0
		fetch: (options) ->
			options or= {}
			if options.reset
				@index = 1 
				@lastLength = 0
				@window = 0
			else 
				if @lastLength == @length then return
				@index++
				@lastLength = @length
				@window or= @length
				options.add = true
			options.data or= {}
			options.data[@indexQuerystring] = @index
			success = options.success
			options.success = (model, resp) => 
				@window or= @length
				@trigger 'fetch:end', (@length > 0 and @window <= @length - @lastLength)
				if success then success model, resp
			error = options.error
			options.error = (originalModel, resp, options) => 
				@trigger 'fetch:end', false
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
			'click .more': 'more'
		initialize: (options) ->
			_.bindAll @, 'render', 'search', 'more', 'start', 'end'
			@template = options.template
			@resultTemplate = options.resultTemplate
			postal.channel('scroll.bottom').subscribe @more
			@collection.on 'fetch:start', @start, @
			@collection.on 'fetch:end', @end, @
		render: ->
			@$el.html @template()
			@resultsView = new SearchResultsView
				el: $ '.search-results'
				template: @resultTemplate
				collection: @collection
		search: (event) ->
			@collection.search $('.search-text').val()
			event.preventDefault()
		more: -> @collection.search()
		start: ->
			@$('.spinner').show()
			@$('.more').hide()
		end: (more) ->
			@$('.spinner').hide()
			if more then @$('.more').show() else @$('.more').hide()

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

		@errorView = new ErrorView
			el: $ '#messages'
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

		@router = new Router 
			searchView: @searchView
			aboutView: @aboutView

		@menuView = new MenuView 
			el: $ '#menu'
			router: @router
			template: _.template menuTemplate

		Backbone.history.start()

		@searchResults.reset results