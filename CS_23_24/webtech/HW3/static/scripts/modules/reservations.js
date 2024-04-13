const database = require('./database');
const path = require('path');

const reservFunctions = {};

reservFunctions.load = function(req, res){
  if(req.session.loggedin){
    const db = database.open();
    let sql = 'SELECT book.title, reservation.LendDate, reservation.returned, reservation.bookID FROM book JOIN reservation ON reservation.BookID = book.ID JOIN user ON user.ID = reservation.UserID WHERE username=? AND password=? ORDER BY Returned';

    const username = req.session.username;
    const password = req.session.password;
    db.all(sql, [username, password], (err, rows) => {
      if (err) {
        database.problem(db, err);
        return;
      }
      database.close(db);
      res.render('reservation-history', { session: req.session, history: rows });
    })
  }
  else{
    res.render('reservation-history', { session: req.session, history: null });
  }
}

reservFunctions.make = function(req, res, bookID) {
  const db = database.open();
  let username = req.session.username;
  let password = req.session.password;
  let sql = "SELECT * FROM user WHERE username = ? AND password = ?";

  db.get(sql, [username, password], (err, row) => {
    if (err) {
      database.problem(db, err);
      return;
    }
    if (!row) {
      database.close(db);
      res.send('Please log in first!');
    }
    else {
      userID = row.ID;
      reservFunctions.inPosession(res, db, bookID, userID, (userHasBook) => {
        if(userHasBook){
          database.close(db);
          res.send('You have this book already ):');
        }
        else{
          reservFunctions.available(res, db, bookID, (bookAvailable) => {
            if(bookAvailable > 0){
              let today = new Date().toISOString().replace(/T.+/, '');
              let sqlInsert = "INSERT INTO reservation(BookID, UserID, LendDate, Returned) VALUES (?,?,?,'false')";
              db.run(sqlInsert, [bookID, userID, today], (err) => {
                if (err) {
                  database.problem(db, err);
                  return;
                }
                else {
                  bookAvailable--;
                  db.run('UPDATE book SET amount = ? WHERE book.ID = ?', [bookAvailable, bookID], (err) => {
                    if (err) {
                      database.problem(db, err);
                      return;
                    }
                    database.close(db);
                    res.send('Success!');
                  });
                }
              });
            }
            else {
              database.close(db);
              res.send('Book is not available ):');
            }
        });
      }  
      });
    }
  });
}

reservFunctions.return = function(req, res, bookID){
  const db = database.open();
  console.log('ik ben begonnen');
  reservFunctions.getUserID(req, db, (userID) => {
    if(userID){
      reservFunctions.inPosession(res, db, bookID, userID, (userHasBook) => {
        console.log(userHasBook);
        if(userHasBook){
          let sqlReturnBook = "UPDATE reservation SET Returned = 'true' WHERE reservation.UserID = ? AND reservation.BookID = ? AND reservation.returned = 'false'";
          let sqlPlusOne = "UPDATE book SET amount = amount + 1 WHERE book.ID = ?";
    
          // We return the book first, setting the 'returned' flag to true, and only then increase the book amount by one.
          // If the system encounters an error while setting the returned flag, we have not yet increased the available amount:
          // Nothing has changed and the user can try returning the book again. If we did this the other way around, we would
          // conjure a new copy of the book out of thin air every time the user tries returning it.
          console.log('ik heb het tot hier gehaald');
          db.run(sqlReturnBook, [userID, bookID], (err) => {
            if (err) {
              console.log('problem');
              database.problem(db, err);
              return;
            }
          })
          db.run(sqlPlusOne, [bookID], (err) => {
            if(err) {
              console.log('problem');
              database.problem(db, err);
              return;
            }
            else{
              res.send(`You have returned the book with Book ID ${bookID}`);
            }
          })
        }
        else{
          res.send('you dont have this book!');
          database.close(db);
        }
      })
    }
    else{
      res.send('userID not found!');
      database.close(db);
    }
  })
}

reservFunctions.available = function(res, db, bookID, callback) {
  let sqlCheck = "SELECT amount FROM book WHERE book.id = ?";
  db.get(sqlCheck, [bookID], (err, row) => {
    if (err) {
      database.problem(db, err);
      return;
    }
    if (row) {
      callback(row.amount);
    } 
    else {
      database.close(db);
      console.error('BookID not found!');
      res.status(500).send('Internal Server Error');
      return;
    }
  });
}

reservFunctions.inPosession = function(res, db, bookID, userID, callback){
  console.log(bookID, userID);
  let sqlCheck = "SELECT * FROM reservation WHERE reservation.BookID = ? AND reservation.UserID = ? AND reservation.Returned = ?";
  db.get(sqlCheck, [bookID, userID, 'false'], (err, row) => {
    if (err) {
      database.problem(db, err);
      return;
    }
    else if(row){
      callback(true);
    }
    else{
      callback(false);
    }
  })
}

reservFunctions.getUserID = function(req, db, callback){
  let username = req.session.username;
  let password = req.session.password;
  let sql = "SELECT * FROM user WHERE username = ? AND password = ?";
  db.get(sql, [username, password], (err, row) => {
    if (err) {
      database.problem(db, err);
      callback(null);
    }
    if (row) {
      callback(row.ID);
    }
    else{
      callback(null);
    }
  })
}

module.exports = reservFunctions;