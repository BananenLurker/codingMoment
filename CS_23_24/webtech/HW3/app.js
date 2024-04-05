const express = require('express');
const session = require('express-session');
const path = require('path');
const fs = require('fs');
const cheerio = require('cheerio');
const morgan = require('morgan');
const rfs = require('rotating-file-stream');
const database = require('./static/scripts/modules/database.js');
const login = require('./static/scripts/modules/login.js');
const signup = require('./static/scripts/modules/signup.js');
const redirect = require('./static/scripts/modules/redirect.js');

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

app.use((req, res, next) => {
  if(!req.url.includes('assets') && !req.url.includes('css') && !req.url.includes('profile') && !req.url.includes('scripts') && req.method === 'GET'){
    console.log('page got: ' + req.url);
  }
  next();
});

app.get('/', (req, res) => {
  console.log('rendering template');
  res.render('index.ejs', { username: req.session.username });
});

app.get('/:page', (req, res) => {
  const page = req.params.page;
  console.log(`Rendering ${page} template`);
  try{
    res.render(page, { session: req.session });
  }
  catch{
    redirect.notFound(req, res);
  }
});


app.use(express.static(path.join(__dirname, 'static'), {
  extensions: ['html']
}));

app.post('/signup', function(req, res) {
  signup.newUser(req, res);
});

app.post('/auth', function(req, res) {
  login.authorise(req, res);
});

app.use((req, res) => {
  res.status(404).sendFile(path.join(__dirname, 'static', '404.html'));
});

//// END MIDDLEWARE

const server = app.listen(8001, () => {
  const host = server.address().address;
  const port = server.address().port;
  console.log(`Server is running on http://${host}:${port}.`);
})