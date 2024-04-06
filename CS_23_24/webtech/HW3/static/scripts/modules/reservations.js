const database = require('./database');

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

function makeReservation(){
  const db = database.open();
  let username = req.session.username;
  let sql = "SELECT * FROM user WHERE username = ? AND password = ?";
  var userID;
  db.get(sql, [username], (err, row) =>{
    if (err) {
      database.close(db);
      console.error(err.message);
      // res.status(500).send('Internal Server Error');
      return;
    }
  })

  let sqlInsert = "INSERT INTO 'reservation' VALUES (?, ?, ?)";
}

module.exports = reservFunctions;