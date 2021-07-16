function showDropData(container) {
    var data, dropData, dropIcon;

    // if container is a combobox
    if(container.classList.contains("combobox-icon-container")){         
        data = container.parentNode.parentNode.childNodes;      
        dropIcon = data[1].childNodes[3].childNodes[1];
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
        
        // hide drop data
        parent.setAttribute('hidden', true);
    } else {
        displayText.innerText = item.children[1].innerText;
    }
}
