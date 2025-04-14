document.addEventListener("DOMContentLoaded", function () {
    var menuToggle = document.getElementById("menu-toggle");
    var sidebarMenu = document.getElementById("sidebarMenu");

    menuToggle.addEventListener("click", function () {
        sidebarMenu.classList.toggle("active");
    });

    // Đánh dấu mục hiện tại
    var currentPath = window.location.pathname.toLowerCase();
    var links = document.querySelectorAll(".sidebar-link");

    links.forEach(function (link) {
        var href = link.getAttribute("href");
        if (href && currentPath.includes(href.toLowerCase())) {
            link.classList.add("active-page");
        }
    });
});
