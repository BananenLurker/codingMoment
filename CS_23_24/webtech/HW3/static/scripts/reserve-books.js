function reserveBook(e) {
  var bookID = window.location.href.split('/')[4];
  var url = `reservebook?bookID=+${bookID}&rnd=` + Math.random();
  var req = new XMLHttpRequest();
  req.open("GET", url, true);
  req.onreadystatechange = function () {
    if (req.readyState === 4 && req.status === 200) {
      let resp = req.responseText;
      let resButton = document.getElementById('reserve');
      if(resp === 'Success!'){
        resButton.classList.add('button--reserved');
        resButton.setAttribute('disabled', '');
        let amount = document.querySelector('.book-information__amount-left');
        amount.innerHTML = amount.innerHTML - 1;
      }
      else{
        resButton.classList.add('button--error');
        resButton.setAttribute('disabled', '');
      }
      resButton.innerHTML = resp;
    }
  }
  req.send();
  e.preventDefault();
}

document.getElementById('reserve').addEventListener("click", reserveBook);