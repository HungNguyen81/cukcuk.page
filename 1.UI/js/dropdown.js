
function showDropData(container) {
    var data = container.childNodes;
    var dropData = data[3];
    var dropIcon = data[1].childNodes[3];

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

    for (let i = 0; i < parent.children.length; i++) {
        let child = parent.children[i];        
        child.classList.remove("item-selected");        
    }

    item.classList.add("item-selected");

    var displayText = parent.parentNode.children[0].children[0];
    displayText.innerText = item.children[1].innerText;
}