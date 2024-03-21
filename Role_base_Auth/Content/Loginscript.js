

// Secure username and password retrieval (avoiding direct DOM manipulation)
function getCredentials() {
    debugger;
    const usernameInput = document.getElementById("txtusername");
    const passwordInput = document.getElementById("txtPassword");

    if (!usernameInput || !passwordInput) {
        throw new Error("Username or password input elements not found.");
    }

    const username = usernameInput.value.trim();
    const Password = passwordInput.value.trim();

    if (!username || !Password) {
        throw new Error("Username or password cannot be empty.");
    }

    return { username, Password };
}

// Improved AJAX method with error handling and token storage
$(document).ready(function () {
    debugger;
    $("#submitButton").click(function (event) {
        debugger;
        event.preventDefault(); // Prevent default form submission

        try {
            const credentials = getCredentials();
            const url = "Home/Gettoken"; // Assuming relative path, adjust if needed

            $.ajax({
                url: url,
                type: "POST", // Assuming endpoint expects POST (adjust if needed)
                dataType: "json",
                contentType: "application/json; charset=utf-8", // Specify JSON format
                data: JSON.stringify(credentials),
                success: function (data) {
                    debugger;
                    if (data.token) {
                        // Store token securely (e.g., session storage, cookies with HttpOnly flag)
                        sessionStorage.setItem("accessToken", data.token);
                        sessionStorage.setItem("userName", data.userName);
                        //window.location.href = "https://localhost:44332/Employee/GetEmployee";
                        window.location.href = "Employee/GetEmployee";
                        //if (data.token != "" && data.token != null && data.token != undefined) {
                          

                        //}
                        //else {
                            
                        //}
                       /* alert("Token received successfully!");*/

                        // Optionally, redirect or perform authorized actions with the token
                    } else {
                   
                        window.location.href = "Account/Login";
                        alert("Invalid credentials or error obtaining token.");
                    }
                },
                error: function (jqXHR, textStatus, errorThrown) {
                    debugger;
                    console.error("AJAX request failed:", textStatus, errorThrown);
                    alert("An error occurred while obtaining the token.");
                }
            });
        } catch (error) {
            console.error("Error getting credentials:", error);
            alert("There was a problem retrieving your credentials.");
        }
    });
});
