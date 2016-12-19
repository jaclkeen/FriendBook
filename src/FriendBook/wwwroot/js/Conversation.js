$(".hideConversation").on("click", function () {
    let conversation = $(this).parent().parent().parent()
    conversation.addClass("minifiedConversation")
    $(this).addClass("hidden")
    $(this).siblings(".showConversation").removeClass("hidden")
})

$(".showConversation").on("click", function () {
    let conversation = $(this).parent().parent().parent()
    conversation.removeClass("minifiedConversation")
    $(this).addClass("hidden")
    $(this).siblings(".hideConversation").removeClass("hidden")
})

$(".removeConversation").on("click", function () {
    let conversation = $(this).parent().parent().parent()
    conversation.remove();
})