function toggleMenu(button) {
    var menu = button.nextElementSibling;
    menu.style.display = menu.style.display === "block" ? "none" : "block";
}

// Đóng menu nếu click ra ngoài
document.addEventListener("click", function (event) {
    var menus = document.querySelectorAll(".options-menu");
    menus.forEach(menu => {
        if (!menu.contains(event.target) && !menu.previousElementSibling.contains(event.target)) {
            menu.style.display = "none";
        }
    });
});



