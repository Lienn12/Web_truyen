function previewAvatar(event) {
    var file = event.target.files[0];
    var avatarElement = document.getElementById("profile-avatar");

    if (file) {
        var reader = new FileReader();
        reader.onload = function (e) {
            avatarElement.src = e.target.result;
        };
        reader.readAsDataURL(file);
    } else {
        var currentAvatar = avatarElement.src;
        if (!currentAvatar.includes("user1.png")) {
            avatarElement.src = currentAvatar;  
        }
    }
}

function showProfileSection(sectionId, button) {
    var section = document.getElementById(sectionId);
    if (section.style.display === "none") {
        section.style.display = "block";
        button.innerText = "Giới thiệu";
    }
}