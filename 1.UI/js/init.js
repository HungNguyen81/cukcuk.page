function Init() {
    document.getElementById('loader').removeAttribute('hidden');
    // initialize data
    GetDepartments();
    GetPositions();

    // initialize event listeners
    InitDropdownListener();
    InitTableTitles();
    InitTableData();

    // init listener for paging-item
    InitPagingSelect();

    // Init listener for page number buttons
    InitPagingBtnListener();

    localStorage['deletelist'] = '[]';

    const closeForm = function() {
        ClosePopup('Xác nhận đóng Form !', 'Bạn có chắc chắn muốn hủy nhập liệu hay không ?');
    }
    $('.form-header #close-button').on('click', closeForm);
    $('.form-footer .button-cancel').on('click', closeForm);
}

// add event listener to all dropdown items
function InitDropdownListener() {
    var dropdownItems = document.querySelectorAll('.dropdown-item');
    var dropdownContainers = document.querySelectorAll('.dropdown-container');
    var comboboxInputs = document.querySelectorAll('.combobox-input');

    for (let i = 0; i < dropdownItems.length; i++) {
        dropdownItems[i].setAttribute('onclick', 'ItemSelect(this)')
    }

    for (let i = 0; i < dropdownContainers.length; i++) {
        dropdownContainers[i].setAttribute('onclick', 'ToggleDropData(this)');
        dropdownContainers[i].setAttribute('onkeydown', 'SelectWhenPressKey(event, this)');
        dropdownContainers[i].setAttribute('index', '0');
    }

    for (let i = 0; i < comboboxInputs.length; i++) {
        comboboxInputs[i].setAttribute('onfocus', 'ComboboxInputChange(this)');
        comboboxInputs[i].setAttribute('onblur', 'HideCloseButton(this)');
    }



    function HideDropDownWhenClickOutside(e) {
        var dropdowns = document.querySelectorAll('.dropdown-data');
        for (let i = 0; i < dropdowns.length; i++) {
            if (!dropdowns[i].contains(e.target) && !dropdowns[i].parentNode.contains(e.target)) {

                var dropIcon;
                if (dropdowns[i].parentNode.classList.contains('combobox-container')) {
                    dropIcon = dropdowns[i].parentNode.children[0].children[2].children[0];
                } else {
                    dropIcon = dropdowns[i].parentNode.children[0].children[1];
                }

                dropdowns[i].setAttribute("hidden", true);
                dropIcon.style.webkitTransitionDuration = "0.2s";
                dropIcon.style.webkitTransform = 'rotate(0deg)';
            }
        }
    }
    document.addEventListener('click', HideDropDownWhenClickOutside)
}

// // add event listener to all table data rows
function InitTableRowListener() {
    var rows = document.querySelectorAll(".table-employee tbody > tr");

    for (let i = 0; i < rows.length; i++) {
        rows[i].setAttribute('ondblclick', 'OpenPopup(this)');
        rows[i].setAttribute('onclick', 'SelectTableRow(this)');
    }

    var checkboxes = document.querySelectorAll("input[name=checkbox]");

    for (let i = 0; i < checkboxes.length; i++) {
        var checkbox = checkboxes[i]
        checkbox.addEventListener('change', function () {
            if (this.checked) {
                console.log("Checkbox is checked..");
                checkbox.parentNode.style.backgrounColor = '#EBF4FF';
            } else {
                console.log("Checkbox is not checked..");
            }
        });
    }
}

// Initialize title attribute for each table cell
function InitTableTitles() {
    var tableHeaders = $("th");
    var tableData = $("td");

    // header title
    tableHeaders.each(function (i) {
        $(this).attr('title', tableHeaders[i].innerText);
    })

    // data title
    tableData.each(function (i) {
        $(this).setAttribute('title', tableData[i].innerText);
    })
}

function InitPagingSelect() {
    var pagingItems = $('.paging-item');
    pagingItems.each(function () {
        $(this).attr('onclick', 'ChangePageSize(this)')
    })
}

function InitPagingBtnListener() {
    var pagingButtons = $('.button-page-number');
    var btnFirstPage = $('.button-firstpage');
    var btnLastPage = $('.button-lastpage');
    var btnNext = $('.button-next-page');
    var btnPrev = $('.button-prev-page');

    pagingButtons.each(function () {
        $(this).attr('onclick', 'ChangePageNumber(this)');
    });

    btnFirstPage.click(FirstPage);
    btnLastPage.click(LastPage);
    btnNext.click(NextPage);
    btnPrev.click(PrevPage);
}