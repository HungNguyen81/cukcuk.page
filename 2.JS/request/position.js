function GetPositions() {
    $.ajax({
        url: 'http://cukcuk.manhnv.net/v1/Positions',
        method: 'GET'
    }).done(data => {
        localStorage['position'] = JSON.stringify(data);
        FillDropdownData(data, true, 'positions');
    }).fail(res => {
        console.log(res);
    })
}

