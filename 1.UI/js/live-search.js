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
            // console.log("s1:", e.EmployeeCode);
            isEmpCodeFound = true;
            result.push(e);
        }

        // Search by fullname
        if (e.FullName.toUpperCase().search(searchContent) != -1) {
            // console.log("s2:", e.EmployeeCode, e.FullName);
            isNameFound = true;
            if (!isEmpCodeFound) {
                result.push(e);
            }
        }

        // Search by phone number
        if (e.PhoneNumber.search(searchContent) != -1) {
            // console.log("s3:", e.PhoneNumber, e.FullName);
            isPhoneNumFound = true;
            if (!isEmpCodeFound && !isNameFound) {
                result.push(e);
            }
        }

        if (isEmpCodeFound || isNameFound || isPhoneNumFound) {
            isFound = true;
        }
    });
    // console.log("..............................")

    if (!isFound) {
        console.log("not found");

        $('.table-employee > tbody').html('');

        localStorage['currentpage'] = 1;
        localStorage['numofemployees'] = result.length;

        ChangeCurrentPageLabel(GetPageSizeDefault(), 1);
        UpdatePagingBar();

        // Toast Message
        // ...
    } else {
        result.sort(function(x,y){
            let X = x.FullName.split(' ');
            let Y = y.FullName.split(' ');
            let a = X[X.length-1];
            let b = Y[Y.length-1];
            return a.localeCompare(b);
        });
        localStorage['currentpage'] = 1;
        localStorage['numofemployees'] = result.length;
        localStorage['employees'] = JSON.stringify(result);
        localStorage['cached-employees-filter'] = JSON.stringify(result);

        let pageSize = GetPageSizeDefault();

        FillTableData(pageSize, 1);
        UpdatePagingBar();
        ChangeCurrentPageLabel(pageSize, 1);
    }
}