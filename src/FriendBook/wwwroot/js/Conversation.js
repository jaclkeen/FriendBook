let ActiveConversations = []

function HideConversation() {
    $(".hideConversation").on("click", function () {
        let conversation = $(this).parent().parent().parent()
        conversation.addClass("minifiedConversation")
        $(this).addClass("hidden")
        $(this).siblings(".showConversation").removeClass("hidden")
    })
}

function ShowConversation() {
    $(".showConversation").on("click", function () {
        let conversation = $(this).parent().parent().parent()
        conversation.removeClass("minifiedConversation")
        $(this).addClass("hidden")
        $(this).siblings(".hideConversation").removeClass("hidden")
    })
}

function RemoveConversation() {
    $(".removeConversation").on("click", function () {
        let RemoveConversation = null
        let conversation = $(this).parent().parent().parent()
        let conversationId = conversation.attr("id")
        EndAConversation(conversationId)
        .then(function () {
            RemoveConversation = ActiveConversations.indexOf(conversationId)
            RemoveConversation != -1 ? ActiveConversations.splice(RemoveConversation) : false;
            conversation.remove();
        })
    })
}

function MessagingEvents() {
    HideConversation()
    ShowConversation()
    RemoveConversation()
}

function AddMessagesToConversation(messages) {
    let ConversationMessage = ""

    messages.forEach(function (message) {
        ConversationMessage += `<p class="convoMessage">${message.messageText}</p>`
    })

    return ConversationMessage
}

function AddConversationToDom(conversation, message, AConvo) {

    let messages = AddMessagesToConversation(message)

    let convo = `<div class="conversation minifiedConversation" id="${conversation.conversationRoomName}">
            <div class="convoHead">
                <div class="convoName">
                    <h5>${conversation.conversationReciever.firstName} ${conversation.conversationReciever.lastName}</h5>
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

function OpenConversation(ClickedUserId){
    if($(this).hasClass("MnName")){
        return false;
    }

    CreateNewConversation(ClickedUserId)
    .then(function (conversation) {
        if (conversation.isActive == false) {
            if (ActiveConversations.length < 4) {
                ActiveConversations.indexOf(conversation.conversationRoomName) === -1 ? ActiveConversations.push(conversation.conversationRoomName) : false;
                SetConversationAsActive(conversation.conversationRoomName)
                .then(function () {
                    GetAllConversationMessages(conversation.conversationRoomName)
                    .then(function (messages) {
                        let output = AddConversationToDom(conversation, messages)
                        $(".conversationWrapper").append(output);
                        MessagingEvents()
                    })
                })
            }
            else {
                //MATERILIAZE.TOAST ONLY 4 ACTIVE CONVOS AT TIME
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
                    MessagingEvents()
                })
            }
        })
    })
}

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
                    messages.forEach(function (m) {
                        y.append(`<p class="convoMessage">${m.messageText}</p>`)
                    })
                })
            }
        })
    })
}

function UpdateUnseenMessages() {
    GetUserMessageNotifications()
    .then(function (notifications) {
        notifications.forEach(function (n) {
            let noti =
            `<div class='NewM' id='${n.SendingUser.UserId}' data='${n.MessageNotificationId}'>
            <img src='${n.SendingUser.ProfileImg}' class='MnImg'/>
            <a asp-action="Index" asp-controller="Profile" asp-route-id="@mn.SendingUser.UserId" class="MnName MnText">@mn.SendingUser.FirstName @mn.SendingUser.LastName</a>
            <span class="MnText">sent you a new message!</span>
            </div>`
        })
    })
}

$("body").on("click", function (e) {
    let context = $(e.target)
    if (context.hasClass("submitNewMessage")) {
        let conversationRoomName = $(context).parent().parent(".conversation").attr("id")
        let ConversationMessageArea = $(context).parent().siblings(".convoMessageContainer")
        let text = $(context).siblings(".newMessage").val()
        let NewMessage = {
            MessageText: text,
            ConversationRoomName: conversationRoomName,
        }

        console.log(conversationRoomName, ConversationMessageArea, text, NewMessage)

        SaveNewMessage(NewMessage)
        ConversationMessageArea.append(`<p>${NewMessage.MessageText}</p>`)
    }
})

$(".MessageAreaUser, .NewM").on("click", function () {
    let UserId = $(this).attr("id")

    OpenConversation(UserId)
})

$(".NewM").on("click", function () {
    if ($(this).hasClass("MnName")) {
        return false;
    }

    $(this).remove()
    UpdateMessageSeen($(this).attr("data"))
    .then(function (UnseenMessageCount) {
        console.log(UnseenMessageCount)
        $(".MnCount").text(`(${UnseenMessageCount})`)
    })
})

ActiveConvo()
setTimeout(setInterval(UpdateConversationMessages, 5000), 3000)
setInterval(UpdateUnseenMessages, 5000)