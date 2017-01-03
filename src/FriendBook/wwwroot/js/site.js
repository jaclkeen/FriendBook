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

    //Purpose to provide color styling and disable the submit post button if the post length is === 0 or greater than 200
    //  characters
    function StatusValidate(StatusVal, button, valDiv) {
        if (StatusVal.length === 0) {
            $(button).attr("disabled", true)
            $(valDiv).css("color", "black")
        }
        if (StatusVal.length > 0 && StatusVal.length < 201) {
            $(button).attr("disabled", false)
            $(valDiv).css("color", "black")
        }
        if (StatusVal.length > 149 && StatusVal.length < 201) {
            $(valDiv).css("color", "orange")
        }
        if (StatusVal.length > 200) {
            $(button).attr("disabled", true)
            $(valDiv).css("color", "red")
        }

        $(valDiv).text(`${200 - StatusVal.length} characters remaining!`)
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

    //Purpose: To disable the submit button and show toast if the status length is 0 or greater than 200
    $(".submitStatus").on("click", function () {
        let PostTextCount = $(".newStatus").val()

        if (PostTextCount.length === 0) {
            $(".submitStatus").attr("disabled", true)
            ToastNotification("You cannot post an empty status!")
        }
        else if (PostTextCount.length > 200) {
            $(".submitStatus").attr("disabled", true)
            ToastNotification("Your status must be less than 200 characters in length")
        }
    })

    //Purpose calls the new status validation function above to validate the new status in the post
    $(".newStatus").on("input", function () {
        let StatusValue = $(this).val()
        let button = $(".submitStatus")
        let valDiv = $(".statusLengthValidation")

        StatusValidate(StatusValue, button, valDiv)
    })

    $(".wallPostText").on("input", function () {
        let wallPostValue = $(this).val()
        let button = $(".submitWallPost")
        let valDiv = $(".wallPostValidation")

        StatusValidate(wallPostValue, button, valDiv)
    })

    $(".clearWallPost").on("click", function () {
        $(".wallPostText").val("")
    })

    $(".submitWallPost").attr("disabled", true)

    //Purpose: Calls event listeners for comments on page load
    CommentEventsForDeleteAndEdit()

    //Purpose: Auto updates all notifications and friend requests if there is a user logged in
    GetCurrentUser()
    .then(function (u) {
        if (u !== undefined) {
            ActiveConvo()
            setInterval(UpdateUnseenMessages, 3000)
            setInterval(UpdateConversationMessages, 3000)
            setInterval(GetCurrentUserNotifications, 3000)
            setInterval(GetAllPendingUserFriendRequests, 3000)
        }
    })
})