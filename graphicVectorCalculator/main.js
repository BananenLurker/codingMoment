const vector1x = document.getElementById('vector1-x')
const vector1y = document.getElementById('vector1-y')
const vector2x = document.getElementById('vector2-x')
const vector2y = document.getElementById('vector2-y')
const calculateButton = document.querySelector('[data-calculate-button]')
const uitkomstx = document.getElementById('uitkomstx')
const uitkomsty = document.getElementById('uitkomsty')

function vectorCheck() {
  const finalx = parseFloat(vector1x.value.replace(/\D/g, '')) + parseFloat(vector2x.value.replace(/\D/g, ''))
  const finaly = parseFloat(vector1y.value.replace(/\D/g, '')) + parseFloat(vector2y.value.replace(/\D/g, ''))
  if(isNaN(finalx) || isNaN(finaly)) return
  uitkomstx.innerHTML = '<h2>' + finalx + '</h2>'
  uitkomsty.innerHTML = '<h2>' + finaly + '</h2>'
}

calculateButton.addEventListener('click', () => {
  vectorCheck();
})
