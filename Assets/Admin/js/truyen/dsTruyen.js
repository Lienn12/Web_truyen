document.addEventListener("DOMContentLoaded", function () {
    var items = document.querySelectorAll(".filter-item");

    items.forEach(function (item) {
        item.addEventListener("click", function (event) {
            items.forEach(i => i.classList.remove("active"));

            item.classList.add("active");
            localStorage.setItem("selectedTab", item.innerText);
        });
    });
    var savedTab = localStorage.getItem("selectedTab");
    if (savedTab) {
        items.forEach(item => {
            if (item.innerText === savedTab) {
                item.classList.add("active");
            }
        });
    }
});
