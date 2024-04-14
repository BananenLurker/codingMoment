// These functions handle everything to do with reservations, such as borrowing and returning books

const database = require('./database');

const reservFunctions = {}; // Collect all reservation functions

// Load the reservations to display them in reservation-history
reservFunctions.load = function(req, res){
  if(req.session.loggedin){ // If the user is logged in...
    const db = database.open();
    let sql = 'SELECT book.title, reservation.LendDate, reservation.returned, reservation.bookID FROM book JOIN reservation ON reservation.BookID = book.ID JOIN user ON user.ID = reservation.UserID WHERE username=? AND password=? ORDER BY Returned';

    const username = req.session.username; // ..get the username
    const password = req.session.password; // ..get the password
    db.all(sql, [username, password], (err, rows) => { // get all reservations associated with this account
      if (err) {
        database.problem(db, err); // log any errors
        return;
      }
      database.close(db);
      // render the reservation-history with all reservations
      res.render('reservation-history', { session: req.session, history: rows });
    })
  }
  else{
    // if there are no reservations, render reservation-history with an empty array.
    // This will be caught by reservation-history and displayed correctly
    res.render('reservation-history', { session: req.session, history: null });
  }
}

// Make a new reservation
reservFunctions.make = function(req, res, bookID) {
  const db = database.open();
  let username = req.session.username; // get username and password
  let password = req.session.password; 
  let sql = "SELECT * FROM user WHERE username = ? AND password = ?";

  db.get(sql, [username, password], (err, row) => {
    if (err) {
      database.problem(db, err); // log any errors
      return;
    }
    if (!row) {
      database.close(db);
      // if there is no user with this username and password, disallow lending the book
      res.send('Please log in first!');
    }
    else {
      userID = row.ID;
      reservFunctions.inPosession(res, db, bookID, userID, (userHasBook) => {
        // Check if user already has this book
        if(userHasBook){
          database.close(db);
          // Disallow lending if the user already has this book
          res.send('You have this book already ):');
        }
        else{
          reservFunctions.available(res, db, bookID, (bookAvailable) => {
            // Check if there are any copies available
            if(bookAvailable > 0){
              // If there are, get the date and insert it with the user and borrowed book
              let today = new Date().toISOString().replace(/T.+/, '');
              let sqlInsert = "INSERT INTO reservation(BookID, UserID, LendDate, Returned) VALUES (?,?,?,'false')";
              db.run(sqlInsert, [bookID, userID, today], (err) => { 
                if (err) {
                  database.problem(db, err); // log any errors
                  return;
                }
                else {
                  // decrease the amount of available books locally
                  bookAvailable--;
                  // decrease the amount of books in the database
                  db.run('UPDATE book SET amount = ? WHERE book.ID = ?', [bookAvailable, bookID], (err) => {
                    if (err) {
                      database.problem(db, err); // log any errors
                      return;
                    }
                    database.close(db);
                    res.send('Success!'); // Let the user know reservation was successful
                  });
                }
              });
            }
            else {
              database.close(db);
              res.send('Book is not available ):'); // Let the user know the book was not available
            }
        });
      }  
      });
    }
  });
}

// Return a previously borrowed book
reservFunctions.return = function(req, res, bookID){
  const db = database.open();
  // First, get the userID
  reservFunctions.getUserID(req, db, (userID) => {
    if(userID){
      // If there is a user logged in, check if they have this book in their posession
      reservFunctions.inPosession(res, db, bookID, userID, (userHasBook) => {
        if(userHasBook){
          // If they do, remove it from their account and add 1 to the available books
          let sqlReturnBook = "UPDATE reservation SET Returned = 'true' WHERE reservation.UserID = ? AND reservation.BookID = ? AND reservation.returned = 'false'";
          let sqlPlusOne = "UPDATE book SET amount = amount + 1 WHERE book.ID = ?";
    
          // We return the book first, setting the 'returned' flag to true, and only then increase the book amount by one.
          // If the system encounters an error while setting the returned flag, we have not yet increased the available amount:
          // Nothing has changed and the user can try returning the book again. If we did this the other way around, we would
          // conjure a new copy of the book out of thin air every time the user tries returning it.
          db.run(sqlReturnBook, [userID, bookID], (err) => { // remove from account
            if (err) {
              database.problem(db, err); // log any errors
              return;
            }
          })
          db.run(sqlPlusOne, [bookID], (err) => { // add 1 to the available books
            if(err) {
              database.problem(db, err); // log any errors
              return;
            }
            else{
              // let the user know the book was returned successfully
              res.send(`You have returned the book with Book ID ${bookID}`);
            }
          })
        }
        else{
          // If the user does not own the book, let the user know
          res.send('you dont have this book!');
          database.close(db);
        }
      })
    }
    else{
      // If there was no user logged in, let the user know
      res.send('userID not found!');
      database.close(db);
    }
  })
}

// Check if book is available
reservFunctions.available = function(res, db, bookID, callback) {
  let sqlCheck = "SELECT amount FROM book WHERE book.id = ?"; // SQL to check availability
  db.get(sqlCheck, [bookID], (err, row) => {
    if (err) {
      database.problem(db, err); // log any errors
      return;
    }
    if (row) {
      callback(row.amount); // return the available amount
    } 
    else {
      database.close(db); // close the database prematurely
      console.error('BookID not found!'); // log an error
      res.status(500).send('Internal Server Error'); // make sure the callstack is not shown to the user
      return; // dont return a callback, ending the function that called this one
    }
  });
}

// Check if the user has a book in their possession
reservFunctions.inPosession = function(res, db, bookID, userID, callback){
  let sqlCheck = "SELECT * FROM reservation WHERE reservation.BookID = ? AND reservation.UserID = ? AND reservation.Returned = ?";
  db.get(sqlCheck, [bookID, userID, 'false'], (err, row) => { // using 'false' makes sure already returned books are excluded
    if (err) {
      database.problem(db, err); // log any errors
      return;
    }
    else if(row){
      callback(true); // if the book is found in the user's reservations, callback true
    }
    else{
      callback(false); // if not, callback false
    }
  })
}

// Get the userID associated with a username and password
reservFunctions.getUserID = function(req, db, callback){
  let username = req.session.username;
  let password = req.session.password;
  let sql = "SELECT * FROM user WHERE username = ? AND password = ?";
  db.get(sql, [username, password], (err, row) => {
    if (err) {
      database.problem(db, err); // log any errors
      callback(null);
    }
    if (row) {
      callback(row.ID); // if a user is found, callback the ID
    }
    else{
      callback(null); // if no user is found, callback null, ending the function that called this one
    }
  })
}

module.exports = reservFunctions; // export the reservation functions