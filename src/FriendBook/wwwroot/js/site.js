$(document).ready(function () {

    function addUsersSearchToDom(userList) {
        let searchInput = $('.userSearch').val();
        $(".searchResults").html("");
        for (var user in userList) {
            let firstName = userList[user].firstName,
                lastName = userList[user].lastName,
                fullName = `${userList[user].firstName} ${userList[user].lastName}`

            if (userList[user].profileImg === null) {
                userList[user].profileImg = "/images/egg.png"
            }

            if (searchInput === firstName || searchInput === lastName || searchInput === fullName
                || searchInput === firstName.toLowerCase() || searchInput === lastName.toLowerCase()
                || searchInput === fullName.toLowerCase()) {

                $(".searchResults").append(`<div class="userInSearch" id="${userList[user].userId}">
                    <img class ="searchProfilePic" src=${userList[user].profileImg}>
                    <p class="searchProfileName">${fullName}</p></div>`);
            }
        }
    }

    function userSearchEvents() {
        $('.userInSearch').on('click', function() {
            let user = $(this).attr("id");
            window.location.href = `/Profile/Profile/${user}`
        })
    }

    $('.friendDiv').hide();

    $('.showPosts').on("click", function () {
        $('.friendDiv').hide();
        $('.posts').show();
    })

    $('.showFriends').on("click", function () {
        $('.posts').hide();
        $('.friendDiv').show();
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

    $(".profileImgUpload").on("change", function () {
        $(".changeProfileImg").submit();
    })

    CommentEventsForDeleteAndEdit()
})