$(document).ready(function () {

    function EditSpecificPost(pID, PostText) {
        return new Promise(function (resolve, reject) {
            $.ajax({
                url: "/Post/EditSpecificPost",
                method: 'POST',
                contentType: "application/json",
                data: JSON.stringify({PostId: pID, text: PostText})
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

    function addUsersSearchToDom(userList) {
        let searchInput = $('.userSearch').val();
        $(".searchResults").html("");
        for (var user in userList)
        {
            let firstName = userList[user].firstName,
                lastName = userList[user].lastName,
                fullName = `${userList[user].firstName} ${userList[user].lastName}`

            if (userList[user].profileImg === null)
            {
                userList[user].profileImg = "/images/egg.png"
            }

            if (searchInput == firstName || searchInput == lastName || searchInput == fullName
                || searchInput == firstName.toLowerCase() || searchInput == lastName.toLowerCase()
                || searchInput == fullName.toLowerCase())
            {
                $(".searchResults").append(`<div class="userInSearch" id="${userList[user].userId}">
                    <img class ="searchProfilePic" src=${userList[user].profileImg}>
                    <p class="searchProfileName">${fullName}</p></div>`);
            }
        }
    }

    function userSearchEvents() {
        $('.userInSearch').on('click', function ()
        {
            let user = $(this).attr("id");
            window.location.href = `/Profile/Profile/${user}`
        })
    }

    $('.friends').hide();

    $('.showPosts').on("click", function () {
        $('.friends').hide();
        $('.posts').show();
    })

    $('.showFriends').on("click", function () {
        $('.posts').hide();
        $('.friends').show();
    })

    $('.userSearch').on("input", function(){
        getUsers()
        .then(function (users)
        {
            addUsersSearchToDom(users)
            userSearchEvents();
        })
    })

    $('.Notifications').on("click", function () {
        $('.notificationArea').toggleClass("hidden");
    })

    $('.frButton').on("click", function () {
        let frId = $(this).attr("id"),
            frText = $(this).parent().parent().text(),
            frHtml = $(this).parent().parent().html("")

        frHtml = frText.split(" ")
        frHtml.join("")

        if ($(this).hasClass("declineFR")) {
            DeclineFR(frId)
            .then(function (data) {
                frHtml = `<p class="successFR">You declined ${frHtml[76]} ${frHtml[77]}'s friend request!</p>`
                //Materialize.Toast(frHtml);
            })
        }
        else {
            AcceptFR(frId)
            .then(function (data) {
                frHtml = `<p class="successFR">You and ${frHtml[76]} ${frHtml[77]} are now friends!</p>`
                //Materialize.Toast(frHtml);
            })
        }
    })

    $(".post").on("click", function (e) {
        if (e.target.classList.contains("EditPost")) {
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

            $('.updatePost').on("click", function () {
                var updatedPostText = $(".editInput").val()
                EditSpecificPost(postId, updatedPostText)
                .then(function (post) {
                    let p = `<span class="postText">${updatedPostText}</span>`
                    postTextDiv.html(p)
                })
            })

            $('.cancelEdit').on("click", function () {
                let p = `<span class="postText">${postText}</span>`
                postTextDiv.html(p)
            })
        }
    })

})