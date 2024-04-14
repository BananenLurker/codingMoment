// Reserving a book using AJAX
function reserveBook(e) {
  var bookID = window.location.href.split('/')[4]; // Get the book ID from the page URL
  var url = `reservebook?bookID=+${bookID}&rnd=` + Math.random(); // Non-cached AJAX request URL
  var req = new XMLHttpRequest();
  req.open("GET", url, true); // Send the request to the server
  req.onreadystatechange = function () {
    if (req.readyState === 4 && req.status === 200) { // Make sure the server is ready before continuing
      let resp = req.responseText;
      let resButton = document.getElementById('reserve');
      if(resp === 'Success!'){ // If the reservation is successful..
        resButton.classList.add('button--reserved'); // ..set button styling accordingly
        resButton.setAttribute('disabled', ''); // ..disable the button
        let amount = document.querySelector('.book-information__amount-left');
        amount.innerHTML = amount.innerHTML - 1; // ..update the amount of books left.
      }
      else{ // If the reservation is not successful..
        resButton.classList.add('button--error'); // ..set the styling accordingly
        resButton.setAttribute('disabled', ''); // ..disable the button
      }
      resButton.innerHTML = resp; // In any case, update to give the user feedback
    }
  }
  req.send();
  e.preventDefault();
}

document.getElementById('reserve').addEventListener("click", reserveBook);