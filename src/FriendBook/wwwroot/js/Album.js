//Purpose: To toggle the form needed to create a new album when the add album button is clicked
function AddAlbumSpa() {
    $(".addAlbum").on("click", function () {
        $(this).removeClass("fa-plus-circle").addClass("fa-minus-circle minus")
        $(".createNewAlbum").toggleClass("hidden")

        $(".minus").on("click", function () {
            $(this).removeClass("fa-minus-circle").addClass("fa-plus-circle")
            $(".createNewAlbum").addClass("hidden")
            AddAlbumSpa()
        })
    })
}

AddAlbumSpa()

//Purpose: To gather input values and perform validation in order to create a new album
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

//Purpose: Adds an image to an album when an image is uploaded by submitting the form on change of the input field
$(".AddIToA").on("change", function () {
    $(".AddImageToAlbum").submit()
})