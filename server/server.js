const config = require("./config");

const express = require("express");
const bodyParser = require("body-parser");
const morgan = require("morgan");
const mongoose = require("mongoose");

const Auth = require("./routes/Auth");
const Leaderboard = require("./routes/Leaderboard");

const token = require("./middlewares/token");

mongoose.connect(
  config.DB_URL,
  { useNewUrlParser: true, useCreateIndex: true },
  err => {
    if (err) {
      throw err;
    }

    app.listen(config.PORT, () => {
      console.log("Server listening");
    });

    console.log("Database connected");
  }
);

const app = express();

app.use(morgan("combined"));
app.use(bodyParser.urlencoded({ extended: true }));
app.use(bodyParser.json());

app.use("/auth", Auth);
app.use("/leaderboard", token, Leaderboard);

app.get("/", (req, res) => {
  res.json({
    title: "API",
    message: "Welcome to API",
    code: 202
  });
});
