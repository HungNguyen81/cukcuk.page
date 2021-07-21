function Init() {
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
}

// add event listener to all dropdown items
function InitDropdownListener() {
    var dropdownItems = document.getElementsByClassName("dropdown-item");
    var dropdownContainers = document.getElementsByClassName("dropdown-container");

    for (let i = 0; i < dropdownItems.length; i++) {
        dropdownItems[i].setAttribute('onclick', 'ItemSelect(this)')
    }

    for (let i = 0; i < dropdownContainers.length; i++) {
        dropdownContainers[i].setAttribute('onclick', 'ShowDropData(this)');
    }
}

// // add event listener to all table data rows
function InitTableRowListener() {
    var rows = document.querySelectorAll(".table-employee tbody > tr");

    for (let i = 0; i < rows.length; i++) {
        rows[i].setAttribute('ondblclick', 'OpenPopup(this)');
        rows[i].setAttribute('onclick', 'SelectTableRow(this)');
    }

    var checkboxes = document.querySelectorAll("input[name=checkbox]");

    for(let i = 0; i < checkboxes.length; i++){
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