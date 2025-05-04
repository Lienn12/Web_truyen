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
document.addEventListener("DOMContentLoaded", function () {
    var canEdit = window.canEdit;
    var isAdminNotAuthor = window.isAdminNotAuthor;

    var chapters = document.querySelectorAll('.chapter');

    var editTemplate = document.getElementById("editUrlTemplate").value;
    var docTemplate = document.getElementById("docUrlTemplate").value;
    console.log("canEdit:", canEdit);
    console.log("isAdminNotAuthor:", isAdminNotAuthor);
    chapters.forEach(function (chapter) {
        chapter.addEventListener('click', function () {
            var chapterId = this.getAttribute('data-id');
            var editUrl = editTemplate.replace("__chapterId__", chapterId);
            var docUrl = docTemplate.replace("__chapterId__", chapterId);

            window.location.href = canEdit ? editUrl : docUrl;
        });
    });
});





