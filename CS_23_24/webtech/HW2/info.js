// Creating classes

// PLEASE NOTE: private variables are used in this context for information
// that is non-volatile and should never be changed during
// the lifespan of the object, such as (relative) links.
 
class Person{
  constructor(name, yearOfBirth){
    this.name = name;
    this.yearOfBirth = yearOfBirth;
  }
}

class Author extends Person{
  #wikipedia
  #portrait
  constructor(name, yearOfBirth, writtenTitles, personWikipedia, portrait){
    super(name, yearOfBirth);
    this.writtenTitles = writtenTitles;
    this.#wikipedia = personWikipedia;
    this.#portrait = portrait;
  }
  set titles(titles){
    this.writtenTitles = titles;
  }
  get wikipedia(){
    return this.#wikipedia;
  }
  get portrait(){
    return this.#portrait;
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
const stephenKing = new Author("Stephen King", 1947, stephenKingBooks, "https://en.wikipedia.org/wiki/Stephen_King", "assets/stephen-king.jpg");

const doubledayTitles = [];
const doubleday = new Publisher("Doubleday", "https://en.wikipedia.org/wiki/Doubleday_(publisher)", doubledayTitles);

const theShiningPlot = "Dikke vette huts";
const theShining = new Book(stephenKing, 1977, "The Shining", "Horror", doubleday, "assets/covers/The_Shining_1977.jpg", theShiningPlot);

// Constants used in generating page content

const d = document;
const dq = (x) => document.querySelector(x);
const dc = (x) => document.createElement(x);

const body = dq("body");
const main = dq(".main--info");
const pageHeader = dq(".header--nav");
const navDesktop = dq(".nav--desktop");
const navMobile = dq(".nav--mobile");
const authorCard = dq(".book-info__card--author");

const hrefs = ["contact", "about", "king-books", "about-author", "review", "book-of-the-month"];
const pageNames = ["Contact", "About", "Other books", "Stephen King", "Review", "Book of the month"];

// Functions used in generating page content

function appendNavLi(x){
  let navUl = dc("ul");
  navUl.classList.add("nav__list", "font-size--medium");
  for(i = hrefs.length; i--;){
    let navLi = dc("li");
    let navA = dc("a");
    navA.textContent = pageNames[i];
    navA.href = hrefs[i] + ".html";
    navLi.appendChild(navA);
    navUl.appendChild(navLi);
  }
  x.appendChild(navUl);
}

function isInViewport(x){
  var t = x.getBoundingClientRect();
  return(t.top >= 100 && t.bottom < window.innerHeight);
}

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

const infoArticle = dq(".book-info");
const infoHeader = dq(".book-info__header");

const infoHeaderTitle = dc("h1");
infoHeaderTitle.textContent = "Info about The Book of the Month";

infoHeader.appendChild(infoHeaderTitle);

const bookInfoTitle = dc("h2");
bookInfoTitle.textContent = theShining.title;
bookInfoTitle.classList.add("font-size--large");
dq(".book-info__card--title").appendChild(bookInfoTitle);

dq(".book-info__cover-image").src = theShining.cover;
dq(".book-info__cover-caption").textContent = "Cover of The Shining";

const genreText = dc("h2");
genreText.textContent = "Genre: " + theShining.genre;
genreText.classList.add("font-size--medium");
dq(".book-info__card--genre").appendChild(genreText);

const yearText = dc("h2");
yearText.textContent = "Year of publication: " + theShining.yearOfCreation;
yearText.classList.add("font-size--medium");
dq(".book-info__card--year").appendChild(yearText);

dq(".book-info__author-image").src = stephenKing.portrait;
dq(".book-info__author-caption").textContent = stephenKing.name;

const publisherText = dc("h2");
publisherText.textContent = "Publisher: " + theShining.publisher.name;
publisherText.classList.add("font-size--medium", "link--simple");
dq(".book-info__card--publisher").appendChild(publisherText);

const plotHeader = dc("h2");
plotHeader.textContent = "Summary of the plot:";
plotHeader.classList.add("book-info__card-plot-header", "font-size--medium");
const plotText = dc("p");
plotText.textContent = theShining.plot;
plotText.classList.add("font-size--small");

dq(".book-info__card--plot").appendChild(plotHeader);
dq(".book-info__card--plot").appendChild(plotText);

// Adding event listeners

const tooltip = d.querySelectorAll(".tooltip");

d.addEventListener("mousemove", showToolTip, false);

function showToolTip(e){
  var mouseX = e.clientX - authorCard.getBoundingClientRect().left;
  var mouseY = e.clientY - authorCard.getBoundingClientRect().top
  for (var i=tooltip.length; i--;) {
    if(mouseX > window.innerWidth - 200){
      tooltip[i].style.left = mouseX - 200 + "px";
      tooltip[i].style.top = mouseY + "px";
    }
    else{
      tooltip[i].style.left = mouseX + "px";
      tooltip[i].style.top = mouseY + "px";
    }
  }
}

window.addEventListener("scroll", loadOnScroll);

function loadOnScroll(){
  const cards = d.querySelectorAll(".book-info__card");
  for(i = cards.length; i--;){
    if(isInViewport(cards[i])){
      cards[i].classList.add("animation--generation");
    }
  }
}

loadOnScroll();