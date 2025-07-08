function initializeCalendar(events) {
    var calendarEl = document.getElementById('calendar');

    function getRandomColor() {
        var letters = '0123456789ABCDEF';
        var color = '#';
        for (var i = 0; i < 6; i++) {
            color += letters[Math.floor(Math.random() * 16)];
        }
        return color;
    }

    var calendar = new FullCalendar.Calendar(calendarEl, {
        headerToolbar: { left: 'dayGridMonth,timeGridWeek,listWeek', center: 'title', right: 'today prev,next' },
        customButtons: {
            dayGridMonth: {
                text: 'Ngày Theo Tháng',
                click: function () {
                    calendar.changeView('dayGridMonth');
                }
            },
            timeGridWeek: {
                text: 'Ngày Theo Tuần',
                click: function () {
                    calendar.changeView('timeGridWeek');
                }
            },
            listWeek: {
                text: 'Chi Tiết Từng Ngày',
                click: function () {
                    calendar.changeView('listWeek');
                }
            }
        },

        buttonText: {
            today: 'Hôm nay',
            month: 'Tháng ',
            week: 'Tuần ',
            day: 'Ngày ',
            list: 'Danh sách ',
            prev: 'Trước ',
            next: ' Sau'
        },

        firstDay: 1,
        
        // Khai báo sự kiện
        events: events.map(event => ({
            title: event.Title,
            start: event.Start,
            end: event.End,
            type:'X',
            eventColor: getRandomColor(),
            allDay: false,
            backgroundColor: getRandomColor(),
            textColor: 'white'
        })),        
        locale: 'vi',
        dayHeaderFormat: { weekday: 'long' },


    });
    calendar.render();
}

