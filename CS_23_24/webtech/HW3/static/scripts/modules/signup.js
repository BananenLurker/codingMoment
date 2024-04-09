// Signup operations are handled in this file

const database = require('./database');
const path = require('path');

const signupFunctions = {};

signupFunctions.newUser = function(req, res){
  let db = database.open();
  let userinfo = [req.body.username, req.body.password, req.body.email, req.body.country, req.body.city, req.body.zip];
  let sql = 'INSERT INTO user(username, password, email, country, city, zip) VALUES(?,?,?,?,?,?)';
  const filePath = path.join(__dirname, '..', '..', 'views', 'signup.ejs');

  checkExistance("username", req.body.username, function(usernameExists) {
    if (usernameExists) {
      database.close(db);
      res.render(filePath, {session: req.session, problem: "Username already exists!"});
      return;
    } 
    else {
      checkExistance("email", req.body.email, function(emailExists) {
        if (emailExists) {
          database.close(db);
          res.render(filePath, {session: req.session, problem: "Email already exists!"});
          return;
        } 
        else if(!checkCharacters(req.body.username, req.body.email)){
          database.close(db);
          res.render(filePath, {session: req.session, problem: "Username or email contains excluded characters!"});
          return;
        }
        else{
          db.run(sql, userinfo, function(err) {
            if (err) {
              return console.log(err.message);
            }
            console.log(`A user has been created with ID ${this.lastID}.`);
            req.session.loggedin = true;
            req.session.username = req.body.username;
            req.session.password = req.body.password;
            req.session.email = req.body.email;
            req.session.country = req.body.country;
            req.session.city = req.body.city;
            req.session.zip = req.body.zip;
            database.close(db);
            res.redirect('/profile');
          });
      }
      });
    }
  });
}

function checkCharacters(str){
  return /^[A-Za-z0-9]*$/.test(str);
}

function checkExistance(rowname, value, callback) {
  let sql = `SELECT * FROM user WHERE ${rowname} = ?`;
  let username = value;

  db.get(sql, [username], (err, row) => {
    if (err) {
      return console.error(err.message);
    }
    if (row) {
      console.log(`${rowname} already exists!`);
      callback(true);
    } 
    else {
      callback(false);
    }
  });
}

module.exports = signupFunctions;