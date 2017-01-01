$(document).ready(function () {

    //Purpose: To add all users provided in the userList parameter to the DOM
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

    //Purpose: Add an event listener to each user div in the user search area for when that particular user is clicked
    //          the current user is sent to that clicked user's profile page
    function userSearchEvents() {
        $('.userInSearch').on('click', function() {
            let user = $(this).attr("id");
            window.location.href = `/Profile/Index/${user}`
        })
    }

    //Purpose: Adds event listener onto the user search input that gets all users and if the input is equal to any
    //  current user, it gets appended to the dom and events are then added to it.
    $('.userSearch').on("input", function(){
        getUsers()
        .then(function(users){
            addUsersSearchToDom(users)
            userSearchEvents();
        })
    })

    //Purpose: To show and hide the message notifications on click of the nav item and show the friend notifications
    //  and user notifications
    $('.messageNotifications').on("click", function () {
        $('.messageNotificationArea').toggleClass("hidden")
        $('.notificationArea').addClass("hidden")
        $(".UserNotificationDiv").addClass("hidden")
    })

    //Purpose: To show and hide the Friend notifications on click of the nav item and hide the message notifications
    //  and user notifications
    $('.Notifications').on("click", function () {
        $('.notificationArea').toggleClass("hidden")
        $('.messageNotificationArea').addClass("hidden")
        $(".UserNotificationDiv").addClass("hidden")
    })

    //Purpose: To show and hide the UserNotifications on click of the nav item and hide the message notifications
    //  and friend request notifications
    $(".ShowUserNotifications").on("click", function () {
        $(".UserNotificationDiv").toggleClass("hidden")
        $('.notificationArea').addClass("hidden")
        $('.messageNotificationArea').addClass("hidden")
    })

    //Purpose: To add an event listener to either accept or decline a friend request 
    $('.frButton').on("click", function () {
        let frId = $(this).attr("id"),
            frText = $(this).parent().parent().text(),
            frHtml = $(this).parent().parent().html("")

        frHtml = frText.split(" ")
        frHtml.join("")

        if ($(this).hasClass("declineFR")) {
            DeclineFR(frId)
            .then(function (data) {
                location.reload();
            })
        }
        else {
            AcceptFR(frId)
            .then(function(){
                location.reload();
            })
        }
    })

    //Purpose: submits a form when there is a change in the current user's cover image
    $(".profileBannerUpload").on("change", function () {
        $(".changeBannerImg").submit();
    })

    //Purpose: submits a form when there is a change in the current user's profile image
    $(".profileImgUpload").on("change", function () {
        $(".changeProfileImg").submit();
    })

    //Purpose: removes the unneeded text in a picture file name when uploaded and shows the file name
    $(".addPToStatus").on("change", function () {
        let selectedFile = $(this).val().replace(/^.*\\/, "");
        $(".photoSelectedArea").html("")
        $(".photoSelectedArea").append(`Selected file: ${selectedFile}`)
    })

    //Purpose: Calls event listeners for comments on page load
    CommentEventsForDeleteAndEdit()

    if (user !== null) {
        ActiveConvo()
        setInterval(UpdateUnseenMessages, 5000)
        setTimeout(setInterval(UpdateConversationMessages, 5000), 3000)
        setInterval(GetCurrentUserNotifications, 5000)
    }
})