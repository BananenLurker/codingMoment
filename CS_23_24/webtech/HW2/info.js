// Creating classes

class Person{
  constructor(name, yearOfBirth){
    this.name = name;
    this.yearOfBirth = yearOfBirth;
  }
}

class Author extends Person{
  #wikipedia
  constructor(name, yearOfBirth, writtenTitles, personWikipedia){
    super(name, yearOfBirth);
    this.writtenTitles = writtenTitles;
    this.#wikipedia = personWikipedia;
  }
  set titles(titles){
    this.writtenTitles = titles;
  }
  get wikipedia(){
    return this.#wikipedia;
  }
}

class CreativeWork{
  constructor(authors, yearOfCreation, title){
    this.authors = authors;
    this.yearOfCreation = yearOfCreation;
    this.title = title;
  }
}

class Book extends CreativeWork{
  #cover
  constructor(authors, yearOfCreation, title, genre, publisher, cover, plot){
    super(authors, yearOfCreation, title)
    this.genre = genre;
    this.publisher = publisher;
    this.plot = plot;
    this.#cover = cover;
  }
  get cover(){
    return this.#cover;
  }
}

class Company{
  #wikipedia
  constructor(name, companyWikipedia){
    this.name = name;
    this.#wikipedia = companyWikipedia;
  }
  get wikipedia(){
    return this.#wikipedia;
  }
}

class Publisher extends Company{
  constructor(name, companyWikipedia, publishedTitles){
    super(name, companyWikipedia);
    this.publishedTitles = publishedTitles;
  }
}

// Initiating objects

const stephenKingBooks = ["It", "The Shining", "The Green Mile", "Carrie", "Salem's Lot"];
const stephenKing = new Author("Stephen King", 1947, stephenKingBooks, "https://en.wikipedia.org/wiki/Stephen_King");

const doubledayTitles = [];
const doubleday = new Publisher("Doubleday", "https://en.wikipedia.org/wiki/Doubleday_(publisher)", doubledayTitles);

const theShiningPlot = "";
const theShining = new Book(stephenKing, 1977, "The Shining", "Horror", doubleday, "assets/covers/The_Shining_1977.jpg", theShiningPlot);

// Functions used in generating page content

function appendNavLi(x){
  const hrefs = ["book-of-the-month", "review", "about-author", "king-books", "about", "contact"];
  const pageNames = ["Book of the month", "Review", "Stephen King", "Other books", "About", "Contact"]
  let navUl = dc("ul");
  navUl.classList.add("nav__list", "font-size--medium");
  for(i = 0; i < hrefs.length; i++){
    let navLi = dc("li");
    let navA = dc("a");
    navA.textContent = pageNames[i];
    navA.href = hrefs[i] + ".html";
    navLi.appendChild(navA);
    navUl.appendChild(navLi);
  }
  x.appendChild(navUl);
}

// Constants used in generating page content

const d = document;
const dq = (x) => document.querySelector(x);
const dc = (x) => document.createElement(x);

const pageHeader = dq(".header--nav");
const navDesktop = dq(".nav--desktop");
const navMobile = dq(".nav--mobile");

// Generating header with nav

const burgerLabel = dc("label");
burgerLabel.classList.add("page-header__burger-menu", "position--sticky");

const burgerInput = dc("input");
burgerInput.id = "toggle";
burgerInput.type = "checkbox";

burgerLabel.appendChild(burgerInput);
pageHeader.insertBefore(burgerLabel, navDesktop);

const logoAnchor = dc("a");
logoAnchor.href = "index.html";

const logoImg = dc("img");
logoImg.classList.add("page-header__logo");
logoImg.src = "assets/homebrew-logo.png";
logoImg.alt = "Homebrew Press Logo";

logoAnchor.appendChild(logoImg);
pageHeader.insertBefore(logoAnchor, navDesktop);

appendNavLi(navDesktop);
appendNavLi(navMobile);

// Generating main content

const testing = dq("#testing").children[0];
testing.src = theShining.cover;

// Adding event listeners

dq("body").addEventListener("click", (e) =>{
  console.log("body geklikt")
})

dq("button").addEventListener("click", (e) =>{
  console.log("button geklikt")
})

testing.addEventListener("click", (e) =>{
  console.log("plaatje geklikt")
})

window.addEventListener("scroll", (e) =>{
  const t = document.body.getBoundingClientRect().top;
  console.log(t);
  if(t < -500){
    testing.classList.add("animation--loading")
  }
})