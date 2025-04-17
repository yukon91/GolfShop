document.addEventListener("DOMContentLoaded", function () {
    const registerSuccess = new URLSearchParams(window.location.search);
    if (registerSuccess.get("registered") === "true") {
        alert("Registrering lyckades!")
    }
});