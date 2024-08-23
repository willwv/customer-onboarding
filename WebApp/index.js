document.addEventListener('DOMContentLoaded', function () {

    const loginBtn = document.getElementById('login-btn');
    loginBtn.addEventListener('click', function () {
        var email = document.getElementById('email-inpt').value;
        var password = document.getElementById('password-inpt').value;

        fetch('http://localhost:8080/api/v1/auth', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify({
                Email: email,
                Password: password
            })
        })
            .then(response => response.ok ? response.json() : null)
            .then(data => {
                if (data != null) {
                    sessionStorage.setItem('userData', JSON.stringify(data.user));
                    sessionStorage.setItem('jwtToken', JSON.stringify(data.jwtToken));
                    sessionStorage.setItem('currentRole', JSON.stringify(data.user.role));

                    if (data.user.role == 'sys_user') {
                        window.location.href = '/pages/users/userDetails.html'
                    }
                    else if (data.user.role == 'sys_admin') {
                        window.location.href = '/pages/users/users.html'
                    }

                }
            })
            .catch((error) => {

            });
    });

    const signupBtn = document.getElementById('signup-btn');
    signupBtn.addEventListener('click', function () {
        var email = document.getElementById('email-sugnup').value;
        var name = document.getElementById('name-sugnup').value;
        var password = document.getElementById('password-sugnup').value;
        const fileInput = document.getElementById('uploadInput');
        let file = fileInput.files[0];

        if (!email) {
            alert("Preencha o email!");
        }
        if (!name) {
            alert("Preencha o Nome!");
        }
        if (!password) {
            alert("Preencha a senha!");
        }
        if (!file) {
            alert("Insira uma imagem!");
        }

        if (email && password && file && name) {

            let formData = new FormData();
            formData.append('Email', email);
            formData.append('Password', password);
            formData.append('Name', name);
            formData.append('Logo', file);


            fetch('http://localhost:8080/api/v1/users', {
                method: 'POST',
                headers: {},
                body: formData
            })
                .then(response => response.ok ? response.json() : null)
                .then(data => {
                    if (data != null) {
                        sessionStorage.setItem('userData', JSON.stringify(data.user));
                        sessionStorage.setItem('jwtToken', JSON.stringify(data.jwtToken));

                        if (data.user.role == 'sys_user') {
                            window.location.href = '/pages/users/userDetails.html'
                        }
                        else if (data.user.role == 'sys_admin') {
                            window.location.href = '/pages/users/users.html'
                        }

                    }
                })
                .catch((error) => {

                });
        }
    });
    let uploadInput = document.getElementById('uploadInput');
    uploadInput.addEventListener('change', function (event) {
        const userLogo = document.getElementById('user-logo');
        const file = event.target.files[0];

        if (file) {
            const reader = new FileReader();

            reader.onload = function (e) {
                userLogo.src = e.target.result;
            }

            reader.readAsDataURL(file);
        }

    });
});