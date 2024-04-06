// Database operations are handled in this file

const sqlite = require('sqlite3');

const dbFunctions = {};

dbFunctions.open = function(){
  return db = new sqlite.Database('./users.db', sqlite.OPEN_READWRITE, (err) => {
    if (err){
      console.error(err.message);
    }
    console.log('Connected to the user database.');
  });
}

dbFunctions.close = function(db){
  db.close((err) => {
    if (err) {
      console.error(err.message);
    }
    console.log('Closed the database connection.');
  });
}

module.exports = dbFunctions;