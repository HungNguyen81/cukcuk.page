function GetDepartments() {    
    $.ajax({
        url: 'https://cukcuk-app.herokuapp.com/api/Department',
        method: 'GET'
    }).done(data => {
        var departmentElements = document.querySelectorAll("#departments");

        localStorage['department'] = JSON.stringify(data);
        console.log(data);

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