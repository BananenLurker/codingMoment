function reserveBook(e) {
  var bookID = document.getElementById("reserveid").value;
  var url = "reservebook?bookID="+bookID;
  var req = new XMLHttpRequest();
  req.open("GET", url, true);
  req.onreadystatechange = function () {
    if (req.readyState === 4 && req.status === 200) {
      alert(req.responseText);
    }
  }
  req.send();
  e.preventDefault();
}

document.getElementById("reserve").addEventListener("submit", reserveBook);

function returnBook(e) {
  var bookID = document.getElementById("returnid").value;
  var url = "returnbook?bookID="+bookID;
  var req = new XMLHttpRequest();
  req.open("GET", url, true);
  req.onreadystatechange = function () {
    if (req.readyState === 4 && req.status === 200) {
      alert(req.responseText);
    }
  }
  req.send();
  e.preventDefault();
}

document.getElementById('return').addEventListener("submit", returnBook);