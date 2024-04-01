const sqlite = require('sqlite3');
const express = require('express');
const session = require('express-session');
const path = require('path');
const cheerio = require('cheerio');
const fs = require('fs');

const app = express();

app.use(session({
	secret: 'secret',
	resave: true,
	saveUninitialized: true
}));

app.use(express.json());
app.use(express.urlencoded({ extended: true }));
app.use((request, response, next) => {
  // INSERT CODE FOR A PROFILE NAVBAR
  console.log('Page loaded:', request.url);
  next();
});

app.get('/profile-template', function(request, response) {
  response.redirect('/404');
})

app.use(express.static(path.join(__dirname, 'static'), {
  extensions: ['html']
}));

app.get('/', function(request, response) {
	response.sendFile(path.join(__dirname + '/static/signup.html'));
});

app.get('/login.css', (request, responseC) => {
  responseC.sendFile(path.join(__dirname, "/static/css/login.css"))
});

app.post('/signup', function(request, response) {
  let db = openDatabase();
  let userinfo = [request.body.username, request.body.password, request.body.email, request.body.country, request.body.city, request.body.zip];
  let sql = 'INSERT INTO user(username, password, email, country, city, zip) VALUES(?,?,?,?,?,?)';
  const filePath = path.join(__dirname, 'static', 'signup.html');

  checkExistance("username", request.body.username, function(usernameExists) {
    if (usernameExists) {
      closeDatabase(db);
      userNotification(response, filePath, "Username already exists!");
      return;
    } 
    else {
      checkExistance("email", request.body.email, function(emailExists) {
        if (emailExists) {
          closeDatabase(db);
          userNotification(response, filePath, "Email already exists!");
          return;
        } 
        else {
          if(checkCharacters(request.body.username, request.body.email)){
            closeDatabase(db);
            userNotification(response, filePath, "Username or email contains excluded characters!");
            return;
          }
          else{
            db.run(sql, userinfo, function(err) {
              if (err) {
                return console.log(err.message);
              }
              console.log(`A user has been created with ID ${this.lastID}.`);
              request.session.username = request.body.username;
              request.session.password = request.body.password;
              closeDatabase(db);
              response.redirect(302, '/profile');
            });
          }
        }
      });
    }
  });
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
      if (err) {
        console.error(err.message);
        response.status(500).send('Internal Server Error');
        return;
      }

      if (row) {
        request.session.loggedin = true;
        request.session.username = username;
        request.session.email = row.email;
        request.session.country = row.country;
        request.session.city = row.city;
        request.session.zip = row.zip;

        response.redirect('/profile');
      }
      else {
        console.log('login failed!');
        response.redirect('/login');
      }
    });
  } 
  else {
    console.log('jup');
    response.redirect('/login');
  }

  closeDatabase(db);
});

app.get('/profile', function(request, response) {
  const filePath = path.join(__dirname, 'static', 'profile-template.html');
  fs.readFile(filePath, 'utf8', (err, data) => {
    if (err) {
      console.error('Error reading file:', err);
      response.status(500).send('Internal Server Error');
      return;
    }

    const $ = cheerio.load(data);

    if(request.session.loggedin){
      $('.profile-info__username').append(request.session.username);
      $('.profile-info__email').append(request.session.email);
      $('.profile-info__country').append(request.session.country);
      $('.profile-info__city').append(request.session.city);
      $('.profile-info__zip').append(request.session.zip);
    }
    else{
      $('.temp').append('<p>Please log in <a href="login" class="link--simple">here</a>!</p>');
    }

    response.send($.html());
  });
});

app.use((request, response) => {
  response.status(404).sendFile(path.join(__dirname, 'static', '404.html'));
});

function userNotification(response, filePath, errorType){
  fs.readFile(filePath, 'utf8', (err, data) => {
    if (err) {
      console.error('Error reading file:', err);
      response.status(500).send('Internal Server Error');
      return;
    }

    const $ = cheerio.load(data);
    $('.user-notification').append(errorType);
    response.send($.html());
  })
}

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