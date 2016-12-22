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
            window.location.href = `/Profile/Index/${user}`
        })
    }

    $(".frButton").on("click", function () {
        location.reload()
    })

    $('.userSearch').on("input", function(){
        getUsers()
        .then(function(users){
            addUsersSearchToDom(users)
            userSearchEvents();
        })
    })

    $('.messageNotifications').on("click", function () {
        $('.notificationArea').addClass("hidden")
        $('.messageNotificationArea').toggleClass("hidden")
    })

    $('.Notifications').on("click", function () {
        $('.notificationArea').toggleClass("hidden");
        $('.messageNotificationArea').addClass("hidden")
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