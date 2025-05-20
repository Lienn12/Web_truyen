document.addEventListener('DOMContentLoaded', function () {
    const docGiaCtx = document.getElementById('docGiaChart').getContext('2d');
    const binhLuanCtx = document.getElementById('binhLuanChart').getContext('2d');

    const docGiaLabels = window.docGiaLabels || [];
    const docGiaData = window.docGiaData || [];
    const binhLuanLabels = window.binhLuanLabels || [];
    const binhLuanData = window.binhLuanData || [];

    // Biểu đồ: Lượt đọc
    new Chart(docGiaCtx, {
        type: 'line',
        data: {
            labels: docGiaLabels,
            datasets: [{
                label: 'Lượt đọc',
                data: docGiaData,
                fill: true,
                borderColor: 'rgba(75, 192, 192, 1)',
                backgroundColor: 'rgba(75, 192, 192, 0.2)',
                pointRadius: 4,
                pointHoverRadius: 6,
                tension: 0.4
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: { display: true },
                tooltip: { mode: 'index', intersect: false }
            },
            scales: {
                x: {
                    title: { display: true, text: 'Ngày' },
                    ticks: {
                        maxRotation: 60,
                        minRotation: 30
                    }
                },
                y: {
                    title: { display: true, text: 'Lượt đọc' },
                    beginAtZero: true
                }
            }
        }
    });

    // Biểu đồ: Bình luận
    new Chart(binhLuanCtx, {
        type: 'line',
        data: {
            labels: binhLuanLabels,
            datasets: [{
                label: 'Bình luận',
                data: binhLuanData,
                fill: true,
                borderColor: 'rgba(255, 99, 132, 1)',
                backgroundColor: 'rgba(255, 99, 132, 0.2)',
                pointRadius: 4,
                pointHoverRadius: 6,
                tension: 0.4
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: { display: true },
                tooltip: { mode: 'index', intersect: false }
            },
            scales: {
                x: {
                    title: { display: true, text: 'Ngày' },
                    ticks: {
                        maxRotation: 60,
                        minRotation: 30
                    }
                },
                y: {
                    title: { display: true, text: 'Số lượng' },
                    beginAtZero: true
                }
            }
        }
    });
});
