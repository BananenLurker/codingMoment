const numberButtons = document.querySelectorAll('[data-number]')
const operationButtons = document.querySelectorAll('[data-operation]')
const equalsButton = document.querySelector('[data-equals]')
const allClearButton = document.querySelector('[data-all-clear]')
const previousOperandTextElement = document.querySelector('[data-previous-operand]')
const currentOperandTextElement = document.querySelector('[data-current-operand]')
const deleteButton = document.querySelector('[data-delete-button]')

class Calculator {
    constructor(previousOperandTextElement, currentOperandTextElement) {
      this.previousOperandTextElement = previousOperandTextElement
      this.currentOperandTextElement = currentOperandTextElement
      this.leegte();
  }

  leegte(){
    this.currentOperand = ''
    this.previousOperand = ''
    this.operation = undefined
  }

    nummerToevoegen(number){
      if(number === '.' && this.currentOperand.includes('.')) return
      this.currentOperand = this.currentOperand.toString() + number.toString()
    }

    nummerVerwijderen(){
      this.currentOperand = this.currentOperand.slice(0, -1);
      this.currentOperandTextElement.innerText = this.currentOperand
    }

    operatie(operation) {
      let operatieTeken
      const curr = parseFloat(this.currentOperand)
      if(curr === '') return
      this.operation = operation
      switch(this.operation){
        case '+':
          operatieTeken = '+';
          break;
        case 'x':
          operatieTeken = 'x';
          break;
        case '/':
          operatieTeken = '/';
          break;
        case '-':
          operatieTeken = '-';
          break;
      }
      this.previousOperand = this.currentOperand + ' ' + operatieTeken
      this.currentOperand = ''
    }

    berekenMoment(){
      let uitrekenen
      const curr = parseFloat(this.currentOperand)
      const prev = parseFloat(this.previousOperand)
      const operatieTeken = this.previousOperandTextElement.innerText.slice(-1);
      function isNum(huts){
        return /\d/.test(huts);
      }
      if(isNaN(curr) || isNaN(prev) || curr === '' || prev === '' || isNum(operatieTeken)) return
      switch(this.operation){
        case '+':
          uitrekenen = prev + curr;
          break;
        case 'x':
          uitrekenen = prev * curr;
          break;
        case '/':
          uitrekenen = prev / curr;
          break;
        case '-':
          uitrekenen = prev - curr;
          break;
      }
      this.currentOperand = uitrekenen;
      this.operation = undefined
      this.previousOperand = this.previousOperand + ' ' + this.currentOperandTextElement.innerText
    }

    updateDisplay(){
      let fatoe = ''
      switch(this.currentOperand){
        case '80085':
          fatoe = 'haha!';
          break;
        case '1337':
          fatoe = 'leet.';
          break;
        case '1414':
          fatoe = 'hihi';
          break;
        case '69':
          fatoe = 'nice.';
          break;
        case '1234567890':
          fatoe = 'https://www.youtube.com/watch?v=xvFZjo5PgG0';
          break;
      }
      if(fatoe === ''){
        this.currentOperandTextElement.innerText = this.currentOperand
        this.previousOperandTextElement.innerText = this.previousOperand
      }
      else{
        this.currentOperandTextElement.innerText = fatoe
      }
    }

  }


const calculator = new Calculator(previousOperandTextElement, currentOperandTextElement)

numberButtons.forEach(button => {
  button.addEventListener('click', () => {
    calculator.nummerToevoegen(button.innerText)
    calculator.updateDisplay()
  })
})

operationButtons.forEach(button => {
  button.addEventListener('click', () => {
    calculator.operatie(button.innerText)
    calculator.updateDisplay()
  })
})

allClearButton.addEventListener('click', () => {
  calculator.leegte()
  calculator.updateDisplay()
})

equalsButton.addEventListener('click', () => {
  calculator.berekenMoment()
  calculator.updateDisplay()
})

deleteButton.addEventListener('click', () => {
  calculator.nummerVerwijderen()
})