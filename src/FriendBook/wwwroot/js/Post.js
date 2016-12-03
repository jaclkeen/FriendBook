function AppendPostEdit(e) {
    let post = $(e.currentTarget),
            editBtn = $(e.target),
            postId = editBtn.attr("id"),
            postTextDiv = post.children(".postTextDiv"),
            postText = postTextDiv.children(".postText").html()

    let editArea = `<div class="editInputArea">
                    <textarea style="color: black; width: 50%;" class ="editInput">${postText}</textarea>
                    <input type="button" value="Update" class ="btn-success EditBtn updatePost">
                    <input type="button" value="Cancel" class ="btn-danger EditBtn cancelEdit">
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
        })
    })
}

$(".post").on("click", function (e) {
    let CurrentPost = $(e.currentTarget);
    let CurrentPostId = CurrentPost.attr("id")

    if (e.target.classList.contains("EditPost")) {
        AppendPostEdit(e);
    }

    if (e.target.classList.contains("comments")) {
        CurrentPost.children(".CommentArea").toggleClass("hidden")
    }

    if (e.target.classList.contains("submitComment")) {
        let ClickedCommentButtonTextArea = CurrentPost.children(".AddNewComment"),
            CommentTextValue = ClickedCommentButtonTextArea.val()

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
                        let NewCommentDiv = $(`<div class="comment" id=${comment.commentId}><img src=${user.profileImg} /><span>${user.firstName} ${user.lastName}</span><p>${comment.text}</p></div>`)
                        let EditOrDeleteComment = `<div class="EditOrDeleteComment"><a class="EditComment">Edit</a><a class="DeleteComment">Delete</a></div>`
                        NewCommentDiv.append(EditOrDeleteComment)
                        CurrentPost.children(".CommentArea").append(NewCommentDiv)
                        CommentEventsForDeleteAndEdit()
                        ClickedCommentButtonTextArea.val("")
                    })
                    $(CommentTag).html(`Comments (${CommentCount})`)
                })
            })
        })
    }
})