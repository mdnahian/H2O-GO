var express = require("express");
var bodyParser = require("body-parser");

var {mongoose} = require("./db/mongoose");
var {Water} = require("./models/water");

var app = express();

app.use(bodyParser.json());

app.post('/waters', (req, res) => {
  var water = new Water({
    country: req.body.country,
    lat: req.body.lat,
    lon: req.body.lon,
    src: req.body.src,
    pay: req.body.pay,
    tow: req.body.tow,
    status: req.body.status
  });

  water.save().then((doc) => {
    res.send(doc);
  }, (e) => {
    res.status(400).send(e);
  });
})

app.get('/waters', (req, res) => {
  Water.find().then((waters) => {
    res.send({waters})
  }, (e) => {
    res.status(400).send(e);
  });
});

app.listen(3000, () => {
  console.log("Started on port 3000!");
});
