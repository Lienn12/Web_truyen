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
document.addEventListener("DOMContentLoaded", function () {
    const urlParams = new URLSearchParams(window.location.search);
    if (urlParams.get("requireLogin") === "true") {
        const result = confirm("Bạn cần đăng nhập để thực hiện chức năng này. Bạn có muốn đăng nhập không?");
        if (!result) {
            history.back(); 
        }
    }
});