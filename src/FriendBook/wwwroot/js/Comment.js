function CommentEventsForDeleteAndEdit(Post) {
    $(".comment").on("click", function (e) {
        let CurrentComment = $(e.currentTarget),
            CurrentPostId = CurrentComment.parent().parent(".post"),
            CommentCountHTML = CurrentPostId.children(".LikeDislikeCommentDiv").children(".comments"),
            CommentTextDiv = CurrentComment.children()[2],
            CommentText = $(CommentTextDiv).text(),
            CommentId = CurrentComment.attr("id"),
            EditOrDelete = $(e.target)

        if (EditOrDelete.hasClass("DeleteComment")) {
            CurrentComment.remove();
            DeleteComment(CommentId)
            .then(function () {
                GetAllCommentsFromSpecificPost(CurrentPostId.attr("id"))
                .then(function (comments) {
                    CommentCountHTML.html(`Comments (${comments.length})`)
                })
            })
        }

        if (EditOrDelete.hasClass("EditComment")) {
            let EditCommentTextDiv = `<div class="editInputArea">
                    <textarea style="color: black; width: 50%;" class ="editInput">${CommentText}</textarea>
                    <input type="button" value="Update" class ="btn-success EditBtn updateComment">
                    <input type="button" value="Cancel" class ="btn-danger EditBtn cancelEdit">
                </div>`

            $(CommentTextDiv).html(EditCommentTextDiv)

            $('.cancelEdit').on("click", function () {
                let canceledText = `<p>${CommentText}</p>`
                $(CommentTextDiv).html(canceledText)
            })

            $(".updateComment").on("click", function () {
                let UpdatedCommentText = $(".editInput").val()
                EditSpecificComment(CommentId, UpdatedCommentText)
                .then(function (success) {
                    let newComment = `<p>${UpdatedCommentText}</p>`
                    $(CommentTextDiv).html(newComment)
                })
            })
        }
    })
}