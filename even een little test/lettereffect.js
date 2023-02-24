const letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

let iterations = 0;
const myHeadings = document.querySelectorAll("h1")

myHeadings.forEach(heading => {heading.onmouseover = 

(event) => {

{
        const interval = setInterval(
            () => {
                event.target.innerText = event.target.innerText
                .split("")
                .map(
                    (letter, index) => {
                        if (index < iterations) {
                            return event.target.dataset.value[index];
                            }
                        return letters[Math.floor(Math.random() * 26)];
                    }
                )
                .join("");

                if (iterations >= event.target.dataset.value.length) {
                    clearInterval(interval);
                    iterations = 0;
                }
                iterations += 1 / 2;
            },
            30
        );
    }
};
});