var express = require('express');
var path = require('path');
var app = express();
var port = 1222 || process.env.port;
var bodyParser = require('body-parser');
var employee = require('./employee/emp');
var package = require('./package/index');


//set view engine
app.set('views', path.join(__dirname, 'views'));
app.set('view engine', 'pug');

app.use("/employee", employee);
app.use("/package", package);

app.use(bodyParser.json);
app.use(bodyParser.urlencoded({
    extended: false
}));

/*app.use(function (req, res) {
    res.status(404).send({ url: req.originalUrl + ' not found' });
});*/
app.get('/', function (req, res) {
    res.render('index.pug');
});

app.listen(port, function () {
    console.log('Running on ' + port);
});