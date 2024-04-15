// Signup operations are handled in this file

const database = require('./database');
const check = require('./check-user-details');
const path = require('path');

const signupFunctions = {}; // collect all signup functions

// Function to make a new user
signupFunctions.newUser = function(req, res){
  let db = database.open();
  // push all userinfo into an array to insert it into the database
  let userinfo = [req.body.username, req.body.password, req.body.email, req.body.country, req.body.city, req.body.zip];
  // sql to insert the new user into a database
  let sql = 'INSERT INTO user(username, password, email, country, city, zip) VALUES(?,?,?,?,?,?)';
  const filePath = 'signup';

  check.checkUserDetails(req, res, db, filePath, null, null, (isUserValid) => {
    if (!isUserValid) {
      console.log('user is invalid');
      return;
    }
    else{
      // If all checks are passed, insert the new user into the dababase
      db.run(sql, userinfo, function(err) {
        if (err) {
          return console.error(err.message);
        }
        console.log(`A user has been created with ID ${this.lastID}.`); // Log the new user
        // Set all session variables
        req.session.loggedin = true;
        req.session.username = req.body.username;
        req.session.password = req.body.password;
        req.session.email = req.body.email;
        req.session.country = req.body.country;
        req.session.city = req.body.city;
        req.session.zip = req.body.zip;
        database.close(db);
        res.redirect('profile'); // redirect the user to the profile page
      });
    }
  })
}

module.exports = signupFunctions; // export signup functions