// Kategori filtreleme işlevi
document.addEventListener('DOMContentLoaded', function() {
    // Kategori butonlarını seç
    const categoryButtons = document.querySelectorAll('.category-filters .btn');

    // Her kategori butonuna tıklama olayı ekle
    categoryButtons.forEach(button => {
        button.addEventListener('click', function() {
            // Tüm butonların aktif sınıfını kaldır
            categoryButtons.forEach(btn => {
                btn.classList.remove('btn-amber');
                btn.classList.add('btn-light-amber');
            });

            // Tıklanan butonu aktif yap
            this.classList.remove('btn-light-amber');
            this.classList.add('btn-amber');

            const categoryId = this.getAttribute('data-category-id');

            // AJAX ile kategori filtrelemesi yap
            fetch(`/Blog/GetBlogsByCategory?categoryId=${categoryId}`)
                .then(response => response.text())
                .then(html => {
                    // Blog listesi konteynırını güncelle
                    document.querySelector('.container .row.g-4').parentElement.innerHTML = html;
                })
                .catch(error => {
                    console.error('Kategori filtreleme hatası:', error);
                });
        });
    });
});