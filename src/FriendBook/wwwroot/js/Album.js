$(".addAlbum").on("click", function () {
    $(".createNewAlbum").toggleClass("hidden")
})

$(".createAlbum").on("click", function () {
    let album = {}
    let name = $(".albumNameInput").val();
    let nameValidation = $(".albumNameValidation").html("")
    let description = $(".albumDescription").val();
    let descriptionValidation = $(".albumDescriptionValidation").html("")

    name === "" || name === " " || name.length > 20 ? nameValidation.html("<span>An album name is required and must be less than 20 characters") 
        : album.AlbumName = name

    description === "" || description === "" || description.length > 40 ? descriptionValidation.html("<span>An album description is required and must be less than 40 characters</span>")
        : album.AlbumDescription = description

    CreateAlbum(album)
    .then(function (success) {
    })
})

$(".BackToAlbums").on("click", function () {
    $(".SelectedAlbumDetailsArea").toggleClass("hidden");
    $(".albumShowArea").show();
})

$(".AddIToA").on("change", function () {
    $(".AddImageToAlbum").submit()
})

$(".ImageArea").on("click", function () {
    let albumId = $(this).attr("id")
    GetAParticularAlbum(albumId)
    .then(function (album) {
        GetAParticularAlbumImages(albumId)
        .then(function (images) {
            $(".AddImageToAlbum").attr("asp-route-id", album.albumId)
            $(".SelectedAlbumImages").html("")
            $(".albumShowArea").hide()
            $(".SelectedAlbumDetailsArea").toggleClass("hidden");
            $(".SelectedAlbumName").text(album.albumName)
            $(".selectedAlbumDescription").text(album.albumDescription)
            $(".albumId").val(albumId)
            
            images.forEach(function (image) {
                image.imageDescription === null ? image.imageDescription = "" : false
                $(".SelectedAlbumImages").append(`<div id=${image.imageId} class='AlbumImageDiv'>
                    <img class ='AlbumImage' src="${image.imagePath}">
                    <p class ='ImageDescription'>${image.imageDescription}</p>
                    </div>`)
            })
        })
    })
})