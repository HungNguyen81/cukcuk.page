var form = document.getElementById("container");

function OpenPopup(row) {
    form.style.display = "block";
    form.addEventListener('click', MyFunc);

    var employeeIdInput = form.children[0]
        .children[1]
        .children[1]
        .children[0]
        .children[1]
        .children[1];
    console.log(employeeIdInput);

    employeeIdInput.focus();

    if (row) {
        InitForm();
    }
}

function ClosePopup() {
    form.style.display = "none";
    form.removeEventListener('click', MyFunc);
}

function MyFunc(e) {
    if (!document.getElementById('form-container').contains(e.target)) {
        closePopup();
    }
}

// Get row data and fill to the form
function InitForm(row) {
    console.log("init form");
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

    $.ajax(settings).done(function (response) {
        console.log(id);
    });
}

function SendDELETERequests() {
    var settings = {
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
}

function SendRequests() {
    for (let i = 1; i < 400; i++) {
        SendRequest(i);
    }

    // SendDELETERequests();
}

function SendRequest(number){

    var Gender = [0, 1, 2, 3]
    var Ho = ['Lê', 'Nguyễn', 'Trần', 'Lê Hữu' , 'Lý', 'Lương', 'Phạm', 'Đỗ', 'Võ', 'Lưu', 'Đậu', 'Trương'];
    var TenLot = ['Ngọc', 'Đức', 'Văn', 'Việt', 'Tiến', 'Thị', 'Khánh', 'Thu', 'Thủy'];
    var Ten = ['Hưng', 'Hùng', 'Dũng', 'Lan', 'Thu', 'Huyền', 'Trâm', 'Huy', 'Đức', 'Long', 'Ngọc', 'Loan', 'Hường', 'Tú', 'Lan', 'Mạnh'];

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
          "employeeCode": `NV0${number}`,
          "firstName": null,
          "lastName": null,
          "fullName": `${Ho[Math.round(Math.random(0,Ho.length)*10)]} ${TenLot[Math.round(Math.random(0,TenLot.length)*10%TenLot.length)]} ${Ten[Math.round(Math.random(0,Ten.length)*100%Ten.length)]}`,
          "gender": Math.round(Math.random(0,4)*10),
          "dateOfBirth": "2000-01-08T12:00:23.591Z",
          "phoneNumber": `0${Math.round(Math.random()*1000000000)}`,
          "email": `hungnguyen.${number}@github.io`,
          "address": "Thanh Hóa",
          "identityNumber": `HUNGNN00${number}`,
          "identityDate": "2021-07-19T14:30:23.591Z",
          "identityPlace": "AMERICA",
          "joinDate": "2021-07-19T14:30:23.591Z",
          "martialStatus": 0,
          "educationalBackground": 0,
          "qualificationId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
          "departmentId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
          "positionId": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
          "workStatus": Math.round(Math.random(0,3)*10),
          "personalTaxCode": `${Math.round(Math.random()*10000000)}`,
          "salary": Math.floor(Math.random()*1000000000),          
          "positionId": "548dce5f-5f29-4617-725d-e2ec561b0f41",
          "departmentId" : "17120d02-6ab5-3e43-18cb-66948daf6128",
          "qualificationName": null
        }),
      };
      
      $.ajax(settings).done(function (response) {
        console.log(number);
      });
}