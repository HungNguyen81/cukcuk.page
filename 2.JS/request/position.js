function GetPositions() {
    $.ajax({
        url: 'http://cukcuk.manhnv.net/v1/Positions',
        method: 'GET'
    }).done(data => {

        localStorage['position'] = JSON.stringify(data);

        FillDropdownData(data, true, 'positions');

        // var selectedIndex = data.findIndex(obj => { return obj.PositionName == input.value });

        // document.querySelector('#positions').children[0].classList.add('item-selected');
    }).fail(res => {
        console.log(res);
    })
}

