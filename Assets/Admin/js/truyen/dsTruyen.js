document.addEventListener("DOMContentLoaded", function () {
    var items = document.querySelectorAll(".filter-item a");

    items.forEach(function (link) {
        link.addEventListener("click", function (event) {
            items.forEach(i => i.parentElement.classList.remove("active"));
            link.parentElement.classList.add("active");
            localStorage.setItem("selectedTab", link.innerText);
        });
    });

    var savedTab = localStorage.getItem("selectedTab");
    if (savedTab) {
        items.forEach(link => {
            if (link.innerText === savedTab) {
                link.parentElement.classList.add("active");
            }
        });
    }
});
document.querySelectorAll('form.form-lock').forEach(form => {
    form.addEventListener('submit', function (e) {
        if (!confirm('Bạn có chắc chắn muốn khóa tài khoản này không?')) {
            e.preventDefault(); // Hủy submit nếu không đồng ý
        }
    });
});

// Bắt sự kiện submit form mở khóa tài khoản (nếu cũng muốn xác nhận)
document.querySelectorAll('form.form-unlock').forEach(form => {
    form.addEventListener('submit', function (e) {
        if (!confirm('Bạn có chắc chắn muốn mở khóa tài khoản này không?')) {
            e.preventDefault();
        }
    });
});