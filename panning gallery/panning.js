window.onmousemove = e => {

    // find mouse location and store in a variable
    const mouseX = e.clientX;
    const mouseY = e.clientY;

    // rewrite the mouse location as a value from 0-1
    const xDecimal = mouseX / window.innerWidth;
    const yDecimal = mouseY / window.innerHeight;

    // set the maximum X and Y values the window can be panned
    const maxX = gallery.offsetWidth - window.innerWidth
    const maxY = gallery.offsetHeight - window.innerHeight

    // find the panning value, inverse to get the correct orientation
    const panX = maxX * xDecimal * -1;
    const panY = maxY * yDecimal * -1;

    document.getElementById("gallery").style.transform = `translate(${panX}px, ${panY}px)`;
}
