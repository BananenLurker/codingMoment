const numberButtons = document.querySelectorAll('[data-number]')
const operationButtons = document.querySelectorAll('[data-operation]')
const equalsButton = document.querySelector('[data-equals]')
const allClearButton = document.querySelector('[data-all-clear]')
const previousOperandTextElement = document.querySelector('[data-previous-operand]')
const currentOperandTextElement = document.querySelector('[data-current-operand]')

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
      if(isNaN(curr) || isNaN(prev) || curr === '' || prev === '') return
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
        this.currentOperandTextElement.innerText = this.currentOperand
        this.previousOperandTextElement.innerText = this.previousOperand
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

equalsButton.addEventListener('click', () =>{
  calculator.berekenMoment()
  calculator.updateDisplay()
})