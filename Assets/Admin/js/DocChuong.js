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

    const toggle = document.getElementById('toggleMucLuc');
    if (toggle) {
        const icon = toggle.querySelector('.toggle-icon');
        toggle.addEventListener('click', function () {
            icon.classList.toggle('fa-chevron-down');
            icon.classList.toggle('fa-chevron-up');
        });
    }

    const textarea = document.getElementById('noiDung');
    if (textarea) {
        textarea.style.height = textarea.scrollHeight + 'px';
        textarea.addEventListener('input', function () {
            this.style.height = 'auto';
            this.style.height = this.scrollHeight + 'px';
        });
    }
});
function capNhatLuotDoc(chuongId, truyenId, token) {
    if (!chuongId || !truyenId || !token) {
        console.error('Thiếu tham số bắt buộc:', { chuongId, truyenId, token });
        return;
    }
    const formData = new URLSearchParams();
    formData.append('chuongId', chuongId);
    formData.append('truyenId', truyenId);
    formData.append('__RequestVerificationToken', token);

    fetch('/Admin/Chuong/CapNhatLuotDoc', {
        method: 'POST',
        headers: {
            'Content-Type': 'application/x-www-form-urlencoded'
        },
        body: formData.toString()
    })
        .catch(err => {
            console.error('Lỗi fetch:', err);
        });
}

document.addEventListener('DOMContentLoaded', function () {
    const dataDiv = document.getElementById('dataLuotDoc');
    if (!dataDiv) {
        console.error('Không tìm thấy phần tử dataLuotDoc');
        return;
    }

    const chuongId = dataDiv.getAttribute('data-chuongid');
    const truyenId = dataDiv.getAttribute('data-truyenid');
    const tokenInput = document.querySelector('#antiForgeryForm input[name="__RequestVerificationToken"]');
    const token = tokenInput ? tokenInput.value : null;


    if (!chuongId || !truyenId || !token) {
        console.error('Thiếu dữ liệu bắt buộc:', { chuongId, truyenId, token });
        return;
    }
    const timeoutId = setTimeout(() => {
        capNhatLuotDoc(chuongId, truyenId, token);
    }, 3000); 

    window.addEventListener('beforeunload', () => {
        clearTimeout(timeoutId);
    });
});