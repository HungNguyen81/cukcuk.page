/**
* Format Date String to dd/mm/yyyy
* @param {datetime string} data
* @param {boolean} isForDateInput
* @returns dd/mm/yyyy date format string
* Author: HungNguyen81
*/
function DateFormat(data, isForDateInput) {
    let date = new Date(data);
    let dd = date.getDate();
    let mm = date.getMonth() + 1;
    let yyyy = date.getFullYear();

    if(isForDateInput){
        return `${yyyy}-${(mm < 10) ? '0' + mm : mm}-${(dd < 10) ? '0' + dd : dd}`;
    }
    return `${(dd < 10) ? '0' + dd : dd}/${(mm < 10) ? '0' + mm : mm}/${yyyy}`;
}

function FormatMoneyString(text){
    return text.toString().replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1.');
}

function WorkStatusCode2Text(statusCode){
    switch (statusCode) {
        case 1 : return "Đang làm việc";
        case 2 : return "Đang thử việc";
        case 3 : return "Sắp nghỉ việc";
        default: return "Đã nghỉ việc";
    }
}

function WorkStatusText2Code(text){
    switch (text) {
        case "Đang làm việc": return 1;
        case "Đang thử việc": return 2;
        case "Sắp nghỉ việc": return 3;
        default: return 3; 
    }
}

function GenderCode2Text(code){
    switch (code) {
        case 0 : return "Nữ";
        case 1 : return "Nam";
        case 2 : return "Không xác định";
        default: return "";
    }
}

function GenderText2Code(text){
    switch (text) {
        case "Nữ"               : return 0;
        case "Nam"              : return 1;
        case "Không xác định"   : return 2;
        default                 : return 3;
    }
}

function GetDepartmentIdFromName(name){
    var stored = localStorage['department'];
    var res = null;
    if(stored){
        let data = JSON.parse(stored);
        for(let i = 0; i < data.length; i++){
            if(data[i].DepartmentName == name){
                // console.log('dep', data[i].DepartmentId);
                res = data[i].DepartmentId;
                break;
            }
        }
    }
    return res;
}

function GetPositionIdFromName(name){
    var stored = localStorage['position'];
    var res = null;
    if(stored){
        let data = JSON.parse(stored);
        for(let i = 0; i < data.length; i++){
            if(data[i].PositionName == name){
                // console.log('pos', data[i].PositionId);
                res = data[i].PositionId;
                break;
            }
        }
    }
    return res;
}

function GetEmployeeIdByEmployeeCode(code){
    var data = JSON.parse(localStorage['employees']);
    for(let i = 0; i < data.length; i++){
        if(code == data[i].EmployeeCode){
            return data[i].EmployeeId;
        }
    }
}