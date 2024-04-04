// Function to hide the password by modifying the HTML,
// then revealing it once it is clicked by the user.
// This is used to prevent passwords being seen and stolen
// whenever the profile page is opened in a public place.
function revealPassword(){
  pw = document.querySelector('.profile-info__password');
  password = pw.innerHTML;
  if(password){
    pw.innerHTML = 'Click te reveal password!';
    pw.addEventListener('click', function(e){
      pw.innerHTML = password;
      pw.classList.remove('cursor--pointer');
      pw.classList.add('cursor--text');
    });
  }
}

revealPassword();