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

    console.log(album)
})
    //CreateAlbum();