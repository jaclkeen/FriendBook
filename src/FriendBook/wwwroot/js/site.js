$(document).ready(function () {

    function addUsersSearchToDom(userList) {
        let searchInput = $('.userSearch').val();
        let listOfSelectedUsers = []
        $(".searchResults").html("");
        for (var user in userList) {
            let firstName = userList[user].firstName,
                lastName = userList[user].lastName,
                fullName = `${userList[user].firstName} ${userList[user].lastName}`

            if (searchInput === firstName || searchInput === lastName || searchInput === fullName
                || searchInput === firstName.toLowerCase() || searchInput === lastName.toLowerCase()
                || searchInput === fullName.toLowerCase()) {
                
                listOfSelectedUsers.push(userList[user])
            }
        }

        listOfSelectedUsers.forEach(function (u) {
            $(".searchResults").append(`<div class="userInSearch" id="${u.userId}">
                <img class ="searchProfilePic" src=${u.profileImg}>
                <p class ="searchProfileName">${u.firstName} ${u.lastName}</p></div>`);
        })
    }

    function userSearchEvents() {
        $('.userInSearch').on('click', function() {
            let user = $(this).attr("id");
            window.location.href = `/Profile/Profile/${user}`
        })
    }

    $('.friendDiv').hide();
    $(".imagesDiv").hide();

    $('.showPosts').on("click", function () {
        $(".imagesDiv").hide();
        $('.friendDiv').hide();
        $('.posts').show();
    })

    $('.showFriends').on("click", function () {
        $(".imagesDiv").hide();
        $('.posts').hide();
        $('.friendDiv').show();
    })

    $('.showAlbums').on("click", function () {
        $(".imagesDiv").show();
        $('.posts').hide();
        $('.friendDiv').hide();
    })

    $('.userSearch').on("input", function(){
        getUsers()
        .then(function(users){
            addUsersSearchToDom(users)
            userSearchEvents();
        })
    })

    $('.Notifications').on("click", function () {
        $('.notificationArea').toggleClass("hidden");
    })

    $('.frButton').on("click", function () {
        let frId = $(this).attr("id"),
            frText = $(this).parent().parent().text(),
            frHtml = $(this).parent().parent().html("")

        frHtml = frText.split(" ")
        frHtml.join("")

        if ($(this).hasClass("declineFR")) {
            DeclineFR(frId)
            .then(function (data) {
                frHtml = `<p class="successFR">You declined ${frHtml[76]} ${frHtml[77]}'s friend request!</p>`
                //Materialize.Toast(frHtml);
            })
        }
        else {
            AcceptFR(frId)
            .then(function (data) {
                frHtml = `<p class="successFR">You and ${frHtml[76]} ${frHtml[77]} are now friends!</p>`
                //Materialize.Toast(frHtml);
            })
        }
    })

    $(".profileBannerUpload").on("change", function () {
        $(".changeBannerImg").submit();
    })

    $(".profileImgUpload").on("change", function () {
        $(".changeProfileImg").submit();
    })

    $(".addPToStatus").on("change", function () {
        let selectedFile = $(this).val().replace(/^.*\\/, "");
        $(".photoSelectedArea").html("")
        $(".photoSelectedArea").append(`Selected file: ${selectedFile}`)
    })

    CommentEventsForDeleteAndEdit()
})