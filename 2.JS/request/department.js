function GetDepartments() {
    $.ajax({
        url: 'https://cukcuk-app.herokuapp.com/api/Department',
        method: 'GET'
    }).done(data => {        
        localStorage['department'] = JSON.stringify(data);
        FillDropdownData(data, true, 'departments');
    }).fail(res => {
        console.log(res);
    })
}