const mongoose = require("mongoose");

const Schema = mongoose.Schema;

const User = new Schema({
  username: {
    type: String,
    required: true,
    unique: true
  },
  password: {
    type: String,
    required: true
  },
  score: {
    type: Number,
    default: 0
  }
});

module.exports = mongoose.model("User", User);
