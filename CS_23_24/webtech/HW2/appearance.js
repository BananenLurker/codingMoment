const selectorMenu = document.querySelector('#element-menu');

class elementMenu {
  constructor(selector) {
    this.element = document.querySelector(selector);
    this.populateDropdown();
  }
  populateDropdown() {
    const elements = document.querySelectorAll('body, header, footer, aside, article, section');
    const dropdown = this.element;

    this.setId(elements);

    // Setting the properties of each option
    elements.forEach(element => {
      let option = document.createElement('option');
      option.value = element.id;
      option.textContent = element.tagName.toLowerCase();
      dropdown.appendChild(option);
    });
  }

  // Gives each element an ID based on the tag name and
  // an increasing number, to later differentiate elements
  setId(elements){
    for(var i = elements.length; i--;){
      elements[i].id = elements[i].tagName + i;
    }
  }
}

class appearanceMenu {
  constructor(selector) {
    this.element = document.querySelector(selector);
    this.element.addEventListener('change', this.handleChange.bind(this));
  }

  handleChange(event) {
    const selectedOption = event.target.value;
    const selectedElement = selectorMenu.options[selectorMenu.selectedIndex].value;

    if (selectedOption === 'font-size') {
      const newSize = prompt('Enter new font size:');
      if (newSize) {
        // Getting an array of a NodeList and the parent element, so both can be changed.
        // Only setting the parent element is less visually appealing.
        var sizeChildren = [document.getElementById(selectedElement).getElementsByTagName("*"), document.getElementById(selectedElement)];
        for(var i = 0; i < sizeChildren[0].length; i++){
          // Children[0] gets the NodeList which needs to be iterated over
          sizeChildren[0][i].style.fontSize = newSize + "px";
        }
        // Children[1] gets the parent element and does not have a second index
        sizeChildren[1].style.fontSize = newSize + "px";
      }
    }

    else if (selectedOption === 'color') {
      const newColor = prompt('Enter new color (e.g., red, #00ff00):');
      if (newColor) {
        var colorChildren = [document.getElementById(selectedElement).getElementsByTagName("*"), document.getElementById(selectedElement)];
        for(var i = 0; i < colorChildren[0].length; i++){
          colorChildren[0][i].style.color = newColor;
        }
        colorChildren[1].style.color = newColor;
      }
    }

    else if (selectedOption === 'background') {
      const newBackground = prompt('Enter new background color (e.g., red, #00ff00):');
      if (newBackground) {
        var backgroundChildren = [document.getElementById(selectedElement).getElementsByTagName("*"), document.getElementById(selectedElement)];
        for(var i = 0; i < backgroundChildren[0].length; i++){
          backgroundChildren[0][i].style.background = newBackground;
        }
        backgroundChildren[1].style.background = newBackground;
      }
    }
    event.target.value = 'blank';
  }
}

const em = new elementMenu('#element-menu');
const am = new appearanceMenu('#appearance-menu');