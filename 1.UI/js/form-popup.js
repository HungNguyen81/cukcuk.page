var form = document.getElementById("container");

function openPopup(row) {
    form.style.display = "block";
    form.addEventListener('click', myFunc);    

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

function initForm(row) {
    console.log("init form");
}