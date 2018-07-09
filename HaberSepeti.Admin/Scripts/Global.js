function KategoriEkle()
{
    Kategori = new Object();
    Kategori.Ad = $("#kategoriAdi").val();
    Kategori.URL = $("#kategoriUrl").val();
    Kategori.AktifMi = $("#kategoriAktifMi").is(":checked");
    Kategori.ParentId = $("#ParentId").val();

    alert(Kategori.ParentId);

    $.ajax({
        url: "/Kategori/Ekle",
        data: Kategori,
        type: "POST",
        success: function (response) {
            if (response.Success)
            {
                bootbox.alert(response.Message, function () {
                    location.reload();
                });
            }
            else
            {
                bootbox.alert(response.Message);
            }
        }
    })
}