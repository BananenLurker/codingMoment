const body = document.querySelector('.body');
const gallery = document.querySelector('.gallery');
const darkmodeButton = document.querySelector('#darkmode-button');

darkmodeButton.addEventListener('click', () => {
    body.classList.toggle('darkmode');
    body.classList.toggle('lightmode');
    gallery.classList.toggle('darkmode');
    darkmodeButton.classList.toggle('button-darkmode');
})