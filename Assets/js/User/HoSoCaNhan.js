function showProfileSection(sectionId, button) {
    document.querySelectorAll('.profile-section').forEach(section => {
        section.style.display = 'none';
    });

    document.getElementById(sectionId).style.display = 'block';

    document.querySelectorAll('.toggle-btn').forEach(btn => {
        btn.classList.remove('active');
    });
    button.classList.add('active');
    localStorage.setItem('activeProfileTab', sectionId);
}

document.addEventListener("DOMContentLoaded", function () {
    const savedTab = localStorage.getItem('activeProfileTab');

    if (savedTab) {
        const button = Array.from(document.querySelectorAll('.toggle-btn'))
            .find(btn => btn.getAttribute('onclick').includes(savedTab));

        if (button) {
            showProfileSection(savedTab, button);
            return;
        }
    }

    const firstBtn = document.querySelector(".toggle-btn");
    if (firstBtn) {
        showProfileSection(firstBtn.getAttribute('onclick').match(/'([^']+)'/)[1], firstBtn);
    }
});


document.querySelectorAll('.popup-trigger').forEach(el => {
    el.addEventListener('click', function () {
        const truyenId = this.getAttribute('data-id');
        document.getElementById('popup-truyenId').value = truyenId;
    });
});

document.addEventListener("DOMContentLoaded", function () {
    const firstBtn = document.querySelector(".toggle-btn");
    if (firstBtn) {
        firstBtn.click();
    }
});
document.addEventListener('DOMContentLoaded', function () {
    const triggers = document.querySelectorAll('.popup-trigger');
    const popup = document.getElementById('story-popup');
    const closeBtn = document.querySelector('.close-btn');

    triggers.forEach(trigger => {
        trigger.addEventListener('click', () => {
            document.getElementById('popup-title').textContent = trigger.dataset.title;
            document.getElementById('popup-image').src = trigger.dataset.image;
            const fullDesc = trigger.dataset.description;
            const shortDescEl = document.getElementById('popup-short-desc');
            const readMoreLink = document.getElementById('popup-read-more');

            const maxDescLength = 150;
            if (fullDesc.length > maxDescLength) {
                shortDescEl.textContent = fullDesc.substring(0, maxDescLength) + '...';
                readMoreLink.style.display = 'inline';
                readMoreLink.href = `/Admin/Truyen/ChiTiet/${trigger.dataset.id}`;

            } else {
                shortDescEl.textContent = fullDesc;
                readMoreLink.style.display = 'none';
            }
            document.getElementById('popup-detail-link').href = `/Admin/Truyen/ChiTiet/?id=${trigger.dataset.id}`;

            updatePopupStatus(trigger.dataset.status);
            document.getElementById('popup-views').textContent = trigger.dataset.views;
            document.getElementById('popup-chapters').textContent = trigger.dataset.chapters;
            popup.style.display = 'flex';
        });
    });

    closeBtn.addEventListener('click', () => {
        popup.style.display = 'none';
    });

    // Đóng popup khi click ra ngoài
    popup.addEventListener('click', (e) => {
        if (e.target === popup) {
            popup.style.display = 'none';
        }
    });

});
function updatePopupStatus(statusText) {
    const statusEl = document.getElementById('popup-status');
    statusEl.textContent = statusText;

    statusEl.classList.remove('status-complete', 'status-writing');

    if (statusText.toLowerCase().includes('hoàn thành')) {
        statusEl.classList.add('status-complete');
        statusEl.innerHTML = statusText + '<i class="fa-solid fa-circle-check"></i>';
    } else {
        statusEl.classList.add('status-writing');
    }
}
document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll(".form-follow").forEach(form => {
        form.addEventListener("submit", function () {
            // Delay reload để server xử lý xong (nếu redirect không xảy ra)
            setTimeout(function () {
                location.reload();
            }, 500);
        });
    });

    // Nếu dùng form theo dõi ở phần đầu trang
    const theoDoiForms = document.querySelectorAll('form[action*="TheoDoiNguoiDung"], form[action*="HuyTheoDoiNguoiDung"]');
    theoDoiForms.forEach(form => {
        form.addEventListener("submit", function () {
            setTimeout(function () {
                location.reload();
            }, 500);
        });
    });
});
