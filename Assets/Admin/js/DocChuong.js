document.addEventListener("DOMContentLoaded", function () {
    const btnBack = document.getElementById("btnback");
    if (btnBack) {
        btnBack.addEventListener("click", function () {
            const url = btnBack.getAttribute("data-url");
            if (url) {
                window.location.href = url;
            }
        });
    }

    // Lắng nghe sự kiện click cho btnSubmit
    const btnSubmit = document.getElementById("btnSubmit");
    if (btnSubmit) {
        btnSubmit.addEventListener("click", function (event) {
            // Tìm kiếm input với name 'Chuong.DaDang' và cập nhật giá trị
            var existingInput = document.querySelector("input[name='Chuong.DaDang']");
            if (existingInput) {
                existingInput.value = "true";  // Đặt DaDang thành true
            }
            // Không cần submit lại form ở đây vì form đã có nút submit
        });
    }

    const toggle = document.getElementById('toggleMucLuc');
    const icon = toggle.querySelector('.toggle-icon');

    toggle.addEventListener('click', () => {
        icon.classList.toggle('fa-chevron-down');
        icon.classList.toggle('fa-chevron-up');
    });
});
