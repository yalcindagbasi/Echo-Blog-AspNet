$(document).ready(function() {
    // Kategori butonlarına tıklama olayını dinle
    $(".category-filters button").click(function() {
        // Tıklanan butonun aktif olmasını sağla
        $(".category-filters button").removeClass("btn-amber").addClass("btn-light-amber");
        $(this).removeClass("btn-light-amber").addClass("btn-amber");

        // Kategori ID'sini al
        var categoryId = $(this).data("category-id");

        // AJAX ile blogları getir
        $.ajax({
            url: '/Blog/GetBlogsByCategory',
            type: 'GET',
            data: { categoryId: categoryId, limit: 6 }, // Her zaman sadece 6 blog için limit ekle
            success: function(response) {
                // Blog listesini güncelle
                $("#blogListContainer").html(response);
            },
            error: function(error) {
                console.log("Hata oluştu: ", error);
            }
        });
    });
});