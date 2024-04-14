const database = require('./modules/database');

// Retrieves book details from the database for a specific book ID.
function getBookDetails(bookId) {
    const db = database.open(); // Open a connection to the database

    // Using a promise to make sure the database returns before continuing
    return new Promise((resolve, reject) => {
        const query = 'SELECT * FROM book WHERE ID = ?';
        db.get(query, [bookId], (err, row) => {
            if (err) {
                console.error('Error fetching book details:', err);
                reject(err);
            } else if (row) {
                const bookDetails = {
                    bookId: row.ID,
                    title: row.title,
                    author: row.author,
                    genre: row.genre,
                    publisher: row.publisher,
                    year: row.year,
                    summary: row.plot,
                    coverImageUrl: row.cover,
                    copiesLeft: row.amount
                };
                resolve(bookDetails);
            } else {
                resolve(null);
            }
        });

        database.close(db); // Close the database connection
    });
}

module.exports = { getBookDetails };