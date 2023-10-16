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


// update phone and email fields of a user profile
function updateMyDetails()
{
    var newEmail = document.getElementById("e").value;
    var newPhone = document.getElementById("ph").value;

    if (notNullOrEmpty(newPhone)  && validEmail(newEmail)) {
        document.getElementById('detailError').style.display = "none";
        var data = {
            UserID: 0,
            Name: "a",
            Email: newEmail,
            Address: "a",
            Phone: newPhone,
            Pwd: "a",
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


// post new user profile to the data server
function createUser() {
    var Name = document.getElementById("createUsername").value;
    var Email = document.getElementById("createPassword").value;
    var Address = document.getElementById("createAddress").value;
    var Phone = document.getElementById("createPhone").value;
    var Pwd = document.getElementById("createPassword").value;


    if (notNullOrEmpty(Name) && notNullOrEmpty(Pwd) && validEmail(Email) && notNullOrEmpty(Address) && notNullOrEmpty(Phone)) {
        document.getElementById("createUserError").style.display = "none";

        var data = {
            Name: Name,
            Email: Email,
            Address: Address,
            Phone: Phone,
            Pwd: Pwd
        }

        const apiUrl = '/api/dash/createUser';

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
                if (jsonObject.ok) {
                    document.getElementById("createUserError").style.display = "none";
                    alert("User created successfully!");
                    loadView("dash");
                }
                else {
                    document.getElementById("createUserError").style.display = "block";
                }
            })
            .catch(error => {
                console.error('Fetch error: ', error);
            });
    }
}


// check passwords match before sending pwd reset request for logged in user
function resetMyPwd()
{
    var newPwd = document.getElementById("NewPwd").value;

    if (notNullOrEmpty(newPwd)) {
        document.getElementById('pwdError').style.display = "none";
        var data = {
            UserID: 0,
            Name: "a",
            Email: "a@a",
            Address: "a",
            Phone: 0123456789,
            Pwd: newPwd
        }

        const apiUrl = '/api/dash/pwdReset';

        const headers = {
            'Content-Type': 'application/json', // specify content type as JSON
        };

        const requestOptions = {
            method: 'PUT',
            headers: headers,
            body: JSON.stringify(data)
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
                    document.getElementById("pwdError").style.display = "none";
                    alert("Password changed successfully!");
                    loadView("dash");
                }
                else {
                    document.getElementById("pwdError").style.display = "block";
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


// get request for user profile information based on supplied username
function searchUser() {
    var username = document.getElementById("editSearch").value;

    if (notNullOrEmpty(username)) {
        document.getElementById("searchUserError").style.display = "none";

        const apiUrl = '/api/dash/searchUser/' + username;

        const headers = {
            'Content-Type': 'application/json', // specify content type as JSON
        };

        const requestOptions = {
            method: 'GET',
            headers: headers
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
                    document.getElementById("searchUserError").style.display = "none";
                    loadView("dash");
                }
                else {
                    document.getElementById("searchUserError").style.display = "block";
                }
            })
            .catch(error => {
                console.error('Fetch error: ', error);
            });
    }
    else {
        document.getElementById("searchUserError").style.display = "block";
    }
}


// returns true if email is valid
function validEmail(email) {
    return (email !== null) && (email !== "") && (email.indexOf('@') === email.lastIndexOf('@')) && !email.startsWith('@') && !email.endsWith('@');
}

// returns true if string is not null or the empty string
function notNullOrEmpty(data) {
    return (data !== null) && (data !== "");
}


document.addEventListener("DOMContentLoaded", loadView);