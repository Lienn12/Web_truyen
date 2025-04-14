
    document.addEventListener("DOMContentLoaded", function () {
            const toggles = document.querySelectorAll(".dropdown-toggle");

    toggles.forEach(function (toggle) {
                const dropdown = toggle.closest(".dropdown");

    toggle.addEventListener("click", function (e) {
        e.preventDefault();
    dropdown.classList.toggle("open");
                });

    document.addEventListener("click", function (e) {
                    if (!dropdown.contains(e.target) && !toggle.contains(e.target)) {
        dropdown.classList.remove("open");
                    }
                });
            });
        });