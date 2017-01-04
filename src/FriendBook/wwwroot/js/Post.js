//Purpose: To append the edited post text to the dom and add event listeners to either cancel or confirm the edit
function AppendPostEdit(e) {
    let post = $(e.currentTarget),
            editBtn = $(e.target),
            postId = editBtn.attr("id"),
            postTextDiv = post.children(".postTextDiv"),
            postText = postTextDiv.children(".postText").html()

    let editArea = `<div class="editInputArea">
                    <textarea style="color: black; width: 50%;" class ="editInput">${postText}</textarea>
                    <input type="button" value="Update" class ="btn-success EditBtn updatePost">
                </div>`

    postTextDiv.html(editArea)

    $('.cancelEdit').on("click", function () {
        let p = `<span class="postText">${postText}</span>`
        postTextDiv.html(p)
    })

    $('.updatePost').on("click", function () {
        var updatedPostText = $(".editInput").val()
        EditSpecificPost(postId, updatedPostText)
        .then(function (post) {
            let p = `<span class="postText">${updatedPostText}</span>`
            postTextDiv.html(p)
            ToastNotification("Post successfully updated!")
        })
    })
}

//Purpose: To provide event listeners to all posts such as like, dislike, edit, and submit comment events.
$(".post").on("click", function (e) {
    let CurrentPost = $(e.currentTarget);
    let CurrentPostId = CurrentPost.attr("data")

    if (e.target.classList.contains("like")) {
        let context = $(e.target)
        AddLike(CurrentPostId)
        .then(function (LikeCount) {
            context.closest(".likeCount").html(`<i class="fa fa-thumbs-up statusIcon like"></i>(${LikeCount})`)
            ToastNotification("Post liked!")
        })
    }

    if (e.target.classList.contains("dislike")) {
        let context = $(e.target)
        AddDislike(CurrentPostId)
        .then(function (DislikeCount) {
            context.closest(".dislikeCount").html(`<i class="fa fa-thumbs-down statusIcon dislike"></i>(${DislikeCount})`)
            ToastNotification("Post disliked!")
        })
    }

    if (e.target.classList.contains("EditPost")) {
        AppendPostEdit(e);
    }

    if (e.target.classList.contains("comments") || e.target.classList.contains("CSI")) {
        CurrentPost.children(".CommentArea").toggleClass("hidden")
    }

    if (e.target.classList.contains("submitComment")) {
        let ClickedCommentButtonTextArea = CurrentPost.children(".AddNewComment"),
            CommentTextValue = ClickedCommentButtonTextArea.val()

        if(CommentTextValue.length === 0){
            return ToastNotification("You cannot post an empty comment")
        }

        if (CommentTextValue.length > 200) {
            return ToastNotification("Your comment must be less than 200 characters long")
        }

        CreateNewComment(CurrentPostId, CommentTextValue)
        .then(function (comment) {
            GetCurrentUser()
            .then(function (user) {
                let CommentDiv = CurrentPost.children(".LikeDislikeCommentDiv")[0],
                    CommentTag = $(CommentDiv)[0].children[2],
                    CommentCount = 0

                GetAllCommentsFromSpecificPost(CurrentPostId)
                .then(function (comments) {
                    CurrentPost.children(".CommentArea").html("")
                    comments.forEach(function (comment) {
                        CommentCount = comments.length;
                        let NewCommentDiv = $(`<div class="comment" id=${comment.commentId}><img src=${comment.user.profileImg} /><a href="/Profile/Index/${comment.user.userId}"><span>${comment.user.firstName} ${comment.user.lastName}</span></a><p>${comment.text}</p></div>`)
                        let EditOrDeleteComment = `<div class="EditOrDeleteComment"><a class="EditComment">Edit</a><a class="DeleteComment">Delete</a></div><hr>`
                        comment.userId === user.userId ? NewCommentDiv.append(EditOrDeleteComment) : false;
                        CurrentPost.children(".CommentArea").append(NewCommentDiv)
                        CommentEventsForDeleteAndEdit()
                        ClickedCommentButtonTextArea.val("")
                    })

                    ToastNotification("Comment posted!")
                    $(CommentTag).html(`<span class="comments addSpaceRight"><i class="fa fa-comments statusIcon CSI"></i>(${CommentCount})</span>`)
                })
            })
        })
    }
})

//Purpose: To clear all values from the new status form on the clear button click
$(".clearStatus").on("click", function () {
    $(".photoSelectedArea").html("")
    $(".addPToStatus").val("")
    $(".newStatus").val("")
    $(".TaggedUsersInput").val("")
    $(".ActualTaggedFriends").html("")
})