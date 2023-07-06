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

function drawGrid(ctx, x1, y1, x2,y2, stroke = 'black', width = 1.5) {
  ctx.beginPath();
  ctx.moveTo(x1 + 1, y1);
  ctx.lineTo(x2 + 1, y2);
  ctx.strokeStyle = stroke;
  ctx.lineWidth = width;
  ctx.stroke();
}
let canvas = document.getElementById('uitkomst'),
    ctx = canvas.getContext('2d');
drawGrid(ctx, 150, 0, 150, 150, 1.5);
drawGrid(ctx, -10, 75, 300, 75, 1.5);

function tekenVector(ctx, x1, y1, x2,y2, width = 1) {
  ctx.beginPath();
  ctx.moveTo(x1 + 1, y1);
  ctx.lineTo(x2 + 1, y2);
  ctx.strokeStyle = stroke;
  ctx.lineWidth = width;
  ctx.stroke();
}

calculateButton.addEventListener('click', () => {
  vectorCheck();
  let canvas = document.getElementById('uitkomst'),
    ctx = canvas.getContext('2d');
  tekenVector(ctx, 0, 0, vector1x, vector1y, 1);
  tekenVector(ctx, vector1x, vector1y, vector2x, vector2y, 1);
  tekenVector(ctx, 0, 0, finalx, finaly, 1.5);
})