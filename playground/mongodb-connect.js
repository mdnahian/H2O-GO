// const MongoClient = require('mongodb').MongoClient;
const {MongoClient, ObjectID} = require('mongodb');

MongoClient.connect('mongodb://localhost:27017/WaterApp', (err, db) => {
  if(err) {
    return console.log("Unable to connect to MongoDB Server");
  }
  console.log("Connect to MongoDB Server");

  db.collection("Water").insertOne({
    country: "Liberia",
    lat: 6.61687,
    lon: -8.82003,
    src: "Borehole",
    pay: "Free",
    tow: "Hand pumps",
    year: "Unknown",
    status: "Pump handles are weak"
  }, (err, result) => {
    if(err) {
      return console.log("Unable to insert water", err);
    }

    console.log(result.ops);
  });

  db.close();
});
