function InitTableData() {
    GetNumberOfEmployees(UpdateEmployeeTable);
    localStorage['currentpage'] = 1;
}

function UpdateEmployeeTable() {
    var pageSize = GetPageSizeDefault();
    FillTableData(pageSize, 1);
    ChangeCurrentPageLabel(pageSize, 1);
    UpdatePagingBar();
    ToggleDeleteButton('hidden');
}

function BackupEmployees() {
    var data = localStorage['cached-employees'];
    if (data) {
        // console.log(data)
        localStorage['employees'] = data;
        localStorage['numofemployees'] = JSON.parse(data).length;
    }
}

function FormatEmployeeData(data) {
    data.forEach(e => {
        e.EmployeeCode = (e.EmployeeCode) ? e.EmployeeCode : '';
        e.FullName = (e.FullName) ? e.FullName : '';
        e.PhoneNumber = (e.PhoneNumber) ? e.PhoneNumber : '';
        e.Email = (e.Email) ? e.Email : '';
        e.PositionName = (e.PositionName) ? e.PositionName : '';
        e.DepartmentName = (e.DepartmentName) ? e.DepartmentName : '';

        // Gender
        e.Gender = GenderCode2Text(e.Gender);

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
            e.Salary = FormatMoneyString(e.Salary);
        }

        // Work Status
        if (e.WorkStatus == null) {
            e.WorkStatus = '';
        } else {
            e.WorkStatus = WorkStatusCode2Text(e.WorkStatus);
        }
    })
    return data;
}

function GetPageSizeDefault() {
    return Number(document.querySelector('#number-of-rows').innerText.split(' ')[0]);
}

function FillTableData(pageSize, pageNumber) {
    var stored = localStorage['employees'];
    var data;

    if (stored) {
        let start = pageSize * (pageNumber - 1);
        let end = pageSize * pageNumber;
        data = JSON.parse(stored).slice(start, end);

        var tableEmployee = $('.table-employee > tbody');
        tableEmployee.html('');

        var promise = data.map(e => {

            // Add row to table body
            let htmlText = `<tr><td><span class="checkbox-container"><input type="checkbox" name="delete">
                                    <span class="checkmark"><i class="fas fa-check check"></i></span></span></td>
                                <td title="${e.EmployeeCode}">${e.EmployeeCode}</td>
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

        Promise.all(promise).then(res => {
            console.log('init row listener');

            // Set listener for currently inserted row 
            InitTableRowListener();
            // ToggleDeleteButton('hidden');
            MarkSelectedEmployees();
            localStorage['checkedcount'] = 0;
        })
    } else {
        //  ...
    }
}

function SelectTableRow(row) {
    var checkbox = row.children[0].children[0].children[0];

    row.style.backgroundColor = (checkbox.checked) ? '#fff' : '#EBF4FF';
    checkbox.checked = !checkbox.checked;
    var checkedCount = localStorage['checkedcount'];
    var employeeCode = row.children[1].innerText;
    var employeeId = GetEmployeeIdByEmployeeCode(employeeCode);

    console.log((checkbox.checked) ? 'selected :' : 'un-selected :', employeeCode, employeeId);

    localStorage['checkedcount'] = (checkbox.checked) ? ++checkedCount : --checkedCount;

    var stored = localStorage['deletelist'];
    var deleteList = [];

    if (stored) {
        deleteList = JSON.parse(stored);
    }

    if (!checkedCount) {
        ToggleDeleteButton('hidden');
        localStorage['deletelist'] = '[]';

    } else {
        ToggleDeleteButton('visible');

        if (checkbox.checked) {
            deleteList.push({
                "code": employeeCode,
                "id": employeeId
            });
        } else {
            deleteList = RemoveFromDeleteList(employeeId);
        }
        localStorage['deletelist'] = JSON.stringify(deleteList);
        console.log(deleteList);
    }
}

function ToggleDeleteButton(value) {
    $('#button-delete').css('visibility', value);
}

function MarkSelectedEmployees() {
    var rows = document.querySelectorAll(".table-employee tbody > tr");
    var deleteList = JSON.parse(localStorage['deletelist']);

    for (let i = 0; i < rows.length; i++) {
        let row = rows[i];
        let eCode = row.children[1].innerText;
        for (let e = 0; e < deleteList.length; e++) {
            if (deleteList[e].code == eCode) {
                row.children[0].children[0].children[0].checked = true;
                row.style.backgroundColor = '#EBF4FF';
            }
        }
    }
}

