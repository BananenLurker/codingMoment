const fs = require('fs');
const cheerio = require('cheerio');
const path = require('path');

const redirectFunctions = {};

redirectFunctions.notFound = function(req, res){
  fs.readFile(path.join(__dirname, '..', '..', 'views','404.ejs'), 'utf8', (err, data) => {
    if (err) {
      console.error('Error reading file:', err);
      res.status(500).send('Internal Server Error');
      return;
    }
  
    const $ = cheerio.load(data);
  
    res.send($.html());
  })
}

module.exports = redirectFunctions;