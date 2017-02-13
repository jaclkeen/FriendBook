//Purpose: To call the factory method that returns all of the current user's friend request, then to pass each request
//  into the CreateFRDomElement function and append that returned DOM element to the DOM.
function GetAllPendingUserFriendRequests() {
    GetUserFriendRequests()
    .then(function (requests) {
        $(".notificationArea").html("")
        $(".notificationArea").append(`<h4 class="NTitle">Friend Requests:</h4><br />`)
        $(".Notifications").html(`<span class='badge'>${requests.length}</span>`)
        if (requests.length === 0) {
            $(".notificationArea").append(`<div class="NoFR">
                                                <p>No new friend requests!</p>
                                           </div>`)
        }
        requests.forEach(function (request) {
            let FRDomElement = CreateFRDomElement(request)
            $(".notificationArea").append(FRDomElement)
        })
    })
}

//Purpose: To create a DOM element based on the friend request passed into it.
function CreateFRDomElement(request) {
    return `<div class="FR">
                <img class="notificationProfileImg" src='${request.senderUser.profileImg}' />
                <div class="receivedFR">
                    <p>
                        <a class ="FRUserName" href="/Profile/Index/${request.senderUser.userId}">${request.senderUser.firstName} ${request.senderUser.lastName}</a>
                        sent you a friend request!
                    </p>
                </div>
                <div class="frButtonDiv">
                    <input type="button" class="btn-success frButton acceptFR" id="${request.relationshipId}" value="Accept" />
                    <input type="button" class="btn-danger frButton declineFR" id="${request.relationshipId}" value="Decline" />
                </div>
            </div>`
}

//Purpose: To add an event listener to either accept or decline a friend request on button click
$('.notificationArea').on("click", function (e) {
    let context = $(e.target)

    if (context.hasClass("declineFR")) {
        let frId = $(context).attr("id")
        DeclineFR(frId)
        .then(function (data) {
            location.reload();
        })
    }
    else if (context.hasClass("acceptFR")) {
        let frId = $(context).attr("id")
        AcceptFR(frId)
        .then(function () {
            location.reload();
        })
    }
})