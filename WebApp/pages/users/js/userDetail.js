document.addEventListener('DOMContentLoaded', function () {
    var jwtToken = JSON.parse(sessionStorage.getItem('jwtToken'));

    var backToListBtn = document.getElementById('users-btn');    
    backToListBtn.addEventListener('click', function (){
        window.location.href = '/pages/users/users.html'
    });

    let logoutBtn = document.getElementById('logout-btn');    
    logoutBtn.addEventListener('click', function (){
        handleUnauthorizedRequest();
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

    let updateUserBtn = document.getElementById('update-user');

    updateUserBtn.addEventListener('click', function (event) {
        const fileInput = document.getElementById('uploadInput');
        var userData = JSON.parse(sessionStorage.getItem('userData'));
        
        let formData = new FormData();
        let file = fileInput.files[0];

        if (file) {
            formData.append('Logo', file);
        }
        else{
            formData.append('Logo',  base64ToBlob(userData.logo));
        }

        let userName = document.getElementById('user-name').value;
        formData.append('Name', userName);

        fetch('http://localhost:8080/api/v1/users/' + userData.id, {
            method: 'PUT',
            headers: {                
                'Authorization': 'Bearer ' + jwtToken
            },
            body: formData
        })
            .then(response => response.ok ? response.json() : null)
            .then(data => {

                if (data != null) {
                    alert("Usuário atualizado com sucesso!");
                    sessionStorage.setItem('userData', JSON.stringify(data));
                }
            })
            .catch((error) => {

            });

    });

    function base64ToBlob(base64) {
        const sliceSize = 512;
        const byteCharacters = atob(base64);
        const byteArrays = [];
    
        for (let offset = 0; offset < byteCharacters.length; offset += sliceSize) {
            const slice = byteCharacters.slice(offset, offset + sliceSize);
    
            const byteNumbers = new Array(slice.length);
            for (let i = 0; i < slice.length; i++) {
                byteNumbers[i] = slice.charCodeAt(i);
            }
    
            const byteArray = new Uint8Array(byteNumbers);
            byteArrays.push(byteArray);
        }
    
        var blob = new Blob(byteArrays, { type: 'image/png' });

        const file = new File([blob], 'image.png', { type: 'image/png'});

        return file;
    }

    function base64ToBytes() {
        const img = document.getElementById('user-logo');
    
        // Cria um canvas para obter a imagem como base64
        const canvas = document.createElement('canvas');
        const ctx = canvas.getContext('2d');
        
        // Define o tamanho do canvas para a imagem
        canvas.width = img.width;
        canvas.height = img.height;
        
        // Desenha a imagem no canvas
        ctx.drawImage(img, 0, 0);
        
        // Obtém a imagem como base64
        const base64Image = canvas.toDataURL('image/jpeg');
        
        // Converte a base64 para Blob
        const byteString = atob(base64Image.split(',')[1]);
        const mimeString = base64Image.split(',')[0].split(':')[1].split(';')[0];
        
        const arrayBuffer = new ArrayBuffer(byteString.length);
        const uint8Array = new Uint8Array(arrayBuffer);
        
        for (let i = 0; i < byteString.length; i++) {
            uint8Array[i] = byteString.charCodeAt(i);
        }
        
        const blob = new Blob([uint8Array], { type: mimeString });

        return blob;
    }

    let createNewAddressBtn = document.getElementById('newaddr-btn');
    createNewAddressBtn.addEventListener('click', () => createNewAddress());

    function fillPageWithUserDetails() {
        var currentRole = JSON.parse(sessionStorage.getItem('currentRole'));
        var userData = JSON.parse(sessionStorage.getItem('userData'));
        
        if(currentRole == 'sys_user')
        {
            backToListBtn.style.display = 'none';
        }

        document.getElementById('user-name').value = userData.name;
        document.getElementById('user-email').value = userData.email;
        document.getElementById('user-id').value = userData.id;
        document.getElementById('user-role').value = userData.role;
        document.getElementById('user-logo').src = "data:image/png;base64," + userData.logo;

        fillAddresses(userData.addresses);
    }

    function fillAddresses(addresses) {

        if (addresses != null) {
            let addressesGroup = document.getElementById('addresses-group');

            addresses.forEach(addr => {
                let addrContainer = document.createElement('div');
                addrContainer.classList.add('addr-group');

                addAddressFields(addrContainer, addr.id, addr.street, addr.number, addr.neighborhood, addr.city, addr.postalCode, addr.state, addr.complement);

                addUpdateButtons(addrContainer);

                addressesGroup.append(addrContainer);
            })
        }
    }

    function addUpdateButtons(element) {
        let btnsGroup = document.createElement('div');
        btnsGroup.classList.add('addr-btns');

        let updateBtn = document.createElement('button');
        updateBtn.classList.add('primary-btn');
        updateBtn.classList.add('updateaddr-btn');
        updateBtn.innerText = 'Atualizar';
        updateBtn.onclick = (event) => onUpdateAddresBtnClick(event);

        let deleteBtn = document.createElement('button');
        deleteBtn.classList.add('secondary-btn');
        deleteBtn.classList.add('deleteaddr-btn');
        deleteBtn.innerText = 'Excluir';
        deleteBtn.onclick = (event) => onDeleteAddresBtnClick(event);

        btnsGroup.append(updateBtn);
        btnsGroup.append(deleteBtn);

        element.append(btnsGroup);
    }

    function createNewAddress() {
        let addressesGroup = document.getElementById('addresses-group');

        let addrContainer = document.createElement('div');
        addrContainer.classList.add('addr-group');

        addAddressFields(addrContainer, '', '', '', '', '', '', '', '');

        addCreateButtons(addrContainer);

        addressesGroup.append(addrContainer);
    }

    function addAddressFields(element, id, street, number, neighborhood, city, postalCode, state, complement) {
        element.append(generateAddresPorpGroup('addr-group-id', 'Id', 'addr-id', id, true));
        element.append(generateAddresPorpGroup('addr-group-street', 'Rua', 'addr-street', street, false));
        element.append(generateAddresPorpGroup('addr-group-number', 'Numero', 'addr-number', number, false));
        element.append(generateAddresPorpGroup('addr-group-neig', 'Bairro', 'addr-neig', neighborhood, false));
        element.append(generateAddresPorpGroup('addr-group-city', 'Cidade', 'addr-city', city, false));
        element.append(generateAddresPorpGroup('addr-group-state', 'Estado', 'addr-state', postalCode, false));
        element.append(generateAddresPorpGroup('addr-group-postal', 'CEP', 'addr-postal', state, false));
        element.append(generateAddresPorpGroup('addr-group-complement', 'Complemento', 'addr-complement', complement, false));
    }

    function addCreateButtons(element) {
        let btnsGroup = document.createElement('div');
        btnsGroup.classList.add('addr-btns');

        let createBtn = document.createElement('button');
        createBtn.classList.add('primary-btn');
        createBtn.classList.add('createaddr-btn');
        createBtn.innerText = 'Criar';
        createBtn.onclick = (event) => onCreateAddresBtnClick(event);

        let removeBtn = document.createElement('button');
        removeBtn.classList.add('secondary-btn');
        removeBtn.classList.add('removeaddr-btn');
        removeBtn.innerText = 'Remover';
        removeBtn.onclick = (event) => onRemoveAddresBtnClick(event);

        btnsGroup.append(createBtn);
        btnsGroup.append(removeBtn);

        element.append(btnsGroup);
    }

    function onUpdateAddresBtnClick(event) {
        let group = event.target.closest('.addr-group');

        const endereco = {
            id: group.querySelector('.addr-id').value,
            rua: group.querySelector('.addr-street').value,
            numero: group.querySelector('.addr-number').value,
            bairro: group.querySelector('.addr-neig').value,
            cidade: group.querySelector('.addr-city').value,
            estado: group.querySelector('.addr-state').value,
            cep: group.querySelector('.addr-postal').value,
            complemento: group.querySelector('.addr-complement').value,
        };

        fetch('http://localhost:8080/api/v1/addresses/' + endereco.id, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + jwtToken
            },
            body: JSON.stringify({
                street: endereco.rua,
                number: endereco.numero,
                neighborhood: endereco.bairro,
                city: endereco.cidade,
                postalCode: endereco.cep,
                state: endereco.estado,
                complement: endereco.complemento,
            })
        })
            .then(response => response.ok ? response.json() : null)
            .then(data => {

                if (data != null) {
                    alert("Endreço atualizado com sucesso!");
                }
            })
            .catch((error) => {

            });
    }
    function onDeleteAddresBtnClick(event) {
        let group = event.target.closest('.addr-group');
        let addressId = group.querySelector('.addr-id').value

        fetch('http://localhost:8080/api/v1/addresses/' + addressId, {
            method: 'DELETE',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + jwtToken
            },
            body: {}
        })
            .then(response => {

                if (response.ok) {
                    alert("Endreço deletado com sucesso!");
                    group.remove();
                }
                else if (response.status >= 400 && response.status <= 499) {
                    handleUnauthorizedRequest();
                }
            })
            .catch((error) => {

            });
    }

    function onCreateAddresBtnClick(event) {
        var userData = JSON.parse(sessionStorage.getItem('userData'));

        let group = event.target.closest('.addr-group');

        const endereco = {
            rua: group.querySelector('.addr-street').value,
            numero: group.querySelector('.addr-number').value,
            bairro: group.querySelector('.addr-neig').value,
            cidade: group.querySelector('.addr-city').value,
            estado: group.querySelector('.addr-state').value,
            cep: group.querySelector('.addr-postal').value,
            complemento: group.querySelector('.addr-complement').value,
        };

        fetch('http://localhost:8080/api/v1/addresses', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
                'Authorization': 'Bearer ' + jwtToken
            },
            body: JSON.stringify({
                userId: userData.id,
                street: endereco.rua,
                number: endereco.numero,
                neighborhood: endereco.bairro,
                city: endereco.cidade,
                postalCode: endereco.cep,
                state: endereco.estado,
                complement: endereco.complemento,
            })
        })
            .then(response => response.ok ? response.json() : null)
            .then(data => {

                if (data != null) {
                    alert("Endreço criado com sucesso!");

                    group.querySelector('.addr-id').value = data.addres.id;
                    group.querySelector('.addr-btns').remove();
                    addUpdateButtons(group);
                }
            })
            .catch((error) => {

            });
    }

    function onRemoveAddresBtnClick(event) {
        const btn = event.target;
        const group = btn.closest('.addr-group');

        if (group) {
            group.remove();
        }
    }

    function generateAddresPorpGroup(groupClass, groupTitleText, inputClass, addrValue, isInputDisabled) {
        let group = document.createElement('span')
        group.classList.add(groupClass);

        let groupTitle = document.createElement('span');
        groupTitle.innerText = groupTitleText;

        let groupInput = document.createElement('input');
        groupInput.classList.add(inputClass);
        groupInput.type = "text";
        groupInput.value = addrValue;
        groupInput.disabled = isInputDisabled;

        group.append(groupTitle);
        group.append(groupInput);

        return group;
    }

    function handleUnauthorizedRequest() {
        sessionStorage.clear();
        window.location.href = '/';
    }

    fillPageWithUserDetails();
});