function InitTableData() {
    var pageSize = GetPageSizeDefault();
    var pageNumber = Number($('.button-current-page').text());

    GetNumberOfEmployeeRecord();
    FillTableData(pageSize, pageNumber);
}

function GetNumberOfEmployeeRecord() {
    $.ajax({
        url: 'http://cukcuk.manhnv.net/v1/Employees',  // https://cukcuk-app.herokuapp.com/api/Employee
        method: 'GET'
    }).done(data => {
        var tableEmployee = $('.table-employee > tbody');
        tableEmployee.innerHTML = '';

        console.log("number of rows", data.length);
        $('.navigator-center').attr('numberofrecords', data.length);
        UpdatePagingBar();

        ChangeCurrentPageLabel(GetPageSizeDefault(), Number($('.button-current-page').text()));
    }).fail(res => {
        // ...
    })
}

function GetPageSizeDefault() {
    return document.querySelector('#number-of-rows').innerText.split(' ')[0];
}

function FillTableData(pageSize, pageNumber) {
    $.ajax({
        url: `https://cukcuk-app.herokuapp.com/api/Employee?pageNumber=${pageNumber}&pageSize=${pageSize}`,
        method: 'GET'
    }).done(data => {
        var tableEmployee = $('.table-employee > tbody');
        tableEmployee.html('');

        data.forEach(e => {
            let EmployeeCode = (e.EmployeeCode) ? e.EmployeeCode : '';
            let FullName = (e.FullName) ? e.FullName : '';
            let Gender;
            let DateOfBirth;
            let PhoneNumber = (e.PhoneNumber) ? e.PhoneNumber : '';
            let Email = (e.Email) ? e.Email : '';
            let PositionName = (e.PositionName) ? e.PositionName : '';
            let DepartmentName = (e.DepartmentName) ? e.DepartmentName : '';
            let Salary = '';
            let WorkStatus;

            // Gender
            if (!e.Gender) Gender = ''
            else {
                switch (e.Gender) {
                    case 0: Gender = "Nữ"; break;
                    case 1: Gender = "Nam"; break;
                    case 2: Gender = "LGBT"; break;
                    default: Gender = "Không xác định"; break;
                }
            }

            // Date of birth
            if (!e.DateOfBirth) {
                DateOfBirth = '';
            } else {
                DateOfBirth = DateFormat(e.DateOfBirth);
            }

            // Salary
            if (!e.Salary) {
                Salary = '';
            } else {
                Salary = e.Salary.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1.');
            }

            // Work Status
            if (!e.WorkStatus) {
                WorkStatus = '';
            } else {
                switch (e.WorkStatus) {
                    case 0: WorkStatus = "Đang làm việc"; break;
                    case 1: WorkStatus = "Đang thử việc"; break;
                    case 2: WorkStatus = "Sắp nghỉ việc"; break;
                    default: WorkStatus = "Đã nghỉ việc"; break;
                }
            }

            // Add row to table body
            let htmlText = `<tr><td title="${EmployeeCode}">${EmployeeCode}</td>
                                <td title="${FullName}">${e.FullName}</td>
                                <td title="${Gender}">${Gender}</td>
                                <td title="${DateOfBirth}">${DateOfBirth}</td>
                                <td title="${PhoneNumber}">${PhoneNumber}</td>
                                <td title="${Email}">${Email}</td>
                                <td title="${PositionName}">${PositionName}</td>
                                <td title="${DepartmentName}">${DepartmentName}</td>
                                <td title="${Salary}">${Salary}</td>
                                <td title="${WorkStatus}">${WorkStatus}</td></tr>`;
            tableEmployee.append(htmlText);
        })

        // Set listener for currently inserted row
        InitTableRowListener();
    }).fail(res => {
        console.log(res);
    })
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
