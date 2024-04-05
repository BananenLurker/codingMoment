// All operations regarding the profile are handled here

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
      $('.header--profile__login').text(req.session.username);
      $('.header--profile__login').attr('onclick', 'location.href="profile"');
      $('.profile-info__username').append(req.session.username);
      $('.profile-info__password').append(req.session.password);
      $('.profile-info__email').append(req.session.email);
      $('.profile-info__country').append(req.session.country);
      $('.profile-info__city').append(req.session.city);
      $('.profile-info__zip').append(req.session.zip);
    }
    else{
      $('.temp').append('<p>Please log in <a href="login" class="link--simple">here</a>!</p>');
      $('.header--profile__login').text('Login');
      $('.header--profile__login').attr('href', 'login');
    }

    res.send($.html());
  });
}

module.exports = profileFunctions;