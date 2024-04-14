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

  // Function to add a delay before the next iteration of any function can be called
  function throttle(func, delay) {
    let timer = null;
    return function(...args) {
      if (!timer) { 
        timer = setTimeout(() => { // Set a timeout, invoke a callback after it is done
          func.apply(this, args); 
          timer = null; // Set timer variable to null to make sure the throttle function can be called again
        }, delay); // Sets the delay after which the throttled function can be called again
      }
    };
  }
  
  // Adding a throttle of 200msec to the handleScroll event to prevent it from firing before the database
  // has returned its content. This prevents queueing of database operations, which makes the database more responsive.
  // In addition, it prevents infinite loading of duplicate books.
  const throttledHandleScroll = throttle(handleScroll, 200); 
  window.addEventListener('scroll', throttledHandleScroll);
  loadBooks();
});