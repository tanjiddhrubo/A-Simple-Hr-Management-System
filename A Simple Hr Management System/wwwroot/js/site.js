// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    const companyCookieName = "SelectedCompanyId";

    // Function to set a cookie
    function setCookie(name, value, days) {
        var expires = "";
        if (days) {
            var date = new Date();
            date.setTime(date.getTime() + (days * 24 * 60 * 60 * 1000));
            expires = "; expires=" + date.toUTCString();
        }
        document.cookie = name + "=" + (value || "") + expires + "; path=/";
    }

    // Function to get a cookie
    function getCookie(name) {
        var nameEQ = name + "=";
        var ca = document.cookie.split(';');
        for (var i = 0; i < ca.length; i++) {
            var c = ca[i];
            while (c.charAt(0) == ' ') c = c.substring(1, c.length);
            if (c.indexOf(nameEQ) == 0) return c.substring(nameEQ.length, c.length);
        }
        return null;
    }

    // When the dropdown changes, save the value to a cookie
    $('#companySelector').on('change', function () {
        var selectedCompanyId = $(this).val();
        setCookie(companyCookieName, selectedCompanyId, 30); // Save for 30 days
        location.reload(); // Reload the page to apply the filter (we'll use this later)
    });

    // On page load, read the cookie and set the dropdown's value
    var savedCompanyId = getCookie(companyCookieName);
    if (savedCompanyId) {
        $('#companySelector').val(savedCompanyId);
    }
});