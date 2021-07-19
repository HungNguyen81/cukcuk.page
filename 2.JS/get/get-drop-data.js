function GetDepartments() {    
    $.ajax({
        url: 'http://cukcuk.manhnv.net/api/Department', //https://cukcuk-app.herokuapp.com/api/Department
        method: 'GET'
    }).done(data => {
        var departmentElements = document.querySelectorAll("#departments");
        departmentElements.forEach(department => {
            data.forEach(depart => {
                let htmlText = `<div class="dropdown-item" onclick="ItemSelect(this)">
                                    <i class="fas fa-check item-icon"></i>
                                <div class="item-text">${depart.DepartmentName}</div></div>`
                department.insertAdjacentHTML('beforeend', htmlText);
            })
        })
    }).fail(res => {
        console.log(res);
    })
}

function GetPositions() {
    $.ajax({
        url: 'http://cukcuk.manhnv.net/v1/Positions', //https://cukcuk-app.herokuapp.com/api/Position
        method: 'GET'
    }).done(data => {
        var positionElements = document.querySelectorAll("#positions");
        positionElements.forEach(position => {
            data.forEach(pos => {
                let htmlText = `<div class="dropdown-item"  onclick="ItemSelect(this)">
                                    <i class="fas fa-check item-icon"></i>
                                <div class="item-text">${pos.PositionName}</div></div>`
                position.insertAdjacentHTML('beforeend', htmlText);
            })
        })
    }).fail(res => {
        console.log(res);
    })
}