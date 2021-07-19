var form = document.getElementById("container");

function OpenPopup(row) {
    form.style.display = "block";
    form.addEventListener('click', MyFunc);    

    var employeeIdInput =   form.children[0]    
                                .children[1]
                                .children[1]
                                .children[0]
                                .children[1]
                                .children[1];
    console.log(employeeIdInput);

    employeeIdInput.focus();

    if(row){        
        InitForm();
    }
}

function ClosePopup() {
    form.style.display = "none";
    form.removeEventListener('click', MyFunc);
}

function MyFunc(e) {
    if (!document.getElementById('form-container').contains(e.target)) {
        closePopup();
    }
}

// Get row data and fill to the form
function InitForm(row) {
    console.log("init form");
}