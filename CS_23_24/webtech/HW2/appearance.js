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

    elements.forEach(element => {
      let option = document.createElement('option');
      option.value = element.id;
      option.textContent = element.tagName.toLowerCase();
      option.dataset.elementId = element.classList[0];
      dropdown.appendChild(option);
    });
  }

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
        var sizeChildren = [document.getElementById(selectedElement).getElementsByTagName("*"), document.getElementById(selectedElement)];
        for(var i = 0; i < sizeChildren[0].length; i++){
          sizeChildren[0][i].style.fontSize = newSize + "px";
        }
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