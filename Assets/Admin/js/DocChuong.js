function toggleReplies(btn, commentId) {
    const wrapper = document.getElementById('replies-' + commentId);
    if (wrapper) {
        wrapper.style.display = (wrapper.style.display === 'none' || wrapper.style.display === '') ? 'block' : 'none';
        btn.textContent = wrapper.style.display === 'block' ? 'Ẩn trả lời' : 'Xem ' + wrapper.children.length + ' trả lời';
    }
}
document.addEventListener("DOMContentLoaded", function () {
    const backButton = document.getElementById('btnback');

    if (backButton) {
        backButton.addEventListener('click', function () {
            const url = backButton.getAttribute('data-url');
            window.location.href = url;
        });
    }
});
document.addEventListener("DOMContentLoaded", function () {
    const backButton = document.getElementById('btn_back');

    if (backButton) {
        backButton.addEventListener('click', function () {
            const url = backButton.getAttribute('data-url');
            window.location.href = url;
        });
    }
});
document.addEventListener("DOMContentLoaded", function () {
    const replyButtons = document.querySelectorAll('.reply-btn');
    replyButtons.forEach(function (btn) {
        btn.addEventListener('click', function () {
            const commentId = this.getAttribute('data-comment-id');
            const form = document.getElementById('reply-form-' + commentId);
            const textarea = document.getElementById('reply-textarea-' + commentId);
            if (form && textarea) {
                form.style.display = (form.style.display === 'none' || form.style.display === '') ? 'block' : 'none';
                if (form.style.display === 'block') {
                    const username = this.getAttribute('data-username');
                    textarea.value = '@' + username + ' ';
                    textarea.focus();
                }
            }
        });
    });
    const backButton = document.getElementById('btnback');
    if (backButton) {
        backButton.addEventListener('click', function () {
            const url = backButton.getAttribute('data-url');
            window.location.href = url;
        });
    }
    // Lắng nghe sự kiện nhấp vào nút "Gửi"
    const submitButtons = document.querySelectorAll('.submit-reply-btn');
    submitButtons.forEach(function (btn) {
        btn.addEventListener('click', function () {
            const commentId = this.getAttribute('data-comment-id');
            const form = document.getElementById('reply-form-' + commentId);

            if (form) {
                form.submit();
            } else {
                console.error("Form không hợp lệ hoặc không có phương thức submit.");
            }
        });
    });

    // Toggle Mục Lục
    const toggle = document.getElementById('toggleMucLuc');
    if (toggle) {
        const icon = toggle.querySelector('.toggle-icon');
        toggle.addEventListener('click', function () {
            icon.classList.toggle('fa-chevron-down');
            icon.classList.toggle('fa-chevron-up');
        });
    }

    // Tự động điều chỉnh chiều cao của textarea
    const textarea = document.getElementById('noiDung');
    if (textarea) {
        textarea.style.height = textarea.scrollHeight + 'px';
        textarea.addEventListener('input', function () {
            this.style.height = 'auto';
            this.style.height = this.scrollHeight + 'px';
        });
    }
});
