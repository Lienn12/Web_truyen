function showProfileSection(sectionId, button) {
    // Ẩn tất cả các phần trong profile
    document.querySelectorAll('.profile-section').forEach(function (section) {
        section.style.display = 'none';
    });

    // Hiện phần được chọn
    document.getElementById(sectionId).style.display = 'block';

    // Cập nhật nút đang active
    document.querySelectorAll('.toggle-btn').forEach(function (btn) {
        btn.classList.remove('active');
    });
    button.classList.add('active');
}

// Tự động click nút đầu tiên khi trang load
document.addEventListener("DOMContentLoaded", function () {
    const firstBtn = document.querySelector(".toggle-btn");
    if (firstBtn) {
        firstBtn.click();
    }
});