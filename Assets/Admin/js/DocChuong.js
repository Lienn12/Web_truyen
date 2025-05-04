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

    const btnSubmit = document.getElementById("btnSubmit");
    if (btnSubmit) {
        btnSubmit.addEventListener("click", function (event) {
            var existingInput = document.querySelector("input[name='Chuong.DaDang']");
            if (existingInput) {
                existingInput.value = "true";  
            }
        });
    }

    const toggle = document.getElementById('toggleMucLuc');
    const icon = toggle.querySelector('.toggle-icon');

    toggle.addEventListener('click', () => {
        icon.classList.toggle('fa-chevron-down');
        icon.classList.toggle('fa-chevron-up');
    });
});

    const textarea = document.getElementById('noiDung');
    if (textarea) {
        textarea.style.height = textarea.scrollHeight + 'px';

    textarea.addEventListener('input', function () {
        this.style.height = 'auto';
    this.style.height = this.scrollHeight + 'px';
        });
    }
document.addEventListener("DOMContentLoaded", function () {
    const replyButtons = document.querySelectorAll('.reply-btn');
    const submitButtons = document.querySelectorAll('.submit-reply-btn');

    // Sự kiện cho nút "Trả lời"
    replyButtons.forEach(function (btn) {
        btn.addEventListener('click', function () {
            const commentId = this.getAttribute('data-comment-id');
            const username = this.getAttribute('data-username'); // Lấy tên người dùng từ data-username
            const form = document.getElementById('reply-form-' + commentId);
            const textarea = document.getElementById('reply-textarea-' + commentId);

            if (form && textarea) {
                form.style.display = 'block';
                textarea.value = '@' + username + ' '; // Lưu ý: Không thể sử dụng Razor syntax trực tiếp trong JavaScript
                textarea.focus();
            }
        });
    });

    // Sự kiện cho nút "Gửi"
    submitButtons.forEach(function (btn) {
        btn.addEventListener('click', function () {
            const commentId = this.getAttribute('data-comment-id');
            const form = document.getElementById('reply-form-' + commentId);

            if (form) {
                if (form.submit) {
                    form.submit(); 
                } else {
                    console.error("Form không hợp lệ hoặc không có phương thức submit.");
                }
            }
        });
    });
});
function toggleReplies(button, commentId) {
    const replyWrapper = document.getElementById('replies-' + commentId);
    if (replyWrapper.style.display === 'none') {
        replyWrapper.style.display = 'block';
        button.innerText = 'Ẩn trả lời';
    } else {
        replyWrapper.style.display = 'none';
        button.innerText = 'Xem trả lời';
    }
}

// Hiển thị/ẩn form trả lời
document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('.reply-btn').forEach(function (btn) {
        btn.addEventListener('click', function () {
            const commentId = this.getAttribute('data-comment-id');
            const form = document.getElementById('reply-form-' + commentId);
            form.style.display = form.style.display === 'none' ? 'block' : 'none';
        });
    });
});