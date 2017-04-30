var mongoose = require("mongoose");

var Water = mongoose.model("Water",
  new mongoose.Schema ({
    country: {
      type: "String"
    },
    lat: {
      type: "Number"
    },
    lon: {
      type: "Number"
    },
    src: {
      type: "String"
    },
    pay: {
      type: "Boolean"
    },
    tow: {
      type: "String"
    },
    status: {
      type: "String"
    },
    desc: {
      type: "String"
    }
}));


module.exports = {Water};
