function AppendTaggingSearch(friend) {
    $(".tagFriendSearchDiv").append(`
        <div class="friendBeingTagged" id="${friend.userId}">
            <img src="${friend.profileImg}" class ="friendBeingTaggedProfileImg"/>
            <p class ="friendBeingTaggedName">${friend.firstName} ${friend.lastName}</p>
        </div>`)
}

function AppendTag(FriendUserId, FriendName) {
    $(".ActualTaggedFriends").append(`
        <div class ="TaggedFriend" id="${FriendUserId}">
            <p class="TaggedFriendName">${FriendName}</p>
            <i class ="fa fa-times RemoveTag" aria-hidden="true"></i>
        </div>`)
}

$("body").on("click", function (e) {
    let context = $(e.target),
        TaggedUsersInputValue = $(".TaggedUsersInput").val()

    if (TaggedUsersInputValue !== "" && context.hasClass("RemoveTag")) {
        TaggedUsersInputValue = TaggedUsersInputValue.split(" ")
    }

    if (context.hasClass("RemoveTag")) {
        let name = context.siblings(".TaggedFriendName").text(),
            UserId = context.parent().attr("id"),
            IndexOfUserId = TaggedUsersInputValue.indexOf(UserId)

        TaggedUsersInputValue.splice(IndexOfUserId, 1)

        $(".TaggedUsersInput").val(TaggedUsersInputValue.join(" "))
        context.parent().remove()
        ToastNotification(`You removed ${name}'s tag from this post!`)
    }
})

//TaggedUsersInput is used to append each chosen tagged user as EX: "1 " that is appended to the form and then
//used by the viewmodel in the c# method. So each time another user is tagged, that userId is appended
//onto that input field in the form.
$(".tagFriendsDiv").on("click", function (e) {
    let context = $(e.target),
        TaggedFriendName = null,
        TaggedUserId = null,
        TaggedUsersInput = $(".TaggedUsersInput").val()

    if (context.hasClass("friendBeingTagged")) {
        $(".tagFriendsInput").val("")
        context.remove()
        TaggedUserId = context.attr("id")
        TaggedFriendName = context.children(".friendBeingTaggedName").text()

        if (TaggedUsersInput.indexOf(TaggedUserId) === -1) {
            ToastNotification(`You tagged ${TaggedFriendName}, in this post!`)
            AppendTag(TaggedUserId, TaggedFriendName)
            $(".TaggedUsersInput").val(TaggedUsersInput + " " + TaggedUserId.toString())
        }
        else {
            ToastNotification(`${TaggedFriendName} is already tagged in this post!`)
        }
    }
    else if (context.hasClass("friendBeingTaggedName")) {
        context.parent().remove()
        $(".tagFriendsInput").val("")
        TaggedUserId = context.parent().attr("id")
        TaggedFriendName = context.text()

        if (TaggedUsersInput.indexOf(TaggedUserId) === -1) {
            ToastNotification(`You tagged ${TaggedFriendName}, in this post!`)
            AppendTag(TaggedUserId, TaggedFriendName)
            $(".TaggedUsersInput").val(TaggedUsersInput + " " + TaggedUserId.toString())
        }
        else {
            ToastNotification(`${TaggedFriendName} is already tagged in this post!`)
        }
    }
    else if (context.hasClass("friendBeingTaggedProfileImg")) {
        context.parent().remove()
        $(".tagFriendsInput").val("")
        TaggedUserId = context.parent().attr("id")
        TaggedFriendName = context.siblings(".friendBeingTaggedName").text()

        if (TaggedUsersInput.indexOf(TaggedUserId) === -1) {
            ToastNotification(`You tagged ${TaggedFriendName}, in this post!`)
            AppendTag(TaggedUserId, TaggedFriendName)
            $(".TaggedUsersInput").val(TaggedUsersInput + " " + TaggedUserId.toString())
        }
        else {
            ToastNotification(`${TaggedFriendName} is already tagged in this post!`)
        }
    }
})

$(".tagFriends").on("click", function () {
    $(".tagFriendsDiv").toggleClass("hidden")
})

$(".tagFriendsDiv").on("input", function () {
    let StatusValue = $(".tagFriendsInput").val().toLowerCase()

    GetCurrentUserFriends()
    .then(function (friends) {
        $(".tagFriendSearchDiv").html("")
        friends.forEach(function (friend) {
            let FullName = friend.firstName.toLowerCase() + " " + friend.lastName.toLowerCase()
            let FirstName = friend.firstName.toLowerCase()
            let LastName = friend.lastName.toLowerCase()

            if (FullName === StatusValue || StatusValue === FirstName || StatusValue === LastName) {
                AppendTaggingSearch(friend)
            }
        })
    })
})