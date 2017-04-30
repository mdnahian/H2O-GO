var express = require("express");
var bodyParser = require("body-parser");
var {ObjectID} = require('mongodb');

var {mongoose} = require("./db/mongoose");
var {Water} = require("./models/water");

var app = express();
const port = process.env.PORT || 3000

app.use(bodyParser.json());

app.post('/waters', (req, res) => {
  var water = new Water({
    country: req.body.country,
    lat: req.body.lat,
    lon: req.body.lon,
    src: req.body.src,
    pay: req.body.pay,
    tow: req.body.tow,
    status: req.body.status,
    desc: req.body.desc
  });

  water.save().then((doc) => {
    res.send(doc);
    console.log(water);
  }, (e) => {
    res.status(400).send(e);
  });
})

var waterPop = new Water({
    country: "Liberia",
    lat: 6.61687,
    lon: -8.82003,
    src: "Borehole",
    pay: false,
    tow: "Hand Pumps",
    status: "Pump handles are weak",
    desc: "This is a good water to drink"
  });


app.get('/waters', (req, res) => {
  Water.find().then((waters) => {
    res.send({waters})
  }, (e) => {
    res.status(400).send(e);
  });
});

// GET Waters
app.get('/waters/:id', (req, res) => {
  var id = req.params.id;

  if(!ObjectID.isValid(id)) {
    return res.status(404).send();
  }

  Water.findById(id).then((water) => {
    if(!water) {
      return res.status(404).send();
    }

    res.send({water});
  }).catch((e) => {
    res.status(400).send();
  });

});

app.listen(port, () => {
  console.log("Started on port: " + port);
});








