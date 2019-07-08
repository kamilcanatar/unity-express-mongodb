const jwt = require("jsonwebtoken");
const config = require("../config");

module.exports = (req, res, next) => {
  const token =
    req.headers["x-access-token"] ||
    req.body.token ||
    req.query.token ||
    req.get("Authorization");

  if (token) {
    jwt.verify(token, config.SECRET_KEY, (err, verified) => {
      if (err) {
        res.json({
          code: 0,
          message: "Token failed."
        });
      } else {
        req.token = verified;
        req.username = verified.username;
        next();
      }
    });
  } else {
    res.json({
      code: 0,
      message: "Token missing!"
    });
  }
};
