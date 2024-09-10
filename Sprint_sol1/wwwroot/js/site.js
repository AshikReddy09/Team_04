$(document).ready(function () {
    $('.dropdown-toggle').dropdown();
    $('a[href*="#"]').on('click', function (e) {
        e.preventDefault();

        $('html, body').animate({
            scrollTop: $($(this).attr('href')).offset().top
        }, 500, 'linear');
    });
});



function validateForm() {
    var name = document.getElementById('name').value;
    var email = document.getElementById('email').value;
    var message = document.getElementById('message').value;
    var emailError = document.getElementById('emailError');

    // Simple validation example
    emailError.textContent = '';
    var isValid = true;

    if (!name) {
        alert('Name is required');
        isValid = false;
    }

    if (!email) {
        alert('Email is required');
        isValid = false;
    } else if (!validateEmail(email)) {
        emailError.textContent = 'Invalid email address';
        isValid = false;
    }

    if (!message) {
        alert('Message is required');
        isValid = false;
    }

    return isValid;
}

function validateEmail(email) {
    var re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(String(email).toLowerCase());
}
