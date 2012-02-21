(function() {
  var __hasProp = Object.prototype.hasOwnProperty,
    __extends = function(child, parent) { for (var key in parent) { if (__hasProp.call(parent, key)) child[key] = parent[key]; } function ctor() { this.constructor = child; } ctor.prototype = parent.prototype; child.prototype = new ctor; child.__super__ = parent.prototype; return child; };

  define(['jquery', 'underscore', 'backbone', 'postal', 'text!error-template.html', 'text!menu-template.html', 'text!about-template.html', 'text!search-template.html', 'text!search-result-template.html'], function($, _, Backbone, postal, errorTemplate, menuTemplate, aboutTemplate, searchTemplate, searchResultTemplate) {
    var AboutView, Entry, ErrorView, LazyCollection, MenuView, Router, SearchResultView, SearchResults, SearchResultsView, SearchView;
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
    ErrorView = (function(_super) {

      __extends(ErrorView, _super);

      function ErrorView() {
        ErrorView.__super__.constructor.apply(this, arguments);
      }

      ErrorView.prototype.initialize = function(options) {
        _.bindAll(this, 'render');
        options.errorChannel.subscribe(this.render);
        return this.template = options.template;
      };

      ErrorView.prototype.render = function(message) {
        var error;
        error = $(this.template({
          message: message
        }));
        this.$el.append(error);
        error.fadeIn('slow');
        return error.delay(3000).fadeOut('slow').hide;
      };

      return ErrorView;

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
    LazyCollection = (function(_super) {

      __extends(LazyCollection, _super);

      function LazyCollection() {
        LazyCollection.__super__.constructor.apply(this, arguments);
      }

      LazyCollection.prototype.indexQuerystring = 'index';

      LazyCollection.prototype.index = 1;

      LazyCollection.prototype.lastLength = 0;

      LazyCollection.prototype.fetch = function(options) {
        var error, success,
          _this = this;
        options || (options = {});
        if (options.reset) {
          this.index = 1;
          this.lastLength = 0;
        } else {
          if (this.lastLength === this.length) return;
          this.lastLength = this.length;
          this.index++;
          options.add = true;
        }
        options.data || (options.data = {});
        options.data[this.indexQuerystring] = this.index;
        success = options.success;
        options.success = function(model, resp) {
          _this.trigger('fetch:end');
          if (success) return success(model, resp);
        };
        error = options.error;
        options.error = function(originalModel, resp, options) {
          _this.trigger('fetch:end');
          if (error) return error(originalModel, resp, options);
        };
        this.trigger('fetch:start');
        return Backbone.Collection.prototype.fetch.call(this, options);
      };

      return LazyCollection;

    })(Backbone.Collection);
    SearchResults = (function(_super) {

      __extends(SearchResults, _super);

      function SearchResults() {
        SearchResults.__super__.constructor.apply(this, arguments);
      }

      SearchResults.prototype.initialize = function() {
        _.bindAll(this, 'search');
        return this.query = '';
      };

      SearchResults.prototype.model = Entry;

      SearchResults.prototype.url = function() {
        return "/directory/entries";
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

      function SearchView() {
        SearchView.__super__.constructor.apply(this, arguments);
      }

      SearchView.prototype.events = {
        'click .search': 'search'
      };

      SearchView.prototype.initialize = function(options) {
        _.bindAll(this, 'render', 'search', 'toggleSpinner');
        this.template = options.template;
        this.resultTemplate = options.resultTemplate;
        options.scrollChannel.subscribe(this.collection.search);
        this.collection.on('fetch:start', this.toggleSpinner, this);
        return this.collection.on('fetch:end', this.toggleSpinner, this);
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

      SearchView.prototype.toggleSpinner = function() {
        return this.$('.spinner').toggle();
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
        var $document, $window, container, errorChannel, scrollChannel;
        container = $('#container');
        errorChannel = postal.channel('error');
        $(document).ajaxError(function(error, xhr, settings, thrownError) {
          var message;
          if (xhr.status !== 0) message = thrownError;
          if (!(xhr.status > 0)) {
            message = 'Unable to communicate with the server. ' + 'Make sure you are connected to the internet and try again.';
          }
          return errorChannel.publish(message);
        });
        scrollChannel = postal.channel('scroll.bottom');
        $window = $(window);
        $document = $(document);
        $window.scroll(function() {
          var scrollTop;
          scrollTop = $window.scrollTop();
          if (!(scrollTop === 0 || scrollTop < ($document.height() - $window.height()) - 100)) {
            return scrollChannel.publish();
          }
        });
        this.errorView = new ErrorView({
          el: $('#messages'),
          errorChannel: errorChannel,
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
          resultTemplate: _.template(searchResultTemplate),
          scrollChannel: scrollChannel
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
