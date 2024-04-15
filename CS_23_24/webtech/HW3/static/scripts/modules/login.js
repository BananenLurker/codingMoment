// Login operations are handled in this file

const database = require('./database.js');
const path = require('path');

const loginFunctions = {} // Collect all login functions

// Function to authorise the user
loginFunctions.authorise = function(req, res){
  let db = database.open();
  // filepath to redirect the user to, should authentication fail
  const filePath = path.join(__dirname, '..', '..', 'views', 'login.ejs');

  let username = req.body.username;
  let password = req.body.password;

  // Check if there is a username and password provided
  if (username && password) {
    let sql = "SELECT * FROM user WHERE username = ? AND password = ?";

    // Use string literals to prevent SQL injections
    db.get(sql, [username, password], (err, row) => {
      if (err) {
        database.problem(db, err); // log any errors
      }

      if (row) { // if a user has been found, authentication has succeeded
        // set all session variables to the ones found in the database
        req.session.loggedin = true;
        req.session.username = username;
        req.session.password = password;
        req.session.email = row.email;
        req.session.country = row.country;
        req.session.city = row.city;
        req.session.zip = row.zip;

        database.close(db);
        res.redirect('profile'); // redirect to the users' profile page
      }
      else { // no user was found, authentication has failed
        database.close(db);
        // render the login page, give the user feedback that this username
        // and password combination was not found in the database
        res.render(filePath, { session: req.session, problem: 'notfound' });
        return;
      }
    });
  } 
  else { // no username or no password was provided
    database.close(db);
    // render the login page, act like the combination was not found
    res.render(filePath, { session: req.session, problem: 'notfound' });
  }
}

// function to logout
loginFunctions.logout = function(req, res){
  // Reset every session value to their starting values, either false or null
  req.session.loggedin = false;
  req.session.password = null;
  req.session.username = null;
  req.session.email = null;
  req.session.country = null;
  req.session.city = null;
  req.session.zip = null;
  // redirect to the root. This updates the 'login' at the topright
  // of the screen.
  res.redirect('catalogue');
}

module.exports = loginFunctions; // Export login functions