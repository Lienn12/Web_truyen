document.addEventListener('DOMContentLoaded', function () {
    const shortDescEl = document.getElementById('short-desc');
    const readMoreEl = document.getElementById('read-more');
    const readLessEl = document.getElementById('read-less');

    const fullText = shortDescEl.textContent.trim();
    const maxLength = 650;

    if (fullText.length > maxLength) {
        const shortText = fullText.substring(0, maxLength) + '...';
        shortDescEl.textContent = shortText;
        readMoreEl.style.display = 'inline';

        readMoreEl.addEventListener('click', function (e) {
            e.preventDefault();
            shortDescEl.textContent = fullText;
            readMoreEl.style.display = 'none';
            readLessEl.style.display = 'inline';
        });

        readLessEl.addEventListener('click', function (e) {
            e.preventDefault();
            shortDescEl.textContent = shortText;
            readMoreEl.style.display = 'inline';
            readLessEl.style.display = 'none';
        });
    }
});
window.onload = function () {
    var alert = document.getElementById('notifyMessage');
    if (alert) {
        setTimeout(function () {
            alert.style.opacity = 0;
            setTimeout(function () {
                alert.style.display = 'none';
            }, 500);
        }, 3000);
    }
}