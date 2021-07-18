function GetDepartments(){
    let xhttp = new XMLHttpRequest();

    xhttp.onreadystatechange = function() {
        if(this.readyState == 4 && this.status == 200){
            
            const obj = JSON.parse(this.responseText);
                             
            var departmentElements = document.querySelectorAll("#departments");
            departmentElements.forEach(department => {
                obj.forEach(depart => {
                    let htmlText =  '<div class="dropdown-item" onclick="itemSelect(this)">'
                                +       '<i class="fas fa-check item-icon"></i>'
                                +   `<div class="item-text">${depart.DepartmentName}</div></div>`
                    department.insertAdjacentHTML('beforeend', htmlText);
                })
            })
        }
    }
    xhttp.open("GET", "https://cukcuk-app.herokuapp.com/api/Department", true);
    xhttp.send();
}

function GetPositions(){
    let xhttp = new XMLHttpRequest();

    xhttp.onreadystatechange = function() {
        if(this.readyState == 4 && this.status == 200){
            
            const obj = JSON.parse(this.responseText);
                             
            var positionElements = document.querySelectorAll("#positions");
            positionElements.forEach(position => {
                obj.forEach(pos => {
                    let htmlText =  '<div class="dropdown-item"  onclick="itemSelect(this)">'
                                +       '<i class="fas fa-check item-icon"></i>'
                                +   `<div class="item-text">${pos.PositionName}</div></div>`                
                    position.insertAdjacentHTML('beforeend', htmlText);
                })
            })
        }
    }
    xhttp.open("GET", "https://cukcuk-app.herokuapp.com/api/Position", true);
    xhttp.send();
}