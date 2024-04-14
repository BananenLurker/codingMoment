// File containing the edit profile function

const path = require('path');
const database = require('./database.js');
const reserv = require('./reservations.js');

const editFunctions = {} // Collect all edit functions

// Fucntion to edit profile
editFunctions.edit = function(req, res){
  // Check if user is logged in, so there is no need to check the password again
  if(req.session.loggedin){
    // Check if there are updated values. If not, use old values
    req.body.username ? newUsername = req.body.username : newUsername = req.session.username;
    req.body.password ? newPassword = req.body.password : newPassword = req.session.password;
    req.body.email ? newEmail = req.body.email : newEmail = req.session.email;
    req.body.country ? newCountry = req.body.country : newCountry = req.session.country;
    req.body.city ? newCity = req.body.city : newCity = req.session.city;
    req.body.zip ? newZip = req.body.zip : newZip = req.session.zip;
  
    let db = database.open();
    reserv.getUserID(req, db, (userID) => {
      if(userID){
        let sql = 'UPDATE user SET username = ?, password = ?, email = ?, country = ?, city = ?, zip = ? WHERE ID = ?';
        // Update values in the database
        db.run(sql, [newUsername, newPassword, newEmail, newCountry, newCity, newZip, userID], (err) => {
          if (err) {
            return console.error(err.message); // log any errors
          }
          else{
            // Update session variables
            req.session.username = newUsername;
            req.session.password = newPassword;
            req.session.email = newEmail;
            req.session.country = newCountry;
            req.session.city = newCity;
            req.session.zip = newZip;
            // redirect to profile to see changes
            res.redirect('/profile');
            database.close(db);
          }
        })
      }
      else{
        // if something fails, simply redirect to the edit-profile screen
        res.redirect('/edit-profile');
        database.close(db);
      }
    })
  }
}

module.exports = editFunctions; // Export edit functions