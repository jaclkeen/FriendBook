//Purpose: To change the text of the 'show comments' when it is clicked
$(".YardSaleItemsContainer").on("click", function (e) {
    let context = $(e.target)
    if (context.hasClass("ViewItemComments")) {
        $(context).parent().siblings(".ItemComments").toggleClass("hidden")

        if ($(".ItemComments").hasClass("hidden")) {
            $(context).children(".ViewCommentText").text("View comments (")
        }
        else {
            $(context).children(".ViewCommentText").text("Hide comments (")
        }
    }
    else if (context.hasClass("ViewCommentText")) {
        $(context).parent().parent().siblings(".ItemComments").toggleClass("hidden")

        if ($(".ItemComments").hasClass("hidden")) {
            $(context).parent().children(".ViewCommentText").text("View comments (")
        }
        else {
            $(context).parent().children(".ViewCommentText").text("Hide comments (")
        }
    }
})

//Purpose: To delete a comment on a yardsaleitem when the delete button is clicked
$(".YardSaleItemsContainer").on("click", function (e) {
    let context = $(e.target);

    if (context.hasClass("yardSaleDeleteComment")) {
        let CommentId = context.parent().attr("id")
        context.parent().remove()

        RemoveCommentToYardSaleItem(CommentId)
        .then(function () {
            ToastNotification("Comment deleted")
        })
    }
})

//Purpose: to submit a new comment onto a yardsale item
$(".YardSaleItemsContainer").on("click", function (e) {
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
                        <a href="/Profile/Index/${user.userId}"><span>${user.firstName} ${user.lastName}</span></a>
                        <p class ="YardSaleItemComment">${CommentText}</p>
                        <a class ="DeleteComment yardSaleDeleteComment">Delete</a>
                    </div>`)

                let CommentCount = context.parent().siblings(".LikeDislikeCommentDiv").children(".ViewItemComments").children(".ViewCommentCount")
                CommentCount = parseInt(CommentCount.text()) + 1
                context.parent().siblings(".LikeDislikeCommentDiv").children(".ViewItemComments").children(".ViewCommentCount").text(`${CommentCount}`)
                context.siblings(".AddNewItemComment").val("")

                ToastNotification("Comment added!")
            })
        })
    }
})

//Purpose: To filter through all YardSaleItems based on the input values given
$(".FilterByName").on("input", function () {
    let NameInput = $(this).val();
    let CategoryInput = $(".FilterByCategory").val();
    $(".YardSaleItemsContainer").html("")

    GetFilteredYardSaleItems(NameInput, CategoryInput)
    .then(function (items) {
        items.forEach(function (item) {
            AddFilteredYardSaleItemsToDom(item)
        })
    })
})

$(".FilterByCategory").on("change", function () {
    let CategoryInput = $(this).val();
    let NameInput = $(".FilterByName").val();
    $(".YardSaleItemsContainer").html("")

    GetFilteredYardSaleItems(NameInput, CategoryInput)
    .then(function (items) {
        items.forEach(function (item) {
            AddFilteredYardSaleItemsToDom(item)
        })
    })
})

$(".YardSaleItemsContainer").on("click", function (e) {
    let context = $(e.target)

    if (context.hasClass("deleteItem")) {
        let ItemId = $(context).parent().parent().attr("id")
        $(context).parent().parent().remove()
        DeleteYardSaleItem(ItemId)
        .then(function () {
            ToastNotification("Item deleted!")
        })
    }
})

function AddDOMCommentsToItem(comment) {
    if (user.userId === comment.user.userId) {
        DOMComments =
                `<div class="ItemComments hidden">
                    <div class="comment" id="${comment.commentId}">
                        <img src="${comment.user.profileImg}" />
                        <a href="/Profile/Index/${comment.user.userId}"><span>${comment.user.firstName} ${comment.user.lastName}</span></a>
                        <p class="YardSaleItemComment">${comment.text}</p>
                        <a class="yardSaleDeleteComment">Delete</a>
                    </div>
            </div>`
    }
    else {
        DOMComments =
        `<div class="ItemComments hidden">
                <div class="comment" id="${comment.commentId}">
                    <img src="${comment.user.profileImg}" />
                    <a href="/Profile/Index/${comment.user.userId}"><span>${comment.user.firstName} ${comment.user.lastName}</span></a>
                    <p class="YardSaleItemComment">${comment.text}</p>
                </div>
            </div>`
    }


    return DOMComments;
}

function AddFilteredYardSaleItemsToDom(YardSaleItem) {
    GetYardSaleItemComments(YardSaleItem.yardSaleItemId)
    .then(function (comments) {
        let deleteButton = ""
        let images = `<img src="/images/${YardSaleItem.itemImage1}" class="YSImage" />`
        if (YardSaleItem.itemImage2 !== null) {
            images += `<img src="/images/${YardSaleItem.itemImage2}" class="YSImage" />`
        }
        if (YardSaleItem.itemImage3 !== null) {
            images += `<img src="/images/${YardSaleItem.itemImage3}" class="YSImage" />`
        }
        if (YardSaleItem.itemImage4 !== null) {
            images += `<img src="/images/${YardSaleItem.itemImage4}" class="YSImage" />`
        }

        if (user.userId === YardSaleItem.postingUser.userId) {
            deleteButton = `<span class ="glyphicon glyphicon-remove deleteItem"></span>`
        }

        let YardSaleItemDOM =
            $(`<div class="YardSaleItem" id="${YardSaleItem.yardSaleItemId}">
            <div class="ItemPosterContainer">
                <img src="${YardSaleItem.postingUser.profileImg}" class ="ItemPosterProfileImg" />
                <a href="/Profile/Index/${YardSaleItem.postingUser.userId}"><h3 class ="ItemPosterName">${YardSaleItem.postingUser.firstName} ${YardSaleItem.postingUser.lastName}</h3></a>
                ${deleteButton}
            </div>

            <div class="SaleItemHead">
                <h3>${YardSaleItem.itemName}</h3>
                <p>$${YardSaleItem.itemPrice}</p>
            </div>

            <div class="SaleItemMain">

                <p><strong>Description: </strong>${YardSaleItem.itemDescription}</p>
                <p><strong>Category: </strong>${YardSaleItem.category}</p><hr />

                <div class="YardSaleItemImages">
                    ${images}
                </div><hr />
            </div>

            <div class="LikeDislikeCommentDiv">
                <strong class="ViewItemComments">
                    <span class="ViewCommentText">View comments (</span>
                    <span class="ViewCommentCount">${comments.length}</span>)
                </strong>
            </div>`)

        comments.forEach(function (comment) {
            YardSaleItemDOM.append(AddDOMCommentsToItem(comment))
        })

        YardSaleItemDOM.append(`
            <div class ="AddAndSeeItemComments" id="${YardSaleItem.yardSaleItemId}"><hr />
                <textarea class="AddNewItemComment" placeholder="Add Comment.."></textarea>
                <input type="button" class="SubmitNewItemComment btn-success" value="Comment" />
            </div>
        </div>`)

        $(".YardSaleItemsContainer").append(YardSaleItemDOM)
    })
}

$(".AddNewYardSaleItem").on("click", function () {
    let ValidName = false,
        ValidCategory = false,
        ValidImages = false,
        ValidImageCount = false,
        ValidDescription = false,
        NewItemName = $(".NewItemName").val(),
        ItemNameValidation = $(".ItemNameValidation").html(""),
        NewItemCategory = $(".NewItemCategory").val(),
        NewItemCategoryValidation = $(".ItemCategoryValidation").html(""),
        NewItemImages = document.querySelector(".NewItemImages").files,
        NewItemImagesValidation = $(".ItemImagesValidation").html(""),
        NewItemDescription = $(".NewItemDescription").val(),
        NewItemDescriptionValidation = $(".ItemDescriptionValidation").html("")

    if (NewItemName === "" || NewItemName.length > 50) {
        ItemNameValidation.html("Your item must have a name and must be less than 50 characters!")
    } else { ValidName = true; }
    if (NewItemCategory == "0") {
        NewItemCategoryValidation.html("You must select a category for you item!")
    } else { ValidCategory = true; }
    if (NewItemImages.length === 0) {
        NewItemImagesValidation.html("You must select at least one image!")
    } else { ValidImages = true; }
    if (NewItemImages.length > 4) {
        NewItemImagesValidation.html("Only 4 images are allowed per item!")
    } else { ValidImageCount = true; }
    if (NewItemDescription === "" || NewItemDescription.length > 200) {
        NewItemDescriptionValidation.html("An item description is requred, and must be less than 200 characters!")
    } else { ValidDescription = true; }

    if (ValidName && ValidCategory && ValidImages && ValidImageCount && ValidDescription) {
        $(".NewItemForm").submit();
    }
})
