var homeSearch = homeSearch || {};
(function (ui) {	

    var app = homeSearch;

	app.init = function(){
	    app.get();
	};
	app.get = function () {

	};


})(homeSearch);

$(document).ready(function(){
	homeSearch.init();
});