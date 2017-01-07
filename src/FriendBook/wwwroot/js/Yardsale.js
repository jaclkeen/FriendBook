$(".ViewItemComments").on("click", function () {
    $(".ItemComments").toggleClass("hidden")

    if ($(".ItemComments").hasClass("hidden")) {
        $(this).children(".ViewCommentText").text("View comments (")
    }
    else {
        $(this).children(".ViewCommentText").text("Hide comments (")
    }
})

$(".ItemComments").on("click", function (e) {
    let context = $(e.target);

    if (context.hasClass("yardSaleDeleteComment")) {
        let CommentId = context.parent().attr("id")
        context.parent().remove()

        RemoveCommentToYardSaleItem(CommentId)
        .then(function () {
            let commentCount = parseInt($(".ViewCommentCount").text())

            $(".ViewCommentCount").text(`${commentCount - 1}`)
            ToastNotification("Comment deleted")
        })
    }
})

$(".YardSaleItem").on("click", function (e) {
    let context = $(e.target);
    let CurrentUser = null

    if (context.hasClass("SubmitNewItemComment")) {
        let CommentText = context.siblings(".AddNewItemComment").val();
        let YardSaleItemId = context.parent().attr("id")
        context.parent().siblings(".ItemComments")

        GetCurrentUser()
        .then(function (user) {
            CurrentUser = user
            AddCommentToYardSaleItem(YardSaleItemId, CommentText)
            .then(function (CommentId) {

                context.parent().siblings(".ItemComments").append(`
                    <div class ="comment" id="${CommentId}">
                        <img src="${user.profileImg}" />
                        <a asp-action="Index" asp-controller="Profile" asp-route-id="${user.userId}"><span>${user.firstName} ${user.lastName}</span></a>
                        <p class ="YardSaleItemComment">${CommentText}</p>
                        <a class ="DeleteComment yardSaleDeleteComment">Delete</a>
                    </div>`)

                let commentCount = parseInt($(".ViewCommentCount").text())

                $(".ViewCommentCount").text(`${commentCount + 1}`)
                context.siblings(".AddNewItemComment").val("")
                ToastNotification("Comment added!")
            })
        })
    }
})