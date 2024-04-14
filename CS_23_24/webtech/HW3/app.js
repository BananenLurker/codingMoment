const express = require('express');
const session = require('express-session');
const path = require('path');
const fs = require('fs');
const morgan = require('morgan');
const rfs = require('rotating-file-stream');
const login = require('./static/scripts/modules/login.js');
const signup = require('./static/scripts/modules/signup.js');
const reservations = require('./static/scripts/modules/reservations.js');
const bookDetails = require('./static/scripts/book-details.js');
const database = require('./static/scripts/modules/database');

const app = express();

//// START LOGGING

// <DEV NOTE>
// Due to the nature of interactive file naming using RFS, logging
// can not be moved to a different module file and has to stay top-level.

// Use padding to fill up day and month entries if they are single digits
const logNamePadding = num => (num > 9 ? "" : "0") + num;
// Construct the log name in the format YYYYMMDD-hhmm__[index]
const logPath = path.join(__dirname, 'log');
function logName(time, index) {
  // On first startup, there will be no time or index available. However, we don't want to
  // override any log files. Thus, we append the amount of log files, using a ~ to represent
  // startup logs. If, for whatever reason, the current time is not accessible, we append the current
  // index, which will always be available after startup and will never loop, making sure log files
  // are never lost.
  if (!time && !index) return `${fs.readdirSync(logPath).length}~access.log`;
  if (!time) return `${index}-access.log`;

  var year = time.getFullYear();
  var month = logNamePadding(time.getMonth() + 1); // Months are zero-indexed
  var day = logNamePadding(time.getDate());
  var hour = logNamePadding(time.getHours());
  var minute = logNamePadding(time.getMinutes());

  return `${year}${month}${day}-${hour}${minute}__${index}-access.log`;
};

// For the log name: get the current date/time object and use the amount of stored logs as an index
const logStream = rfs.createStream(logName, {
  interval: '1d',
  size: '100M',
  path: logPath
});

// If res.statusCode => 400, an error has occurred and this will be sent to console
// If there are no errors, logging will be done with less detail

app.use(morgan('dev', {skip: function(req, res) { return res.statusCode < 400 }}));
app.use(morgan('common', { stream: logStream }));

//// END LOGGING

//// START MIDDLEWARE

app.set('view engine', 'ejs'); // Use EJS as the view engine
app.set('views', path.join(__dirname, 'static', 'views')); // Direct all render requests to /static/views

app.use('/static', express.static(path.join(__dirname, 'static'))); // Direct all requets to static

app.use(session({ // Set session options
	secret: 'this_is_a_very_important_secret_key_which_is_not_arbitrary_at_all',
	resave: true,
	saveUninitialized: true
}));

app.use(express.json()); // Parse POST requests which are not HTML
app.use(express.urlencoded({ extended: true })); // Parse POST requests which are HTML

app.get('/', (req, res) => {
  // If the root is accessed, we render the catalogue. This is done to preserve the index page,
  // should it ever be necessary to restore.
  res.render('catalogue.ejs', { session: req.session });
});

app.use((req, res, next) => {
  // Remove trailing slash if present
  if (req.path !== '/' && req.path.endsWith('/')) {
    const newPath = req.path.slice(0, -1);
    // Redirect to the new path without the trailing slash
    return res.redirect(301, newPath + req.url.slice(req.path.length));
  }
  // If there's no trailing slash, proceed to the next middleware
  next();
});

app.get('/logging-out', (req, res) =>{
  // Pass control to the logout function
  login.logout(req, res);
});

app.get('/returnbook', function (req, res) {
  // Pass control to the return function
  reservations.return(req, res, req.query.bookID);
});

app.get('/book/reservebook', function (req, res) {
  // Pass control to the reservation function
  reservations.make(req, res, req.query.bookID);
});

app.get('/book/:bookId', async (req, res) => {
  const bookId = req.params.bookId; // Get the bookId from the URL
  const bookInfo = await bookDetails.getBookDetails(bookId); // Get the bookInfo from the database using the bookId

  if (bookInfo) {
    // If there is bookInfo, the ID is valid, so we load the page
    res.render('book', { session: req.session, book: bookInfo });
  } 
  else {
    // If the bookInfo cannot be retrieved, an invalid bookID has been manually inserted by the user.
    // We redirect to book 1 instead of 404 to stay in the book section.
    res.redirect(301, 'book/1');
  }
});

// If the user sends a request to /book without an ID, send them to the book with ID 1
app.get('/book', (req, res) => {
  res.redirect(301, 'book/1');
})

app.get('/:page', (req, res) => {
  const page = req.params.page;
  fs.access(`./static/views/${page}.ejs`, fs.F_OK, (err) => { // Check if file exists
    if (err) {
      console.error(err);
      res.render('404', { session: req. session }); // If file does not exist, pass it to the 404 page
    }
    else if(page.includes('login') || page.includes('signup')){
      // If the login or signup page is requested, EJS needs to know there are currently no user errors to display
      res.render(page, { session: req.session, problem: null });
    }
    else if(page.includes('reservation-history')){
      // reservation-history needs more complex functionality, so we send those requests to their own file
      reservations.load(req, res);
    }
    else{
      res.render(page, { session: req.session }); // The file is readable AND is not a special case
    }
  });
});

// The static directory has to be linked after the views have been rendered,
// as the view directory is already set to /static/views. Setting this
// before the view redirect would result in path /static/static/views.
app.use(express.static(path.join(__dirname, 'static')));

// Get books that are displayed in the catalogue at root
app.get('/catalogue/books', async (req, res) => {
  try{
    const limit = 20; // Limit the amount of books loaded at once
    const page = req.query.page ? parseInt(req.query.page) : 1;
    const offset = (page - 1) * limit; // Get the page of already-loaded books, to not display duplicates
  
    const db = database.open();
    db.all("SELECT * FROM book ORDER BY ID LIMIT ? OFFSET ?", [limit, offset], (err, rows) => {
        database.close(db); // Close the database ASAP to make sure it will always be closed
        if (err) {
            console.error('Error fetching books:', err);
            res.status(500).json({error: 'Internal server error'}); // Dont display the callstack on the users' screen
        }
        else {
          const books = rows.map(row => ({ // Set the loaded books' properties for another file to extract when rendering
            bookId: row.ID,
            title: row.title,
            author: row.author,
            coverImageUrl: row.cover,
            copiesLeft: row.amount
          }));
          res.json(books); // Pass control back to the AJAX requester
        }
    });
  }
  catch(err){
    next(err); // Pass control to error logging
  }
});

app.post('/signup', function(req, res) {
  signup.newUser(req, res); // Redirect to the signup function
});

app.post('/auth', function(req, res) {
  login.authorise(req, res); // Redirect to the login function
});

// If the user is getting a page that is not caught yet, for example
// if the file is corrupt or could not be read by fs, we redirect them to 404.
app.get('*', (req, res) => {
  console.error('User requested unknown page: ' + req.url);
  res.render('404', { session: req. session });
});

//// START ERROR STACK

app.use((err, req, res, next) => {
  console.error(err.stack); // Log every error to console
  next(err);
});

app.use((err, req, res, next) => {
  if(req.xhr){ // Catch AJAX errors
    res.status(500).send('AJAX error!');
  }
  else{
    next(err);
  }
});

// Most functions handle their own error and logging, but in the case an error still persists,
// it will be caught and logged here, without displaying the callstack on a users' screen.
app.use((err, req, res, next) => {
  console.error('An error occurred: ', err); // Log any non-AJAX errors
  res.status(500).send('Internal server error');
});

//// END ERROR STACK
//// END MIDDLEWARE

const server = app.listen(8001, () => {
  const host = server.address().address;
  const port = server.address().port;
  console.log(`Server is running on http://${host}:${port}.`); // Log at what URL the server started
})