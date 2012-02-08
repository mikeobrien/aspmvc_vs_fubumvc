(function() {
  var __hasProp = Object.prototype.hasOwnProperty,
    __extends = function(child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; };

  define(function() {
    var AboutView, Entry, MenuView, Router, SearchResultView, SearchResults, SearchResultsView, SearchView;
    MenuView = (function(_super) {

      __extends(MenuView, _super);

      function MenuView() {
        MenuView.__super__.constructor.apply(this, arguments);
      }

      MenuView.prototype.initialize = function(options) {
        _.bindAll(this, 'render');
        options.router.on('all', this.render, this);
        return this.template = options.template;
      };

      MenuView.prototype.render = function(route) {
        return this.$el.html(this.template({
          route: route.split(':')[1]
        }));
      };

      return MenuView;

    })(Backbone.View);
    AboutView = (function(_super) {

      __extends(AboutView, _super);

      function AboutView() {
        AboutView.__super__.constructor.apply(this, arguments);
      }

      AboutView.prototype.initialize = function(options) {
        _.bindAll(this, 'render');
        return this.template = options.template;
      };

      AboutView.prototype.render = function() {
        return this.$el.html(this.template());
      };

      return AboutView;

    })(Backbone.View);
    Entry = (function(_super) {

      __extends(Entry, _super);

      function Entry() {
        Entry.__super__.constructor.apply(this, arguments);
      }

      return Entry;

    })(Backbone.Model);
    SearchResults = (function(_super) {

      __extends(SearchResults, _super);

      function SearchResults() {
        SearchResults.__super__.constructor.apply(this, arguments);
      }

      SearchResults.prototype.model = Entry;

      SearchResults.prototype.url = '/directory/entries';

      SearchResults.prototype.search = function(query) {
        return this.fetch({
          url: this.url + '?query=' + query
        });
      };

      return SearchResults;

    })(Backbone.Collection);
    SearchResultView = (function(_super) {

      __extends(SearchResultView, _super);

      function SearchResultView() {
        SearchResultView.__super__.constructor.apply(this, arguments);
      }

      SearchResultView.prototype.tagName = 'tr';

      SearchResultView.prototype.events = {
        'click .delete': 'delete'
      };

      SearchResultView.prototype.initialize = function(options) {
        _.bindAll(this, 'render', 'delete');
        this.model.on('destroy', this.remove, this);
        return this.template = options.template;
      };

      SearchResultView.prototype.render = function() {
        this.$el.html(this.template(this.model.toJSON()));
        return this;
      };

      SearchResultView.prototype["delete"] = function() {
        return this.model.destroy({
          wait: true
        });
      };

      return SearchResultView;

    })(Backbone.View);
    SearchResultsView = (function(_super) {

      __extends(SearchResultsView, _super);

      function SearchResultsView() {
        SearchResultsView.__super__.constructor.apply(this, arguments);
      }

      SearchResultsView.prototype.initialize = function(options) {
        _.bindAll(this, 'render');
        this.template = options.template;
        return this.collection.on('reset', this.render, this);
      };

      SearchResultsView.prototype.render = function() {
        var _this = this;
        this.$el.empty();
        return this.collection.each(function(result) {
          var view;
          view = new SearchResultView({
            template: _this.template,
            model: result
          });
          return _this.$el.append(view.render().el);
        });
      };

      return SearchResultsView;

    })(Backbone.View);
    SearchView = (function(_super) {

      __extends(SearchView, _super);

      function SearchView() {
        SearchView.__super__.constructor.apply(this, arguments);
      }

      SearchView.prototype.events = {
        'click .search': 'search'
      };

      SearchView.prototype.initialize = function(options) {
        _.bindAll(this, 'render', 'search');
        this.template = options.template;
        return this.resultTemplate = options.resultTemplate;
      };

      SearchView.prototype.render = function() {
        this.$el.html(this.template());
        return this.resultsView = new SearchResultsView({
          el: $('.search-results'),
          template: this.resultTemplate,
          collection: this.collection
        });
      };

      SearchView.prototype.search = function(e) {
        this.collection.search($('.search-text').val());
        return event.preventDefault();
      };

      return SearchView;

    })(Backbone.View);
    Router = (function(_super) {

      __extends(Router, _super);

      function Router() {
        Router.__super__.constructor.apply(this, arguments);
      }

      Router.prototype.initialize = function(options) {
        this.searchView = options.searchView;
        return this.aboutView = options.aboutView;
      };

      Router.prototype.routes = {
        '': 'search',
        'about': 'about'
      };

      Router.prototype.search = function() {
        return this.searchView.render();
      };

      Router.prototype.about = function() {
        return this.aboutView.render();
      };

      return Router;

    })(Backbone.Router);
    return {
      start: function(results) {
        this.aboutView = new AboutView({
          el: $('#container'),
          template: _.template($('#about-template').html())
        });
        this.searchResults = new SearchResults;
        this.searchView = new SearchView({
          el: $('#container'),
          collection: this.searchResults,
          template: _.template($('#search-template').html()),
          resultTemplate: _.template($('#search-result-template').html())
        });
        this.router = new Router({
          searchView: this.searchView,
          aboutView: this.aboutView
        });
        this.menuView = new MenuView({
          el: $('#menu'),
          router: this.router,
          template: _.template($('#menu-template').html())
        });
        Backbone.history.start();
        return this.searchResults.reset(results);
      }
    };
  });

}).call(this);
