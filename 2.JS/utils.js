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

    if (isForDateInput) {
        return `${yyyy}-${(mm < 10) ? '0' + mm : mm}-${(dd < 10) ? '0' + dd : dd}`;
    }
    return `${(dd < 10) ? '0' + dd : dd}/${(mm < 10) ? '0' + mm : mm}/${yyyy}`;
}

function FormatMoneyString(text) {
    text = text.toString().replace(/\s+/, '');
    text = text.replace(/[a-zA-Z@#$%^&*()<>?:";'{[}\]\|\\\/]+/, '');
    return text.replace(/(\d)(?=(\d{3})+(?!\d))/g, '$1.');
}

function WorkStatusCode2Text(statusCode) {
    switch (statusCode) {
        case 1: return "Đang làm việc";
        case 2: return "Đang thử việc";
        case 3: return "Sắp nghỉ việc";
        default: return "Đã nghỉ việc";
    }
}

function WorkStatusText2Code(text) {
    switch (text) {
        case "Đang làm việc": return 1;
        case "Đang thử việc": return 2;
        case "Sắp nghỉ việc": return 3;
        default: return 3;
    }
}

function GenderCode2Text(code) {
    switch (code) {
        case 0: return "Nữ";
        case 1: return "Nam";
        case 2: return "Không xác định";
        default: return "";
    }
}

function GenderText2Code(text) {
    switch (text) {
        case "Nữ": return 0;
        case "Nam": return 1;
        case "Không xác định": return 2;
        default: return 3;
    }
}

function GetDepartmentIdFromName(name) {
    var stored = localStorage['department'];
    var res = null;
    if (stored) {
        let data = JSON.parse(stored);
        for (let i = 0; i < data.length; i++) {
            if (data[i].DepartmentName == name) {
                res = data[i].DepartmentId;
                break;
            }
        }
    }
    return res;
}

function GetPositionIdFromName(name) {
    var stored = localStorage['position'];
    var res = null;
    if (stored) {
        let data = JSON.parse(stored);
        for (let i = 0; i < data.length; i++) {
            if (data[i].PositionName == name) {
                res = data[i].PositionId;
                break;
            }
        }
    }
    return res;
}

function GetEmployeeIdByEmployeeCode(code) {
    var data = JSON.parse(localStorage['employees']);
    for (let i = 0; i < data.length; i++) {
        if (code == data[i].EmployeeCode) {
            return data[i].EmployeeId;
        }
    }
}

function RemoveFromDeleteList(id) {
    var deleteList = JSON.parse(localStorage['deletelist']);

    for (let i = 0; i < deleteList.length; i++) {
        if (deleteList[i].id == id) {
            deleteList.splice(i, 1);
        }
    }

    localStorage['deletelist'] = JSON.stringify(deleteList);
    return deleteList;
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

function IsValidEmail(email) {
    var reg = /\S+@\S+\.\S+/;
    return reg.test(email);
}

function IsValidPhoneNumber(phoneNumber) {
    var reg = /0[0-9]{9,11}$/;
    return reg.test(phoneNumber);
}

/**
 * Chuyển chuỗi từ có dấu về không dấu
 * @param {string} str 
 * @returns chuỗi không dấu
 */
function RemoveAccents(str) {
    return str.normalize('NFD').replace(/[\u0300-\u036f]/g, '');
}