class Popup {
    /**
     * 
     * @param {JSON array} data 
     */
    constructor(data) {
        this._data = data;
        this._popup = $('#popup');
        this._timeRemain = 55;
        this._popup.text('');
        this._listEmployeeCodes = this.ConvertData(data);
        this.DisplayPopup();
    }

    ConvertData = data => {
        var res = [];
        for (let i = 0; i < data.length; i++) {
            res.push(data[i].code);
        }
        return res;
    }

    DisplayPopup() {
        this._popup.append(`
            <div class="popup-wrapper">
                <div class="popup-container">
                    <div class="close-button"></div>
                    <div class="popup-header">Xóa bản ghi</div>
                <div class="popup-body">
                    <div class="msg-icon"></div>
                    <div class="msg-content">
                        <p>Bạn có chắc muốn xóa nhân viên "<b>${this._listEmployeeCodes.join(',')}</b>" hay không ?</p>
                        <p>Tự động đóng sau <b class="countnumber">${this._timeRemain}</b> giây</p></div>
                </div>
                <div class="popup-footer">
                    <div class="button button-cancel popup-button">Hủy</div>
                    <div class="button button-delete popup-button">Xóa</div>
                </div>
            </div>`
        );

        this._timeInterval = setInterval(this.Countdown, 1000);
        $('#popup .close-button').on('click', this.CloseErrorPopup);
        $('#popup .button-cancel').on('click', this.CloseErrorPopup);
        $('#popup .button-delete').on('click', this.SendRequests);
    }

    CloseErrorPopup = () => {
        this._popup.text('');
        clearInterval(this._timeInterval);
    }

    Countdown = () => {
        if (this._timeRemain == 0) {
            clearInterval(this._timeInterval);
            this.CloseErrorPopup();
        } else {
            $('#popup .countnumber').text(--this._timeRemain);
        }
    }

    SendRequests = () => {        
        this.CloseErrorPopup();
        this._data.forEach(e => {
            SendDELETERequest(e.id);              
        });
        ToggleDeleteButton('hidden');
    }
}