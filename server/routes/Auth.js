const express = require("express");
const bcrypt = require("bcrypt");
const router = express.Router();
const jwt = require("jsonwebtoken");
const config = require("../config");

const User = require("../models/User");

router.get("/", (req, res) => {
  res.send("Auth page");
});

router.post("/register", async (req, res) => {
  const { username, password } = req.body;

  try {
    const existingUser = await User.findOne({ username: username });

    if (existingUser) {
      console.log("User not found");
      throw new Error("User already exist!");
    }

    const hashedPassword = await bcrypt.hash(password, 12).catch(() => {
      console.log("Error");
      throw new Error("Something wrong");
    });

    const user = new User({
      username: username,
      password: hashedPassword
    });

    const result = await user.save();

    if (result) {
      res.json({
        message: "User created",
        code: 1
      });
    }
  } catch (err) {
    res.json({
      message: err.message,
      code: 0
    });
  }
});

router.post("/login", async (req, res) => {
  const { username, password } = req.body;

  try {
    const user = await User.findOne({ username: username });

    if (!user) {
      throw new Error("User does not found!");
    }

    const isPasswordEqual = await bcrypt
      .compare(password, user.password)
      .catch(() => {
        throw new Error("Something wrong");
      });

    if (!isPasswordEqual) {
      throw new Error("Password is incorrect!");
    }

    const token = jwt.sign(
      { username: username, userId: user.id },
      config.SECRET_KEY
    );

    res.json({
      code: 1,
      message: token
    });
  } catch (err) {
    res.json({
      message: err.message,
      code: 0
    });
  }
});

module.exports = router;
