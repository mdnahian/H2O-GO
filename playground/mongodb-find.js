// const MongoClient = require('mongodb').MongoClient;
const {MongoClient, ObjectID} = require('mongodb');

MongoClient.connect('mongodb://localhost:27017/WaterApp', (err, db) => {
  if(err) {
    return console.log("Unable to connect to MongoDB Server");
  }
  console.log("Connect to MongoDB Server");

  db.collection('Water').find().toArray().then((docs) => {
    console.log("Water Data");
    console.log(JSON.stringify(docs, undefined, 2));

  }, (err) => {

    console.log("Unable to fetch water data");
  });

  //db.close();
});
