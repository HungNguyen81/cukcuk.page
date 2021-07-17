function init(){
    // initialize data
    GetDepartments();
    GetPositions();

    // initialize event listeners
    initDropdownListener();
    initTableRowListener();
    initTableTitles();    
}

// add event listener to all dropdown items
function initDropdownListener() {    
    var dropdownItems      = document.getElementsByClassName("dropdown-item");
    var dropdownContainers = document.getElementsByClassName("dropdown-container");
    
    for (let i = 0; i < dropdownItems.length; i++) {
        dropdownItems[i].setAttribute('onclick', 'itemSelect(this)')
    }

    for (let i = 0; i < dropdownContainers.length; i++) {
        dropdownContainers[i].setAttribute('onclick', 'showDropData(this)');
    }
}

// // add event listener to all table data rows
function initTableRowListener(){
    var rows = document.querySelectorAll(".table-employee tbody > tr");    

    for(let i = 0; i < rows.length; i++){
        rows[i].setAttribute('ondblclick', 'openPopup(this)');
    }
}

// Initialize title attribute for each table cell
function initTableTitles(){
    var tableHeaders = document.querySelectorAll("th");
    var tableData    = document.querySelectorAll("td");

    // header title
    for(let i = 0; i < tableHeaders.length; i++){ 
        tableHeaders[i].setAttribute('title', tableHeaders[i].innerText);
    }

    // data title
    for(let i = 0; i < tableData.length; i++){    
        tableData[i].setAttribute('title', tableData[i].innerText);
    }
}