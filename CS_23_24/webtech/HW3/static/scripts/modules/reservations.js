const database = require('./database');
const path = require('path');

const reservFunctions = {};

reservFunctions.get = function(req, res){
  if(req.session.loggedin){
    const db = database.open();
    let sql = "SELECT book.title, reservation.LendDate FROM book JOIN reservation ON reservation.BookID = book.ID JOIN user ON user.ID = reservation.UserID WHERE username=? AND password=?";

    const username = req.session.username;
    const password = req.session.password;
    db.all(sql, [username, password], (err, rows) => {
      if (err) {
        database.close(db);
        console.error(err.message);
        // res.status(500).send('Internal Server Error');
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

      // Call the function to check book availability inside the callback
      reservFunctions.available(req, res, db, bookID, userID, (bookAvailable) => {
        if (bookAvailable > 0) {
          let today = new Date().toISOString().replace(/T/, ' ').replace(/\..+/, '');
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
                reservFunctions.get(req, res);
              });
            }
          });
        } 
        else {
          console.log('No books available!');
          reservFunctions.get(req, res);
        }
      });
    } 
    else {
      database.close(db);
      res.render(path.join(__dirname, '..', '..', 'views', 'login.ejs'), { session: req.session, problem: 'notfound' });
    }
  });
}

reservFunctions.available = function(req, res, db, bookID, userID, callback) {
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


module.exports = reservFunctions;