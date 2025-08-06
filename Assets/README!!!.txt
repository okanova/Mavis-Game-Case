# 🎯 Modular Mini-Game Framework (Unity Case)

## 1. 📖 Proje Hakkında / Overview

- **Projenin amacı nedir?**
  Amacım farklı tür oyunlarda sorunsuz kullanılabilecek bir framework hazırlamak ve bunu iki mini oyun üzerinde göstermek. Esnekliğe daha çok önem verdiğim için kısıtlamaya yol açmaması için bazı yerlerde derinliği göz ardı ettim. Proje bazında geliştirilebilir.

- **Kaç mini oyun var ve neler?**
  İki tane mini oyun var. Biri Endless Runner, diğeri Match 3D türünde. Framework üzerine odaklandığım için oyunları sadece çalışır şekilde yazdım. Oyunlar ve UI geliştirilebilir. Örneğin Match 3D'de objeleri kendim kutulardan oluşturdum ve sadece bir level tasarımı yaptım. Eğer böyle bir oyun geliştirmem istenseydi yuvaları, meyve türlerini ve sayılarını esnek hale getirip ScriptableObjects ile belirlerdim, ayrıca her ekranda çalışır hale getirirdim ama bu oyun özelinde 1080x1920 ölçeğini baz aldım.

## 2. 🏗️ Mimari Yapı / Architecture

- **Neden bu mimari tercih edildi?**  
  Projeyi geliştirilebilir ve genişletilebilir tutmak amacıyla SOLID prensiplerine uygun, bağımlılığı düşük bir yapı kurmaya çalıştım. Özellikle mini oyunların bağımsız olmasına ve yalnızca temel sistemleri kullanarak entegre olmasına dikkat ettim.

- **Kod düzeni ve geliştirme ortamı:**  
  Kod yazarken JetBrains Rider kullandım. Rider, otomatik isimlendirme, hata tespiti ve refactor olanakları sayesinde daha düzenli ve okunabilir kod yazmamı sağladı. 

## 3. 🧱 Modüller / Systems

Yeni bir sistemi sıfırdan oluşturdum. Benim sistemimde tools'tan oluşturma sistemi kullanılıyordu ve objeleri tools'a kaydetmeme gibi sorunlar yaşanabiliyordu. Burada genel bir ScriptableObjects yapısı kullandım ve benim açımdan daha sağlıklı ve kullanışlı bir sistem oldu.

- **ObjectPoolManager:**  
  Hem addressable yapısına uygun çalışıyor hem de oyun içerisinde esnetme şansımız mümkün. Elimden geldiğince esnek ve optimize tutmaya çalıştım. Ekstra özellik entegre etmeye uygun.

- **EventManager:**  
  Kullanımı ve yeni event eklemesi çok kolay bir yöntem.

- **SceneLoader:**  
  Sahne geçişleri gayet iyi çalışıyor, Addressable ile entegreli fakat Loading Scene kullanmak isterdim. Sahnelerde obje bulunmuyor ve objeler yükleninceye kadar Loading Scene aktif olmasını sağlardım. Case süresinde her şeyi sıfırdan yaptığım için buna dikkat etmedim ama kesinlikle yapmak istediğim geliştirmelerden biri.

- **SaveManager:**  
  Interface sayesinde hem ulaşılması hem de kullanılması kolay. Class kaydettiği için esneklik mümkün.

- **UIManager:**  
  Aslında neredeyse kendimi hiç gösteremediğim kısım. Burada modelle çalışmayı seviyorum ve mekanikten sonra ilgileniyorum. Esnek bir UI sistemi kullanmak isterdim. Feel ve Dotween kullanarak UI'ı eğlenceli bir hale getirebilirdim. Bunu bu oyunda gösteremedim ama oyunlarımda görmeniz mümkün (RogueRush). Yetişmediği için üzgünüm ama UI ayarlarında kötü değilim.

- **State Machine:**  
  Bunun çok basit bir örneğini GameManager'da oyunun durumunda kullandım fakat bu benim kullanım şeklim değil. Referans olması için göstermek istedim.

- **Input Manager:**  
  Tüm tuş hareketlerini buraya ekliyorum ve event ile tetikliyorum. Aslında kodlarımda tek update olan yer bu kodda bulunuyor ve start yerine Initialize kullanırım. Manuel tetiklerim ama mini oyunlarda bu kuralıma dikkat etmedim. Normalde start ve update görmeyeceğinizi bildirmek isterim :)

- **Audio Manager:**  
  Oyunda kullanmadım ama sisteme ekledim. Ses loop'ta mı yoksa tek seferlik mi, ses seviyesi gibi ayarlar yapılıyor ve ekleme çıkarma işlemi diğerleriyle aynı yöntemde.

## 4. 🎮 Mini Oyunlar

### Match3D
Random meyveler 3'erli olacak şekilde spawnlanıyor. Objeler pooldan oluşturuluyor. Her hamleden sonra hareket bitinceye kadar yeni hamle yapılmıyor. Bunun sebebi olası bugları kitlemek için. Hissiyat açısından kötü bir durum olduğunun farkındayım, amacım çalışır mini oyun yapmaktı. Yuvalarda aynı türden 3 meyve varsa yok oluyorlar ve tüm meyveler biterse kazanıyoruz. Yuva dolarsa kaybediyoruz.

### Endless Runner
Yolları 50 metre olarak tasarladım. Yolun temeli yani prefab base'i 10 metre ve ben 5'li yol tasarladım. Bunu manuel yaptım ama 50'yi de otomatik belirlediğimiz bir sistem yapabilirdim. Her 50 metrede bir pooldan yeni yol geliyor ve üzerinde bizim belirlediğimiz ihtimallerde engel ve altın oluşuyor. Engelin ve altının çakışmamasına özellikle dikkat ettim. Basit bir puan hesaplaması yaptım ve high score'u kaydettim.

## 5. ⚙️ Kullanılan Teknolojiler

- Unity `6000.0.33f1`
- C# (Rider IDE)
- Addressables
- DOTween

## 6. 🧪 Unit Testler

- Input Manager testi  
- Object Pool testi  
- Save/Load testi  

Ancak bu testleri bir sahnede görselleştirmedim. Kendi kontrolümle test edip temizledim.

## 7. 🧰 Editor Araçları

- AudioEvents, GameEvents ve PoolObjectType enumları otomatik yenileniyor.
- Tool penceresi üzerinden GUI ile tasarım yapabiliyorum ama bu projede kullanmadım.
- Yeni bir ekleme yapıldığında butonla enum'lar güncelleniyor.
- Ayrıca çok sayıda objeye child veya Rigidbody eklemek gibi işlemleri Editor Script üzerinden hızla yapabiliyorum.

## 8. 🔮 Gelecek Geliştirmeler / Future Improvements

- Zenject'in ne olduğunu biliyorum, kullanabilirdim ama açıkçası yetiştiremem diye kendime güvenemedim çünkü fazla temelim yok. Yüzeysel biliyorum.
- UI template'i animasyonlarıyla hazırlamak isterdim ama bunun için hem iyi bir pakete hem de ekstra bir güne daha ihtiyacım vardı.
- Test Scene oluşturabilirdim ama açıkçası birinin beni içinde nelerin olması gerektiği konusunda yönlendirmesi gerekirdi.

## 10. 📁 Proje Kurulumu / How to Run

UNITY SÜRÜMÜ: 6000.0.33f1
LÜTFEN "Boot" sahnesinden oyunu başlatın!!!
