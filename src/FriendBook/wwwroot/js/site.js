$(document).ready(function () {

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
        for (var user in userList) {
            let firstName = userList[user].firstName
            let lastName = userList[user].lastName
            let fullName = `${userList[user].firstName} ${userList[user].lastName}`

            if (userList[user].profileImg === null) {
                userList[user].profileImg = "/images/egg.png"
            }

            if (searchInput == firstName || searchInput == lastName || searchInput == fullName
                || searchInput == firstName.toLowerCase() || searchInput == lastName.toLowerCase()
                || searchInput == fullName.toLowerCase()) {

                $(".searchResults").append(`<div class="userInSearch" id="${userList[user].userId}">
                    <img class ="searchProfilePic" src=${userList[user].profileImg}>
                    <p class="searchProfileName">${fullName}</p></div>`);
            }
        }
    }

    function userSearchEvents() {
        $('.userInSearch').on('click', function () {
            let user = $(this).attr("id");
            window.location.href = `/Profile/Profile/${user}`
        })
    }

    $('.userSearch').on("input", function(){
        getUsers()
        .then(function (users) {
            addUsersSearchToDom(users)
            userSearchEvents();
        })
    })

})