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
    // Example validation function
    var email = document.getElementById('email').value;
    var emailError = document.getElementById('emailError');
    var emailPattern = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;

    if (!emailPattern.test(email)) {
        emailError.textContent = 'Please enter a valid email address.';
        return false;
    }

    emailError.textContent = '';
    return true;
}