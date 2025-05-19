document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll('.write-button').forEach(button => {
        button.addEventListener('click', function () {
            const storyCard = button.closest('.story-card');
            const popup = storyCard.querySelector('.chapter-popup');
            if (popup.style.display === 'block') {
                popup.style.display = 'none';
            } else {
                // Ẩn tất cả popup khác trước khi hiện
                document.querySelectorAll('.chapter-popup').forEach(p => p.style.display = 'none');
                popup.style.display = 'block';
            }
        });
    });

    // Ẩn popup nếu click ra ngoài
    document.addEventListener('click', function (e) {
        if (!e.target.closest('.story-card')) {
            document.querySelectorAll('.chapter-popup').forEach(p => p.style.display = 'none');
        }
    });
});
function togglePopup(btn) {
    const popup = btn.nextElementSibling;
    if (!popup) return;

    // Đóng tất cả popup khác trước
    document.querySelectorAll('.popup-menu').forEach(p => {
        if (p !== popup) p.style.display = 'none';
    });

    // Bật/tắt popup hiện tại
    popup.style.display = popup.style.display === 'block' ? 'none' : 'block';
}

// Đóng popup khi click ra ngoài
document.addEventListener('click', function (event) {
    if (!event.target.closest('.top-buttons')) {
        document.querySelectorAll('.popup-menu').forEach(p => p.style.display = 'none');
    }
});
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
