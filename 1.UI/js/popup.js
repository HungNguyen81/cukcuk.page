class Popup {
    /**
     * 
     * @param {JSON array} data 
     */
    constructor() {
        this._popup = $('#popup');
        this._timeRemain = 55;
        this._popup.text('');
    }

    ConvertData = data => {
        var res = [];
        for (let i = 0; i < data.length; i++) {
            res.push(data[i].code);
        }
        return res;
    }

    /**
     * 
     * @param {string} message 
     * @param {string} action 
     * @param {book} countdown 
     * @param {function} callback 
     */
    DisplayPopup(message, action, countdown, callback) {
        var countdownText = `<p>Tự động đóng sau <b class="countnumber">${this._timeRemain}</b> giây</p>`;
        this._popup.append(`
            <div class="popup-wrapper">
                <div class="popup-container">
                    <div class="close-button"></div>
                    <div class="popup-header">Xóa bản ghi</div>
                <div class="popup-body">
                    <div class="msg-icon"></div>
                    <div class="msg-content">
                        <p>${message}</p>
                        ${(countdown)? countdownText:''}    
                    </div>
                </div>
                <div class="popup-footer">
                    <div class="button button-cancel popup-button">Hủy</div>
                    <div class="button button-ok popup-button">${action}</div>
                </div>
            </div>`
        );

        if(countdown) this._timeInterval = setInterval(this.Countdown, 1000);
        $('#popup .close-button').on('click', this.CloseErrorPopup);
        $('#popup .button-cancel').on('click', this.CloseErrorPopup);
        $('#popup .button-ok').on('click', callback, this.ButtonOkClicked);
    }

    CloseErrorPopup = () => {
        this._popup.text('');
        if(this._timeInterval) clearInterval(this._timeInterval);
    }

    Countdown = () => {
        if (this._timeRemain == 0) {
            if(this._timeInterval) clearInterval(this._timeInterval);
            this.CloseErrorPopup();
        } else {
            $('#popup .countnumber').text(--this._timeRemain);
        }
    }

    ButtonOkClicked = () => {
        this.CloseErrorPopup();
    }
}


class PopupDelete extends Popup {
    constructor(data) {
        super();
        this._data = data;
        this._listEmployeeCodes = this.ConvertData(data);
        this.DisplayPopup(`Bạn có chắc muốn xóa nhân viên "<b>${this._listEmployeeCodes.join(', ')}</b>" hay không ?`, 'Xóa', true);
    }

    ButtonOkClicked = () => {
        this.CloseErrorPopup();
        this._data.forEach(e => {
            SendDELETERequest(e.id);
        });
        ToggleDeleteButton('hidden');
    }
}

class PopupMessage extends Popup {
    /**
     * 
     * @param {String} msg       
     * @param {function} callback 
     * @param {bool} countdown
     */
    constructor(msg, callback, countdown) {
        super();
        this.DisplayPopup(msg, 'OK', countdown, callback);
    }

    ButtonOkClicked = (e) => {
        this.CloseErrorPopup();
        if(e.data) e.data(); 
    }
}