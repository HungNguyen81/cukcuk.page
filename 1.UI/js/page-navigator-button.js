function ChangePageNumber(button) {
    var currentPageNumber = Number(button.innerText);
    // document.querySelector('.navigator-center').setAttribute('currentpage', currentPageNumber);
    localStorage['currentpage'] = currentPageNumber;

    FillTableData(GetPageSizeDefault(), currentPageNumber);
    UpdateCurrentPageButton();
    ChangeCurrentPageLabel(GetPageSizeDefault(), currentPageNumber);
    UpdatePagingBar();
}

function NextPage(btn) {
    var pagingBar = btn.parentNode;
    var currentPageNumber = Number(localStorage['currentpage']);
    var pageSize = GetPageSizeDefault();

    currentPageNumber++;

    if (currentPageNumber <= Number(pagingBar.getAttribute('numofpages'))) {
        FillTableData(pageSize, currentPageNumber);
        ChangeCurrentPageLabel(pageSize, currentPageNumber);
        // pagingBar.setAttribute('currentpage', currentPageNumber);
        localStorage['currentpage'] = currentPageNumber;
        UpdateCurrentPageButton();
    }
}

function PrevPage(btn) {
    var pagingBar = btn.parentNode;
    // var currentPageNumber = Number(pagingBar.getAttribute('currentpage'));
    var currentPageNumber = Number(localStorage['currentpage']);
    var pageSize = GetPageSizeDefault();

    currentPageNumber--;

    if (currentPageNumber > 0) {
        FillTableData(pageSize, currentPageNumber);
        ChangeCurrentPageLabel(pageSize, currentPageNumber);
        // pagingBar.setAttribute('currentpage', currentPageNumber);
        localStorage['currentpage'] = currentPageNumber;
        UpdateCurrentPageButton();
    }
}

function FirstPage() {
    // ChangePageNumber(document.querySelector('.navigator-center').children[2].children[0]);
    localStorage['currentpage'] = 1;
    
}

function LastPage() {
    var tmp = document.querySelector('.navigator-center').children[2].children;
    ChangePageNumber(tmp[tmp.length - 1]);
}

// call when select another page size
function ChangePageSize(item) {
    ItemSelect(item);
    var pageSize = item.children[1].innerText.split(' ')[0];

    ChangeCurrentPageLabel(pageSize, 1);

    GetNumberOfEmployees();
    FillTableData(pageSize, 1);
    UpdatePagingBar();

    // document.querySelector('.navigator-center').setAttribute('currentpage', 1);
    localStorage['currentpage'] = 1;
    UpdateCurrentPageButton();
}

function UpdateCurrentPageButton() {
    // var pagingBar = document.querySelector('.navigator-center');
    var currentPage = Number(localStorage['currentpage']); //Number(pagingBar.getAttribute('currentpage'));

    // for(let i = 2; i < pagingBar.children.length-2; i++){
    //     let child = pagingBar.children[i];
    //     if(child.classList.contains('button-current-page')){
    //         child.classList.toggle('button-current-page');
    //     }
    //     if(i == currentPage + 1){
    //         child.classList.toggle('button-current-page');
    //     }
    // }

    // New paging bar
    $('.page-buttons > div').each(function (index) {
        console.log($(this).attr('class').split(' '));
        if ($(this).attr('class').split(' ').includes('button-current-page')) {
            $(this).removeClass('button-current-page');
        }
        console.log("this", $(this).text())
        if ($(this).text() == currentPage) {
            $(this).addClass('button-current-page');
        }
    })

    // pagingBar.children[currentPage + 1].classList.add('button-current-page');
}

function ChangeCurrentPageLabel(pageSize, pageNumber) {
    var numofrows = Number($('.navigator-center').attr('numberofrecords'));
    var start = pageSize * (pageNumber - 1) + 1;
    var end = (pageSize * pageNumber < numofrows) ? pageSize * pageNumber : numofrows;

    $('#current-pagesize').html(`Hiển thị <b>${start}-${end}/${numofrows}</b> nhân viên`);
}

// call this funtion when page size has changed
function UpdatePagingBar() {
    var pagingBar = document.querySelector('.navigator-center');
    var pageBtns = document.querySelector('.page-buttons');

    var numOfRecords = Number(pagingBar.getAttribute('numberofrecords'));
    var pageSize = GetPageSizeDefault();

    var numOfPages = (numOfRecords % pageSize != 0) ? Math.floor(numOfRecords / pageSize) + 1
        : Math.floor(numOfRecords / pageSize);

    pagingBar.setAttribute('numofpages', numOfPages);

    // Remove all page btn (child of paging bar from index 3 to number-of-children - 3)

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
            var newBtnNode = document.createElement('div');
            newBtnNode.classList.add('button-page-number');
            if(i+1 == currentPage) newBtnNode.classList.add('button-current-page');
            newBtnNode.innerText = i + 1;
            newBtnNode.setAttribute('onclick', 'ChangePageNumber(this)');
            pageBtns.insertBefore(newBtnNode, pageBtns.children[i]);
        }
    } else {
        for (let i = currentPage-5; i < Math.min(numOfPages, 9); i++) {
            var newBtnNode = document.createElement('div');
            newBtnNode.classList.add('button-page-number');
            if(i+1 == currentPage) newBtnNode.classList.add('button-current-page');
            newBtnNode.innerText = i + 1;
            newBtnNode.setAttribute('onclick', 'ChangePageNumber(this)');
            pageBtns.insertBefore(newBtnNode, pageBtns.children[i]);
        }
    }

    // for (let i = 1; i < numOfPages; i++) {
    //     var newBtnNode = document.createElement('div');
    //     newBtnNode.classList.add('button-page-number');
    //     newBtnNode.innerText = i + 1;
    //     newBtnNode.setAttribute('onclick', 'ChangePageNumber(this)');
    //     pageBtns.insertBefore(newBtnNode, pageBtns.children[i]);
    // }
}
