$(".Register").hide();

$(".showLogin").on("click", function () {
    $(this).addClass("active")
    $(".showRegister").removeClass("active")
    $(".Login").show();
    $(".Register").hide();
})

$(".showRegister").on("click", function () {
    $(this).addClass("active")
    $(".showLogin").removeClass("active")
    $(".Register").show();
    $(".Login").hide();
})

$(".submitLogin").on("click", function () {
    let email = $(".email").val();
    let pass = $(".password").val();
    let UserAccount = null;
    let correctEmail = false
    let correctPass = false

    if (email.indexOf("@") > -1 && email.indexOf(".") > -1) {
        getUsers()
        .then(function (users) {
            users.forEach(function (user) {
                if (correctEmail === false) {
                    if (email === user.email) {
                        correctEmail = true
                        UserAccount = user
                    }
                }
            })

            if (correctEmail === true) {
                if (UserAccount.password === pass) {
                    correctPass = true
                    console.log('hey')
                    window.location.href = `/Login/LoginUser/${UserAccount.userId}`
                }
            }

            if (correctEmail === false) {
                $(".LoginEmailValidation").html("Your email address is invalid")
            }else {
                $(".LoginEmailValidation").html("")
            }

            if (correctPass === false) {
                $(".LoginPasswordValidation").html("Your password is incorrect")
            }
        })
    }
    else {
        $(".LoginEmailValidation").html("Your email address is invalid")
    }
})