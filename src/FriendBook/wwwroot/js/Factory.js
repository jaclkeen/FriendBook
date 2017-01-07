//Purpose: To show a toast notification based on the message passed in
function ToastNotification(message) {
    $(".toast").removeClass("hidden")
    $(".toast").html(message)
    $(".toast").fadeIn(2000)
    $(".toast").fadeOut(2000)
}

function GetYardSaleItemComments(ItemId) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/YardSale/GetYardSaleItemComments",
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify(ItemId)
        }).done(function (comments) {
            resolve(comments)
        }).error(function (err) {
            reject(err)
        })
    })
}

function GetFilteredYardSaleItems(Name, Category) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/YardSale/FilteredYardSaleItems",
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify({ ItemNameFilter: Name, ItemCategoryFilter: Category })
        }).done(function (Items) {
            resolve(Items)
        }).error(function (err) {
            reject(err)
        })
    })
}

function RemoveCommentToYardSaleItem(Id) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/YardSale/RemoveCommentOnYardSaleItem",
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify(Id)
        }).done(function (ListOfComments) {
            resolve(ListOfComments)
        }).error(function (err) {
            reject(err)
        })
    })
}

function AddCommentToYardSaleItem(YardSaleId, CommentText) {
    return new Promise(function(resolve, reject){
        $.ajax({
            url: "/YardSale/CommentOnYardSaleItem",
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify({ YardSaleItemId : YardSaleId, Text: CommentText})
        }).done(function (ListOfComments) {
            resolve(ListOfComments)
        }).error(function (err) {
            reject(err)
        })
    })
}

//Purpose: To get all of the current user's pending friend request
function GetUserFriendRequests() {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/Home/UserFriendRequests"
        }).done(function (FRs) {
            resolve(FRs)
        }).error(function (err) {
            reject(err)
        })
    })
}

//Purpose: To update that a notification has been seen
function UserNotificationSeen(NotificationId) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/Home/SeenUserNotification",
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(NotificationId)
        }).done(function (success) {
            resolve(success)
        }).error(function (err){
            reject(err)
        })
    })
}

//Purpose: To get all of the current user's notifications
function GetUserNotifications() {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/Home/UserNotifications"
        }).done(function (notifications) {
            resolve(notifications)
        }).error(function (err) {
            reject(err)
        })
    })
}

//Purpose: To get all of the currentUser's friends
function GetCurrentUserFriends() {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/Home/UserFriends"
        }).done(function (friends) {
            resolve(friends)
        }).error(function (err) {
            reject(err)
        })
    })
}

//Purpose: To add a dislike to a particular post
function AddDislike(PostId) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/Post/AddDislike",
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify(PostId)
        }).done(function (success) {
            resolve(success)
        }).error(function (err) {
            reject(err)
        })
    })
}

//Purpose: To add a like to a particular post
function AddLike(PostId) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/Post/AddLike",
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify(PostId)
        }).done(function (success) {
            resolve(success)
        }).error(function (err) {
            reject(err)
        })
    })
}

//Purpose: To get all message notifications where the recieving user is the current user
function GetUserMessageNotifications() {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/Conversation/MessageNotifications"
        }).done(function (success) {
            resolve(success)
        }).error(function (err) {
            reject(err)
        })
    })
}

//Purpose: To set a current user's conversation as active
function SetConversationAsActive(RoomName) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/Conversation/ActivateConversation",
            method: "POST",
            contentType: 'application/json',
            data: JSON.stringify(RoomName)
        }).done(function (nothing) {
            resolve(nothing)
        }).error(function (err) {
            reject(err)
        })
    })
}

//Purpose: To set a current user's converstion as inactive
function EndAConversation(conversationId) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/Conversation/EndConversation",
            method: 'POST',
            contentType: "application/json",
            data: JSON.stringify(conversationId)
        }).done(function (success) {
            resolve(success)
        }).error(function (err) {
            reject(err)
        })
    })
}

//Purpose: To return all of the current user's active conversations
function UserActiveConversations() {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/Conversation/ActiveConversations"
        }).done(function (success) {
            resolve(success)
        }).error(function (err) {
            reject(err)
        })
    })
}

//Purpose: To update that a message notification has been seen
function UpdateMessageSeen(id) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/Conversation/MessageSeen",
            method: 'POST',
            contentType: "application/json",
            data: JSON.stringify(id)
        }).done(function (success) {
            resolve(success)
        }).error(function (err) {
            reject(err)
        })
    })
}

//Purpose: To save a new message to a particular conversation
function SaveNewMessage(message) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/Conversation/PostToConversation",
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify(message)
        }).done(function (success) {
            resolve(success)
        }).error(function (err) {
            reject(err)
        })
    })
}

//Purpose: To get all message that are in a particular conversation
function GetAllConversationMessages(ConversationName) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/Conversation/GetAllConversationMessages",
            method: "POST",
            contentType: 'application/json',
            data: JSON.stringify(ConversationName)
        }).done(function (messages) {
            resolve(messages)
        }).error(function (err) {
            reject(err)
        })
    })
}

//Purpose: To create a new conversation or return an already created existing one
function CreateNewConversation(ClickedUserId) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/Conversation/CreateConversation",
            method: "POST",
            contentType: 'application/json',
            data: JSON.stringify(ClickedUserId)
        }).done(function (convo) {
            resolve(convo)
        }).error(function (err) {
            reject(err)
        })
    })
}

//Purpose: To get all images of a particular album
function GetAParticularAlbumImages(id) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: `/Album/GetAlbumImages/${id}`
        }).done(function (images) {
            resolve(images)
        }).error(function (err) {
            reject(err)
        })
    })
}

//Purpose: To get a particular album
function GetAParticularAlbum(id) {
    return new Promise(function(resolve, reject){
        $.ajax({
            url: `/Album/GetSpecificAlbum/${id}`
        }).done(function (albums) {
            resolve(albums)
        }).error(function (err) {
            reject(err)
        })
    })
}

//Purpose: to create a new album
function CreateAlbum(Album) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/Album/CreateNewAlbum",
            method: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(Album)
        }).done(function (data) {
            resolve(data)
            location.reload()
        }).error(function (err) {
            reject(err)
        })
    })
}

//Purpose: To delete a comment from a particular post
function DeleteComment(CommentId) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: `/Post/DeleteComment/${CommentId}`,
            method: 'DELETE'
        }).done(function (deleted) {
            resolve(deleted)
        }).error(function (err) {
            reject(err)
        })
    })
}

//Purpose: To get all comments from a specific post
function GetAllCommentsFromSpecificPost(PostId) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: `/Post/GetAllCommentsFromSpecificPost/${PostId}`
        }).done(function (comments) {
            resolve(comments)
        }).error(function (err) {
            reject(err)
        })
    })
}

//Purpose: To get the current user
function GetCurrentUser() {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/Home/GetCurrentUser",
        }).done(function (currentUser) {
            resolve(currentUser)
        }).error(function (err) {
            reject(err)
        })
    })
}

//Purpose: To create a new comment onto a particular post
function CreateNewComment(pId, CommentText) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/Post/CreateNewCommentOnPost",
            method: "POST",
            contentType: "application/json",
            data: JSON.stringify({ PostId: pId, text: CommentText })
        }).done(function (data) {
            resolve(data)
        }).error(function (err) {
            reject(err)
        })
    })
}

//Purpose: To edit the text of a specific comment
function EditSpecificComment(cId, CommentText) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/Post/EditSpecificCommentOnPost",
            method: 'POST',
            contentType: "application/json",
            data: JSON.stringify({ CommentId: cId, text: CommentText })
        }).done(function (updatedComment) {
            resolve(updatedComment)
        }).error(function (err) {
            reject(err)
        })
    })
}

//Purpose: To edit the text of a specific post
function EditSpecificPost(pID, PostText) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/Post/EditSpecificPost",
            method: 'POST',
            contentType: "application/json",
            data: JSON.stringify({ PostId: pID, text: PostText })
        }).done(function (data) {
            resolve(data)
        }).error(function (err) {
            reject(err)
        })
    })
}

//Purpose: To accept a friend request
function AcceptFR(RelationshipId) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/Home/AcceptFR",
            method: 'POST',
            contentType: 'application/json',
            data: RelationshipId
        }).done(function (data) {
            resolve(data)
        }).error(function (er) {
            reject(er)
        })
    })
}

//Purpose: To decline a friend request
function DeclineFR(RelationshipId) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/Home/DeclineFR",
            method: 'POST',
            contentType: 'application/json',
            data: RelationshipId
        }).done(function (data) {
            resolve(data)
        }).error(function (er) {
            reject(er)
        })
    })
}

//Purpose: To get all users
function getUsers() {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "/Home/GetUsers",
            dataType: "json",
            method: "GET"
        }).done(function (users) {
            resolve(users)
        }).error(function (e) {
            reject(e)
        })
    })
}

//Purpose: To create a new user on register
function CreateNewUser(NewUser) {
    return new Promise(function (resolve, reject) {
        $.ajax({
            url: "Login/RegisterNewUser",
            method: "POST",
            contentType: 'application/json',
            data: JSON.stringify(NewUser)
        }).done(function (newUser) {
            resolve(newUser)
        }).error(function (err) {
            reject(err)
        })
    })
}