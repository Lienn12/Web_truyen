document.addEventListener("DOMContentLoaded", function () {
    var menuToggle = document.getElementById("menu-toggle");
    var sidebarMenu = document.getElementById("sidebarMenu");

    menuToggle.addEventListener("click", function () {
        sidebarMenu.classList.toggle("active");
    });

    var currentPath = window.location.pathname.toLowerCase();
    var links = document.querySelectorAll(".sidebar-link");

    links.forEach(function (link) {
        var href = link.getAttribute("href");
        if (href) {
            var hrefPath = href.split('?')[0].toLowerCase();
            if (
                (hrefPath !== "/" && currentPath.startsWith(hrefPath)) ||
                (hrefPath === "/" && currentPath === "/")
            ) {
                link.classList.add("active-page");
            }
        }
    });
});
