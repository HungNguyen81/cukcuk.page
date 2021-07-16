var form = document.getElementById("container");

function openPopup() {
    form.style.display = "block";
    form.addEventListener('click', myFunc);    
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