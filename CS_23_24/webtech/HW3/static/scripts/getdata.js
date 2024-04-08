function mylogin(e) {
  var bookID = document.getElementById("bookid").value;
  var url = "getdata.js?bookID="+bookID;
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

document.getElementById("login").
addEventListener("submit", mylogin);