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
      this.clear()
  }

  leegte(){
    this.currentOperand = ''
    this.previousOperand = ''
    this.operatie = undefined
  }

    nummerToevoegen(number){
        this.currentOperand = number
    }

    operatie(operation){

    }

    berekenMoment(){

    }

    updateDisplay(){
        this.currentOperandTextElement.innerText = this.currentOperand
    }

  }


const calculator = new Calculator(previousOperandTextElement, currentOperandTextElement)

numberButtons.forEach(button => {
    button.addEventListener('click', () => {
        calculator.nummerToevoegen(button.innerText)
        calculator.updateDisplay()
    })
})