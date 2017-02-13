function GetCurrentUserNotifications() {
    GetUserNotifications()
    .then(function (notifications) {
        $(".UserNotificationDiv").html("")
        $(".UserNotificationDiv").append(`<h4 class="NotiTitle">Notifications:</h4><br />`)
        notifications.forEach(function (notification) {
            let NotificationDOMElement = AddSingleUserNotificationToDom(notification)
            $(".UserNotificationDiv").append(NotificationDOMElement)
        })

        if (notifications.length === 0) {
            $(".UserNotificationDiv").append(`<div class="NoNewNotifications" id="0" data="0">
                                                    <p class="NoNewNotificationsText">No new notifications!</p>
                                              </div>`)
        }

        $(".ShowUserNotifications").html(`<a class="UserNotifications glyphicon glyphicon-globe navGlyph"><span class='badge'>${notifications.length}</span></a>`)
    })
}

function AddSingleUserNotificationToDom(un) {
    if (un.notificationType == "Tag" || un.notificationType == "FR"){
        return `<div class="NewUserNotification" id="${un.postId}" data="${un.sendingUser.userId} ${un.notificationId} ${un.notificationType}">
                    <img src="${un.sendingUser.profileImg}" class="UserNotificationsImage" />
                    <p class ="UserNotificationsText">${un.notificationText}</p>
                </div>`
    }
    else if (un.notificationType == "Sale") {
        return `<div class="NewUserNotification" id="${un.yardSaleItemId}" data="${un.sendingUser.userId} ${un.notificationId} ${un.notificationType}">
                    <img src="${un.sendingUser.profileImg}" class="UserNotificationsImage" />
                    <p class ="UserNotificationsText">${un.notificationText}</p>
                </div>`
    }
    else{
        return `<div class="NewUserNotification" id="${un.postId}" data="${un.recievingUser.userId} ${un.notificationId} ${un.notificationType}">
                    <img src="${un.sendingUser.profileImg}" class="UserNotificationsImage" />
                    <p class ="UserNotificationsText">${un.notificationText}</p>
                </div>`
    }
}

$(".UserNotificationDiv").on("click", function (e) {
    let context = $(e.target),
        ClickedNotification = false,
        NotificationData = null,
        PostId = null

    if (context.hasClass("UserNotificationsText") || context.hasClass("UserNotificationsImage")) {
        NotificationData = context.parent().attr("data").split(" ")
        PostId = context.parent().attr("id")
        context.parent().remove();
        ClickedNotification = true
    }
    else if (context.hasClass("NewUserNotification")) {
        NotificationData = context.attr("data").split(" ")
        PostId = context.attr("id")
        context.remove()
        ClickedNotification = true
    }

    if(ClickedNotification){
        let NotificationType = NotificationData[2],
            NotificationId = NotificationData[1],
            SendingOrRecieverId = NotificationData[0]

        if (NotificationType === "Tag" || NotificationType === "FR") {
            UserNotificationSeen(NotificationId)
            .then(function(){
                window.location = `/Profile/Index/${SendingOrRecieverId}/#'${PostId}'`
            })
        }
        else if (NotificationType === "Sale") {
            UserNotificationSeen(NotificationId)
            .then(function () {
                window.location = `/YardSale/#${PostId}`
            })
        }
        else if (NotificationType === "ProfileView") {
            UserNotificationSeen(NotificationId)
            .then(function () {})
        }
        else {
            UserNotificationSeen(NotificationId)
            .then(function(){
                window.location = `/Profile/Index/${SendingOrRecieverId}/#'${PostId}'`
            })
        }
    }
})
