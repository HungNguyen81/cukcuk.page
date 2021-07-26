function InitTableData() {
    GetNumberOfEmployees(UpdateEmployeeTable);
    localStorage['currentpage'] = 1;
}

/**
 * Refresh table
 * Hungnn
 */
function UpdateEmployeeTable() {
    $('#search-box').val('');
    $('.dropdown-positions .combobox input').val('Tất cả vị trí');
    $('.dropdown-departments .combobox input').val('Tất cả phòng ban');
    var pageSize = GetPageSizeDefault();
    FillTableData(pageSize, 1);
    ChangeCurrentPageLabel(pageSize, 1);
    UpdatePagingBar();    
}

/**
 * Restore employee data when using search by code, name, phone-number
 * Hungnn
 */
function BackupEmployees() {
    var data = localStorage['cached-employees'];
    if (data) {        
        localStorage['employees'] = data;
        localStorage['numofemployees'] = JSON.parse(data).length;
    }
}

/**
 * Restore employee data when using search by filter
 * Hungnn
 */
function BackupEmployeesForFilter(){
    var data = localStorage['cached-employees-filter'];
    if (data) {        
        localStorage['employees'] = data;
        localStorage['numofemployees'] = JSON.parse(data).length;
    }
}

/**
 * 
 * @param {JSON} data 
 * @returns formatted data from raw data
 * Hungnn
 */
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

/**
 * 
 * @returns page size from dropdown
 */
function GetPageSizeDefault() {
    return Number(document.querySelector('#number-of-rows').innerText.split(' ')[0]);
}

/**
 * Append table row to table
 * @param {Number} pageSize 
 * @param {Number} pageNumber 
 */
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
            // Set listener for currently inserted row 
            InitTableRowListener();
            MarkSelectedEmployees();
            localStorage['checkedcount'] = 0;            
        })
    } else {
        //  ...
    }
}

/**
 * Handle onclick event
 * @param {tr element} row
 * Hungnn 
 */
function SelectTableRow(row) {
    var checkbox = row.children[0].children[0].children[0];

    row.style.backgroundColor = (checkbox.checked) ? '#fff' : '#EBF4FF';
    checkbox.checked = !checkbox.checked;
    var deleteList =  JSON.parse(localStorage['deletelist']);
    var checkedCount = deleteList.length;
    var employeeCode = row.children[1].innerText;
    var employeeId = GetEmployeeIdByEmployeeCode(employeeCode);    

    if(checkbox.checked){
        ++checkedCount 
    } else {
        --checkedCount;
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
    }
}

function ToggleDeleteButton(value) {
    $('#button-delete').css('visibility', value);
}

/**
 * Mark selected rows when change page
 * Hungnn
 */
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

