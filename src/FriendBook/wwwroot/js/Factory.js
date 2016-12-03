
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