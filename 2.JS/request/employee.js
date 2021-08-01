
/**
 * GET All employee data
 * @param {callback function} callback 
 */
function GetNumberOfEmployees(callback) {    
    document.getElementById('loader').removeAttribute('hidden');
    $.ajax({
        url: 'http://cukcuk.manhnv.net/v1/Employees', //https://cukcuk-app.herokuapp.com/api/Employee
        method: 'GET'
    }).done(data => {
        // console.log("ok");
        var tableEmployee = $('.table-employee > tbody');
        tableEmployee.innerHTML = '';

        localStorage['employees'] = JSON.stringify(FormatEmployeeData(data));
        localStorage['numofemployees'] = data.length;
        localStorage['currentpage'] = 1;
        localStorage['cached-employees'] = localStorage['employees'];
        
        document.getElementById('loader').setAttribute('hidden', true);
        callback();
    }).fail(res => {
        // ...
    })
}

/**
 * GET recommend new employee code
 * @param {function} callback 
 */
function GetNewEmployeeCode(callback) {
    $.ajax({
        url: 'http://cukcuk.manhnv.net/v1/Employees/NewEmployeeCode',
        method: 'GET',
        success: callback
    }).done(res => {
        console.log("New Employee Code:", res);
    }).fail(res => {
        callback(`NV${Math.round(Math.random() * 100000)}`);
    })
}

function PostNewEmployee() {
    for (let i = 1500; i < 2500; i++) {
        SendRandomRequest(i);
    }
}

/**
 * POST new employee to server
 * Author: Hungnguyen81
 */
function PostNewEmployee1() {
    var id = '';
    var action = $('#save-button').attr('action');
    var postData = ValidateForm(action);

    if(!postData){
        // new PopupMessage("Thông tin bạn nhập chưa đúng định dạng. Vui lòng nhập lại!", "OK");
        return;
    };
    
    if (action == "PUT") {
        id = localStorage['employeeid'];
    }

    var settings = {
        "url": `http://cukcuk.manhnv.net/v1/Employees/${id}`,
        "method": action,
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json"
        },
        "data": JSON.stringify(postData),
    };

    $.ajax(settings).done(function (response) {
        ClosePopup('Thông báo', 
            `Xác nhận ${(action == 'PUT')? 'sửa':'thêm'} nhân viên <strong>${postData.employeeCode}</strong>!`, 
            InitTableData);        
    });
}

function SendRandomRequest(number) {
    var Ho = ['Lê', 'Nguyễn', 'Trần', 'Lê Hữu', 'Lý', 'Lương', 'Phạm', 'Đỗ', 'Võ', 'Lưu', 'Đậu', 'Trương', 'Hồ', 'Văn', 'Mạc'];
    var TenLot = ['Ngọc', 'Đức', 'Văn', 'Việt', 'Tiến', 'Thị', 'Khánh', 'Thu', 'Thủy'];
    var Ten = ['Hưng', 'Hùng', 'Dũng', 'Lan', 'Thu', 'Huyền', 'Trâm', 'Huy', 'Đức', 'Long', 'Tú', 'Tùng', 'Đức', 'Ngọc', 'Loan', 'Hường', 'Tú', 'Lan', 'Mạnh'];

    var pos = JSON.parse(localStorage['position']);
    var dep = JSON.parse(localStorage['department']);
    var settings = {
        "url": "http://cukcuk.manhnv.net/v1/Employees",
        "method": "POST",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json"
        },
        "data": JSON.stringify({
            "createdDate": "2021-07-19T14:30:23.591Z",
            "createdBy": "Hungnn",
            "modifiedBy": "Hungnn",
            "employeeId": "0760cdff-e8a0-11eb-94eb-42010a8c0002",
            "employeeCode": `NV-${number}`,
            "fullName": `${Ho[getRandomInt(0, Ho.length)]} ${TenLot[getRandomInt(0, TenLot.length)]} ${Ten[getRandomInt(0, Ten.length)]}`,
            "gender": getRandomInt(1, 4),
            "dateOfBirth": "2000-01-08T12:00:23.591Z",
            "phoneNumber": `0${Math.round(Math.random() * 1000000000)}`,
            "email": `nhanvien.MF${number}@misa.cukcuk.vn`,
            "address": "TH",
            "identityNumber": `0382${Math.round(Math.random() * 100000000)}`,
            "identityDate": "2021-07-19T14:30:23.591Z",
            "identityPlace": "AMERICA",
            "joinDate": "2021-07-19T14:30:23.591Z",
            "workStatus": getRandomInt(1, 4),
            "personalTaxCode": `${Math.round(Math.random() * 10000000)}`,
            "salary": Math.floor(Math.random() * 1000000000),
            "positionId": pos[getRandomInt(0, pos.length) % pos.length].PositionId,
            "departmentId": dep[getRandomInt(0, dep.length) % dep.length].DepartmentId,
        }),
    };

    $.ajax(settings).done(function (response) {
        console.log(number);
    });
}

function SendDELETERequest(id) {
    var settings = {
        "url": `http://cukcuk.manhnv.net/v1/Employees/${id}`,
        "method": "DELETE",
        "timeout": 0,
        "headers": {
            "Content-Type": "application/json"
        }
    };

    $.ajax(settings).done(function () {
        RemoveFromDeleteList(id);
        GetNumberOfEmployees(UpdateEmployeeTable);
    });
}

function DeleteSelectedEmployees() {
    var deleteList = JSON.parse(localStorage['deletelist']);
    new PopupDelete(deleteList);
}

