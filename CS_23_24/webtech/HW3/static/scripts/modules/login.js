// Login operations are handled in this file

const cheerio = require('cheerio');
const database = require('./database.js');

const loginFunctions = {}

loginFunctions.authorise = function(req, res){
  let db = database.open();

  let username = req.body.username;
  let password = req.body.password;

  if (username && password) {
    let sql = "SELECT * FROM user WHERE username = ? AND password = ?";

    db.get(sql, [username, password], (err, row) => {
      if (err) {
        console.error(err.message);
        res.status(500).send('Internal Server Error');
        return;
      }

      if (row) {
        req.session.loggedin = true;
        req.session.username = username;
        req.session.password = password;
        req.session.email = row.email;
        req.session.country = row.country;
        req.session.city = row.city;
        req.session.zip = row.zip;

        res.redirect('/profile');
      }
      else {
        console.log('login failed!');
        res.redirect('/login');
      }
    });
  } 
  else {
    console.log('jup');
    res.redirect('/login');
  }

  database.close(db);
}

module.exports = loginFunctions;