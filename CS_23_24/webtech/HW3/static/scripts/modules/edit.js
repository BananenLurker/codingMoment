// File containing the edit profile function

const path = require('path');
const database = require('./database.js');
const reserv = require('./reservations.js');
const check = require('./check-user-details.js');

const editFunctions = {} // Collect all edit functions

// Fucntion to edit profile
editFunctions.edit = function(req, res) {
  let db = database.open();
  // Check if user is logged in, so there is no need to check the password again
  if (req.session.loggedin) {
    const variables = ['username', 'password', 'email', 'country', 'city', 'zip'];
    const newVariables = [];
    for (let str of variables) {
      console.log(req.body[str]);
      // Push the value directly into newVariables array
      // Use the value from req.body if present, otherwise use the value from session
      newVariables.push(req.body[str] || req.session[str]);
    }

    const filePath = 'edit-profile';
    check.checkUserDetails(req, res, db, filePath, req.session.username, req.session.email, (isUserValid) => {
      if (!isUserValid) {
        return; // We only need to terminate the function, as the error handling is done by check.checkUserDetails
      } 
      else {
        console.log('user is valid');
        reserv.getUserID(req, db, (userID) => {
          if (userID) {
            let sql = 'UPDATE user SET username = ?, password = ?, email = ?, country = ?, city = ?, zip = ? WHERE ID = ?';
            // Update values in the database
            db.run(sql, [newVariables[0], newVariables[1], newVariables[2], newVariables[3], newVariables[4], newVariables[5], userID], (err) => {
              if (err) {
                return console.error(err.message); // log any errors
              } 
              else {
                // Update session variables
                var i = 0;
                for (let str of variables) {
                  req.session[str] = newVariables[i];
                  i++;
                }
                // redirect to profile to see changes
                res.redirect('/profile');
                database.close(db);
              }
            })
          }
        })
      }
    })
  }
  else{
    // if something fails, simply redirect to the edit-profile screen
    res.redirect('/edit-profile');
    database.close(db);
  }
}

editFunctions.checking = function(){

}

module.exports = editFunctions; // Export edit functions