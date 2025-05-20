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
function confirmPause(truyenId) {
    if (!confirm("Bạn có chắc muốn dừng đăng tải truyện này không?")) return;

    const tokenInput = document.querySelector('input[name="__RequestVerificationToken"]');
    const token = tokenInput ? tokenInput.value : "";

    fetch(dungDangTaiUrl, {
        method: 'POST',
        headers: {
            'Content-Type': 'application/json',
            'RequestVerificationToken': token
        },
        body: JSON.stringify({ id: truyenId })
    })
        .then(response => {
            if (response.ok) {
                alert("Đã dừng đăng tải truyện.");
                location.reload();
            } else {
                alert("Lỗi khi dừng đăng tải.");
            }
        })
        .catch(error => {
            console.error("Lỗi:", error);
            alert("Có lỗi xảy ra.");
        });
}

function confirmDelete(id) {
    if (confirm('Bạn có chắc chắn muốn xóa truyện này không?')) {
        const returnUrl = '@Url.Action("TruyenCuaToi", "User", new { userId = userId })';
        window.location.href = `/Admin/Truyen/Delete?id=${id}&returnUrl=${encodeURIComponent(returnUrl)}`;
    }
}
