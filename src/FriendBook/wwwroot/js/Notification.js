function GetCurrentUserNotifications() {
    GetUserNotifications()
    .then(function (notifications) {
        $(".UserNotificationDiv").html("")
        notifications.forEach(function (notification) {

        })
    })
}

setInterval(GetCurrentUserNotifications, 5000)