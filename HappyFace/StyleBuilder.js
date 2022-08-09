var fs = require('fs'), xml2js = require('xml2js');
var parser = new xml2js.Parser();
var ViewsImport = "";
fs.readFile(__dirname + '/SiteVariables.config', function (err, data) {
	parser.parseString(data, function (err, result) {
	    for (var key in result.variables.variable) {
			var ItemKey = result.variables.variable[key].$.key;
			var ItemStyle = result.variables.variable[key].$.style;
			var ItemState = result.variables.variable[key].$.state;
			if (ItemStyle != undefined && ItemState != "false") {
			    ViewsImport += "@import \"" + ItemStyle + "\";\n";
			}
	    }
	    console.log(ViewsImport);
	});
	fs.writeFile(__dirname + '/Content/css/Views/views.scss', ViewsImport, function (err) {
	    if (err) {
	        return console.log(err);
	    }
	    console.log("views.scss file was saved!");
	});
});