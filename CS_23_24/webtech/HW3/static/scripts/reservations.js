const database = require('./modules/database');

function getReservations(){
  let sql = "SELECT * FROM book JOIN reservation ON reservation.BookID = book.ID JOIN user ON user.ID = reservation.UserID WHERE username = ? AND password = ?";

  db.get(sql, [username, password], (err, row) => {})
}