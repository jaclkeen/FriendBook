let ActiveConversations = []
let user;

GetCurrentUser()
.then(function (u) {
    user = u
})


//Purpose: To create a DOM element for all messages to a particular conversation
function AddMessagesToConversation(messages) {
    let ConversationMessage = ""

    if (messages.length > 0) {
        messages.forEach(function (message) {
            ConversationMessage +=
                `<div class="message"">
                    <img class ="messageSenderImage" src="${message.sendingUser.profileImg}">
                    <p class ="convoMessage">${message.messageText}</p>
                </div>`
        })
    }

    return ConversationMessage
}

//Purpose: To create a DOM element for a new conversation
function AddConversationToDom(conversation, message, AConvo) {
    let conversationName = ""
    let messages = AddMessagesToConversation(message)
    let i = 0
    let messageTitle = ""
    
    if (conversation.conversationReciever.userId === user.userId) {
        conversationName = conversation.conversationStarter.firstName + " " + conversation.conversationStarter.lastName
    }
    else {
        conversationName = conversation.conversationReciever.firstName + " " + conversation.conversationReciever.lastName
    }

    let convo = `<div class="conversation minifiedConversation" id="${conversation.conversationRoomName}">
            <div class="convoHead">
                <div class ="convoName">
                    <h5>${conversationName}</h5>
                </div>

                <div class="minifyOrExpand">
                    <i class ="fa fa-minus hidden hideConversation" aria-hidden="true"></i>
                    <i class="fa fa-plus showConversation" aria-hidden="true"></i>
                    <i class="fa fa-times removeConversation" aria-hidden="true"></i>
                </div>
            </div>

            <div class="convoMessageContainer">
                ${messages}
            </div>

            <div class="addNewMessage">
                <textarea class ="newMessage" placeholder="Message.."></textarea>
                <i class ="fa fa-arrow-circle-up submitNewMessage" aria-hidden="true"></i>
            </div>
        </div>`

    return convo;
}

//Purpose: To create a function that creates a new conversation, ensures there are less than 4 conversations open,
//          ensures the conversation being opened isn't already open, and appends the conversation along with its
//          messages to the DOM
function OpenConversation(ClickedUserId) {
    let CurrentUserIsRecieverUser = false
    let CurrentUserIsStartingUser = false

    CreateNewConversation(ClickedUserId)
    .then(function (conversation) {

        conversation.conversationStarterId === user.userId ? CurrentUserIsStartingUser = true
                : CurrentUserIsRecieverUser = true

        //IF THE CONVERSATION IS NOT ACTIVE, IF THERE ARE NOT CURRENTLY 4 ACTIVE CONVERSATIONS SET CONVERSATION AS ACTIVE
        //AND APPEND THAT CONVERSATION TO THE DOM
        if (CurrentUserIsRecieverUser && conversation.conversationRecieverIsActive == false || CurrentUserIsStartingUser && conversation.conversationStarterIsActive == false) {
            if (ActiveConversations.length < 4) {
                ActiveConversations.push(conversation.conversationRoomName)
                SetConversationAsActive(conversation.conversationRoomName)
                .then(function () {
                    GetAllConversationMessages(conversation.conversationRoomName)
                    .then(function (messages) {
                        let output = AddConversationToDom(conversation, messages)
                        $(".conversationWrapper").append(output);
                    })
                })
            }
            else {
                ToastNotification("Only 4 active conversations allowed at a time.. Close one to start another!")
            }
        }
        else {
            let conversations = $(".conversation").toArray();

            conversations.forEach(function (convo) {
                convo = $(convo)
                if (convo.attr("id") === conversation.conversationRoomName) {
                    let ShowHideDiv = convo.children(".convoHead").children(".minifyOrExpand")
                    ShowHideDiv.children(".showConversation").addClass("hidden")
                    ShowHideDiv.children(".hideConversation").removeClass("hidden")
                    convo.removeClass("minifiedConversation")
                }
            })
        }
    })
}

//Purpose: On page load, get all active conversation and append them with their messages to the DOM. This is used to 
//          carry conversations over from page to page.
function ActiveConvo() {
    UserActiveConversations()
    .then(function (conversations) {
        conversations.forEach(function (convo) {
            if (ActiveConversations.length < 4) {
                ActiveConversations.push(convo.conversationRoomName)
                GetAllConversationMessages(convo.conversationRoomName)
                .then(function (messages) {
                    let output = AddConversationToDom(convo, messages)
                    $(".conversationWrapper").append(output);
                })
            }
        })
    })
}

//Purpose: searches through all conversations in the conversation array and looks for messages for that conversation
function UpdateConversationMessages() {
    let Conversations = $(".conversation").toArray();

    ActiveConversations.forEach(function (convo) {
        Conversations.forEach(function (c) {
            let co = $(c)
            if (co.attr("id") === convo) {
                let y = co.children(".convoMessageContainer")
                GetAllConversationMessages(convo)
                .then(function (messages) {
                    y.html("")
                    let DomMessage = AddMessagesToConversation(messages)
                    y.append(DomMessage)
                })
            }
        })
    })
}

//Purpose: To load all message notifications where the current user is the recieving user and append them to the dom
function UpdateUnseenMessages() {
    GetUserMessageNotifications()
    .then(function (notifications) {
        $(".messageNotificationArea").html("")

        if (notifications.length === 0) {
            let noti = `<h4 style="text-align:center">No new messages!</h4>`
            $(".messageNotificationArea").append(noti)
        }

        notifications.forEach(function (n) {
            let noti =
                `<div class='NewM' id='${n.sendingUser.userId}' data='${n.messageNotificationId}'>
                    <img src=${n.sendingUser.profileImg} class='MnImg'/>
                    <a asp-action="Index" asp-controller="Profile" asp-route-id="${n.sendingUser.userId}" class="MnName MnText">${n.sendingUser.firstName} ${n.sendingUser.lastName}</a>
                    <span class="MnText">sent you a new message!</span>
                </div>`

            $(".messageNotificationArea").append(noti)
        })

        $(".messageNotifications").html(`<a class="glyphicon glyphicon-inbox navGlyph">(${notifications.length})</a>`)
    })
}

//Purpose: Creates a DOM element and appends for each friend that is passed into the function in the friend search message area
function AddUserFriendsToDom(friends) {
    $(".UserFriendsList").html("")

    if (friends.length > 1) {
        friends.forEach(function (friend) {
            $(".UserFriendsList").append(`<div class="MessageAreaUser" id="${friend.userId}">
                <img src= ${friend.profileImg} class="messageProfileImg" />
                <span class="MessageAreaName">${friend.firstName} ${friend.lastName}</span>
            </div>`)
        })
    }
    else {
        $(".UserFriendsList").append(`<div class="MessageAreaUser" id="${friends[0].userId}">
                <img src= "${friends[0].profileImg}" class="messageProfileImg" />
                <span class="MessageAreaName">${friends[0].firstName} ${friends[0].lastName}</span>
            </div>`)
    }
}

//Purpose: Event listener for the friend search area that gets the current user's friend and determines if any of 
//          those friends fullname, firstname, or lastname matches the input of the search input. If it does, it calls
//          the AddUserFriendsToDom function the user(s) to the dom.
$(".messageFriendSearch").on("input", function () {
    let search = $(this).val().toLowerCase()
    let FoundFriends = []
    GetCurrentUserFriends()
    .then(function (friends) {
        friends.forEach(function (friend) {
            let FullName = friend.firstName + friend.lastName
            search === FullName.toLowerCase() ? FoundFriends.push(friend) : false
            search === friend.firstName.toLowerCase() ? FoundFriends.push(friend) : false
            search === friend.lastName.toLowerCase() ? FoundFriends.push(friend) : false
            search === "" ? AddUserFriendsToDom(friends) : false

        })

        FoundFriends.length > 0 ? AddUserFriendsToDom(FoundFriends) : false
    })
})

//Purpose: To create an event listener that hides a particular conversations (slides down the page) when the - button 
//          is clicked
$("body").on("click", function (e) {
    let context = $(e.target)
    if (context.hasClass("hideConversation")) {
        let conversation = $(context).parent().parent().parent()
        conversation.addClass("minifiedConversation")
        $(context).addClass("hidden")
        $(context).siblings(".showConversation").removeClass("hidden")
    }
})

//Purpose: To create an event listener that shows a particular conversation (slides up the page) when the + button
//          is clicked
$("body").on("click", function (e) {
    let context = $(e.target)
    if (context.hasClass("showConversation")) {
        let conversation = $(context).parent().parent().parent()
        conversation.removeClass("minifiedConversation")
        $(context).addClass("hidden")
        $(context).siblings(".hideConversation").removeClass("hidden")
    }
})

//Purpose: To create an event listener that removes and ends a particular conversation when the x button is clicked
$("body").on("click", function (e) {
    let context = $(e.target)
    if (context.hasClass("removeConversation")) {
        let RemoveConversation = null
        let conversation = $(context).parent().parent().parent()
        let conversationId = conversation.attr("id")
        EndAConversation(conversationId)
        .then(function () {
            console.log(ActiveConversations.length)
            RemoveConversation = ActiveConversations.indexOf(conversationId)
            RemoveConversation != -1 ? ActiveConversations.splice(RemoveConversation, 1) : false;
            conversation.remove();
        })
    }
})

//Purpose: To create an event listener that submits and posts a new message to a conversation when the submit button
//          is pressed in a conversation
$("body").on("click", function (e) {
    let context = $(e.target)
    if (context.hasClass("submitNewMessage")) {
        GetCurrentUser()
        .then(function (u) {
            let conversationRoomName = $(context).parent().parent(".conversation").attr("id")
            let ConversationMessageArea = $(context).parent().siblings(".convoMessageContainer")
            let text = $(context).siblings(".newMessage").val()
            let NewMessage = {
                MessageText: text,
                ConversationRoomName: conversationRoomName,
            }

            let ConversationMessage =
                `<div class="message"">
                    <img class ="messageSenderImage" src="${u.profileImg}">
                    <p class ="convoMessage">${NewMessage.MessageText}</p>
                </div>`

            if (NewMessage.MessageText != "") {
                SaveNewMessage(NewMessage)
                ConversationMessageArea.append(`${ConversationMessage}`)
                $(context).siblings(".newMessage").val("")
                ToastNotification("Message sent!")
            }
            else {
                ToastNotification("You cannot send an empty message!")
            }
        })
    }
})

//Purpose: To create an event listener that opens a conversation when a user is clicked on in a user's profile page
$(".sendMessageToUser").on("click", function () {
    let UserId = $(this).attr("id")
    OpenConversation(UserId)
})

//Purpose: To create an event listener that opens a conversation when a user is clicked on in the userFriends message area
$("body").on("click", function (e) {
    let context = $(e.target)
    if (context.hasClass("MessageAreaUser")) {
        let UserId = $(context).attr("id")
        OpenConversation(UserId)
    }

    else if (context.hasClass("MessageAreaName") || context.hasClass("messageProfileImg")) {
        let UserId = $(context).parent().attr("id")
        OpenConversation(UserId)
    }

})

//Purpose: Removes a message notification, updates the notification count, and posts that notification as seen
$("body").on("click", function (e) {
    let context = $(e.target)

    if ($(context).hasClass("MnName")) {}

    else if ($(context).hasClass("NewM")) {
        let UserId = $(context).attr("id")
        $(context).remove()
        UpdateMessageSeen($(context).attr("data"))
        .then(function (UnseenMessageCount) {
            $(".MnCount").text(`(${UnseenMessageCount})`)
            OpenConversation(UserId)
        })
    }

    else if($(context).hasClass("MnText")){
        let newContext = $(context).parent();
        let UserId = $(newContext).attr("id")
        $(newContext).remove()
        UpdateMessageSeen($(newContext).attr("data"))
        .then(function (UnseenMessageCount) {
            $(".MnCount").text(`(${UnseenMessageCount})`)
            OpenConversation(UserId)
        })
    }
})