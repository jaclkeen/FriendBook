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
            console.log(err)
            reject(err)
        })
    })
}

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