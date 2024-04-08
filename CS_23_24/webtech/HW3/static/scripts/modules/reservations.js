const database = require('./database');
const path = require('path');

const reservFunctions = {};

reservFunctions.load = function(req, res){
  if(req.session.loggedin){
    const db = database.open();
    let sql = "SELECT book.title, reservation.LendDate FROM book JOIN reservation ON reservation.BookID = book.ID JOIN user ON user.ID = reservation.UserID WHERE username=? AND password=? AND reservation.Returned = 'false'";

    const username = req.session.username;
    const password = req.session.password;
    db.all(sql, [username, password], (err, rows) => {
      if (err) {
        database.close(db);
        console.error(err.message);
        res.status(500).send('Internal Server Error');
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
  var userID;

  db.get(sql, [username, password], (err, row) => {
    if (err) {
      database.close(db);
      console.error(err.message);
      res.status(500).send('Internal Server Error');
      return;
    }
    if (row) {
      userID = row.ID;

      reservFunctions.available(res, db, bookID, (bookAvailable) => {
        reservFunctions.inPosession(res, db, bookID, userID, (userHasBook) => {
          if(!userHasBook){
            if (bookAvailable > 0) {
              let today = new Date().toISOString().replace(/T.+/, '');
              let sqlInsert = "INSERT INTO reservation(BookID, UserID, LendDate, Returned) VALUES (?,?,?,'false')";
              db.run(sqlInsert, [bookID, userID, today], (err) => {
                if (err) {
                  database.close(db);
                  console.error(err.message);
                  res.status(500).send('Internal Server Error');
                  return;
                } 
                else {
                  bookAvailable--;
                  db.run('UPDATE book SET amount = ? WHERE book.ID = ?', [bookAvailable, bookID], (err) => {
                    if (err) {
                      console.error(`Book amount could not be updated for bookID ${bookID}`);
                    }
                    database.close(db);
                    console.log('Reservation successful!');
                    res.send('You have lent book ' + bookID);
                  });
                }
              });
            } 
            else {
              database.close(db);
              res.send('There are no books available!');
            }
          }
          else{
            database.close(db);
            res.send('You already have this book!');
          }
        })
      });
    } 
    else {
      database.close(db);
      res.send('Please log in!');
    }
  });
}

reservFunctions.available = function(res, db, bookID, callback) {
  let sqlCheck = "SELECT amount FROM book WHERE book.id = ?";
  db.get(sqlCheck, [bookID], (err, row) => {
    if (err) {
      database.close(db);
      console.error(err.message);
      res.status(500).send('Internal Server Error');
      return;
    }
    if (row) {
      callback(row.amount);
    } 
    else {
      console.error('BookID not found!');
      res.status(500).send('Internal Server Error');
      return;
    }
  });
}

reservFunctions.inPosession = function(res, db, bookID, userID, callback){
  let sqlCheck = "SELECT * FROM reservation WHERE reservation.BookID = ? AND reservation.UserID = ? AND reservation.Returned = ?";
  db.get(sqlCheck, [bookID, userID, 'false'], (err, row) => {
    if (err) {
      database.close(db);
      console.error(err.message);
      res.status(500).send('Internal Server Error');
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

module.exports = reservFunctions;