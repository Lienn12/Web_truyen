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

let quill;

document.addEventListener("DOMContentLoaded", function () {
    // 1. Khởi tạo Quill editor
    quill = new Quill('#quillEditor', {
        theme: 'bubble',
        modules: {
            toolbar: [
                ['bold', 'italic', 'underline'],
                [{ list: 'ordered' }, { list: 'bullet' }],
                ['link'],
                ['clean']
            ]
        },
        resize: true,  
        formats: ['bold', 'italic', 'underline', 'list', 'image', 'video']
        
    });

    const plusButton = document.getElementById('plus-button');
    const insertMenu = document.getElementById('insert-menu');
    const charCount = document.getElementById('charCount');
    const editor = document.querySelector('.ql-editor');

    // 2. Đếm ký tự và hiển thị nút +
    quill.on('text-change', function () {
        const text = quill.getText().trim();
        charCount.textContent = `(${text.length})`;

        setTimeout(() => {
            const range = quill.getSelection();
            if (!range) return;

            const [line] = quill.getLine(range.index);
            const textLine = line?.domNode?.textContent?.replace(/\u00A0/g, '').trim() || '';
            const isLineEmpty = textLine === '' && !line?.domNode?.querySelector('img, video, iframe');

            if (isLineEmpty) {
                const bounds = quill.getBounds(range);
                plusButton.style.top = (bounds.top + 370) + 'px';
                plusButton.style.left = plusButton.offsetLeft + 'px';
                plusButton.style.display = 'block';
            } else {
                plusButton.style.display = 'none';
                insertMenu.style.display = 'none';
            }
        }, 10);
    });

    // 3. Hiển thị tooltip khi chọn văn bản
    quill.on("selection-change", function (range) {
        const tooltip = document.querySelector(".ql-tooltip");
        if (range && range.length > 0) {
            tooltip?.classList.add("ql-active");
        } else {
            tooltip?.classList.remove("ql-active");
        }
    });

    // 4. Ẩn insert menu khi click bên ngoài
    document.addEventListener('click', function (e) {
        if (!insertMenu.contains(e.target) && e.target !== plusButton) {
            insertMenu.style.display = 'none';
            plusButton.style.display = 'none';
        }
    });

    // 5. Hiển thị menu khi nhấn nút +
    plusButton.addEventListener('click', function () {
        const bounds = plusButton.getBoundingClientRect();
        insertMenu.style.left = `${bounds.left - 300}px`;
        insertMenu.style.top = `${parseInt(plusButton.style.top) + 20}px`;
        insertMenu.style.display = 'block';
    });

    // 6. Chèn hình ảnh từ local
    document.getElementById("insertImageButton")?.addEventListener("click", function () {
        const input = document.createElement('input');
        input.type = 'file';
        input.accept = 'image/*';
        input.onchange = (e) => {
            const file = e.target.files[0];
            if (!file) return;

            const reader = new FileReader();
            reader.onload = (event) => {
                const imageUrl = event.target.result;
                const range = quill.getSelection() || { index: quill.getLength(), length: 0 };
                quill.insertEmbed(range.index, 'image', imageUrl);
                plusButton.style.display = 'none';
                insertMenu.style.display = 'none';
            };
            reader.readAsDataURL(file);
        };
        input.click();
    });

    // 7. Chèn video từ YouTube hoặc Vimeo
    document.getElementById("insertVideoButton")?.addEventListener("click", function () {
        const videoUrl = prompt("Nhập URL video (YouTube, Vimeo):");
        if (!videoUrl) return;

        const range = quill.getSelection(true);
        if (!range) return;

        let inserted = false;

        if (videoUrl.includes("youtube.com") || videoUrl.includes("youtu.be")) {
            const embedUrl = convertYoutubeUrl(videoUrl);
            const videoDiv = `<div style="text-align: center;">
                            <iframe width="560" height="315" src="${embedUrl}" frameborder="0" allowfullscreen></iframe>
                          </div>`;
            quill.insertEmbed(range.index, 'video', embedUrl);
            inserted = true;
        } else if (videoUrl.includes("vimeo.com")) {
            quill.insertEmbed(range.index, 'video', videoUrl);
            inserted = true;
        } else {
            alert("Chỉ hỗ trợ video YouTube hoặc Vimeo.");
        }

        if (inserted) {
            plusButton.style.display = 'none';
            insertMenu.style.display = 'none';
        }
    });
    window.addEventListener("load", function () {
        const savedStatus = localStorage.getItem("savedStatus");

        if (savedStatus === "true") {
            document.getElementById("saveSuccess").style.display = "inline";
            document.getElementById("saveNotSaved").style.display = "none";
        } else {
            document.getElementById("saveSuccess").style.display = "none";
            document.getElementById("saveNotSaved").style.display = "inline";
        }
    });


    quill.on('text-change', function (delta, oldDelta, source) {
        if (!isChanged) {
            document.getElementById("saveNotSaved").style.display = "inline";
            isChanged = true;
            localStorage.setItem("savedStatus", "false");
            document.getElementById("saveSuccess").style.display = "none";
        }
    });
    let isSaved = false;
    let isChanged = false;
    document.getElementById("btnLuu")?.addEventListener("click", function () {
        const form = document.getElementById("formSuaChuong");
        document.getElementById("NoiDung").value = quill.root.innerHTML;

        if (!form.querySelector("input[name='isPublished']")) {
            const input = document.createElement("input");
            input.type = "hidden";
            input.name = "isPublished";
            input.value = "false";
            form.appendChild(input);
        }
        localStorage.setItem("savedStatus", "true");

        form.submit();
    });
  
    document.getElementById("btnDangThayDoi")?.addEventListener("click", function () {
        const form = document.getElementById("formSuaChuong");
        document.getElementById("NoiDung").value = quill.root.innerHTML;
        localStorage.setItem("savedStatus", "true");
        form.submit();
    });
    
    document.getElementById("saveNotSaved").style.display = "none";  

    document.getElementById("btnSubmit")?.addEventListener("click", function () {
        const form = document.getElementById("formSuaChuong");
        document.getElementById("NoiDung").value = quill.root.innerHTML;

        const dataContainer = document.getElementById("data-container");
        var tenTruyen = dataContainer.getAttribute("data-ten-truyen").trim();
        var theLoai = dataContainer.getAttribute("data-the-loai").trim();
        var role = dataContainer.getAttribute("data-role")?.trim().toLowerCase();

        if (!tenTruyen || tenTruyen === "Truyện chưa có tiêu đề" || !theLoai) {
            document.getElementById("editTruyenModal").style.display = "block";
            document.getElementById("menu").style.zIndex = "-1";
        } else {
            let isPublishedInput = form.querySelector("input[name='Chuong.DaDang']");
            if (!isPublishedInput) {
                isPublishedInput = document.createElement("input");
                isPublishedInput.type = "hidden";
                isPublishedInput.name = "isPublished";
                form.appendChild(isPublishedInput);
            }

            let trangThaiDuyetInput = form.querySelector("input[name='Chuong.TrangThaiDuyet']");
            if (!trangThaiDuyetInput) {
                trangThaiDuyetInput = document.createElement("input");
                trangThaiDuyetInput.type = "hidden";
                trangThaiDuyetInput.name = "Chuong.TrangThaiDuyet";
                form.appendChild(trangThaiDuyetInput);
            }

            if (role === "admin") {
                isPublishedInput.value = "true";
                trangThaiDuyetInput.value = "Đã duyệt";
            } else {
                isPublishedInput.value = "false";
                trangThaiDuyetInput.value = "chờ duyệt";
            }

            form.submit();

            setTimeout(function () {
                window.location.href = "DocChuong";
            }, 1000);
        }
    });



    // 10. Xem trước chương
    document.getElementById("btnPreview")?.addEventListener("click", function () {
        const url = this.getAttribute("data-url");
        if (url) {
            window.location.href = url;
        }
    });
    document.getElementById("btnCancel")?.addEventListener("click", function () {
        const url = this.getAttribute("data-url");
        if (url) {
            window.location.href = url;
        }
    });
    document.getElementById("back").addEventListener("click", function () {
        var url = this.getAttribute("data-url");
        if (url) {
            window.location.href = url;
        }
    });
    // 11. Ghi nội dung vào form khi submit (dự phòng)
    document.getElementById("formSuaChuong").addEventListener("submit", function () {
        document.getElementById("NoiDung").value = quill.root.innerHTML;
    });

    // 12. Hàm chuyển URL YouTube sang embed
    function convertYoutubeUrl(url) {
        let videoId = "";
        if (url.includes("watch?v=")) {
            videoId = url.split("watch?v=")[1].split("&")[0];
        } else if (url.includes("youtu.be/")) {
            videoId = url.split("youtu.be/")[1].split("?")[0];
        }
        return `https://www.youtube.com/embed/${videoId}`;
    }
});
function closeEditTruyenModal() {
    document.getElementById("editTruyenModal").style.display = "none";
    document.getElementById("menu").style.zIndex = "999";
}   
$(document).ready(function () {
    $("#EditTruyen").submit(function (event) {
        var isValid = true;

        var tieuDe = $("#TieuDe").val().trim();
        if (tieuDe === "" || tieuDe === "Truyện chưa có tiêu đề") {
            $("#TieuDeError").show();
            isValid = false;
        } else {
            $("#TieuDeError").hide();
        }

        var moTa = $("#MoTa").val().trim();
        if (moTa === "" || moTa === "Mô tả chưa có") {
            $("#MoTaError").show();
            isValid = false;
        } else {
            $("#MoTaError").hide();
        }

        var theLoaiId = $("#TheLoaiId").val();
        if (theLoaiId === "" || theLoaiId == 2) {
            $("#TheLoaiError").show();
            isValid = false;
        } else {
            $("#TheLoaiError").hide();
        }

        if (!isValid) {
            event.preventDefault();
        }
    });
});

$(document).ready(function () {
    $('#btnEdit').on('click', function () {
        $('#TrangThaiDuyet').val('Chờ duyệt');
        document.getElementById('NextAction').value = 'DocChuong';
        $('#formSuaChuong').submit();
        $('#EditTruyen').submit();
        console.log("Đã nhấn nút #btnEdit");
    });
});