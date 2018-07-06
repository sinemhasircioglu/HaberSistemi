function KategoriEkle()
{
    Kategori = new Object();
    Kategori.Ad = $("#kategoriAdi").val();
    Kategori.URL = $("#kategoriUrl").val();
    Kategori.AktifMi = $("#kategoriAktifMi").is(":checked");

    $.ajax({
        url: "Kategori/Ekle",
        data: Kategori,
        type: "POST",
        success: function (response) {
            if (response.success) {
                alert(1);
            }
            else {
                alert(2);
            }
        }
    })
}