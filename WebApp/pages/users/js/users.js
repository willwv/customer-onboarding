document.addEventListener('DOMContentLoaded', function () {
    let users = [];

    function getAllUsersFromPage(page){
        let jwtToken = JSON.parse(sessionStorage.getItem('jwtToken'));        
        
        fetch('http://localhost:8080/api/v1/users?page=' + page, {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + jwtToken
            }
        })
        .then(response => response.ok ? response.json() : null)
        .then(data => {

            if(data != null)
            {
                if(data.users){
                    data.users.forEach(usr => users.push(usr));
                    sessionStorage.setItem('users', JSON.stringify(users));

                    clearAllUsersData();
                    fillPageWithUsers(data.users);
                    fillPageController(data.totalPages);
                }
            }
        })
        .catch((error) => {
            
        });
    }

    function clearAllUsersData(){
        let elements = document.getElementsByClassName('user-info');

        for (let i = elements.length - 1; i >= 0; i--) {
            elements[i].parentNode.removeChild(elements[i]);
        }
    }

    function fillPageController(totalPages){
        let pages = document.getElementById('pages');

        if(pages){
            pages.remove();
        }

        let newPages = document.createElement('div');
        newPages.id = 'pages';

        for(i=0; i< totalPages; i++){
            let btn = document.createElement('button');
            btn.classList.add('page-number');
            btn.innerText = i;
            btn.onclick = (event) => onPageNumberClickHandler(event );
            newPages.append(btn);
        }

        let pagesGroup = document.getElementById('pages-group');
        pagesGroup.append(newPages);
    }

    function onPageNumberClickHandler(event){
        let pageNumber = event.target.innerText;
        getAllUsersFromPage(pageNumber);
    }

    function handleOnClickEditUser(event){
        let userInfo = event.target.closest('.user-info');
        let userInput = userInfo.querySelector('.user-inputs');
        let userId = userInput.querySelector('.user-id').innerText;

        let userFinded = users.find(obj => obj.id == userId);

        sessionStorage.setItem('userData', JSON.stringify(userFinded));

        window.location.href = '/pages/users/userDetails.html';
    }

    function fillPageWithUsers(users){
        let usersGroup = document.getElementById('users-group');

        users.forEach(user => {
            let userInfo = document.createElement('div');
            userInfo.classList.add('user-info');
    
            let userData = document.createElement('div');
            userData.classList.add('user-data');
    
            let logoContainer = document.createElement('div');
            let userLogo = document.createElement('img');
            userLogo.classList.add('user-logo');
            userLogo.src = "data:image/png;base64,"+ user.logo;;
            logoContainer.append(userLogo);
    
            let userInputs = document.createElement('div');
            userInputs.classList.add('user-inputs');
            let userId = document.createElement('p');
            userId.classList.add('user-id');
            userId.innerText = user.id;
            let userName = document.createElement('p');
            userName.classList.add('user-name');
            userName.innerText = user.name;
            let userEmail = document.createElement('p');
            userEmail.classList.add('user-email');
            userEmail.innerText = user.email;
            let userRole = document.createElement('p');
            userRole.classList.add('user-role');
            userRole.innerText = user.role;
            userInputs.append(userId);
            userInputs.append(userName);
            userInputs.append(userEmail);
            userInputs.append(userRole);
    
            userData.append(logoContainer);
            userData.append(userInputs);
    
            let editBtn = document.createElement('button');
            editBtn.classList.add('edit-btn');
            editBtn.classList.add('primary-btn');
            editBtn.innerText = 'Editar'
            editBtn.onclick = (event) => handleOnClickEditUser(event);

    
            userInfo.append(userData);
            userInfo.append(editBtn);
    
            usersGroup.append(userInfo);
        });
    }

    getAllUsersFromPage(0);
});