// Database operations are handled in this file

const sqlite = require('sqlite3');

const dbFunctions = {}; // Collect all database functions

dbFunctions.open = function(){
  // Open the database with read-write permissions
  return db = new sqlite.Database('./library.db', sqlite.OPEN_READWRITE, (err) => {
    if (err){
      console.error(err.message); // Catch any errors and log them
    }
  });
}

// Close the database
dbFunctions.close = function(db){
  db.close((err) => {
    if (err) {
      console.error(err.message); // Catch any errors and log them
    }
  });
}

// This function is here purely to not bloat other files
// and make sure error logging is done consistently and accurately
dbFunctions.problem = function(db, err){
  dbFunctions.close(db);
  console.error(err.message);
  res.status(500).send('Internal Server Error');
  return;
}

module.exports = dbFunctions; // Export database functions