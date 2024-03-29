const sqlite = require('sqlite3');
const express = require('express');
const session = require('express-session');
const path = require('path');

const app = express();

app.use(session({
	secret: 'secret',
	resave: true,
	saveUninitialized: true
}));

app.use(express.json());
app.use(express.urlencoded({ extended: true }));
app.use(express.static(path.join(__dirname, 'static')));

app.get('/', function(request, response) {
	response.sendFile(path.join(__dirname + '/static/html/signup.html'));
});

app.get('/login.css', (request, responseC) => {
  responseC.sendFile(path.join(__dirname, "/static/css/login.css"))
});

app.post('/signup', function(request, response) {
  let db = openDatabase();
  let userinfo = [request.body.username, request.body.password, request.body.email, request.body.country, request.body.city, request.body.zip];
  let sql = 'INSERT INTO user(username, password, email, country, city, zip) VALUES(?,?,?,?,?,?)';

  checkExistance("username", request.body.username, function(usernameExists) {
    if (usernameExists) {
      return;
    } 
    else {
      checkExistance("email", request.body.email, function(emailExists) {
        if (emailExists) {
          return;
        } 
        else {
          if(checkCharacters(request.body.username, request.body.email)){
            return;
          }
          else{
            db.run(sql, userinfo, function(err) {
              if (err) {
                return console.log(err.message);
              }
              console.log(`A user has been created with ID ${this.lastID}.`);
            });
          }
        }
      });
    }
  });
  closeDatabase(db);
  response.redirect('/');
  response.end();
});

app.post('/auth', function(request, response) {
  let db = openDatabase();

	let user = request.body.username;
	let passw = request.body.password;
	if (user && passw) {

    let sql = "SELECT * FROM user WHERE username = ? AND password = ?";
    let username = user;
    let password = passw;

    db.get(sql, [username, password], (err, row) => {
      if(err){
        return console.error(err.message);
      }
      if (row) {
        console.log(row.ID, row.username);
      } 
      else {
        console.log('login failed!');
      }
    });

    response.redirect('/');
    response.end();

	// 	connection.query('SELECT * FROM accounts WHERE username = ? AND password = ?', [username, password], function(error, results, fields) {
	// 		if (error) throw error;

	// 		if (results.length > 0) {
	// 			request.session.loggedin = true;
	// 			request.session.username = username;
	// 			response.redirect('/home');
	// 		} 
  //     else {
	// 			response.send('Incorrect Username and/or Password!');
	// 		}
	// 		response.end();
	// 	});
	// } 
  // else {
	// 	response.send('Please enter Username and Password!');
	// 	response.end();
	// }
  }
  closeDatabase(db);
});


app.get('/home', function(request, response) {
	if (request.session.loggedin) {
		response.send('Welcome back, ' + request.session.username + '!');
	} 
  else {
		response.send('Please login to view this page!');
	}
	response.end();
});

function openDatabase(){
  return db = new sqlite.Database('./users.db', sqlite.OPEN_READWRITE, (err) => {
    if (err){
      console.error(err.message);
    }
    console.log('Connected to the user database.');
  });
}

function closeDatabase(db){
  db.close((err) => {
    if (err) {
      console.error(err.message);
    }
    console.log('Closed the database connection.');
    console.log('--------------------------------');
  });
}

function checkExistance(rowname, value, callback) {
  let sql = `SELECT * FROM user WHERE ${rowname} = ?`;
  let username = value;

  db.get(sql, [username], (err, row) => {
    if (err) {
      return console.error(err.message);
    }
    if (row) {
      console.log(`${rowname} already exists!`);
      callback(true);
    } 
    else {
      callback(false);
    }
  });
}

function checkCharacters(username, email){
  if(username.includes(" ", ";", "~", "/", ",", "?", "!", "^")){
    console.log("Username or email contains invalid characters");
    return true;
  }
  else{
    return false;
  }
}

app.listen(3000);

console.log("The app is running on http://localhost:3000/");