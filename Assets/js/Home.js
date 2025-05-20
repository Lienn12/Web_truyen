document.querySelectorAll('.tab-btn').forEach(btn => {
    btn.addEventListener('click', () => {
        // Đổi active tab
        document.querySelectorAll('.tab-btn').forEach(el => el.classList.remove('active'));
        btn.classList.add('active');

        // Ẩn/hiện nội dung tab tương ứng
        const target = btn.dataset.target; // "hot-week" hoặc "hot-month"
        document.querySelectorAll('.hot-section').forEach(section => {
            section.classList.remove('active');
        });
        document.querySelector('.hot-section.' + target).classList.add('active');
    });
});

document.querySelectorAll('.carousel-wrapper').forEach(wrapper => {
    const carousel = wrapper.querySelector('.carousel');
    const prevBtn = wrapper.querySelector('.carousel-btn.prev');
    const nextBtn = wrapper.querySelector('.carousel-btn.next');
    const scrollAmount = 300; // pixels để di chuyển mỗi lần click

    prevBtn.addEventListener('click', () => {
        carousel.scrollBy({
            left: -scrollAmount,
            behavior: 'smooth'
        });
    });

    nextBtn.addEventListener('click', () => {
        carousel.scrollBy({
            left: scrollAmount,
            behavior: 'smooth'
        });
    });

    // Ẩn nút nếu không cần thiết
    function checkButtons() {
        prevBtn.style.display = carousel.scrollLeft > 0 ? 'block' : 'none';
        nextBtn.style.display = (carousel.scrollLeft + carousel.clientWidth) < carousel.scrollWidth ? 'block' : 'none';
    }

    carousel.addEventListener('scroll', checkButtons);
    window.addEventListener('resize', checkButtons);

    checkButtons(); // chạy ngay khi load trang
});
