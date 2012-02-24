define ['jquery', 'postal'], ($, postal) ->
	top = true
	bottom = false
	options = 
		topOffset: 0
		bottomOffset: 0
	$window = $ window
	$document = $ document
	$window.scroll ->
		topPosition = $window.scrollTop()
		bottomPosition = topPosition + $window.height()
		if topPosition <= options.topOffset
			if top then return	
			postal.channel('scroll.top').publish()
			top = true
			bottom = false
		else if bottomPosition >= $document.height() - options.bottomOffset
			if bottom then return
			postal.channel('scroll.bottom').publish() 
			top = false
			bottom = true
		else 
			top = false
			bottom = false
	options