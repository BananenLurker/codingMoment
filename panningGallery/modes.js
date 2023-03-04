const body = document.querySelector('.body');
const gallery = document.querySelector('.gallery');
const darkmodeButton = document.querySelector('#darkmode-button');

darkmodeButton.addEventListener('click', () => {
    body.classList.toggle('darkmode');
    gallery.classList.toggle('darkmode');
    darkmodeButton.classList.toggle('button-darkmode');
})