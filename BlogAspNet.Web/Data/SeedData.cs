using BlogAspNet.Web.Models;
using BlogAspNet.Web.Models.Entities;
using BlogAspNet.Web.Models.Repositories;
using BlogAspNet.Web.Models.Repositories.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlogAspNet.Web.Data;

public static class SeedData
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var context = new AppDbContext(
            serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());

        var userManager = serviceProvider.GetRequiredService<UserManager<AppUser>>();

        try
        {
            if (context.Blogs.Any()) return;

            var users = await SeedUsers(userManager);

            var categories = await SeedCategories(context);

            await SeedBlogs(context, users, categories);
        }
        catch (Exception ex)
        {
            var logger = serviceProvider.GetRequiredService<ILogger<AppDbContext>>();
            logger.LogError(ex, "Veritabanı seed işlemi sırasında bir hata oluştu.");
            throw;
        }
    }

    private static async Task<List<AppUser>> SeedUsers(UserManager<AppUser> userManager)
    {
        var users = new List<AppUser>();
        var random = new Random();

        var profileImages = new List<string>
        {
            "https://i.pravatar.cc/150?img=1",
            "https://i.pravatar.cc/150?img=2",
            "https://i.pravatar.cc/150?img=3",
            "https://i.pravatar.cc/150?img=4",
            "https://i.pravatar.cc/150?img=5",
            "https://i.pravatar.cc/150?img=6",
            "https://i.pravatar.cc/150?img=7",
            "https://i.pravatar.cc/150?img=8",
            "https://i.pravatar.cc/150?img=9",
            "https://i.pravatar.cc/150?img=10"
        };

        var aboutMeTexts = new List<string>
        {
            "Yazılım geliştirme ve teknoloji konularında blog yazıları yazıyorum. Özellikle web teknolojileri ve mobil uygulama geliştirme alanında deneyim sahibiyim.",
            "Seyahat tutkunu ve fotoğrafçıyım. Gezdiğim yerleri ve deneyimlerimi paylaşmayı seviyorum.",
            "Yemek yapmayı ve yeni tarifler denemeyi seviyorum. Blog yazılarımda lezzetli ve pratik tarifleri paylaşıyorum.",
            "Kitap kurdu ve sinema tutkunuyum. Okuduğum kitaplar ve izlediğim filmler hakkında incelemeler yazıyorum.",
            "Fitness ve sağlıklı yaşam konusunda içerikler üretiyorum. Spor, beslenme ve mental sağlık üzerine yazılar yazıyorum.",
            "Doğa ve çevre konularında farkındalık yaratmayı amaçlıyorum. Sürdürülebilirlik ve ekolojik yaşam hakkında paylaşımlar yapıyorum.",
            "Yazılım mühendisi ve eğitimciyim. Öğrencilere yardımcı olmak için programlama ve teknoloji içerikleri üretiyorum.",
            "Sanat ve tasarım alanında çalışıyorum. Grafik tasarım, dijital sanat ve yaratıcılık üzerine yazılar yazıyorum.",
            "Ekonomi ve finans konularında uzmanım. Yatırım, tasarruf ve finansal okuryazarlık hakkında bilgiler paylaşıyorum.",
            "Kişisel gelişim ve motivasyon konularında içerikler üretiyorum. Daha iyi bir yaşam için pratik ipuçları sunuyorum."
        };

        var testUsers = new[]
        {
            new { Name = "Ahmet Yılmaz", Email = "ahmet@example.com", Username = "ahmetyilmaz" },
            new { Name = "Ayşe Demir", Email = "ayse@example.com", Username = "aysedemir" },
            new { Name = "Mehmet Kaya", Email = "mehmet@example.com", Username = "mehmetkaya" },
            new { Name = "Fatma Şahin", Email = "fatma@example.com", Username = "fatmasahin" },
            new { Name = "Ali Öztürk", Email = "ali@example.com", Username = "aliozturk" },
            new { Name = "Zeynep Aydın", Email = "zeynep@example.com", Username = "zeynepaydin" },
            new { Name = "Mustafa Çelik", Email = "mustafa@example.com", Username = "mustafacelik" },
            new { Name = "Esra Aksoy", Email = "esra@example.com", Username = "esraaksoy" },
            new { Name = "Burak Yıldız", Email = "burak@example.com", Username = "burakyildiz" },
            new { Name = "Sibel Korkmaz", Email = "sibel@example.com", Username = "sibelkorkmaz" }
        };

        int i = 0;
        foreach (var userInfo in testUsers)
        {
            var existingUser = await userManager.FindByNameAsync(userInfo.Username);
            if (existingUser != null)
            {
                users.Add(existingUser); 
                i++;
                continue;
            }
            var user = new AppUser
            {
                
                UserName = userInfo.Username,
                NormalizedUserName = userInfo.Username.ToUpper(),
                Email = userInfo.Email,
                NormalizedEmail = userInfo.Email.ToUpper(),
                EmailConfirmed = true,
                PhoneNumberConfirmed = true,
                FullName = userInfo.Name,
                ProfilePhotoUrl = profileImages[i],
                AboutMe = aboutMeTexts[i],
                CreatedDate = DateTime.Now.AddDays(-random.Next(1, 365)),
                
            };

            var result = await userManager.CreateAsync(user, "Pass123!");
            if (result.Succeeded)
            {
                users.Add(user);
            }

            i++;
        }

        return users;
    }

    private static async Task<List<Category>> SeedCategories(AppDbContext context)
    {
        var categories = new List<Category>
        {
            new Category { Name = "Genel", },
            new Category { Name = "Yazılım", },
            new Category { Name = "Seyahat", },
            new Category { Name = "Yemek", },
            new Category { Name = "Teknoloji", },
            new Category { Name = "Spor", },
            new Category { Name = "Yaşam", },
            new Category { Name = "Eğitim", },
            new Category { Name = "Sanat", },
            new Category { Name = "Finans", },
            new Category { Name = "Kişisel Gelişim", }
        };

        await context.Categories.AddRangeAsync(categories);
        await context.SaveChangesAsync();

        return categories;
    }

    private static async Task SeedBlogs(AppDbContext context, List<AppUser> users, List<Category> categories)
{
    if (await context.Blogs.AnyAsync())
    {
        Console.WriteLine("Veritabanında zaten blog var, yenileri atlanıyor.");
        return;
    }

    var random = new Random();
    var blogs = new List<Blog>();

    var titles = new List<string>
    {
        "Yazılım Geliştirme Sürecinde En İyi Pratikler",
        "Türkiye'nin En Güzel 10 Kamp Alanı",
        "Evde Kolayca Yapabileceğiniz 5 Lezzetli Yemek Tarifi",
        "Teknoloji Dünyasındaki Son Gelişmeler",
        "Başlangıç Seviyesi İçin Fitness Önerileri",
        "Minimalist Yaşam: Daha Azla Daha Çok Mutluluk",
        "Öğrenmeyi Öğrenmek: Etkili Çalışma Teknikleri",
        "Modern Sanat Akımları ve Etkileri",
        "Kişisel Finans Yönetimi İçin Temel İpuçları",
        "Kendini Geliştirmenin 7 Etkili Yolu"
    };

    var imageUrls = new List<string>
    {
        "https://picsum.photos/id/0/800/450",
        "https://picsum.photos/id/10/800/450",
        "https://picsum.photos/id/20/800/450",
        "https://picsum.photos/id/30/800/450",
        "https://picsum.photos/id/40/800/450",
        "https://picsum.photos/id/50/800/450",
        "https://picsum.photos/id/60/800/450",
        "https://picsum.photos/id/70/800/450",
        "https://picsum.photos/id/80/800/450",
        "https://picsum.photos/id/90/800/450"
    };

    for (int i = 0; i < 50; i++)
    {
        var paragraphCount = random.Next(3, 7);
        var contentBuilder = new System.Text.StringBuilder();

        for (int p = 0; p < paragraphCount; p++)
        {
            var sentences = random.Next(3, 8);
            for (int s = 0; s < sentences; s++)
            {
                var words = random.Next(5, 15);
                contentBuilder.Append(GenerateRandomSentence(words)).Append(" ");
            }
            contentBuilder.Append("\n\n");
        }

        if (users.Count == 0 || categories.Count == 0)
        {
            Console.WriteLine("Kullanıcı veya kategori listesi boş, blog oluşturulamıyor.");
            return;
        }

        var userId = users[random.Next(users.Count)].Id;
        var categoryId = categories[random.Next(categories.Count)].Id;

        var title = titles[i % titles.Count];
        var content = contentBuilder.ToString();
        var imageUrl = imageUrls[i % imageUrls.Count];
        var createdDate = DateTime.Now.AddDays(-random.Next(60));

        var blog = new Blog
        {
            Id = Guid.NewGuid(),
            Title = title,
            Content = content,
            ImageUrl = imageUrl,
            UserId = userId,
            CategoryId = categoryId,
            User = await context.Users.FindAsync(userId), 
            Category = await context.Categories.FindAsync(categoryId), 
            CreatedAt = createdDate,
            UpdatedAt = createdDate.AddHours(random.Next(1, 24)),
            ViewCount = random.Next(5, 1000),
        };

        blogs.Add(blog);
    }

    try
    {
        foreach (var blog in blogs)
        {
            blog.User = await context.Users.FindAsync(blog.UserId) ?? 
                throw new InvalidOperationException($"Kullanıcı bulunamadı: {blog.UserId}");
            
            blog.Category = await context.Categories.FindAsync(blog.CategoryId) ?? 
                throw new InvalidOperationException($"Kategori bulunamadı: {blog.CategoryId}");
            
            context.Blogs.Add(blog);
        }

        await context.SaveChangesAsync();
        Console.WriteLine($"{blogs.Count} blog başarıyla eklendi.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Blog ekleme hatası: {ex.Message}");
        throw;
    }

        
    await SeedComments(context, users, blogs);
    }
    private static string GenerateRandomSentence(int wordCount)
    {
        var words = new[] { "lorem", "ipsum", "dolor", "sit", "amet", "consectetur", "adipiscing", "elit", 
            "sed", "do", "eiusmod", "tempor", "incididunt", "ut", "labore", "et", "dolore", "magna", "aliqua" };
    
        var random = new Random();
        var sentence = new System.Text.StringBuilder();
    
        for (int i = 0; i < wordCount; i++)
        {
            if (i == 0)
            {
                var word = words[random.Next(words.Length)];
                sentence.Append(char.ToUpper(word[0]) + word.Substring(1));
            }
            else
            {
                sentence.Append(words[random.Next(words.Length)]);
            }
        
            if (i < wordCount - 1)
                sentence.Append(" ");
        }
    
        sentence.Append(".");
        return sentence.ToString();
    }
    private static async Task SeedComments(AppDbContext context, List<AppUser> users, List<Blog> blogs)
{
    if (await context.Comments.AnyAsync())
    {
        return;
    }

    var random = new Random();
    var comments = new List<Comment>();

    var commentTexts = new List<string>
    {
        "Harika bir yazı olmuş, teşekkür ederim.",
        "Bu konuda daha detaylı bilgi verebilir misiniz?",
        "Çok bilgilendirici bir paylaşım.",
        "Katılıyorum, bu konu gerçekten önemli.",
        "Yazınızı beğendim, başka benzer içerikler de paylaşır mısınız?",
        "Bu bakış açısı benim için yeni, teşekkürler.",
        "Farklı bir perspektif sunmuşsunuz, tebrikler.",
        "Bu konuda başka kaynak önerebilir misiniz?",
        "Yazım hataları var, düzeltirseniz daha iyi olur.",
        "Kesinlikle katılıyorum, çok doğru tespitler."
    };

    foreach (var blog in blogs)
    {
        var commentCount = random.Next(15);
        
        for (int i = 0; i < commentCount; i++)
        {
            var userId = users[random.Next(users.Count)].Id;
            var commentText = commentTexts[random.Next(commentTexts.Count)];
            var createdDate = blog.CreatedAt.AddHours(random.Next(1, 24 * 30)); 

            var comment = new Comment
            {
                Id = Guid.NewGuid(),
                Content = commentText,
                CreatedAt = createdDate,
                UserId = userId,  
                BlogId = blog.Id, 
            };

            comments.Add(comment);
        }
    }

    try
    {
        await context.Comments.AddRangeAsync(comments);
        await context.SaveChangesAsync();
        Console.WriteLine($"{comments.Count} yorum başarıyla eklendi.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Yorum ekleme hatası: {ex.Message}");
        throw;
    }
}
}