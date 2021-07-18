// show list of item when click
function showDropData(container) {
    var data, dropData, dropIcon;

    // if container is a combobox
    if(container.classList.contains("combobox-icon-container")){  
               
        data = container.parentNode.parentNode.childNodes;
        dropIcon = data[1].childNodes[5].childNodes[1];
    } else {        
        data = container.childNodes;
        dropIcon = data[1].childNodes[3];
    }
    dropData = data[3];

    // toggle hidden display status
    if (dropData.getAttribute("hidden")) {
        dropData.removeAttribute("hidden");
        dropIcon.style.webkitTransitionDuration = "0.2s";
        dropIcon.style.webkitTransform = 'rotate(180deg)';
    } else {
        dropData.setAttribute("hidden", true);
        dropIcon.style.webkitTransitionDuration = "0.2s";
        dropIcon.style.webkitTransform = 'rotate(0deg)';
    }
}

// handle when an item is selected
function itemSelect(item) {
    var parent = item.parentNode;

    // remove all item-selected class, make all item as non-selected
    for (let i = 0; i < parent.children.length; i++) {
        let child = parent.children[i];        
        child.classList.remove("item-selected");        
    }

    // add class to selected item 
    item.classList.add("item-selected");

    // change display text content after selection
    var displayText = parent.parentNode.children[0].children[0];

    if(parent.parentNode.classList.contains("combobox-container")){
        displayText.value = item.children[1].innerText;

        // rotate drop icon and hide drop data
        showDropData(parent.parentNode.children[0].children[2])
    } else {
        displayText.innerText = item.children[1].innerText;
    }
}

// Handle when text input not empty, then set x button not hidden
function ComboboxInputChange(input){
    var parent = input.parentNode;

    var xIcon = parent.children[1];

    if(input.value == ""){
        xIcon.setAttribute('hidden', true);
    } else {
        xIcon.removeAttribute('hidden');
    }
}

// Clear input when click x button
function ClearInputText(icon){
    var parent = icon.parentNode;
    var input  = parent.children[0];
    input.value = "";
    icon.setAttribute('hidden', true);   
}