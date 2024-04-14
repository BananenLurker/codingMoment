// Function to return books using AJAX
function returnBook(e) {
  var bookID = e.target.dataset.bookId; // Get the bookID from the button. The bookID is stored using a data attribute.
  var url = `returnbook?bookID=${bookID}&rnd=` + Math.random(); // Make a non-cached AJAX req URL
  var req = new XMLHttpRequest();
  req.open("GET", url, true); // Send the URL
  req.onreadystatechange = function () {
    if (req.readyState === 4 && req.status === 200) { // Make sure the server is done before continuing
      let returnLink = document.querySelector(`[data-book-id="${bookID}"]`);
      returnLink.innerHTML = 'returned!'; // Update the button to give the user feedback
      returnLink.classList.remove('link--simple-white'); // Remove link styling
      returnLink.setAttribute('disabled', ''); // Set the button to be disabled
    }
  }
  req.send();
  e.preventDefault();
}

document.querySelectorAll('.return').forEach(button => {
  button.addEventListener("click", returnBook);
});