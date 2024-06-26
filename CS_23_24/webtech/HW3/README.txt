<!-- GROUP INFORMATION -->
Group ID: 1
Group members:
	Daan van der Putten - 8493375;
	Simon de Jong - 8562024;
	Yoeri Hoebe - 4769341;

<!-- DIRECT LINK TO WEBPAGE -->
http://webtech.science.uu.nl/group01/

<!-- DESCRIPTION -->
We have created a web app for a library, where users can register, login, reserve and return books using node.js and express.
The styling is tested to be suitable for desktops (1920x1080) and phones (428x926).
The file structure is as follows:

Top level (general-purpose and application wide files): 
	app.js (entry)
	node.js necessities (PACKAGE and modules)
	users.db
	README.txt
	log/ (logs)
	static/ (files that are gotten with GET requests)
		css/ (css files)
		views/ (EJS files, used instead of HTML)
		scripts/ (JS files)
			modules/ (JS files used in other JS files - f.e. in app.js)
			[vanilla JS files for specific applications]
		assets/ (everything that is not css, JS or EJS)
			covers/ (book cover images)
			team-images/ (images for about page)
			[svgs, gifs, images used in other places than catalogue or about]


<!-- USERNAMES AND PASSWORDS -->
NOTE: some of these users were created before username and password restrictions were installed,
thus they might not follow the restrictions that are imposed upon newly registred users.

user0 (has a lot of reservations){
	hansjeP
	notpassword1
}
user1{
	mysticwhisperer
	OHMYGODILOVEMYNEWPASSWORDSOMUCH123yes!
}
user2{
	atlantalover15
	ILOVEATLANTASOMUCH
}
user3{
	user3
	ThisIsUser3sPassword!
}
user4{
	isthisjokegettingstale
	yesitis
}

<!-- DDL STATEMENTS -->

CREATE TABLE IF NOT EXISTS 'user'(
	'ID' INTEGER NOT NULL,
	'username' VARCHAR(50) NOT NULL UNIQUE,
	'password' VARCHAR(255) NOT NULL,
	'email' VARCHAR(100) NOT NULL UNIQUE,
	'country' VARCHAR(100),
	'city' VARCHAR(100),
	'zip' VARCHAR(31),
	PRIMARY KEY('ID' AUTOINCREMENT)
);

CREATE TABLE IF NOT EXISTS 'book'(
	'ID' INTEGER NOT NULL,
	'title' VARCHAR(255) NOT NULL,
	'genre' VARCHAR(100),
	'year' INTEGER,
	'author' VARCHAR(100),
	'publisher' VARCHAR(100),
	'cover' VARCHAR(100),
	'plot' TEXT,
	'amount' INTEGER NOT NULL,
	PRIMARY KEY('ID' AUTOINCREMENT)
);

CREATE TABLE IF NOT EXISTS 'reservation'(
	'ID' INTEGER NOT NULL,
	'BookID' INTEGER NOT NULL,
	'UserID' INTEGER NOT NULL,
	'LendDate' DATE NOT NULL,
	'Returned' BOOL NOT NULL,
	PRIMARY KEY ('ID' AUTOINCREMENT),
	FOREIGN KEY ('UserID') REFERENCES 'user'('ID'),
	FOREIGN KEY ('BookID') REFERENCES 'book'('ID')
);