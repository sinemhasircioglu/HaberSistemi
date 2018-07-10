function KategoriDuzenle() {
    Kategori = new Object();
    Kategori.Ad = $("#kategoriAdi").val();
    Kategori.URL = $("#kategoriUrl").val();
    Kategori.AktifMi = $("#kategoriAktifMi").is(":checked");
    Kategori.ParentId = $("#ParentId").val();
    Kategori.Id = $("#Id").val();
    alert(Kategori.Id);

    $.ajax({
        url: "/Kategori/Duzenle",
        data: Kategori,
        type: "POST",
        success: function (response) {
            if (response.Success) {
                bootbox.alert(response.Message, function () {
                    location.reload();
                });
            }
            else {
                bootbox.alert(response.Message);
            }
        }
    })
}

function KategoriEkle()
{
    Kategori = new Object();
    Kategori.Ad = $("#kategoriAdi").val();
    Kategori.URL = $("#kategoriUrl").val();
    Kategori.AktifMi = $("#kategoriAktifMi").is(":checked");
    Kategori.ParentId = $("#ParentId").val();

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

function KategoriSil()
{
    var gelenId = $("#KategoriDelete").attr("data-id");
    alert(gelenId);
    $.ajax({
        url: '/Kategori/Sil/' + gelenId,
        type: "POST",
        dataType: 'json',
        success: function (response) {
            if (response.Success) {
                bootbox.alert(response.Message, function () {
                    location.reload();
                });
            }
            else {
                bootbox.alert(response.Message);
            }
        }
    })
}

