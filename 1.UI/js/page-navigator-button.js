function ChangePageNumber(button) {
    var currentPageNumber = Number(button.innerText);
    // document.querySelector('.navigator-center').setAttribute('currentpage', currentPageNumber);
    localStorage['currentpage'] = currentPageNumber;

    FillTableData(GetPageSizeDefault(), currentPageNumber);
    // UpdateCurrentPageButton();
    ChangeCurrentPageLabel(GetPageSizeDefault(), currentPageNumber);
    UpdatePagingBar();
}

function NextPage() {
    var currentPage = Number(localStorage['currentpage']);
    var numOfPages = Number(localStorage['numofpages']);

    if (currentPage >= numOfPages) return;

    currentPage++;
    localStorage['currentpage'] = currentPage;
    UpdatePagingBar()
    FillTableData(GetPageSizeDefault(), currentPage);
    ChangeCurrentPageLabel(GetPageSizeDefault(), currentPage);
}

function PrevPage() {
    var currentPage = Number(localStorage['currentpage']);
    if (currentPage == 1) return;

    currentPage--;
    localStorage['currentpage'] = currentPage;
    UpdatePagingBar()
    FillTableData(GetPageSizeDefault(), currentPage);
    ChangeCurrentPageLabel(GetPageSizeDefault(), currentPage);
}

function FirstPage() {
    localStorage['currentpage'] = 1;
    UpdatePagingBar();
    FillTableData(GetPageSizeDefault(), 1);
    ChangeCurrentPageLabel(GetPageSizeDefault(), 1);
}

function LastPage() {
    var number = Number(localStorage['numofpages']);
    localStorage['currentpage'] = number;
    UpdatePagingBar();
    FillTableData(GetPageSizeDefault(), number);
    ChangeCurrentPageLabel(GetPageSizeDefault(), number);
}

// call when select another page size
function ChangePageSize(item) {
    ItemSelect(item);

    var pageSize = Number(item.children[1].innerText.split(' ')[0]);

    localStorage['currentpage'] = 1;
    GetNumberOfEmployees(function () {
        ChangeCurrentPageLabel(pageSize, 1);
        FillTableData(pageSize, 1);
        UpdatePagingBar();
    });
}

function UpdateCurrentPageButton() {
    var currentPage = Number(localStorage['currentpage']);

    // New paging bar
    $('.page-buttons > div').each(function (index) {
        ;
        if ($(this).attr('class').split(' ').includes('button-current-page')) {
            $(this).removeClass('button-current-page');
        }
        if ($(this).text() == currentPage) {
            $(this).addClass('button-current-page');
        }
    })
}

function ChangeCurrentPageLabel(pageSize, pageNumber) {
    var numofrows = Number(localStorage['numofemployees']);
    var start = pageSize * (pageNumber - 1) + 1;
    var end = (pageSize * pageNumber < numofrows) ? pageSize * pageNumber : numofrows;

    $('#current-pagesize').html(`Hiển thị <b>${start}-${end}/${numofrows}</b> nhân viên`);
}

// call this funtion when page size has changed
function UpdatePagingBar() {
    var pageBtns = document.querySelector('.page-buttons');

    var numOfRecords = Number(localStorage['numofemployees']);
    var pageSize = GetPageSizeDefault();

    var numOfPages = (numOfRecords % pageSize != 0) ? Math.floor(numOfRecords / pageSize) + 1
        : Math.floor(numOfRecords / pageSize);

    localStorage['numofpages'] = numOfPages;

    // Remove all page buttons
    for (let i = 0; i < pageBtns.children.length; i++) {
        let child = pageBtns.children[i];
        if (child.classList.contains('button-page-number')) {
            pageBtns.removeChild(child);
            i--;
        }
    }

    // add new buttons
    var currentPage = Number(localStorage['currentpage']);
    if (currentPage < 6) {
        for (let i = 0; i < Math.min(numOfPages, 9); i++) {
            pageBtns.insertBefore(CreateNewPageButton(i + 1, currentPage), pageBtns.children[i]);
        }
    } else {
        let from = currentPage - 5;
        let to = Math.min(numOfPages, currentPage + 4);

        for (let i = from; i < to; i++) {
            pageBtns.insertBefore(CreateNewPageButton(i + 1, currentPage), pageBtns.children[i]);
        }
    }
}

function CreateNewPageButton(num, currentPage) {
    var newBtnNode = document.createElement('div');
    newBtnNode.classList.add('button-page-number');
    if (num == currentPage) newBtnNode.classList.add('button-current-page');
    newBtnNode.innerText = num;
    newBtnNode.setAttribute('onclick', 'ChangePageNumber(this)');
    return newBtnNode;
}
