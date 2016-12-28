//Purpose: To hide the register page
$(".Register").hide();

//Purpose: To show the login page and hide the register page on 'login' click
$(".showLogin").on("click", function () {
    $(this).addClass("active")
    $(".showRegister").removeClass("active")
    $(".Login").show();
    $(".Register").hide();
})

//Purpose: To show the register page and hide the login page on 'register' click
$(".showRegister").on("click", function () {
    $(this).addClass("active")
    $(".showLogin").removeClass("active")
    $(".Register").show();
    $(".Login").hide();
})

//Purpose: To provide form validation for a user trying to login and to log that user in if his/her credentials 
//          match that of an existing user
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

//Purpose: To provide for validation for the user trying to register and call the post method in the factory to register
//         a new user
$(".submitRegister").on("click", function () {
    let FirstName = $(".registerFN").val(),
        LastName = $(".registerLN").val(),
        Email = $(".registerEmail").val(),
        Pass = $(".registerPass").val(),
        ConfirmPass = $(".registerConfirmPass").val(),
        FirstNameValidation = $(".RegisterFirstNameValidation").html(""),
        LastNameValidation = $(".RegisterLastNameValidation").html(""),
        EmailValidation = $(".RegisterEmailValidation").html(""),
        PassValidation = $(".RegisterPasswordValidation").html(""),
        ConfirmPassValidation = $(".ConfirmPasswordValidation").html(""),
        ValidFirstName = false,
        ValidLastName = false,
        ValidEmail = false,
        ValidPass = false,
        PassConfirmed = false

    FirstName === "" ? FirstNameValidation.html("A first name is required") : ValidFirstName = true
    LastName === "" ? LastNameValidation.html("A last name is required") : ValidLastName = true
    Email.indexOf("@") === -1 || Email.indexOf(".") === -1 ? EmailValidation.html("You entered an invalid email address") : ValidEmail = true
    Pass.length < 5 ? PassValidation.html("Your password must be at least 5 characters in length") : ValidPass = true
    ConfirmPass !== Pass ? ConfirmPassValidation.html("Your passwords do not match") : PassConfirmed = true

    if (ValidFirstName && ValidLastName && ValidEmail && ValidPass && PassConfirmed) {
        let user = {
            "FirstName": FirstName,
            "LastName": LastName,
            "Email": Email,
            "Password": Pass
        }

        CreateNewUser(user)
        .then(function (id) {
            window.location.href = `/Login/LoginUser/${id}`
        })
    }
})