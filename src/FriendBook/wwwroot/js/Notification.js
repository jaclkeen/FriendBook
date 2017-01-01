﻿function GetCurrentUserNotifications() {
    GetUserNotifications()
    .then(function (notifications) {
        $(".UserNotificationDiv").html("")
        notifications.forEach(function (notification) {
            let NotificationDOMElement = AddSingleUserNotificationToDom(notification)
            $(".UserNotificationDiv").append(NotificationDOMElement)
        })
    })
}

function AddSingleUserNotificationToDom(un) {
    if (un.notificationType == "Tag" || un.notificationType == "FR"){
        return `<div class="NewUserNotification" id="${un.postId}" data="${un.sendingUser.userId} ${un.notificationId} ${un.notificationType}">
                    <img src="${un.sendingUser.profileImg}" class="UserNotificationsImage" />
                    <p class="UserNotificationsText">${un.notificationText}</p>
                </div>`
    }
    else{
        return `<div class="NewUserNotification" id="${un.postId}" data="${un.recievingUser.userId} ${un.notificationId} ${un.notificationType}">
                    <img src="${un.sendingUser.profileImg}" class="UserNotificationsImage" />
                    <p class="UserNotificationsText">${un.notificationText}</p>
                </div>`
    }
}

$(".UserNotificationDiv").on("click", function (e) {
    let context = $(e.target)

    if (context.hasClass("UserNotificationsText") || context.hasClass("UserNotificationsImage")) {
        let NotificationData = context.parent().attr("data").split(" ")
        console.log(NotificationData)


        let NotificationType = NotificationData[2]
        let NotificationId = NotificationData[1]
        let SendingOrRecieverId = NotificationData[0]
        let PostId = context.parent().attr("id")

        if (NotificationType === "Tag" || NotificationType === "FR") {
            console.log(NotificationId)
            UserNotificationSeen(NotificationId)
            .then(function(){
                window.location = `/Profile/Index/${SendingOrRecieverId}/#'${PostId}'`
            })
        }
        else {
            console.log(NotificationId)
            UserNotificationSeen(NotificationId)
            .then(function(){
                window.location = `/Profile/Index/${SendingOrRecieverId}/#'${PostId}'`
            })
        }
    }
})

//setInterval(GetCurrentUserNotifications, 5000)