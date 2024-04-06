// Login operations are handled in this file

const database = require('./database.js');
const path = require('path');

const loginFunctions = {}

loginFunctions.authorise = function(req, res){
  let db = database.open();

  const filePath = path.join(__dirname, '..', '..', 'views', 'login.ejs');

  let username = req.body.username;
  let password = req.body.password;

  if (username && password) {
    let sql = "SELECT * FROM user WHERE username = ? AND password = ?";

    db.get(sql, [username, password], (err, row) => {
      if (err) {
        database.close(db);
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

        database.close(db);
        res.redirect('/profile');
      }
      else {
        database.close(db);
        res.render(filePath, { session: req.session, problem: 'notfound' });
        return;
      }
    });
  } 
  else {
    database.close(db);
    res.redirect('/login');
  }
}

module.exports = loginFunctions;