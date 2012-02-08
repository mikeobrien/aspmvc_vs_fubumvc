define ->
    
	class MenuView extends Backbone.View
		initialize: (options) ->
			_.bindAll @, 'render'
			options.router.on 'all', @render, @
			@template = options.template
		render: (route) ->
			@$el.html @template(route: route.split(':')[1])
	
	class AboutView extends Backbone.View
		initialize: (options) ->
			_.bindAll @, 'render'
			@template = options.template
		render: ->
			@$el.html @template()

	class Entry extends Backbone.Model

	class SearchResults extends Backbone.Collection
		model: Entry
		url: '/directory/entries'
		search: (query) -> 
			@fetch url: @url + '?query=' + query

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
			_.bindAll @, 'render'
			@template = options.template
			@collection.on 'reset', @render, @
		render: ->
			@$el.empty()
			@collection.each (result) => 
				view = new SearchResultView 
					template: @template
					model: result
				@$el.append view.render().el

	class SearchView extends Backbone.View
		events:
			'click .search': 'search'
		initialize: (options) ->
			_.bindAll @, 'render', 'search'
			@template = options.template
			@resultTemplate = options.resultTemplate
		render: ->
			@$el.html @template()
			@resultsView = new SearchResultsView
				el: $ '.search-results'
				template: @resultTemplate
				collection: @collection
		search: (e) ->
			@collection.search $('.search-text').val()
			event.preventDefault()

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
		@aboutView = new AboutView
			el: $ '#container'
			template: _.template $('#about-template').html()

		@searchResults = new SearchResults

		@searchView = new SearchView
			el: $ '#container'
			collection: @searchResults
			template: _.template $('#search-template').html()
			resultTemplate: _.template $('#search-result-template').html()

		@router = new Router 
			searchView: @searchView
			aboutView: @aboutView

		@menuView = new MenuView 
			el: $ '#menu'
			router: @router
			template: _.template $('#menu-template').html()

		Backbone.history.start()

		@searchResults.reset results