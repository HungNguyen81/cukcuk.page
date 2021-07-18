function initTableData() {
    var pageSize = getPageSizeDefault();
    var pageNumber = Number(document.querySelector('.button-current-page').innerText);

    GetNumberOfEmployeeRecord();
    fillTableData(pageSize, pageNumber);        
}

function GetNumberOfEmployeeRecord(){
    let xhttp = new XMLHttpRequest();

    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {

            const obj = JSON.parse(this.responseText);

            var tableEmployee = document.querySelector('.table-employee > tbody');
            tableEmployee.innerHTML = '';
            
            console.log(obj.length);
            document.querySelector('.navigator-center').setAttribute('numberofrecords', obj.length);
            UpdatePagingBar();       

            changeCurrentPageLabel( getPageSizeDefault(), 
                                    Number(document.querySelector('.button-current-page').innerText));     
        }
    }
    xhttp.open("GET", `https://cukcuk-app.herokuapp.com/api/Employee`, true);
    xhttp.send();
}

function getPageSizeDefault(){
    return document.querySelector('#number-of-rows').innerText.split(' ')[0];
}

function fillTableData(pageSize, pageNumber){
    let xhttp = new XMLHttpRequest();

    xhttp.onreadystatechange = function () {
        if (this.readyState == 4 && this.status == 200) {

            const obj = JSON.parse(this.responseText);

            var tableEmployee = document.querySelector('.table-employee > tbody');
            tableEmployee.innerHTML = '';

            // console.log(obj);
            obj.forEach(e => {
                let EmployeeCode = (e.EmployeeCode)? e.EmployeeCode:'';
                let FullName = (e.FullName)? e.FullName:'';
                let Gender;
                let DateOfBirth;
                let PhoneNumber = (e.PhoneNumber)? e.PhoneNumber:'';
                let Email = (e.Email)? e.Email:'';
                let PositionName = (e.PositionName)? e.PositionName:'';
                let DepartmentName = (e.DepartmentName)? e.DepartmentName:'';
                let Salary = '';
                let WorkStatus;

                // Gender
                if(!e.Gender) Gender = ''
                else{
                    switch(e.Gender){
                        case 0: Gender = "Nữ"; break;
                        case 1: Gender = "Nam"; break;
                        case 2: Gender = "LGBT"; break;
                        default: Gender = "Không xác định"; break;
                    }
                }                

                // Date of birth
                if(!e.DateOfBirth){
                    DateOfBirth = '';
                } else {
                    let date = new Date(e.DateOfBirth);
                    DateOfBirth = `${date.getDate()}/${date.getMonth()+1}/${date.getFullYear()}`;
                }

                // Salary
                if(!e.Salary) {
                    Salary = '';
                } else {
                    Salary = e.Salary.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1.');
                }                

                // Work Status
                if(!e.WorkStatus){
                    WorkStatus = '';
                } else {
                    switch(e.WorkStatus){
                        case 0: WorkStatus = "Đang làm việc"; break;
                        case 1: WorkStatus = "Đang thử việc"; break;
                        case 2: WorkStatus = "Sắp nghỉ việc"; break;
                        default: WorkStatus = "Đã nghỉ việc"; break;
                    }
                }
                              
                // Add row to table body
                let htmlText =  `<tr><td title="${EmployeeCode}">${EmployeeCode}</td>`
                            +   `<td title="${FullName}">${e.FullName}</td>`
                            +   `<td title="${Gender}">${Gender}</td>`
                            +   `<td title="${DateOfBirth}">${DateOfBirth}</td>`
                            +   `<td title="${PhoneNumber}">${PhoneNumber}</td>`
                            +   `<td title="${Email}">${Email}</td>`
                            +   `<td title="${PositionName}">${PositionName}</td>`
                            +   `<td title="${DepartmentName}">${DepartmentName}</td>`
                            +   `<td title="${Salary}">${Salary}</td>`
                            +   `<td title="${WorkStatus}">${WorkStatus}</td></tr>`;
                tableEmployee.insertAdjacentHTML('beforeend', htmlText);                
            })

            // Set listener for currently inserted row
            initTableRowListener();
        }
    }
    xhttp.open("GET", `https://cukcuk-app.herokuapp.com/api/Employee?pageNumber=${pageNumber}&pageSize=${pageSize}`, true);
    xhttp.send();
}
