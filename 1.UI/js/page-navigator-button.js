function ChangePageNumber(button){
    var currentPageNumber = button.innerText;
    document.querySelector('.navigator-center').setAttribute('currentpage', currentPageNumber);

    FillTableData( GetPageSizeDefault(), currentPageNumber );
    UpdateCurrentPageButton();
}

function NextPage(btn){
    var pagingBar = btn.parentNode;
    var currentPageNumber = Number(pagingBar.getAttribute('currentpage'));
    var pageSize = GetPageSizeDefault();

    currentPageNumber++;

    if(currentPageNumber <= Number(pagingBar.getAttribute('numofpages'))){
        FillTableData(pageSize, currentPageNumber);
        ChangeCurrentPageLabel(pageSize, currentPageNumber);
        pagingBar.setAttribute('currentpage', currentPageNumber);
        UpdateCurrentPageButton();
    }
}

function PrevPage(btn){
    var pagingBar = btn.parentNode;
    var currentPageNumber = Number(pagingBar.getAttribute('currentpage'));
    var pageSize = GetPageSizeDefault(); 

    currentPageNumber--;

    if(currentPageNumber > 0){
        FillTableData(pageSize, currentPageNumber);
        ChangeCurrentPageLabel(pageSize, currentPageNumber);
        pagingBar.setAttribute('currentpage', currentPageNumber);
        UpdateCurrentPageButton();
    }
}

function FirstPage(){
    ChangePageNumber(document.querySelector('.navigator-center').children[2]);
}

function LastPage(){
    var tmp = document.querySelector('.navigator-center').children;
    ChangePageNumber(tmp[tmp.length-3]);
}

// call when select another page size
function ChangePageSize(item){
    ItemSelect(item);
    var pageSize = item.children[1].innerText.split(' ')[0];
    // var pageNumber = pagingBar.getAttribute('currentpage');

    ChangeCurrentPageLabel(pageSize, 1);

    FillTableData(pageSize, 1);
    UpdatePagingBar();

    document.querySelector('.navigator-center').setAttribute('currentpage', 1);
    UpdateCurrentPageButton();
}

// reset current-page button to button 1
function UpdateCurrentPageButton(){
    var pagingBar = document.querySelector('.navigator-center');
    var currentPage = Number(pagingBar.getAttribute('currentpage'));

    for(let i = 2; i < pagingBar.children.length-2; i++){
        let child = pagingBar.children[i];
        if(child.classList.contains('button-current-page')){
            child.classList.toggle('button-current-page');
        }
        if(i == currentPage + 1){
            child.classList.toggle('button-current-page');
        }
    }
    // pagingBar.children[currentPage + 1].classList.add('button-current-page');
}

function ChangeCurrentPageLabel(pageSize, pageNumber){
    var numofrows   = Number($('.navigator-center').attr('numberofrecords'));
    var start       = pageSize*(pageNumber-1)+1;
    var end         = (pageSize*pageNumber < numofrows)? pageSize*pageNumber : numofrows;

    $('#current-pagesize').html(`Hiển thị <b>${start}-${end}/${numofrows}</b> nhân viên`);         
}

// call this funtion when page size has changed
function UpdatePagingBar(){
    var pagingBar = document.querySelector('.navigator-center');
    
    var numOfRecords = Number(pagingBar.getAttribute('numberofrecords'));    
    var pageSize = GetPageSizeDefault();

    var numOfPages = (numOfRecords % pageSize != 0)?   Math.floor(numOfRecords / pageSize) + 1
                                                     : Math.floor(numOfRecords/ pageSize);

    pagingBar.setAttribute('numofpages', numOfPages);

    // Remove all page btn (child of paging bar from index 3 to number-of-children - 3)
    for(let i = 3; i < pagingBar.children.length; i++){
        let child = pagingBar.children[i];
        if(child.classList.contains('button-page-number')){
            pagingBar.removeChild(child);
            i--;
        }
    }

    // add new buttons
    for(let i = 1; i < numOfPages; i++){
        var newBtnNode = document.createElement('div');
        newBtnNode.classList.add('button-page-number');
        newBtnNode.innerText = i + 1;
        newBtnNode.setAttribute('onclick','ChangePageNumber(this)');

        pagingBar.insertBefore(newBtnNode, pagingBar.children[i+2]);
    }
}
