document.addEventListener('DOMContentLoaded', function () {
    const input = document.getElementById('AnhBiaFileInput');
    const preview = document.getElementById('previewImage');

    if (input) {
        input.addEventListener('change', function (event) {
            const file = event.target.files[0];
            if (file && preview) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    preview.src = e.target.result;
                };
                reader.readAsDataURL(file);
            }
        });
    }
});
document.getElementById("btnCancel").addEventListener("click", function () {
    var cancelUrl = this.getAttribute("data-url");
    var isConfirmed = window.confirm("Bạn có chắc chắn muốn huỷ?");
    if (isConfirmed) {
        window.location.href = cancelUrl;
    }
});

document.getElementById("btnSaveAndNext").addEventListener("click", function () {
    var form = document.getElementById("ThemMoi");

    // Gửi form để lưu truyện trước
    form.submit();

    setTimeout(function () {
        var truyenId = document.getElementById("TruyenIdHidden").value;

        // Gửi yêu cầu tạo chương mới
        fetch("/Admin/Chuong/CreateEmpty?truyenId=" + truyenId, {
            method: "POST"
        })
            .then(response => response.json())
            .then(data => {
                if (data.success) {
                    window.location.href = "/Admin/Chuong/Edit?id=" + data.chuongId ;
                } else {
                    alert("Lỗi khi tạo chương mới!");
                }
            });
    }, 2000); // Đợi 2 giây để đảm bảo truyện được lưu
});

