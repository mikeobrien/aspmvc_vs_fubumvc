(function() {
  var __hasProp = {}.hasOwnProperty,
    __extends = function(child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; };

  define(['jquery', 'underscore', 'backbone', 'postal', 'text!error-template.html', 'text!menu-template.html', 'text!about-template.html', 'text!search-template.html', 'text!search-result-template.html'], function($, _, Backbone, postal, errorTemplate, menuTemplate, aboutTemplate, searchTemplate, searchResultTemplate) {
    var AboutView, Entry, ErrorView, LazyCollection, MenuView, Router, SearchResultView, SearchResults, SearchResultsView, SearchView;
    MenuView = (function(_super) {

      __extends(MenuView, _super);

      MenuView.name = 'MenuView';

      function MenuView() {
        return MenuView.__super__.constructor.apply(this, arguments);
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
    ErrorView = (function(_super) {

      __extends(ErrorView, _super);

      ErrorView.name = 'ErrorView';

      function ErrorView() {
        return ErrorView.__super__.constructor.apply(this, arguments);
      }

      ErrorView.prototype.initialize = function(options) {
        _.bindAll(this, 'render');
        postal.channel('ajax.error.*').subscribe(this.render);
        return this.template = options.template;
      };

      ErrorView.prototype.render = function(error) {
        var message;
        message = $(this.template({
          message: error.message
        }));
        this.$el.append(message);
        message.fadeIn('slow');
        return message.delay(3000).fadeOut('slow').hide;
      };

      return ErrorView;

    })(Backbone.View);
    AboutView = (function(_super) {

      __extends(AboutView, _super);

      AboutView.name = 'AboutView';

      function AboutView() {
        return AboutView.__super__.constructor.apply(this, arguments);
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

      Entry.name = 'Entry';

      function Entry() {
        return Entry.__super__.constructor.apply(this, arguments);
      }

      return Entry;

    })(Backbone.Model);
    LazyCollection = (function(_super) {

      __extends(LazyCollection, _super);

      LazyCollection.name = 'LazyCollection';

      function LazyCollection() {
        return LazyCollection.__super__.constructor.apply(this, arguments);
      }

      LazyCollection.prototype.indexQuerystring = 'index';

      LazyCollection.prototype.index = 1;

      LazyCollection.prototype.lastLength = 0;

      LazyCollection.prototype.window = 0;

      LazyCollection.prototype.fetch = function(options) {
        var error, success,
          _this = this;
        options || (options = {});
        if (options.reset) {
          this.index = 1;
          this.lastLength = 0;
          this.window = 0;
        } else {
          if (this.lastLength === this.length) {
            return;
          }
          this.index++;
          this.lastLength = this.length;
          this.window || (this.window = this.length);
          options.add = true;
        }
        options.data || (options.data = {});
        options.data[this.indexQuerystring] = this.index;
        success = options.success;
        options.success = function(model, resp) {
          _this.window || (_this.window = _this.length);
          _this.trigger('fetch:end', _this.length > 0 && _this.window <= _this.length - _this.lastLength);
          if (success) {
            return success(model, resp);
          }
        };
        error = options.error;
        options.error = function(originalModel, resp, options) {
          _this.trigger('fetch:end', false);
          if (error) {
            return error(originalModel, resp, options);
          }
        };
        this.trigger('fetch:start');
        return Backbone.Collection.prototype.fetch.call(this, options);
      };

      return LazyCollection;

    })(Backbone.Collection);
    SearchResults = (function(_super) {

      __extends(SearchResults, _super);

      SearchResults.name = 'SearchResults';

      function SearchResults() {
        return SearchResults.__super__.constructor.apply(this, arguments);
      }

      SearchResults.prototype.initialize = function() {
        _.bindAll(this, 'search');
        return this.query = '';
      };

      SearchResults.prototype.model = Entry;

      SearchResults.prototype.url = function() {
        return "entries";
      };

      SearchResults.prototype.search = function(query) {
        this.query = query != null ? query : this.query;
        return this.fetch({
          reset: query != null,
          data: {
            query: this.query
          }
        });
      };

      return SearchResults;

    })(LazyCollection);
    SearchResultView = (function(_super) {

      __extends(SearchResultView, _super);

      SearchResultView.name = 'SearchResultView';

      function SearchResultView() {
        return SearchResultView.__super__.constructor.apply(this, arguments);
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

      SearchResultsView.name = 'SearchResultsView';

      function SearchResultsView() {
        return SearchResultsView.__super__.constructor.apply(this, arguments);
      }

      SearchResultsView.prototype.initialize = function(options) {
        _.bindAll(this, 'render', 'renderResult');
        this.template = options.template;
        this.collection.on('reset', this.render, this);
        return this.collection.on('add', this.renderResult, this);
      };

      SearchResultsView.prototype.render = function() {
        this.$el.empty();
        return this.collection.each(this.renderResult);
      };

      SearchResultsView.prototype.renderResult = function(result) {
        var view;
        view = new SearchResultView({
          template: this.template,
          model: result
        });
        return this.$el.append(view.render().el);
      };

      return SearchResultsView;

    })(Backbone.View);
    SearchView = (function(_super) {

      __extends(SearchView, _super);

      SearchView.name = 'SearchView';

      function SearchView() {
        return SearchView.__super__.constructor.apply(this, arguments);
      }

      SearchView.prototype.events = {
        'click .search': 'search',
        'click .more': 'more'
      };

      SearchView.prototype.initialize = function(options) {
        _.bindAll(this, 'render', 'search', 'more', 'start', 'end');
        this.template = options.template;
        this.resultTemplate = options.resultTemplate;
        postal.channel('scroll.bottom').subscribe(this.more);
        this.collection.on('fetch:start', this.start, this);
        return this.collection.on('fetch:end', this.end, this);
      };

      SearchView.prototype.render = function() {
        this.$el.html(this.template());
        return this.resultsView = new SearchResultsView({
          el: $('.search-results'),
          template: this.resultTemplate,
          collection: this.collection
        });
      };

      SearchView.prototype.search = function(event) {
        this.collection.search($('.search-text').val());
        return event.preventDefault();
      };

      SearchView.prototype.more = function() {
        return this.collection.search();
      };

      SearchView.prototype.start = function() {
        this.$('.spinner').show();
        return this.$('.more').hide();
      };

      SearchView.prototype.end = function(more) {
        this.$('.spinner').hide();
        if (more) {
          return this.$('.more').show();
        } else {
          return this.$('.more').hide();
        }
      };

      return SearchView;

    })(Backbone.View);
    Router = (function(_super) {

      __extends(Router, _super);

      Router.name = 'Router';

      function Router() {
        return Router.__super__.constructor.apply(this, arguments);
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
        var container;
        container = $('#container');
        this.errorView = new ErrorView({
          el: $('#messages'),
          template: _.template(errorTemplate)
        });
        this.aboutView = new AboutView({
          el: container,
          template: _.template(aboutTemplate)
        });
        this.searchResults = new SearchResults;
        this.searchView = new SearchView({
          el: container,
          collection: this.searchResults,
          template: _.template(searchTemplate),
          resultTemplate: _.template(searchResultTemplate)
        });
        this.router = new Router({
          searchView: this.searchView,
          aboutView: this.aboutView
        });
        this.menuView = new MenuView({
          el: $('#menu'),
          router: this.router,
          template: _.template(menuTemplate)
        });
        Backbone.history.start();
        return this.searchResults.reset(results);
      }
    };
  });

}).call(this);
