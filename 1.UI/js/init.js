function init(){
    initDropdownListener();
    initTableRowListener();
}

function initDropdownListener() {
    // add event listener to all dropdown items
    var dropdownItems
         = document.getElementsByClassName("dropdown-item");
    var dropdownContainers
         = document.getElementsByClassName("dropdown-container");
    
    for (let i = 0; i < dropdownItems.length; i++) {
        dropdownItems[i].setAttribute('onclick', 
                                'itemSelect(this)')
    }

    for (let i = 0; i < dropdownContainers.length; i++) {
        dropdownContainers[i].setAttribute('onclick',
                                'showDropData(this)');
    }
}

function initTableRowListener(){
    var rows = document.querySelectorAll(".table-employee tbody > tr");

    console.log(rows)

    for(let i = 0; i < rows.length; i++){
        rows[i].setAttribute('onclick', 'openPopup(this)');
    }
}