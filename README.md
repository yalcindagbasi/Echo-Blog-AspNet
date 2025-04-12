# Blog Uygulaması

Bu proje, ASP.NET Core ve Entity Framework Core kullanarak geliştirilmiş bir blog uygulamasıdır. Kullanıcılar, blog yazıları oluşturabilir, kategoriler oluşturabilir ve mevcut yazılara yorum yapabilir. Ayrıca, yazılar popülerliklerine göre sıralanabilir ve görüntülenme sayıları takip edilebilir. Uygulama, kullanıcı yönetimi için ASP.NET Core Identity kullanmaktadır ve veritabanı işlemleri için MSSQL Server kullanılmaktadır.

## Proje Özellikleri

- **Blog Yönetimi**: Kullanıcılar blog yazıları oluşturabilir, düzenleyebilir ve silebilir.
- **Kategori Yönetimi**: Bloglar kategorilere ayrılabilir. Her blog yalnızca bir kategoriye ait olabilir.
- **Yorumlar**: Her blog yazısına kullanıcılar yorum yapabilir.
- **Popüler Yazılar**: Yazılar, görüntülenme sayısına göre sıralanabilir.
- **Kullanıcı Sistemi**: Kullanıcılar kayıt olabilir, giriş yapabilir ve kendi yazılarını görüntüleyebilir.
- **Veritabanı Yönetimi**: Entity Framework Core kullanarak veritabanı işlemleri yapılır.

## Teknolojiler

- **Backend**: ASP.NET Core
- **Veritabanı**: MSSQL Server, Entity Framework Core
- **Kimlik Doğrulama**: ASP.NET Core Identity
- **ORM**: Entity Framework Core
- **Diğer**: ASP.NET Core MVC, HTML/CSS, JavaScript

### Proje Tasarımı ve Kullanıcı Arayüzü
**Not:** Görsellerde bulunan bloglar ve kullanıcılar **Data Seeding** yöntemiyle tamamen rastgele şekilde oluşturulmuştur. İsim, resim ve kategori uyumu beklenmemektedir.

## Ana Sayfa
Ana sayfada; 
- **Popüler bloglar** : En yüksek okunmaya sahip bloglar.
- **Blog kartları ve kategori filtreleri** : Rastgele seçilmiş bloglar.
- **En iyi yazarlar** : Blogları en çok okunan yazarlar.

  kısımları bulunur.
  
![2025-04-12 16 31 14 localhost f188eb0968ed](https://github.com/user-attachments/assets/a6a61faf-f81d-4a1b-8240-54332e476626)

## Keşfet
Keşfette tüm bloglar sayfalanmış halde bulunur. Çeşitli filtreler kullanılarak istenen blogun bulunmasının kolaylaştırılması amaçlanmıştır.

![2025-04-10 19 08 27 localhost b5884a8b8356](https://github.com/user-attachments/assets/4ce3d338-aca4-47f2-a44b-53099e1fc2a0)

## Blog Detayları
Bu sayfada;
- **Blog İçeriği**
- **İlgili Bloglar**
- **Yorumlar**
  
  kısımları bulunur. Kullanıcı yorum yapmak için giriş yapmalıdır.

![2025-04-12 16 33 50 localhost 36f174c3e5d4](https://github.com/user-attachments/assets/a7e3da3e-aa67-44b0-8cf8-55a62b763679)

## Profil
Profil sayfası, kullanıcının bilgilerini içerir. Ayrıca yazdığı bloglar ve istatistikleri de gösterilmektedir.

![2025-04-12 16 30 55 localhost 40779540a3c6](https://github.com/user-attachments/assets/41798a75-6d9d-42f5-bbe3-bed3a910e551)

## Bloglarım
Kullanıcı bloglarını bu sayfadan yönetebilir.

![2025-04-12 16 31 04 localhost 28d6e8513da2](https://github.com/user-attachments/assets/ff35b2fc-6c19-4d9a-86da-ad3683fad48c)

## Admin Paneli
Admin Paneline sadece admin erişebilir. Sitenin istatistiklerini görebildiği gibi kullanıcıları, blogları, yorumları, rolleri ve kategorileri yönetebilir.

![2025-04-12 16 32 20 localhost 3d423bcd2f4e](https://github.com/user-attachments/assets/de0efaa8-f2fa-4396-8384-d480314c8edd)
![2025-04-12 16 32 35 localhost 640e94d6faaf](https://github.com/user-attachments/assets/bd57cb9d-c6d9-46c5-8610-ef7034d553de)



## Kurulum

Projeyi bilgisayarınızda çalıştırmak için aşağıdaki adımları takip edebilirsiniz:

1. Bu repository'yi bilgisayarınıza klonlayın:
    ```bash
    git clone https://github.com/yalcindagbasi/Echo-Blog-AspNet.git
    ```

2. Proje klasörüne gidin:
    ```bash
    cd Echo-Blog-AspNet
    ```

3. NuGet paketlerini yükleyin:
    ```bash
    dotnet restore
    ```

4. Veritabanını oluşturmak için migrasyonları uygulayın:
    ```bash
    dotnet ef database update
    ```

5. Uygulamayı başlatın:
    ```bash
    dotnet run
    ```



