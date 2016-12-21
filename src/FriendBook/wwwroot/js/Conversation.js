﻿function HideConversation() {
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
        let conversation = $(this).parent().parent().parent()
        conversation.remove();
    })
}

function SubmitNewMessage() {
    $(".submitNewMessage").on("click", function () {
        let conversationRoomName = $(this).parent().parent(".conversation").attr("id")
        let text = $(this).siblings(".newMessage").val()
        let NewMessage = {
            MessageText: text,
            ConversationRoomName: conversationRoomName,
        }

        SaveNewMessage(NewMessage)
        AddSingleMessageToConversation(NewMessage, this)
    })
}

function MessagingEvents() {
    HideConversation()
    ShowConversation()
    RemoveConversation()
    SubmitNewMessage()
}

function AddSingleMessageToConversation(message, context) {
    let ConversationMessageArea = $(context).parent().siblings(".convoMessageContainer")
    ConversationMessageArea.append(`<p>${message.MessageText}</p>`)
}

function AddMessagesToConversation(messages) {
    let ConversationMessage = ""

    messages.forEach(function (message) {
        ConversationMessage += `<p class="convoMessage">${message.messageText}</p>`
    })

    return ConversationMessage
}

function AddConversationToDom(conversation, message) {
    let messages = AddMessagesToConversation(message)

    let convo = `<div class="conversation" id="${conversation.conversationRoomName}">
            <div class="convoHead">
                <div class="convoName">
                    <h5>${conversation.conversationReciever.firstName} ${conversation.conversationReciever.lastName}</h5>
                </div>

                <div class="minifyOrExpand">
                    <i class="fa fa-minus hideConversation" aria-hidden="true"></i>
                    <i class="fa fa-plus hidden showConversation" aria-hidden="true"></i>
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

$(".MessageAreaUser").on("click", function () {
    let ClickedUserId = $(this).attr("id")

    GetCurrentUser()
    .then(function (user) {
        let ChatRoomName = ClickedUserId.toString() + user.userId.toString()
        ConnectToHub(ChatRoomName)
    })

    CreateNewConversation(ClickedUserId)
    .then(function (conversation) {
        GetAllConversationMessages(conversation.conversationRoomName)
        .then(function (messages) {
            let output = AddConversationToDom(conversation, messages)
            $(".conversationWrapper").append(output);
            MessagingEvents()
        })
    })
})

var hub = $.connection.broadcaster;
$.connection.broadcaster.client.addChatMessage = AddSingleMessageToConversation
$.connection.hub.logging = true;

function ConnectToHub(ChatRoomName) {
    $.connection.hub.start().done(function (signalr) {
        console.log('Connected!');
        console.log('SignalR object: ', signalr);

        hub.server.subscribe(ChatRoomName);
    }).fail(function (error) {
        console.log('Failed to start connection! Error: ', error);
    });
}