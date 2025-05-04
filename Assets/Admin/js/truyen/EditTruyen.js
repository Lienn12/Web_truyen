function showSection(sectionId, button) {
    document.querySelectorAll('.section').forEach(function (section) {
        section.style.display = 'none';
    });

    var addBtn = document.getElementById('addChapterBtn');
    if (addBtn) {
        addBtn.style.display = (sectionId === 'chapter-section') ? 'block' : 'none';
    }

    document.getElementById(sectionId).style.display = 'block';

    document.querySelectorAll('.tab-btn').forEach(function (tab) {
        tab.classList.remove('active');
    });
    button.classList.add('active');
}
document.addEventListener("DOMContentLoaded", function () {
    // 1. Quản lý quyền chỉnh sửa
    var canEdit = document.getElementById('canEditValue').value === 'true';

    document.getElementById('TieuDe').readOnly = !canEdit;
    document.getElementById('MoTa').readOnly = !canEdit;
    document.getElementById('TheLoaiId').disabled = !canEdit;
    document.getElementById('TrangThai').disabled = !canEdit;

    // 2. Nút quay lại theo role
    var backButton = document.getElementById('back');
    if (backButton) {
        var adminUrl = backButton.getAttribute('data-admin-url');
        var userUrl = backButton.getAttribute('data-user-url');
        var userRole = document.getElementById('userRole').value;

        backButton.addEventListener('click', function () {
            if (userRole === 'admin') {
                window.location.href = adminUrl;
            } else if (userRole === 'author') {
                window.location.href = userUrl;
            }
        });
    }

    // 3. Nút Cancel và Preview chỉ hoạt động khi canEdit là true
    if (canEdit) {
        var btnCancel = document.getElementById("btnCancel");
        if (btnCancel) {
            btnCancel.addEventListener("click", function () {
                var url = this.getAttribute("data-url");
                window.location.href = url;
            });
        }

        var btnPreview = document.getElementById("btn-preview");
        if (btnPreview) {
            btnPreview.addEventListener("click", function () {
                var url = this.getAttribute("data-url");
                window.location.href = url;
            });
        }
    }


    // 4. Xem trước ảnh bìa
    var fileInput = document.getElementById("AnhBiaFile");
    if (fileInput) {
        fileInput.addEventListener("change", function (event) {
            var file = event.target.files[0];
            if (file && file.type.startsWith("image/")) {
                var reader = new FileReader();
                reader.onload = function (e) {
                    document.getElementById("previewImage").src = e.target.result;
                };
                reader.readAsDataURL(file);
            } else {
                alert("Vui lòng chọn một tệp hình ảnh hợp lệ.");
            }
        });
    }

    // 5. Tự ẩn thông báo sau 3 giây
    setTimeout(function () {
        var msg = document.querySelector('.update-success');
        if (msg) {
            msg.style.display = 'none';
        }
    }, 3000);

    // 6. Submit form và upload ảnh
    var formEdit = document.getElementById("formEdit");
    if (formEdit) {
        formEdit.addEventListener("submit", function (event) {
            var fileInput = document.getElementById("AnhBiaFile");
            if (fileInput && fileInput.files.length > 0) {
                event.preventDefault();
                var formData = new FormData(formEdit);
                formData.append("AnhBiaFile", fileInput.files[0]);

                fetch(formEdit.action, {
                    method: "POST",
                    body: formData
                })
                    .then(response => response.text())
                    .then(data => {
                        console.log("Cập nhật thành công!", data);
                        window.location.reload();
                    })
                    .catch(error => console.error("Có lỗi xảy ra:", error));
            }
        });
    }
});


