var form = document.getElementById("container");

function OpenPopup(row) {
    form.children[0].classList.toggle('open');
    form.children[0].classList.toggle('close');    
    form.style.visibility = 'visible';
    form.children[0].style.top = '50px';
    form.children[0].style.left = '50%';    
    form.addEventListener('click', MyFunc);

    var formContainer = form.children[0];
    var formBody = formContainer.children[1];
    var inputBody = formBody.children[1];
    var inputRow = inputBody.children[1];
    var inputField = inputRow.children[0];
    var employeeIdInput = inputField.children[1];

    if (row) {
        InitForm(row);
        $('#save-button').attr('action', 'PUT');
    } else {
        $('#save-button').attr('action', 'POST');

        FillForm();

        // Get new Employee Code from API
        function ReturnNewEmployeeCode(result) {
            employeeIdInput.value = result;
        }
        // async action
        GetNewEmployeeCode(ReturnNewEmployeeCode);
    }

    employeeIdInput.focus();
}

function ClosePopup(title, msg, callback) {
    new PopupMessage(title, msg, function () {
        console.log("close form");        
        form.children[0].classList.toggle('open');
        form.children[0].classList.toggle('close');
        form.removeEventListener('click', MyFunc);
        setTimeout(function(){
            form.style.visibility = 'hidden';
        }, 1000);
        if (callback) callback();
    }, false, null);
}

function MyFunc(e) {
    if (!document.getElementById('form-container').contains(e.target)) {
        ClosePopup('Xác nhận đóng Form !', 'Bạn có chắc chắn muốn hủy nhập liệu hay không ?');
    }
}

function LiveFormatSalaryInput(input) {
    var rawNumber = input.value.replaceAll(/^0/g, '').replaceAll(/\.+/g, '');
    input.value = FormatMoneyString(rawNumber)
}

function ValidateForm(action) {
    var employeeCode = $('#employee-code').val();
    var fullName = $('#fullname').val();
    var dob = $('#dob').val();
    var gender = GenderText2Code($('#gender').text());
    var identityNumber = $('#identity-number').val();
    var identityDate = $('#identity-date').val();
    var identityPlace = $('#identity-place').val();
    var email = $('#email').val();
    var phoneNumber = $('#phone-number').val();
    var positionId = GetPositionIdFromName($('#position-name').text());
    var departmentId = GetDepartmentIdFromName($('#department-name').text());
    var taxCode = $('#tax-code').val();
    var salary = $('#salary').val();
    var joinDate = $('#join-date').val();
    var workStatus = WorkStatusText2Code($('#work-status').text());

    if (action == 'POST') {
        // Check if employeeCode is existed
        var stored = JSON.parse(localStorage['employees']);
        if (stored) {
            for (let i = 0; i < stored.length; i++) {
                let e = stored[i];
                if (e.EmployeeCode == employeeCode) {
                    new PopupMessage('Mã nhân viên đã tồn tạo',
                    `Mã nhân viên <b>${employeeCode}</b> đã tồn tại trong hệ thống. Vui lòng tạo mã nhân viên khác!`,
                        null, true);
                    return null;
                }
            }
        }
    }

    // make sure required field not empty

    var isEmpty1 = IsEmpty(employeeCode, 'employee-code');
    var isEmpty2 = IsEmpty(fullName, 'fullname');
    var isEmpty3 = IsEmpty(identityNumber, 'identity-number');
    var isEmpty4 = IsEmpty(email, 'email');
    var isEmpty5 = IsEmpty(phoneNumber, 'phone-number');
    var isEmpty = isEmpty1 || isEmpty2 || isEmpty3 || isEmpty4 || isEmpty5;

    var isValidEmail = IsValidEmail(email);
    var isValidPhoneNumber = IsValidPhoneNumber(phoneNumber);

    var listRequired = [];
    if (isEmpty1) listRequired.push('Mã NV');
    if (isEmpty2) listRequired.push('Họ và tên');
    if (isEmpty3) listRequired.push('Số CMND/CCCD');
    if (isEmpty4) listRequired.push('Email');
    if (isEmpty5) listRequired.push('Số điện thoại');


    var isValid = isValidEmail && isValidPhoneNumber;
    var listValid = [];
    if (!isValidEmail) listValid.push('Email');
    if (!isValidPhoneNumber) listValid.push('Số điện thoại');

    if (isEmpty) {
        new PopupMessage('Thông tin chưa đúng định dạng', 
                `<b>${listRequired.join(', ')}</b> đang để trống. Vui lòng nhập lại!`, null, true, 'warning');
        return null
    };
    if (!isValid) {
        new PopupMessage('Thông tin chưa đúng định dạng',
                `<b>${listValid.join(', ')}</b> chưa đúng định dạng. Vui lòng kiểm tra lại`, null, true, 'warning');
        return null;
    }

    // validate email format
    // ...

    // return employee data object
    return {
        "createdDate": new Date().toISOString(),
        "createdBy": "Hungnn",
        "modifiedDate": new Date().toISOString(),
        "employeeId": "0760cdff-e8a0-11eb-94eb-42010a8c0002",
        "employeeCode": employeeCode,
        "fullName": fullName,
        "gender": gender,
        "dateOfBirth": new Date(dob).toISOString(),
        "phoneNumber": phoneNumber,
        "email": email,
        "identityNumber": identityNumber,
        "identityDate": new Date(identityDate).toISOString(),
        "identityPlace": identityPlace,
        "departmentId": departmentId,
        "positionId": positionId,
        "personalTaxCode": taxCode,
        "salary": Number(salary.replaceAll('.', '')),
        "workStatus": Number(workStatus),
        "joinDate": new Date(joinDate).toISOString()
    }
}

function TextInputChange(input) {
    IsEmpty(input.value, input.id);
}

function IsEmpty(value, id) {
    if (value.trim() == "") {
        $(`#${id}`).css('border-color', 'red');
        return true;
    } else {
        $(`#${id}`).css('border-color', '#bbbbbb');
        return false;
    }
}

// Get row data and fill to the form
function InitForm(row) {
    console.log("init form");

    var employeeCode = row.children[1].innerText;
    var fullName = row.children[2].innerText;
    var gender = row.children[3].innerText;
    var dob = row.children[4].innerText;
    var identityNumber = null;
    var identityDate = null;
    var identityPlace = null;
    var phoneNumber = row.children[5].innerText;
    var email = row.children[6].innerText;
    var positionName = row.children[7].innerText;
    var departmentName = row.children[8].innerText;
    var taxCode = null;
    var salary = row.children[9].innerText;
    var joinDate = null;
    var workStatus = row.children[10].innerText;

    dobArray = dob.split('/');
    dob = DateFormat(`${dobArray[1]}/${dobArray[0]}/${dobArray[2]}`, true);

    var stored = JSON.parse(localStorage['employees']);

    stored.forEach(e => {
        if (e.EmployeeCode == employeeCode) {
            identityNumber = e.IdentityNumber;
            identityDate = DateFormat(e.IdentityDate, true);
            identityPlace = e.IdentityPlace;
            taxCode = e.PersonalTaxCode;
            joinDate = DateFormat(e.JoinDate, true);
        }
    });

    localStorage['employeeid'] = GetEmployeeIdByEmployeeCode(employeeCode);

    $('#employee-code').val(employeeCode);
    $('#fullname').val(fullName);
    $('#dob').val(dob);
    $('#gender').text(gender);
    $('#identity-number').val(identityNumber);
    $('#identity-date').val(identityDate);
    $('#identity-place').val(identityPlace);
    $('#email').val(email);
    $('#phone-number').val(phoneNumber);
    $('#position-name').text(positionName);
    $('#department-name').text(departmentName);
    $('#tax-code').val(taxCode);
    $('#salary').val(salary);
    $('#join-date').val(joinDate);
    $('#work-status').text(workStatus);

    if (gender) SetElementSelected(gender, 'gender');
    if (positionName) SetElementSelected(positionName, 'position-name');
    if (departmentName) SetElementSelected(departmentName, 'department-name');
    if (workStatus) SetElementSelected(workStatus, 'work-status');

    // reset border color of required inputs
    $('#employee-code').css('border-color', '#bbbbbb');
    $('#fullname').css('border-color', '#bbbbbb');
    $('#identity-number').css('border-color', '#bbbbbb');
    $('#email').css('border-color', '#bbbbbb');
    $('#phone-number').css('border-color', '#bbbbbb');
}

function SetElementSelected(value, id) {
    var combobox = document.getElementById(id).parentNode.parentNode;
    var items = combobox.children[1].children;

    for (let i = 0; i < items.length; i++) {
        if (value.toUpperCase() == items[i].children[1].innerText.toUpperCase()) {
            items[i].classList.add('item-selected');
        } else {
            items[i].classList.remove('item-selected');
        }
    }
}

function FillForm() {
    $('#fullname').val('');
    $('#dob').val(DateFormat(new Date(), true));
    $('#gender').text('Nam');
    $('#identity-number').val('');
    $('#identity-date').val(DateFormat(new Date(), true));
    $('#identity-place').val('');
    $('#email').val('');
    $('#phone-number').val('');
    $('#position-name').text('');
    $('#department-name').text('');
    $('#tax-code').val('');
    $('#salary').val('');
    $('#join-date').val(DateFormat(new Date(), true));
    $('#work-status').text('');
}