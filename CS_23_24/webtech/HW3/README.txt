<!-- GROUP INFORMATION -->
Group ID: 1
Group members:
	Daan van der Putten - 8493375;
	Simon de Jong - 8562024;
	Yoeri Hoebe - 4769341;

<!-- DIRECT LINK TO WEBPAGE -->
A direct link to the online web app is not available, due to webtech servers being down.
Please find attached the proof of this, in addition to the email we sent to bring this to the admin's attention.

<!-- DESCRIPTION -->

<!-- USERNAMES AND PASSWORDS -->
user0 (has a lot of reservations){
	hansjeP
	notpassword1
}
user1{
	mysticwhisperer
	jupjupjep
}
user2{
	atlantalover
	ILOVEATLANTASOMUCH
}
user3{
	username
	password2
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
	PRIMARY KEY('id' AUTOINCREMENT)
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