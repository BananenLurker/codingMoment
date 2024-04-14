// Signup operations are handled in this file

const database = require('./database');
const path = require('path');

const signupFunctions = {}; // collect all signup functions

// Function to make a new user
signupFunctions.newUser = function(req, res){
  let db = database.open();
  // push all userinfo into an array to insert it into the database
  let userinfo = [req.body.username, req.body.password, req.body.email, req.body.country, req.body.city, req.body.zip];
  // sql to insert the new user into a database
  let sql = 'INSERT INTO user(username, password, email, country, city, zip) VALUES(?,?,?,?,?,?)';
  const filePath = path.join(__dirname, '..', '..', 'views', 'signup.ejs');

  // Check if the username already exists
  checkExistance("username", req.body.username, function(usernameExists) {
    if (usernameExists) { // Using a callback to force sync in database operations
      database.close(db);
      // If there already exists a user with this username, let the user know
      res.render(filePath, {session: req.session, problem: "Username already exists!"});
      return;
    } 
    else {
      // Check if the email already exists
      checkExistance("email", req.body.email, function(emailExists) {
        if (emailExists) { // Again, using a callback to force sync in database operations
          database.close(db);
          // Give the user feedback
          res.render(filePath, {session: req.session, problem: "Email already exists!"});
          return;
        }
        // Check if the password passes the check
        else if(!checkPassword(req.body.password)){
          database.close(db);
          // Give the user feedback
          res.render(filePath, {session: req.session, problem: "Passwords should include one number, one special character, one uppercase and one lowercase letter and be at least 16 characters long!"});
          return;
        }
        // Check if there are invalid characters in the username or email
        else if(!checkCharacters(req.body.username) && !checkCharacters(req.body.email)){
          database.close(db);
          // Give the user feedback
          res.render(filePath, {session: req.session, problem: "Username or email contains excluded characters (only a-z and 0-9 allowed, no capitals)!"});
          return;
        }
        // Check if there are invalid characters in any other user-provided information,
        // sanitizing the inputs to prevent XSS.
        else if(!checkOther([req.body.email, req.body.country, req.body.city, req.body.zip])){
          database.close(db);
          // Give the user feedback
          res.render(filePath, {session: req.session, problem: "Please enter a valid email, country, city and ZIP code!"});
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
            res.redirect('/profile'); // redirect the user to the profile page
          });
      }
      });
    }
  });
}

// Function to check if a password is strong enough.
// Passwords should conform to the following:
// 1 uppercase letter, 1 lowercase letter, 1 number, 1 special character and be at least 16 characters long
function checkPassword(str){
  if(str.match(/[a-z]+/) && str.match(/[A-Z]+/) && str.match(/[0-9]+/) && str.match(/[$@#&!]+/) && str.length > 15){
    return true;
  }
  return false;
}

// Check if there are only lowercase letters and numbers
function checkCharacters(str){
  return /^[a-z0-9]*$/.test(str);
}

// Check if there are only letters, numbers, @'s, fullstops and spaces in an array of strings.
// This is used for the country, city, ZIP code and email.
function checkOther(arr) {
  for (let str of arr) {
    if (!(/^[a-zA-Z0-9@. ]*$/.test(str) && str.length < 100)) {
      return false;
    }
  }
  return true;
}

// Check if a user or email already exists
function checkExistance(rowname, value, callback) {
  let sql = `SELECT * FROM user WHERE ${rowname} = ?`; // insert either email or username into the sql...
  let username = value;

  // ... while preventing SQL injections using string literals
  db.get(sql, [username], (err, row) => {
    if (err) {
      return console.error(err.message); // log any errors
    }
    if (row) {
      callback(true); // if the user or email already exists, callback true
    } 
    else {
      callback(false); // if not, callback false
    }
  });
}

module.exports = signupFunctions; // export signup functions