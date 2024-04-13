document.addEventListener('DOMContentLoaded', function() {
  let currentPage = 1;
  let hasMoreBooks = true;
  const container = document.getElementById('book-container');

  function loadBooks() {
    if (!hasMoreBooks) return;

    fetch(`/catalogue/books?page=${currentPage}`)
    .then(response => response.json())
    .then(books => {
        if (books.length === 0 || books.length < 20) {
            hasMoreBooks = false;
        }
        books.forEach(book => {
            const bookDiv = document.createElement('div');
            bookDiv.className = 'book';
            bookDiv.innerHTML = `
                <img src="${book.coverImageUrl}" alt="${book.title}">
                <h3>${book.title}</h3>
                <p>Author: ${book.author}</p>
                <p class="copies ${book.copiesLeft === 0 ? 'red' : (book.copiesLeft === 1 || book.copiesLeft === 2) ? 'orange' : 'green'}">
                    Copies left: ${book.copiesLeft}
                </p>
            `;
            bookDiv.onclick = () => window.location.href = '/book/' + book.bookId;
            container.appendChild(bookDiv);
        });
        currentPage++;
    })
    .catch(error => console.error('Error loading the books:', error));
  }

  function handleScroll() {
    const nearBottom = window.innerHeight + window.scrollY >= document.body.offsetHeight - 500;
    if (nearBottom && hasMoreBooks) {
      loadBooks();
    }
  }

  function throttle(func, delay) {
    let timer = null;
    return function(...args) {
      if (!timer) {
        timer = setTimeout(() => {
          func.apply(this, args);
          timer = null;
        }, delay);
      }
    };
  }
  
  const throttledHandleScroll = throttle(handleScroll, 200); 
  window.addEventListener('scroll', throttledHandleScroll);

  loadBooks();
});