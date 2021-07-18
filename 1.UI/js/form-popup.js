var form = document.getElementById("container");

function openPopup(row) {
    form.style.display = "block";
    form.addEventListener('click', myFunc);    

    var employeeIdInput =   form.children[0]    
                                .children[1]
                                .children[1]
                                .children[0]
                                .children[1]
                                .children[1];
    console.log(employeeIdInput);

    employeeIdInput.focus();

    if(row){        
        initForm();
    }
}

function closePopup() {
    form.style.display = "none";
    form.removeEventListener('click', myFunc);
}

function myFunc(e) {
    if (!document.getElementById('form-container').contains(e.target)) {
        closePopup();
    }
}

// Get row data and fill to the form
function initForm(row) {
    console.log("init form");
}