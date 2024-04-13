const express = require('express');
const session = require('express-session');
const path = require('path');
const fs = require('fs');
const morgan = require('morgan');
const rfs = require('rotating-file-stream');
const login = require('./static/scripts/modules/login.js');
const signup = require('./static/scripts/modules/signup.js');
const redirect = require('./static/scripts/modules/redirect.js');
const reservations = require('./static/scripts/modules/reservations.js');
const bookDetails = require('./static/scripts/book-details.js');
const database = require('./static/scripts/modules/database');

const app = express();

// //// START LOGGING

// // <DEV NOTE>
// // Due to the nature of interactive file naming using RFS and Morgan, logging
// // can not be moved to a different module file and has to stay top-level.

// // Use padding to fill up day and month entries if they are single digits
// const logNamePadding = num => (num > 9 ? "" : "0") + num;
// // Construct the log name in the format YYYYMMDD-hhmm__[index]
// const logPath = path.join(__dirname, 'log');
// function logName(time, index) {
//   // On first startup, there will be no time or index available. However, we don't want to
//   // override any log files. Thus, we append the amount of log files, using a ~ to represent
//   // startup logs. If, for whatever reason, the current time is not accessible, we append the current
//   // index, which will always be available after startup and will never loop, making sure log files
//   // are never lost.
//   if (!time && !index) return `${fs.readdirSync(logPath).length}~access.log`;
//   if (!time) return `${index}-access.log`;

//   var year = time.getFullYear();
//   var month = logNamePadding(time.getMonth() + 1); // Months are zero-indexed
//   var day = logNamePadding(time.getDate());
//   var hour = logNamePadding(time.getHours());
//   var minute = logNamePadding(time.getMinutes());

//   return `${year}${month}${day}-${hour}${minute}__${index}-access.log`;
// };

// // For the log name: get the current date/time object and use the amount of stored logs as an index
// const logStream = rfs.createStream(logName, {
//   interval: '1d',
//   size: '100M',
//   path: logPath
// });

// // If res.statusCode => 400, an error has occurred and this will be sent to console
// // If there are no errors, logging will be done with less detail to save on storage space

// app.use(morgan('dev', {skip: function(req, res) { return res.statusCode < 400 }}));
// app.use(morgan('common', { stream: logStream }));

// //// END LOGGING

//// START MIDDLEWARE

app.set('view engine', 'ejs');
app.set('views', path.join(__dirname, 'static', 'views'));

app.use('/static', express.static(path.join(__dirname, 'static')));

app.use(session({
	secret: 'secret',
	resave: true,
	saveUninitialized: true
}));

app.use(express.json());
app.use(express.urlencoded({ extended: true }));

app.get('/', (req, res) => {
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

app.get('/returnbook', function (req, res) {
  reservations.return(req, res, req.query.bookID);
});

app.get('/book/reservebook', function (req, res) {
  reservations.make(req, res, req.query.bookID);
});

app.get('/book/:bookId', async (req, res) => {
  const bookId = req.params.bookId;
  const bookInfo = await bookDetails.getBookDetails(bookId);
  console.log(req.params);

  if (bookInfo) {
    res.render('book', { session: req.session, book: bookInfo });
  } 
  else {
    redirect.notFound(req, res);
  }
});

app.get('/:page', (req, res) => {
  const page = req.params.page;
  fs.access(`./static/views/${page}.ejs`, fs.F_OK, (err) => {
    if (err) {
      console.error(err);
      redirect.notFound(req, res);
    }
    else if(page.includes('login') || page.includes('signup')){
      res.render(page, { session: req.session, problem: null });
    }
    else if(page.includes('reservation-history')){
      reservations.load(req, res);
    }
    else if(page === 'book'){
      redirect.notFound(req, res);
    }
    else{
      res.render(page, { session: req.session });
    }
  });
});

app.use(express.static(path.join(__dirname, 'static')));

// app.use(express.static(path.join(__dirname, 'static'), {
//   extensions: ['html']
// }));

app.get('/catalogue/books', async (req, res) => {
  const limit = 20;
  const page = req.query.page ? parseInt(req.query.page) : 1;
  const offset = (page - 1) * limit;

  const db = database.open();
  db.all("SELECT * FROM book ORDER BY ID LIMIT ? OFFSET ?", [limit, offset], (err, rows) => {
      database.close(db);
      if (err) {
          console.error('Error fetching books:', err);
          res.status(500).json({error: 'Internal server error'});
      }
      else {
        const books = rows.map(row => ({
          bookId: row.ID,
          title: row.title,
          author: row.author,
          coverImageUrl: row.cover,
          copiesLeft: row.amount
        }));
        res.json(books);
      }
  });
});

app.post('/signup', function(req, res) {
  signup.newUser(req, res);
});

app.post('/auth', function(req, res) {
  login.authorise(req, res);
});

app.get('*', (req, res) => {
  redirect.notFound(req, res);
});

//// END MIDDLEWARE

const server = app.listen(8001, () => {
  const host = server.address().address;
  const port = server.address().port;
  console.log(`Server is running on http://${host}:${port}.`);
})