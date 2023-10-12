// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


// load appropriate view
// Alterred from week 9 sample codes by Dr. Sajib Mistry
function loadView(status)
{
    var apiUrl = '/api/login/defaultview';
    if (status === "authview")
        apiUrl = '/api/login/authview';
    if (status === "error")
        apiUrl = '/api/login/error';
    if (status === "logout")
        apiUrl = '/api/logout';
    if (status === 'dash')
        apiUrl = '/api/dash/view'

    fetch(apiUrl)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.text();
        })
        .then(data => {
            // Handle the data from the API
            document.getElementById('main').innerHTML = data;
            if (status === "logout") {
                document.getElementById('LogoutButton').style.display = "none";
                document.getElementById('DashButton').style.display = "none";
            }
        })
        .catch(error => {
            // Handle any errors that occurred during the fetch
            console.error('Fetch error:', error);
        });
}


// authenticate user login
// Alterred from week 9 sample codes by Dr. Sajib Mistry
function authenticate()
{
    var name = document.getElementById("UsernameInput").value;
    var pwd = document.getElementById("PwdInput").value;
    var data = {
        Username: name,
        Password: pwd,
    };

    const apiUrl = '/api/login/auth';

    const headers = {
        'Content-Type': 'application/json', // specify content type as JSON
    };

    const requestOptions = {
        method: 'POST',
        headers: headers,
        body: JSON.stringify(data) // convert data object to a JSON string
    };

    fetch(apiUrl, requestOptions)
        .then(response => {
            if (!response.ok) {
                throw new Error('Network response was not ok');
            }
            return response.json();
        })
        .then(data => {
            const jsonObject = data;
            if (jsonObject.login) {
                document.getElementById('LogoutButton').style.display = "block";
                document.getElementById('DashButton').style.display = "block";
                loadView("authview");
            }
            else {
                loadView("error");
            }
        })
        .catch(error => {
            console.error('Fetch error: ', error);
        });
}


// 
function updateMyDetails()
{
    var newEmail = document.getElementById("e").value;
    var newPhone = document.getElementById("ph").value;

    if (newEmail !== null && newPhone !== null && newEmail !== "" && newPhone !== "") {
        document.getElementById('detailError').style.display = "none";
        var data = {
            UserID = 0;
            Name = "";
            Email = newEmail;
            Address = "";
            Phone = newPhone;
            Pwd = "";
        }

        const apiUrl = '/api/dash/updateMyDetails';

        const headers = {
            'Content-Type': 'application/json', // specify content type as JSON
        };

        const requestOptions = {
            method: 'PUT',
            headers: headers,
            body: JSON.stringify(data) // convert data object to a JSON string
        };

        fetch(apiUrl, requestOptions)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                const jsonObject = data;
                if (jsonObject.ok) {
                    document.getElementById("detailError").style.display = "none";
                    loadView("dash");
                }
                else {
                    document.getElementById("detailError").style.display = "block";
                }
            })
            .catch(error => {
                console.error('Fetch error: ', error);
            });

    }
    else {
        document.getElementById('detailError').style.display = "block";
    }
}


// check passwords match before sending pwd reset request
function pwdReset()
{
    var newPwd = document.getElementById("NewPwd").value;
    var rNewPwd = document.getElementById("RNewPwd").value;

    const apiUrl = '/api/dash/pwdReset';

    const headers = {
        'Content-Type': 'application/json', // specify content type as JSON
    };

    const requestOptions = {
        method: 'POST',
        headers: headers,
        body: newPwd
    };

    if (newPwd === rNewPwd) {
        document.getElementById('pwdError').style.display = "none";

        fetch(apiUrl, requestOptions)
            .then(response => {
                if (!response.ok) {
                    throw new Error('Network response was not ok');
                }
                return response.json();
            })
            .then(data => {
                const jsonObject = data;
                if (jsonObject.login) {
                    loadView("authview");
                }
                else {
                    loadView("error");
                }
            })
            .catch(error => {
                console.error('Fetch error: ', error);
            });
    }
    else {
        document.getElementById('pwdError').style.display = "block";
    }
}


// change page to see profile edit functions
function toggleEditProfile() {
    if (document.getElementById('EditProfile').style.display.value === "block") {
        document.getElementById('EditProfile').style.display = "none";
    }
    else {
        document.getElementById('EditProfile').style.display = "block";
    }
}

document.addEventListener("DOMContentLoaded", loadView);