document.addEventListener('DOMContentLoaded', function () {
    const keepImage = document.getElementById('keepImage');
    const imageUpload = document.getElementById('imageUpload');
    const imageUrl = document.getElementById('imageUrl');

    const uploadSection = document.getElementById('uploadSection');
    const urlSection = document.getElementById('urlSection');

    function toggleSections() {
        if (imageUpload && imageUpload.checked) {
            if (uploadSection) uploadSection.style.display = 'block';
            if (urlSection) urlSection.style.display = 'none';
        } else if (imageUrl && imageUrl.checked) {
            if (uploadSection) uploadSection.style.display = 'none';
            if (urlSection) urlSection.style.display = 'block';
        } else {
            if (uploadSection) uploadSection.style.display = 'none';
            if (urlSection) urlSection.style.display = 'none';
        }
    }

    if (keepImage) keepImage.addEventListener('change', toggleSections);
    if (imageUpload) imageUpload.addEventListener('change', toggleSections);
    if (imageUrl) imageUrl.addEventListener('change', toggleSections);

    toggleSections();
});