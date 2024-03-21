$(document).ready(function () {
    debugger;
    $("#SaveEmployee").click(function (event) {
        debugger;
        event.preventDefault();
        $.ajax({
            url: 'Account/GetEmployee',
            type: 'GET',
            beforeSend: function (xhr) {
                var token = sessionStorage.getItem("accessToken");
                var username = sessionStorage.getItem("userName");
                var authHeaderValue = 'Bearer ' + token + ':' + username;
                xhr.setRequestHeader('Authorization', authHeaderValue);
            },
            success: function (data) {
                debugger;
                
            },
            error: function (xhr, textStatus, errorThrown) {
                // Handle error
            }
        });

    })
});
