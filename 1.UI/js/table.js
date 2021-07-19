function InitTableData() {
    var pageSize = GetPageSizeDefault();
    var pageNumber = Number($('.button-current-page').text());

    GetNumberOfEmployees();
    FillTableData(pageSize, pageNumber);
}

function GetNumberOfEmployees() {
    $.ajax({
        url: 'http://cukcuk.manhnv.net/v1/Employees',
        method: 'GET'
    }).done(data => {
        var tableEmployee = $('.table-employee > tbody');
        tableEmployee.innerHTML = '';

        localStorage['employees'] = JSON.stringify(FormatEmployeeData(data));

        console.log("number of rows", data.length);
        $('.navigator-center').attr('numberofrecords', data.length);
        UpdatePagingBar();

        ChangeCurrentPageLabel(GetPageSizeDefault(), Number($('.button-current-page').text()));
    }).fail(res => {
        // ...
    })
}

function FormatEmployeeData(data){
    data.forEach(e => {
        e.EmployeeCode = (e.EmployeeCode) ? e.EmployeeCode : '';
        e.FullName = (e.FullName) ? e.FullName : '';
        e.PhoneNumber = (e.PhoneNumber) ? e.PhoneNumber : '';
        e.Email = (e.Email) ? e.Email : '';
        e.PositionName = (e.PositionName) ? e.PositionName : '';
        e.DepartmentName = (e.DepartmentName) ? e.DepartmentName : '';        

        // Gender
        
            switch (e.Gender) {
                case 0: e.Gender = "Nam"; break;
                case 1: e.Gender = "Nữ"; break;
                case 2: e.Gender = "Không xác định"; break;
                default: e.Gender = ""; break;
            }
        

        // Date of birth
        if (!e.DateOfBirth) {
            e.DateOfBirth = '';
        } else {
            e.DateOfBirth = DateFormat(e.DateOfBirth);
        }

        // Salary
        if (!e.Salary) {
            e.Salary = '';
        } else {
            e.Salary = e.Salary.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1.');
        }

        // Work Status
        if (!e.WorkStatus) {
            e.WorkStatus = '';
        } else {
            switch (e.WorkStatus) {
                case 0: e.WorkStatus = "Đang làm việc"; break;
                case 1: e.WorkStatus = "Đang thử việc"; break;
                case 2: e.WorkStatus = "Sắp nghỉ việc"; break;
                default:e.WorkStatus = "Đã nghỉ việc"; break;
            }
        }
    })
    console.log(data);
    return data;
}

function GetPageSizeDefault() {
    return Number(document.querySelector('#number-of-rows').innerText.split(' ')[0]);
}

function FillTableData(pageSize, pageNumber) {
    var stored = localStorage['employees'];
    var data;

    if(stored){
        let start = pageSize * (pageNumber - 1);
        let end = pageSize * pageNumber;
        data = JSON.parse(stored).slice(start, end);

        var tableEmployee = $('.table-employee > tbody');
        tableEmployee.html('');

        data.forEach(e => {
            
            // Add row to table body
            let htmlText = `<tr><td title="${e.EmployeeCode}">${e.EmployeeCode}</td>
                                <td title="${e.FullName}">${e.FullName}</td>
                                <td title="${e.Gender}">${e.Gender}</td>
                                <td title="${e.DateOfBirth}">${e.DateOfBirth}</td>
                                <td title="${e.PhoneNumber}">${e.PhoneNumber}</td>
                                <td title="${e.Email}">${e.Email}</td>
                                <td title="${e.PositionName}">${e.PositionName}</td>
                                <td title="${e.DepartmentName}">${e.DepartmentName}</td>
                                <td title="${e.Salary}">${e.Salary}</td>
                                <td title="${e.WorkStatus}">${e.WorkStatus}</td></tr>`;
            tableEmployee.append(htmlText);
        })

        // Set listener for currently inserted row
        InitTableRowListener();
    } else {
        //  ...
    }    
}

/**
* Format Date String to dd/mm/yyyy
* @param {datetime string} data 
* @returns dd/mm/yyyy date format string
* Author: HungNguyen81
*/
function DateFormat(data) {
    let date = new Date(data);
    let dd = date.getDate();
    let mm = date.getMonth() + 1;
    let yyyy = date.getFullYear();

    return `${(dd < 10) ? '0' + dd : dd}/${(mm < 10) ? '0' + mm : mm}/${yyyy}`;
}
