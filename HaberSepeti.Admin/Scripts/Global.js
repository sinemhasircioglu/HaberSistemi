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

//$(document).on("click", "#KategoriDelete", function () {
//    var gelenid = $(this).attr("data-id");
//    var siltr = $(this).closest("tr");
//    $.ajax({
//        url: '/Kategori/Sil/' + gelenid,
//        type: "POST",
//        dataType: 'json',
//        success: function (response) {
//            siltr.fadeOut(300, function () {
//                siltr.remove();
//            })
//        }
//    })
//})

//function KategoriEkle()
//{
//    Kategori = new Object();
//    Kategori.Ad = $("#kategoriAdi").val();
//    Kategori.URL = $("#kategoriUrl").val();
//    Kategori.AktifMi = $("#kategoriAktifMi").is(":checked");
//    Kategori.ParentId = $("#ParentId").val();

//    $.ajax({
//        url: "/Kategori/Ekle",
//        data: Kategori,
//        type: "POST",
//        success: function (response) {
//            if (response.Success)
//            {
//                bootbox.alert(response.Message, function () {
//                    location.reload();
//                });
//            }
//            else
//            {
//                bootbox.alert(response.Message);
//            }
//        }
//    })
//}