// handle on icon-container clicked
function ShowDropData(container) {
    // console.log("show");
    var input = container.parentNode.children[0];
    var dataFilter = input.getAttribute('filter');
    var data = JSON.parse(localStorage[`${dataFilter}`]);
    var key = dataFilter.charAt(0).toUpperCase() + dataFilter.slice(1);
    var value = 'Tất cả ' + ((dataFilter == 'position') ? 'vị trí' : 'phòng ban');

    var item = JSON.parse(`{
        "${key}Name" : "${value}"
    }`);

    if (container.classList.contains("combobox-icon-container")) {
        data.unshift(item);
    }

    FillDropdownData(data, false, `${dataFilter}s`);
    ToggleDropData(container);
}

/**
 * show list of item
 * @param {element} container : icon container
 * Hungnn
 */
function ToggleDropData(container, mode) {
    // console.log("toggle");
    var data, dropData, dropIcon;

    // if container is a combobox
    if (container.classList.contains("combobox-icon-container")) {
        data = container.parentNode.parentNode;
        dropIcon = container.children[0];
    } else {
        data = container;
        dropIcon = container.children[0].children[1];
    }
    dropData = data.children[1];

    // toggle hidden display status
    if (dropData.getAttribute("hidden")) {
        dropData.removeAttribute("hidden");
        dropIcon.style.webkitTransitionDuration = "0.2s";
        dropIcon.style.webkitTransform = 'rotate(180deg)';

        if (container.classList.contains("combobox-icon-container")) {
            container.setAttribute('onclick', 'ShowDropData(this)');
            // set selected item
            var items = dropData.children;

            for (let i = 0; i < items.length; i++) {
                var item = items[i];
                // console.log(i,item)
                if (item.children[1].innerText == container.parentNode.children[0].value) {
                    item.classList.add('item-selected');
                } else {
                    item.classList.remove('item-selected');
                }
            }
        }
    } else {
        if (mode != 'show') {
            dropData.setAttribute("hidden", true);
            dropIcon.style.webkitTransitionDuration = "0.2s";
            dropIcon.style.webkitTransform = 'rotate(0deg)';
            if (container.classList.contains("combobox-icon-container")) {
                container.setAttribute('onclick', 'ToggleDropData(this)');
            }
        }
    }
}

// handle when an item is selected
function ItemSelect(item) {
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

    if (parent.parentNode.classList.contains("combobox-container")) {
        displayText.value = item.children[1].innerText;

        DisplayFilterResult();
        // rotate drop icon and hide drop data
        ToggleDropData(parent.parentNode.children[0].children[2])
    } else {
        displayText.innerText = item.children[1].innerText;
    }
}

/**
 * on keyup and on focus
 * @param {div.button} input 
 */
// Handle when text input not empty, then set x button not hidden
function ComboboxInputChange(input) {
    var parent = input.parentNode;

    var xIcon = parent.children[1];

    if (input.value == "") {
        xIcon.setAttribute('hidden', true);
        AutoCompleteCombobox(input, true);
    } else {
        xIcon.removeAttribute('hidden');
        AutoCompleteCombobox(input);
    }
}

/**
 * On keyup event 
 * @param {input} input 
 * Hungnn (23/07/21)
 */
function AutoCompleteCombobox(input, isUpshift) {
    // console.log("auto");
    var dataFilter = input.getAttribute('filter');
    var key = dataFilter.charAt(0).toUpperCase() + dataFilter.slice(1);
    var value = 'Tất cả ' + ((dataFilter == 'position') ? 'vị trí' : 'phòng ban');
    var item = JSON.parse(`{
        "${key}Name" : "${value}"
    }`);

    $(`#${dataFilter}s`).text('');

    var data = JSON.parse(localStorage[`${dataFilter}`]);

    if (isUpshift) data.unshift(item);

    newData = [];
    for (let i = 0; i < data.length; i++) {
        if (dataFilter == 'position') {
            if (data[i].PositionName.toUpperCase().includes(input.value.toUpperCase())) {
                newData.push(data[i]);
            }
        } else {
            if (data[i].DepartmentName.toUpperCase().includes(input.value.toUpperCase())) {
                newData.push(data[i]);
            }
        }
    }

    FillDropdownData(newData, false, `${dataFilter}s`, (input.value) ? item : null); // input.value (= '' ~ = false)
    ToggleDropData(input.parentNode.children[2], 'show');
}

function DisplayFilterResult() {
    BackupEmployeesForFilter();
    var positionFilter = $('.dropdown-positions input').val().trim().toUpperCase();
    var departmentFilter = $('.dropdown-departments input').val().trim().toUpperCase();

    if (positionFilter.includes('TẤT CẢ VỊ TRÍ')) {
        positionFilter = '';
    }
    if (departmentFilter.includes('TẤT CẢ PHÒNG BAN')) {
        departmentFilter = '';
    }

    var employees = JSON.parse(localStorage['employees']);

    // clear table content
    $('.table-employee > tbody').html('');

    let result = [];
    let isFound = false;

    for(let i = 0; i < employees.length; i++){
        let e = employees[i];
        if (!positionFilter && !departmentFilter) {
            isFound = true;
            result = employees;
            break;
        } else {
            if (e.PositionName.toUpperCase().includes(positionFilter) 
            && e.DepartmentName.toUpperCase().includes(departmentFilter)) 
            {
                isFound = true;
                result.push(e);
            }
        }
    };

    if (!isFound) {
        console.log("not found");
        $('.table-employee > tbody').html('');

        localStorage['currentpage'] = 1;
        localStorage['numofemployees'] = result.length;

        ChangeCurrentPageLabel(GetPageSizeDefault(), 1);
        UpdatePagingBar();

        // Toast Message
        // ...
    } else {
        localStorage['currentpage'] = 1;
        localStorage['numofemployees'] = result.length;
        localStorage['employees'] = JSON.stringify(result);

        let pageSize = GetPageSizeDefault();

        FillTableData(pageSize, 1);
        UpdatePagingBar();
        ChangeCurrentPageLabel(pageSize, 1);        
    }
}

// Clear input when click x button
function ClearInputText(icon) {
    var parent = icon.parentNode;
    var input = parent.children[0];
    input.value = "";

    icon.setAttribute('hidden', true);
    ToggleDropData(parent.children[2]);
    input.focus();
}

function HideCloseButton(input) {
    setTimeout(function () {
        input.parentNode.children[1].setAttribute("hidden", true);
    }, 300);
}

/**
 * 
 * @param {json} data 
 * @param {bool} isAll 
 * @param {string} id 
 * @param {json} item 
 * Hungnn (23/07/21)
 *
 */
function FillDropdownData(data, isAll, id, item) {
    var elements = (isAll) ? document.querySelectorAll(`.${id}`)
        : document.querySelectorAll(`#${id}`);

    if (item) data.unshift(item);

    elements.forEach(e => {
        e.innerText = '';
        data.forEach(pos => {
            let htmlText = `<div class="dropdown-item"  onclick="ItemSelect(this)">
                                <i class="fas fa-check item-icon"></i>
                            <div class="item-text">${(!pos.PositionName) ? pos.DepartmentName : pos.PositionName}</div></div>`
            e.insertAdjacentHTML('beforeend', htmlText);
        })
    })

    return data;
}

/**
 * Select dropdown item by pressing up/down arrow key
 * @param {Event} event 
 * @param {.dropdown-container Element} dropContainer 
 * Hungnn (26/07/21)
 */
function SelectWhenPressKey(event, dropContainer){
    var name = event.key;
    var items = dropContainer.children[1].children;
    var maxIndex = items.length;
    var index = Number(dropContainer.getAttribute('index'));    

    if(name == 'Enter'){
        ToggleDropData(dropContainer);
    }
    if(!dropContainer.getAttribute("hidden")){
        if(name == 'ArrowDown'){            
            ItemSelect(items[index]);
            index = (index+1) % maxIndex;
            dropContainer.setAttribute('index', index);         
        }
        if(name == 'ArrowUp'){            
            ItemSelect(items[index]);
            index = (index == 0)? maxIndex-1 : index-1;
            dropContainer.setAttribute('index', index);         
        }
    }
}