// Creating classes
 
class person{
  constructor(name, yearOfBirth){
    this.name = name;
    this.yearOfBirth = yearOfBirth;
  }
}

class author extends person{
  #wikipedia
  #portrait
  constructor(name, yearOfBirth, writtenTitles, personWikipedia, portrait){
    super(name, yearOfBirth);
    this.writtenTitles = writtenTitles;
    this.#wikipedia = personWikipedia;
    this.#portrait = portrait;
  }
  get wikipedia(){
    if(this.#wikipedia.includes("wikipedia"))
      return this.#wikipedia;
    else
      return "undefined";
  }
  get portrait(){
    if(this.#portrait.includes("assets"))
      return this.#portrait;
    else
      return "assets/img-not-found.jpg";
  }
}

class creativeWork{
  constructor(authors, yearOfCreation, title){
    this.authors = authors;
    this.yearOfCreation = yearOfCreation;
    this.title = title;
  }
}

class book extends creativeWork{
  #cover
  constructor(authors, yearOfCreation, title, genre, publisher, cover, plot){
    super(authors, yearOfCreation, title)
    this.genre = genre;
    this.publisher = publisher;
    this.plot = plot;
    this.#cover = cover;
  }
  get cover(){
    if(this.#cover && this.#cover.includes("assets"))
      return this.#cover;
    else
      return "assets/img-not-found.jpg";
  }
}

class company{
  #wikipedia
  constructor(name, companyWikipedia){
    this.name = name;
    this.#wikipedia = companyWikipedia;
  }
  get wikipedia(){
    if(this.#wikipedia.includes("wikipedia"))
      return this.#wikipedia;
    else
      return "undefined";
  }
}

class publisher extends company{
  constructor(name, companyWikipedia, publishedTitles){
    super(name, companyWikipedia);
    this.publishedTitles = publishedTitles;
  }
}

// Initiating objects

const stephenKingBooks = ["It", "The Shining", "The Green Mile", "Carrie", "Salem's Lot"];
const stephenKing = new author("Stephen King", 1947, stephenKingBooks, "https://en.wikipedia.org/wiki/Stephen_King", "assets/stephen-king.jpg");

const doubledayTitles = ["The Shining", "The Da Vinci Code", "Clockwork", "Bill - the Galactic Hero", "A Concise Chinese-English Dictionary for Lovers"];
const doubleday = new publisher("Doubleday", "https://en.wikipedia.org/wiki/Doubleday_(publisher)", doubledayTitles);

const theShiningPlot = "The Shining follows Jack Torrance, a struggling writer and recovering alcoholic, who takes a job as the winter caretaker of the isolated Overlook Hotel in Colorado. He moves in with his wife, Wendy, and their young son, Danny, who possesses psychic abilities known as the shining. As the winter sets in, the hotel's malevolent spirits begin to manipulate Jack's weaknesses, driving him to madness and violence. Danny's psychic abilities allow him to see the hotel's horrific past and communicate with the hotel's cook, Dick Hallorann, who shares the same gift. As Jack descends into insanity, Danny and Wendy must confront the supernatural forces within the hotel to survive. The novel explores themes of addiction, family dynamics, and the nature of evil, culminating in a terrifying showdown between the Torrance family and the malevolent forces of the Overlook Hotel.";
const theShining = new book(stephenKing, 1977, "The Shining", "Horror", doubleday, "assets/covers/The_Shining_1977.jpg", theShiningPlot);

// Constant variables used in generating page content

const d = document;
const dq = (x) => document.querySelector(x);
const dc = (x) => document.createElement(x);
const dt = (x) => document.createTextNode(x);

const body = dq("body");
const main = dq(".main--info");
const pageHeader = dq(".header--nav");

const navDesktop = dq(".nav--desktop");
const navMobile = dq(".nav--mobile");

const authorCard = dq(".book-info__card--author");
const publisherCard = dq(".book-info__card--publisher");
const plotCard = dq(".book-info__card--plot");

const hrefs = ["contact", "about", "king-books", "about-author", "info", "review", "book-of-the-month"];
const pageNames = ["Contact", "About", "Other books", "Stephen King", "Info", "Review", "Book of the month"];

// Functions used in generating page content

function appendNavLi(x){
  let navUl = dc("ul");
  navUl.classList.add("nav__list", "font-size--medium");
  for(i = hrefs.length; i--;){
    let navLi = dc("li");
    let navA = dc("a");
    navA.appendChild(dt(pageNames[i]));
    navA.href = hrefs[i] + "";
    navLi.appendChild(navA);
    if(hrefs[i] === "info"){
      navLi.classList.add("nav__current-page");
    }
    navUl.appendChild(navLi);
  }
  x.appendChild(navUl);
}

function generateFigure(parentElement, imageAlt, imageType, imageSrc){
  var figure = dc("figure");
  var img = dc("img");
  var figcaption = dc("figcaption");

  img.alt = imageAlt;
  img.src = imageSrc;
  var imgClass = "book-info__" + imageType + "-image";
  img.classList.add(imgClass);

  var captionClass = "book-info__" + imageType + "-caption";
  figcaption.classList.add(captionClass, "font-size--small");

  figure.appendChild(img);
  figure.appendChild(figcaption);
  parentElement.appendChild(figure);
}

function generateTooltipAttributes(tooltip, entity){
  const attributes = Object.values(entity).concat(Object.values(Object.getPrototypeOf(entity))).concat(entity.wikipedia);
  const attributeNames = Object.getOwnPropertyNames(entity).concat(Object.values(Object.getPrototypeOf(entity))).concat("wikipedia");

  if(attributes.length !== attributeNames.length){
    throw("Tooltip entity does not contain all required attributes for a tooltip!")
  }
  if(entity.wikipedia){
    attributes.concat(entity.wikipedia);
    attributeNames.concat("wikipedia");
  }

  addToolTipInfo(tooltip, attributes, attributeNames)
}

function addToolTipInfo(tooltip, attributes, attributeNames){
  for(i = 0; i < attributes.length; i++){
    if(attributes[i] == "constructor" || attributes[i] == "portrait"){ break; }
    else{
      if(i !== 0){
        tooltip.appendChild(dc("br"));
      }

      var attributeTextWrapper = dc("span");
      attributeTextWrapper.classList.add("font-size--small");

      var attributeNameTextWrapper = dc("span");
      attributeNameTextWrapper.style.fontWeight = "bold";

      switch(attributeNames[i]){
        case("name"):
          var attributeNameText = dt("Name: ")
          break;
        case("yearOfBirth"):
          var attributeNameText = dt("Year of birth: ");
          break;
        case("writtenTitles"):
          var attributeNameText = dt("Written titles: ");
          attributes[i] = attributes[i].map(i => " " + i);
          break;
        case("wikipedia"):
          var attributeNameText = dt("Wikipedia: ");
          break;
        case("publishedTitles"):
        var attributeNameText = dt("Published titles: ");
        attributes[i] = attributes[i].map(i => " " + i);
        default:
          break;
      }

      attributeNameTextWrapper.appendChild(attributeNameText);
      attributeTextWrapper.appendChild(attributeNameTextWrapper);

      var attributeText = dt(attributes[i]);
      attributeTextWrapper.appendChild(attributeText);
      
      tooltip.appendChild(attributeTextWrapper);
    }
  }
}

function generateFooter(){
  var footer = dq(".footer");
  var logoWrapper = dq(".footer__logo--wrapper");
  var footerTitle = dc("h3");
  footerTitle.appendChild(dt("Homebrew Press ©"));
  footerTitle.classList.add("footer__title", "font-size--medium");
  footer.insertBefore(footerTitle, logoWrapper);


  // These anchor tags can be generated by a function. However,
  // this function would be so large that just instantiating them
  // one by one is more efficient, as they share less similarities
  // than you would think. Trust us, we tried.

  // Youtube anchor tag
  var anchorYoutube = dc("a");
  anchorYoutube.classList.add("footer__logo", "footer__logo--youtube");
  anchorYoutube.href = "https://www.youtube.com/watch?v=xvFZjo5PgG0";
  anchorYoutube.target = "_blank";

  var anchorYoutubeImg = dc("img");
  anchorYoutubeImg.src = "assets/youtube.svg";
  anchorYoutubeImg.alt = "YouTube logo";

  anchorYoutube.appendChild(anchorYoutubeImg);

  // Twitter anchor tag
  var anchorTwitterx = dc("a");
  anchorTwitterx.classList.add("footer__logo", "footer__logo--twitterx");
  anchorTwitterx.href = "https://twitter.com/";
  anchorTwitterx.target = "_blank";

  var anchorTwitterxImg = dc("img");
  anchorTwitterxImg.src = "assets/twitterx.svg";
  anchorTwitterxImg.alt = "X (formerly known as Twitter) logo";

  anchorTwitterx.appendChild(anchorTwitterxImg);

  // instagram anchor tag
  var anchorInstagram = dc("a");
  anchorInstagram.classList.add("footer__logo", "footer__logo--instagram");
  anchorInstagram.href = "https://www.instagram.com/stickyutrecht/";
  anchorInstagram.target = "_blank";

  var anchorInstagramImg = dc("img");
  anchorInstagramImg.src = "assets/instagram.svg";
  anchorInstagramImg.alt = "Instagram logo";

  anchorInstagram.appendChild(anchorInstagramImg);

  logoWrapper.appendChild(anchorYoutube);
  logoWrapper.appendChild(anchorTwitterx);
  logoWrapper.appendChild(anchorInstagram);
}

function addSelectOptions(parentElement){
  const options = ["Background", "Color", "Font Size", "Choose"];
  const values = ["background", "color", "font-size", "blank"];
  for(i = options.length; i--;){
    var option = dc("option");
    option.textContent = options[i];
    option.value = values[i];
    parentElement.appendChild(option);
  }
}

generateFooter();

// Generating header with nav

const burgerLabel = dc("label");
burgerLabel.classList.add("page-header__burger-menu", "position--sticky");

const burgerInput = dc("input");
burgerInput.id = "toggle";
burgerInput.type = "checkbox";

burgerLabel.appendChild(burgerInput);
pageHeader.insertBefore(burgerLabel, navDesktop);

const logoAnchor = dc("a");
logoAnchor.href = "/";

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
infoHeaderTitle.appendChild(dt("Info about The Book of the Month"));

infoHeader.appendChild(infoHeaderTitle);

const bookInfoTitle = dc("h2");
bookInfoTitle.classList.add("font-size--large");
bookInfoTitle.appendChild(dt(theShining.title))
dq(".book-info__card--title").appendChild(bookInfoTitle);

generateFigure(dq(".book-info__card--cover"), "Cover of the book The Shining", "cover", theShining.cover);
const coverCaption = dq(".book-info__cover-caption");
coverCaption.appendChild(dt("Cover of The Shining"));
coverCaption.appendChild(dc("br"));
coverCaption.appendChild(dt("Click the image to go to the image source"));
coverCaption.appendChild(dc("br"));
coverCaption.appendChild(dt("Click anywhere else in the box to read our review of The Shining"));

const genreText = dc("h2");
genreText.classList.add("font-size--medium");
genreText.appendChild(dt("Genre: " + theShining.genre));
dq(".book-info__card--genre").appendChild(genreText);

const yearText = dc("h2");
yearText.classList.add("font-size--medium");
yearText.appendChild(dt("Year of publication: " + theShining.yearOfCreation));
dq(".book-info__card--year").appendChild(yearText);

generateFigure(dq(".book-info__card--author"), "Stephen King", "author", stephenKing.portrait);
dq(".book-info__author-image").src = stephenKing.portrait;
dq(".book-info__author-caption").appendChild(dt("Author: " + stephenKing.name));

const publisherText = dc("h2");
publisherText.classList.add("font-size--medium", "link--simple");
publisherText.appendChild(dt("Publisher: " + theShining.publisher.name));
dq(".book-info__card--publisher").appendChild(publisherText);

const plotHeader = dc("h2");
plotHeader.appendChild(dt("Summary of the plot:"));
plotHeader.classList.add("book-info__card-plot-header", "font-size--medium");
const plotText = dc("p");
plotText.appendChild(dt(theShining.plot));
plotText.classList.add("font-size--small");

plotCard.appendChild(plotHeader);
plotCard.appendChild(plotText);
plotCard.appendChild(dc("br"));

var plotSource = dc("p");
plotSource.style.fontStyle = "italic";
plotSource.classList.add("font-size--small");

var plotSourceLink = dc("a");
plotSourceLink.appendChild(dt("ChatGPT."));
plotSourceLink.href = "https://chat.openai.com/";
plotSourceLink.classList.add("link--simple");

plotSource.appendChild(dt("This text was generated using "));
plotSource.appendChild(plotSourceLink);
plotCard.appendChild(plotSource);

const tooltipArray = [d.querySelectorAll(".book-info__author-tooltip"), 
                      d.querySelectorAll(".book-info__publisher-tooltip")];
generateTooltipAttributes(tooltipArray[0][0], stephenKing);
generateTooltipAttributes(tooltipArray[1][0], doubleday);

// Adding event listeners

dq(".book-info__card--cover").addEventListener("click", () => { location.href = "review" }, true ); // Event propagation: setting the event to capturing instead of bubbling
dq(".book-info__cover-image").addEventListener("click", () => { location.href = "https://en.wikipedia.org/wiki/The_Shining_(novel)"} );

window.addEventListener("mousemove", showToolTip);
window.addEventListener("scroll", loadOnScroll);

function showToolTip(e){
  var mouseX = e.pageX;
  var mouseY = e.pageY;
  for (var i = tooltipArray.length; i--;) {
    // A tooltip width will always be 300px, offsetting it by
    // 300 will make it 'stick' to the right side of the screen
    if(mouseX >= window.innerWidth - 300){
      tooltipArray[i][0].style.left = window.innerWidth - 300 + "px";
    }
    else{
      tooltipArray[i][0].style.left = mouseX + "px";
    }
    tooltipArray[i][0].style.top = mouseY + "px";
  }
}

function loadOnScroll(){
  var cards = d.querySelectorAll(".book-info__card");
  for(i = cards.length; i--;){
    // Only load a section when it is in the viewport.
    if(isInViewport(cards[i])){
      cards[i].classList.add("animation--generation");
    }
  }
}

function isInViewport(x){
  var t = x.getBoundingClientRect();
  // We count an element as being 'in the viewport' when the top is at least 100 pixels 
  // higher than the bottom of the viewport and the distance to the bottom of the element is less
  // than the window height. This is done to give the same effect on smaller devices.
  if(window.innerHeight < 1000){
    return true;
  }
  return(t.top >= 100 && t.bottom < window.innerHeight);
}

loadOnScroll();