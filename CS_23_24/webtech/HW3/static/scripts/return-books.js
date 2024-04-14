function returnBook(e) {
  var bookID = e.target.dataset.bookId;
  var url = `returnbook?bookID=${bookID}&rnd=` + Math.random();
  var req = new XMLHttpRequest();
  req.open("GET", url, true);
  req.onreadystatechange = function () {
    if (req.readyState === 4 && req.status === 200) {
      req.open("GET", `/returnbook?bookID=${bookID}`, true);
      let returnLink = document.querySelector(`[data-book-id="${bookID}"]`);
      returnLink.innerHTML = 'returned!';
      returnLink.classList.remove('link--simple-white');
      returnLink.setAttribute('disabled', '');
    }
  }
  req.send();
  e.preventDefault();
}

document.querySelectorAll('.return').forEach(button => {
  button.addEventListener("click", returnBook);
});