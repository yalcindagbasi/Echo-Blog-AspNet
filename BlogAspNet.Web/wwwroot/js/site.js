$(document).ready(function() {
    $(".category-filters button").click(function() {
        $(".category-filters button").removeClass("btn-amber").addClass("btn-light-amber");
        $(this).removeClass("btn-light-amber").addClass("btn-amber");

        var categoryId = $(this).data("category-id");

        $.ajax({
            url: '/Blog/GetBlogsByCategory',
            type: 'GET',
            data: { categoryId: categoryId, limit: 6 }, 
            success: function(response) {
                $("#blogListContainer").html(response);
            },
            error: function(error) {
                console.log("Hata oluştu: ", error);
            }
        });
    });
});