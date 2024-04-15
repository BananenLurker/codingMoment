// The functions that check user information are stored here

const database = require('./database');

const checkFunctions = {};

checkFunctions.checkUserDetails = function(req, res, db, filePath, oldUsername, oldEmail, callback) {
  // Check if the username already exists
  checkExistance("username", req.body.username, oldUsername, oldEmail, function(usernameExists) {
    if (usernameExists) { // Using a callback to force sync in database operations
      database.close(db);
      // If there already exists a user with this username, let the user know
      res.render(filePath, {session: req.session, problem: "Username already exists!"});
      callback(false);
    }
    else {
      // Check if the email already exists
      checkExistance("email", req.body.email, oldUsername, oldEmail, function(emailExists) {
        if (emailExists) { // Again, using a callback to force sync in database operations
          database.close(db);
          // Give the user feedback
          res.render(filePath, {session: req.session, problem: "Email already exists!"});
          callback(false);
        }
        // Check if the password passes the check, but only if there is
        // a new password. Otherwise, this will always callback false.
        else if(req.body.password && !checkPassword(req.body.password)){
          database.close(db);
          // Give the user feedback
          res.render(filePath, {session: req.session, problem: "Passwords should include one number, one special character, one uppercase and one lowercase letter and be at least 16 characters long!"});
          callback(false);
        }
        // Check if there are invalid characters in the username or email
        else if(!checkCharacters(req.body.username) && !checkCharacters(req.body.email)){
          database.close(db);
          // Give the user feedback
          res.render(filePath, {session: req.session, problem: "Username or email contains excluded characters (only a-z and 0-9 allowed, no capitals)!"});
          callback(false);
        }
        // Check if there are invalid characters in any other user-provided information,
        // sanitizing the inputs to prevent XSS.
        else if(!checkOther([req.body.email, req.body.country, req.body.city, req.body.zip])){
          database.close(db);
          // Give the user feedback
          res.render(filePath, {session: req.session, problem: "Please enter a valid email, country, city and ZIP code!"});
          callback(false);
        }
        else{
          callback(true);
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
function checkExistance(rowname, value, oldUsername, oldEmail, callback) {
  let sql = `SELECT * FROM user WHERE ${rowname} = ?`; // insert either email or username into the sql...

  // ... while preventing SQL injections using string literals
  db.get(sql, [value], (err, row) => {
    if (err) {
      return console.error(err.message); // log any errors
    }
    if (row) { // if there is an entry, check if it is the user's own entry
      if(rowname === 'username' && oldUsername === value){
        callback(false); // if yes, they are allowed to keep their own username
      }
      else if(rowname === 'email' && oldEmail === value){
        callback(false); // if yes, they are allowed to keep their own email
      }
      else{
        callback(true); // if not, they cannot have the same username or email as another person
      }
    } 
    else {
      callback(false); // if there is no entry found, callback false
    }
  });
}

module.exports = checkFunctions;