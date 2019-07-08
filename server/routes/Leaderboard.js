const express = require("express");
const router = express.Router();

const User = require("../models/User");

router.get("/", (req, res) => {
  res.json({
    message: "Welcome"
  });
});

router.get("/list", async (req, res) => {
  User.find({})
    .sort({ score: -1 })
    .then(users => {
      const result = users.map(user => {
        return { score: user.score, username: user.username };
      });

      res.json({
        message: JSON.stringify(result),
        code: 1
      });
    })
    .catch(err => {
      throw err;
    });
});

router.put("/update", async (req, res) => {
  const { score } = req.body;

  try {
    if (score) {
      const user = await User.findOne({ username: req.username });

      if (!user) {
        throw new Error("User does not exist!");
      }

      if (score > user.score) {
        user.score = score;

        const result = await user.save();

        if (result) {
          res.json({
            message: "Succesfuly updated"
          });
        } else {
          throw new Error("Somethings wrong");
        }
      } else {
        res.json({
          message: "Succesfuly updated"
        });
      }
    } else {
      throw new Error("Somethings wrong");
    }
  } catch (error) {
    res.json({
      message: error.message
    });
  }
});

module.exports = router;
