function togglePasswordVisibility() {
    var passwordField = document.getElementById('password'); 
    var passwordIcon = document.getElementById('togglePassword'); 

    if (passwordField.type === "password") {
        passwordField.type = "text"; 
        passwordIcon.classList.remove("fa-eye");  
        passwordIcon.classList.add("fa-eye-slash");  
    } else {
        passwordField.type = "password"; 
        passwordIcon.classList.remove("fa-eye-slash");  
        passwordIcon.classList.add("fa-eye");  
    }
}
