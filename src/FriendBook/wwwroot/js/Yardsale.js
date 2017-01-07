//Purpose: To change the text of the 'show comments' when it is clicked
$(".ViewItemComments").on("click", function () {
    $(".ItemComments").toggleClass("hidden")

    if ($(".ItemComments").hasClass("hidden")) {
        $(this).children(".ViewCommentText").text("View comments (")
    }
    else {
        $(this).children(".ViewCommentText").text("Hide comments (")
    }
})

//Purpose: To delete a comment on a yardsaleitem when the delete button is clicked
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

//Purpose: to submit a new comment onto a yardsale item
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
                        <a href="/Profile/Index/${user.userId}"><span>${user.firstName} ${user.lastName}</span></a>
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

//Purpose: To filter through all YardSaleItems based on the input values given
$(".FilterByName").on("input", function () {
    let NameInput = $(this).val();
    let CategoryInput = $(".FilterByCategory").val();

    GetYardSaleItems()
    .then(function (items) {
        $(".YardSaleItemsContainer").html("")
        items.forEach(function (item) {
            if (item.itemName.toLowerCase() === NameInput.toLowerCase() && CategoryInput === "0") {
                let DOMItem = AddFilteredYardSaleItemsToDom(item)
                $(".YardSaleItemsContainer").append(DOMItem)
            }
            
            else if (item.itemName.toLowerCase() === NameInput.toLowerCase() && CategoryInput === item.category) {
                let DOMItem = AddFilteredYardSaleItemsToDom(item)
                $(".YardSaleItemsContainer").append(DOMItem)
            }
        })
    })
})

$(".FilterByCategory").on("change", function () {
    let CategoryInput = $(this).val();
    let NameInput = $(".FilterByName").val();

    GetYardSaleItems()
    .then(function (items) {
        $(".YardSaleItemsContainer").html("")
        items.forEach(function (item) {
            let DOMItem = AddFilteredYardSaleItemsToDom(item)
            $(".YardSaleItemsContainer").append(DOMItem)
        })
    })
})

function AddFilteredYardSaleItemsToDom(YardSaleItem) {
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

    let YardSaleItemDOM =
        `<div class="YardSaleItem" id="${YardSaleItem.yardSaleItemId}">
            <div class="ItemPosterContainer">
                <img src="${YardSaleItem.postingUser.profileImg}" class ="ItemPosterProfileImg" />
                <a asp-action="Index" asp-controller="Profile" asp-route-id="${YardSaleItem.postingUser.userId}"><h3 class ="ItemPosterName">${YardSaleItem.postingUser.firstName} ${YardSaleItem.postingUser.lastName}</h3></a>
                <span class="ItemDatePosted">${YardSaleItem.datePosted}</span>
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
                </div>
                <hr />
            </div>`

    return YardSaleItemDOM;
}
