// All general functions used by multiple different modules are stored and accessed here.

const fs = require('fs');
const cheerio = require('cheerio');

const functions = {};

functions.userNotification = function(res, filePath, errorType){
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

module.exports = functions;