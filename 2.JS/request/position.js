function GetPositions() {
    $.ajax({
        url: 'https://cukcuk-app.herokuapp.com/api/Position',
        method: 'GET'
    }).done(data => {
        var positionElements = document.querySelectorAll("#positions");

        localStorage['position'] = JSON.stringify(data);

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