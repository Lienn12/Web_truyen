document.addEventListener("DOMContentLoaded", function () {
    var toggle = document.getElementById("toggleMucLuc");
    var mucLuc = document.getElementById("mucLucContainer");
    if (toggle && mucLuc) {
        toggle.addEventListener("click", function (event) {
            event.stopPropagation();
            if (mucLuc.style.display === "none" || mucLuc.style.display === "") {
                mucLuc.style.display = "block";
            } else {
                mucLuc.style.display = "none";
            }
        });

        document.addEventListener("click", function (event) {
            if (!mucLuc.contains(event.target) && event.target !== toggle) {
                mucLuc.style.display = "none";
            }
        });
    }
});