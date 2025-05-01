function showSection(sectionId, button) {
    document.querySelectorAll('.section').forEach(function (section) {
        section.style.display = 'none';
    });

    if (sectionId === 'chapter-section') {
        document.getElementById('addChapterBtn').style.display = 'block';
    } else {
        document.getElementById('addChapterBtn').style.display = 'none';
    }

    // Hiển thị section hiện tại
    document.getElementById(sectionId).style.display = 'block';

    document.querySelectorAll('.tab-btn').forEach(function (tab) {
        tab.classList.remove('active');
    });
    button.classList.add('active');
}

document.getElementById("btnCancel").addEventListener("click", function () {
    var url = this.getAttribute("data-url");
    window.location.href = url;
});

document.getElementById("btn-preview").addEventListener("click", function () {
    var url = this.getAttribute("data-url");
    window.location.href = url;
});
document.getElementById("AnhBiaFile").addEventListener("change", function (event) {
    var file = event.target.files[0];
    if (file) {
        var reader = new FileReader();
        reader.onload = function (e) {
            // Cập nhật hình ảnh trong thẻ <img>
            document.getElementById("previewImage").src = e.target.result;
        }
        reader.readAsDataURL(file);
    }
});
setTimeout(function () {
    const msg = document.querySelector('.update-success');
    if (msg) msg.style.display = 'none';
}, 3000);
document.getElementById("formEdit").addEventListener("submit", function (event) {
    const fileInput = document.getElementById("AnhBiaFile");

    if (fileInput.files.length > 0) {
        const formData = new FormData(this);
        formData.append("AnhBiaFile", fileInput.files[0]); // Đính kèm file ảnh

        fetch(this.action, {
            method: "POST",
            body: formData
        }).then(response => response.text())
            .then(data => {
                console.log("Cập nhật thành công!", data);
                window.location.reload(); // Tải lại trang để hiển thị ảnh mới
            })
            .catch(error => console.error("Có lỗi xảy ra:", error));

        event.preventDefault(); // Ngăn form gửi đi theo cách thông thường
    }
});

