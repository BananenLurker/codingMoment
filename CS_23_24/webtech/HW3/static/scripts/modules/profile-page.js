// All operations regarding the profile page are handled here

const path = require('path');
const fs = require('fs');
const cheerio = require('cheerio');

const profileFunctions = {};

profileFunctions.getProfilePage = function(req, res){
  const filePath = path.join(__dirname, '..', '..', 'profile-template.html');
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
}

module.exports = profileFunctions;