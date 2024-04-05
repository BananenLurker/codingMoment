// Signup operations are handled in this file

const database = require('./database');
const cheerio = require('cheerio');
const path = require('path');
const fs = require('fs');

const signupFunctions = {};

signupFunctions.newUser = function(req, res){
  let db = database.open();
  let userinfo = [req.body.username, req.body.password, req.body.email, req.body.country, req.body.city, req.body.zip];
  let sql = 'INSERT INTO user(username, password, email, country, city, zip) VALUES(?,?,?,?,?,?)';
  const filePath = path.join(__dirname, '..', '..', 'signup.html');

  checkExistance("username", req.body.username, function(usernameExists) {
    if (usernameExists) {
      database.close(db);
      userNotification(res, filePath, "Username already exists!");
      return;
    } 
    else {
      checkExistance("email", req.body.email, function(emailExists) {
        if (emailExists) {
          database.close(db);
          userNotification(res, filePath, "Email already exists!");
          return;
        } 
        else {
          if(checkCharacters(req.body.username, req.body.email)){
            database.close(db);
            userNotification(res, filePath, "Username or email contains excluded characters!");
            return;
          }
          else if(hasUppercase(req.body.username)){
            database.close(db);
            userNotification(res, filePath, "Username cannot contain uppercase letters!");
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
        }
      });
    }
  });
}

function userNotification(res, filePath, errorType){
  fs.readFileSync(filePath, 'utf8', (err, data) => {
    if (err) {
      console.error('Error reading file:', err);
      res.status(500).send('Internal Server Error');
      return;
    }

    const $ = cheerio.load(data);
    $('.user-notification').append(errorType);
    res.send($.html());
  })
}

function hasUppercase(str){
  return /^[A-Z]/.test(str);
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

function checkCharacters(username, email){
  if(username.includes(" ", ";", "~", "/", ",", "?", "!", "^")){
    console.log("Username or email contains invalid characters");
    return true;
  }
  else{
    return false;
  }
}

module.exports = signupFunctions;