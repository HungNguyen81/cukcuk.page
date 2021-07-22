
/**
 * GET All employee data
 * @param {callback function} callback 
 */
function GetNumberOfEmployees(callback) {
    $.ajax({
        url: 'http://cukcuk.manhnv.net/v1/Employees', //https://cukcuk-app.herokuapp.com/api/Employee
        method: 'GET'
    }).done(data => {
        var tableEmployee = $('.table-employee > tbody');
        tableEmployee.innerHTML = '';

        localStorage['employees']       = JSON.stringify(FormatEmployeeData(data));
        localStorage['numofemployees']  = data.length;
        localStorage['currentpage']     = 1;
        console.log("number of rows", data.length);

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

function PostNewEmployeeBigAmount(){
    for(let i = 1; i < 400; i++){
        SendRandomRequest(i);
    }
}

/**
 * POST new employee to server
 * Author: Hungnguyen81
 */
function PostNewEmployee() {    
    var postData = ValidateForm();
    var id = '';
    var action = $('#save-button').attr('action');
    if(action == "PUT"){
        id = localStorage['employeeid'];
    }

    if (!postData) {
        console.log("not validated");
        return;
    }
    console.log("validated", action);
    // console.log('data:',postData);

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
        console.log(response);
        ClosePopup();
        InitTableData();
    });
}

/**
 * 
 * @param {any} min 
 * @param {any} max 
 * @returns 
 * From stackoverflow
 */
function getRandomInt(min, max) {
    min = Math.ceil(min);
    max = Math.floor(max);
    return Math.floor(Math.random() * (max - min + 1)) + min;
}

function SendRandomRequest(number) {
    var Ho = ['Lê', 'Nguyễn', 'Trần', 'Lê Hữu', 'Lý', 'Lương', 'Phạm', 'Đỗ', 'Võ', 'Lưu', 'Đậu', 'Trương', 'Hồ', 'Văn', 'Mạc'];
    var TenLot = ['Ngọc', 'Đức', 'Văn', 'Việt', 'Tiến', 'Thị', 'Khánh', 'Thu', 'Thủy'];
    var Ten = ['Hưng', 'Hùng', 'Dũng', 'Lan', 'Thu', 'Huyền', 'Trâm', 'Huy', 'Đức', 'Long', 'Tú', 'Tùng', 'Đức', 'Ngọc', 'Loan', 'Hường', 'Tú', 'Lan', 'Mạnh'];

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
            "modifiedDate": "2021-07-19T14:30:23.591Z",
            "modifiedBy": "Hungnn",
            "employeeId": "0760cdff-e8a0-11eb-94eb-42010a8c0002",
            "employeeCode": `MF-${number}`,
            "fullName": `${Ho[getRandomInt(0, Ho.length)]} ${TenLot[getRandomInt(0, TenLot.length)]} ${Ten[getRandomInt(0, Ten.length)]}`,
            "gender": getRandomInt(1,4),
            "dateOfBirth": "2000-01-08T12:00:23.591Z",
            "phoneNumber": `0${Math.round(Math.random() * 1000000000)}`,
            "email": `nhanvien.MF${number}@misa.cukcuk.vn`,
            "address": "TH",
            "identityNumber": `0382${Math.round(Math.random() * 100000000)}`,
            "identityDate": "2021-07-19T14:30:23.591Z",
            "identityPlace": "AMERICA",
            "joinDate": "2021-07-19T14:30:23.591Z",
            "workStatus": getRandomInt(1,4),
            "personalTaxCode": `${Math.round(Math.random() * 10000000)}`,
            "salary": Math.floor(Math.random() * 1000000000),
            "positionId": "548dce5f-5f29-4617-725d-e2ec561b0f41",
            "departmentId": "17120d02-6ab5-3e43-18cb-66948daf6128",
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
        console.log('DELETE :', id);
        RemoveFromDeleteList(id);
        GetNumberOfEmployees(UpdateEmployeeTable);
    });
}

function DeleteSelectedEmployees() {
    var deleteList = JSON.parse(localStorage['deletelist']);

    deleteList.forEach(e => {
        SendDELETERequest(e.id);
    });
}


/**
 * var settings = {
        "url": "http://cukcuk.manhnv.net/v1/Employees/",
        "method": "GET",
        "timeout": 0,
    };

    $.ajax(settings).done(function (response) {
        response.forEach(e => {
            if (e.Address == "Thanh Hóa") {
                SendDELETERequest(e.EmployeeId);
            }
        });
    });
 */