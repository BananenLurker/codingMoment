const sqlite = require('sqlite3');
const express = require('express');
const session = require('express-session');
const path = require('path');
const cheerio = require('cheerio');
const fs = require('fs');
const morgan = require('morgan');
const rfs = require('rotating-file-stream');

const app = express();

// // Use padding to fill up day and month entries if they are single digits
// const logNamePadding = num => (num > 9 ? "" : "0") + num;
// // Construct the log name in the format YYYYMMDD-hhmm__[index]
// const logName = (time, index) => {
//   if (!time) return "file.log";

//   var year = time.getFullYear();
//   var month = logNamePadding(time.getMonth());
//   var day = logNamePadding(time.getDate());
//   var hour = logNamePadding(time.getHours());
//   var minute = logNamePadding(time.getMinutes());

//   return `${year}${month}${day}-${hour}${minute}__${index}-file.log`;
// };

// // For the log name: get the current date/time object and use the amount of stored logs as an index
// var accessLogStream = rfs.createStream(logName(new Date(), fs.readdirSync('./log').length), {
//   interval: '1d',
//   size: '100M',
//   path: path.join(__dirname, 'log')
// });

// // If res.statusCode => 400, an error has occurred and this will be sent to console
// // If there are no errors, logging will be done with less detail to save on storage space

// app.use(morgan('dev', {skip: function(req, res) { return res.statusCode < 400 }}));
// app.use(morgan('common', { stream: accessLogStream }));

app.use(session({
	secret: 'secret',
	resave: true,
	saveUninitialized: true
}));

app.use(express.json());
app.use(express.urlencoded({ extended: true }));
app.use((req, res, next) => {
  if(!req.url.includes('assets') && !req.url.includes('css') && !req.url.includes('scripts')){
    // INSERT CODE FOR A PROFILE NAVBAR
    console.log('Page loaded:', req.url);
  }
  next();
});

app.get('/profile-template', function(req, res) {
    fs.readFile(path.join(__dirname, 'static', '404.html'), 'utf8', (err, data) => {
    if (err) {
      console.error('Error reading file:', err);
      res.status(500).send('Internal Server Error');
      return;
    }

    const $ = cheerio.load(data);

    res.send($.html());
  })
})

app.get('/profile-template.html', function(req, res) {
  res.redirect('/404');
})

app.use(express.static(path.join(__dirname, 'static'), {
  extensions: ['html']
}));

app.get('/', function(req, res) {
	res.sendFile(path.join(__dirname + '/static/signup.html'));
});

app.get('/login.css', (req, responseC) => {
  responseC.sendFile(path.join(__dirname, "/static/css/login.css"))
});

app.post('/signup', function(req, res) {
  let db = openDatabase();
  let userinfo = [req.body.username, req.body.password, req.body.email, req.body.country, req.body.city, req.body.zip];
  let sql = 'INSERT INTO user(username, password, email, country, city, zip) VALUES(?,?,?,?,?,?)';
  const filePath = path.join(__dirname, 'static', 'signup.html');

  checkExistance("username", req.body.username, function(usernameExists) {
    if (usernameExists) {
      closeDatabase(db);
      userNotification(res, filePath, "Username already exists!");
      return;
    } 
    else {
      checkExistance("email", req.body.email, function(emailExists) {
        if (emailExists) {
          closeDatabase(db);
          userNotification(res, filePath, "Email already exists!");
          return;
        } 
        else {
          if(checkCharacters(req.body.username, req.body.email)){
            closeDatabase(db);
            userNotification(res, filePath, "Username or email contains excluded characters!");
            return;
          }
          else{
            if(hasUppercase(req.body.username)){
              closeDatabase(db);
              userNotification(res, filePath, "Username cannot contain uppercase letters!");
              return;
            }
            else{
              db.run(sql, userinfo, function(err) {
                if (err) {
                  return console.log(err.message);
                }
                console.log(`A user has been created with ID ${this.lastID}.`);
                req.session.loggedin = true;
                req.session.username = req.body.username;
                req.session.password = req.body.password;
                req.session.email = req.body.email;
                req.session.country = req.body.country;
                req.session.city = req.body.city;
                req.session.zip = req.body.zip;
                closeDatabase(db);
                res.redirect('/profile');
              });
            }
          }
        }
      });
    }
  });
});

app.post('/auth', function(req, res) {
  let db = openDatabase();

  let username = req.body.username;
  let password = req.body.password;

  if (username && password) {
    let sql = "SELECT * FROM user WHERE username = ? AND password = ?";

    db.get(sql, [username, password], (err, row) => {
      if (err) {
        console.error(err.message);
        res.status(500).send('Internal Server Error');
        return;
      }

      if (row) {
        req.session.loggedin = true;
        req.session.username = username;
        req.session.password = password;
        req.session.email = row.email;
        req.session.country = row.country;
        req.session.city = row.city;
        req.session.zip = row.zip;

        res.redirect('/profile');
      }
      else {
        console.log('login failed!');
        res.redirect('/login');
      }
    });
  } 
  else {
    console.log('jup');
    res.redirect('/login');
  }

  closeDatabase(db);
});

app.get('/profile', function(req, res) {
  const filePath = path.join(__dirname, 'static', 'profile-template.html');
  fs.readFile(filePath, 'utf8', (err, data) => {
    if (err) {
      console.error('Error reading file:', err);
      res.status(500).send('Internal Server Error');
      return;
    }

    const $ = cheerio.load(data);

    if(req.session.loggedin){
      $('.profile-info__username').append(req.session.username);
      $('.profile-info__password').append(req.session.password);
      $('.profile-info__email').append(req.session.email);
      $('.profile-info__country').append(req.session.country);
      $('.profile-info__city').append(req.session.city);
      $('.profile-info__zip').append(req.session.zip);
    }
    else{
      $('.temp').append('<p>Please log in <a href="login" class="link--simple">here</a>!</p>');
    }

    res.send($.html());
  });
});

app.use((req, res) => {
  res.status(404).sendFile(path.join(__dirname, 'static', '404.html'));
});

function userNotification(res, filePath, errorType){
  fs.readFile(filePath, 'utf8', (err, data) => {
    if (err) {
      console.error('Error reading file:', err);
      res.status(500).send('Internal Server Error');
      return;
    }

    const $ = cheerio.load(data);
    $('.user-notification').append(errorType);
    res.send($.html());
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

function hasUppercase(str){
  return /^[A-Z]/.test(str);
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

app.listen(8001);

console.log("The app is running on http://localhost:8001/");