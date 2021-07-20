var form = document.getElementById("container");

function OpenPopup(row) {
    form.style.display = "block";
    form.addEventListener('click', MyFunc);

    var formContainer = form.children[0];
    var formBody = formContainer.children[1];
    var inputBody = formBody.children[1];    
    var inputRow = inputBody.children[1];
    var inputField = inputRow.children[0];
    var employeeIdInput = inputField.children[1];
    
    // Get new Employee Code from API
    function ReturnNewEmployeeCode(result){
        employeeIdInput.value = result;
    }
    // async action
    GetNewEmployeeCode(ReturnNewEmployeeCode);

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
        ClosePopup();
    }
}

function LiveFormatSalaryInput(input){
    var rawNumber = input.value.replaceAll('.', '');    
    input.value = FormatMoneyString(rawNumber)
}

function ValidateForm(){
    var employeeCode    = $('#employee-code').val();
    var fullName        = $('#fullname').val();
    var dob             = $('#dob').val();
    var gender          = GenderText2Code($('#gender').text());
    var identityNumber  = $('#identity-number').val();
    var identityDate    = $('#identity-date').val();
    var identityPlace   = $('#identity-place').val();
    var email           = $('#email').val();
    var phoneNumber     = $('#phone-number').val();
    var positionId      = GetPositionIdFromName($('#position-name').text());
    var departmentId    = GetDepartmentIdFromName($('#department-name').text());
    var taxCode         = $('#tax-code').val();
    var salary          = $('#salary').val();
    var joinDate        = $('#join-date').val();
    var workStatus      = WorkStatusText2Code($('#work-status').text());

    // make sure required field not empty
    
    var isEmpty1 = IsEmpty(employeeCode, 'employee-code');
    var isEmpty2 = IsEmpty(fullName, 'fullname');
    var isEmpty3 = IsEmpty(identityNumber, 'identity-number');
    var isEmpty4 = IsEmpty(email, 'email');
    var isEmpty5 = IsEmpty(phoneNumber, 'phone-number');
    var isEmpty = isEmpty1 || isEmpty2 || isEmpty3 || isEmpty4 || isEmpty5;

    if(isEmpty) return null;

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
        "identityNumber":identityNumber,
        "identityDate":new Date(identityDate).toISOString(),
        "identityPlace": identityPlace,
        "departmentId": departmentId,
        "positionId": positionId,
        "personalTaxCode" : taxCode,
        "salary" : Number(salary.replaceAll('.','')),
        "workStatus": Number(workStatus),
        "joinDate": new Date(joinDate).toISOString()
    }
}

function IsEmpty(value, id){
    console.log(value.trim(), id)
    if(value.trim() == ""){
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
}