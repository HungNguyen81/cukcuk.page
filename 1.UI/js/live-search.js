function TableLiveSearch(input) {
    var searchContent = input.value.trim().toUpperCase();
    BackupEmployees();
    var employees = JSON.parse(localStorage['employees']);

    // clear table content
    $('.table-employee > tbody').html('');

    // localStorage['cached-employees'] = localStorage['employees'];

    let result = [];
    let isFound = false;

    // search by employee id first
    employees.forEach(e => {
        let isEmpCodeFound = false;
        let isNameFound = false;
        let isPhoneNumFound = false;

        // Search by employee code
        if (e.EmployeeCode.toUpperCase().search(searchContent) != -1) {
            console.log("s1:", e.EmployeeCode);
            isEmpCodeFound = true;
            result.push(e);
        }

        // Search by fullname
        if (e.FullName.toUpperCase().search(searchContent) != -1) {
            console.log("s2:", e.EmployeeCode, e.FullName);
            isNameFound = true;
            if (!isEmpCodeFound) {
                result.push(e);
            }
        }

        // Search by phone number
        if (e.PhoneNumber.search(searchContent) != -1) {
            console.log("s3:", e.PhoneNumber, e.FullName);
            isPhoneNumFound = true;
            if (!isEmpCodeFound && !isNameFound) {
                result.push(e);
            }
        }

        if (isEmpCodeFound || isNameFound || isPhoneNumFound) {
            isFound = true;
        }
    });

    console.log("..............................")

    if (!isFound) {
        console.log("not found");
        // InitTableData();
        $('.table-employee > tbody').html('');

        localStorage['currentpage'] = 1;
        localStorage['numofemployees'] = result.length;

        ChangeCurrentPageLabel(GetPageSizeDefault(), 1);
        UpdatePagingBar();

        // Toast Message
        // ...
    } else {
        localStorage['currentpage'] = 1;
        localStorage['numofemployees'] = result.length;
        localStorage['employees'] = JSON.stringify(result);

        console.log('Search result length:', result.length)

        let pageSize = GetPageSizeDefault();

        FillTableData(pageSize, 1);
        UpdatePagingBar();
        ChangeCurrentPageLabel(pageSize, 1);
        // BackupEmployees();
    }
}

// function ComboboxLiveSearch(input){
//     var filter = input.getAttribute('filter');
//     switch(filter){
//         case 'department':
//             CbxDepartmentFilter();
//             break;
//         case 'position':
//             CbxPositionFilter();
//             break;
//     }
// }

// function CbxDepartmentFilter(){
//     console.log('d');
// }

// function CbxPositionFilter(){
//     console.log('p');
// }