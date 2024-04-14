document.addEventListener('DOMContentLoaded', function() {
  let currentPage = 1; // Track the current page of book results
  let hasMoreBooks = true; // Flag to check if there are more books to load
  const container = document.getElementById('book-container'); // The container for book entries

  // Loads books dynamically as the user scrolls
  function loadBooks() {
      if (!hasMoreBooks) return; // Stop if there are no more books to load

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
          currentPage++; // Increment to load the next page of books
      })
      .catch(error => console.error('Error loading the books:', error));
  }
  
  // Event listener to handle infinite scrolling
  function handleScroll() {
      const nearBottom = window.innerHeight + window.scrollY >= document.body.offsetHeight - 500;
      if (nearBottom && hasMoreBooks) {
          loadBooks();
      }
  }

  window.addEventListener('scroll', handleScroll);
  loadBooks();
});