let date = new Date();
let formattedDate =
	date.getFullYear() +
	"-" +
	String(date.getMonth() + 1).padStart(2, "0") +
	"-" +
	String(date.getDate()).padStart(2, "0");

let futureDate = new Date(date.getTime()); // Create a copy of the original date
futureDate.setMonth(futureDate.getMonth() + 1);
let futureFormattedDate =
	futureDate.getFullYear() +
	"-" +
	String(futureDate.getMonth() + 1).padStart(2, "0") +
	"-" +
	String(futureDate.getDate()).padStart(2, "0");

mobiscroll.setOptions({
	theme: "ios",
	themeVariant: "light",
});

mobiscroll.datepicker("#demo-single-select-date", {
	controls: ["calendar"],
	selectMultiple: false,
});

mobiscroll.datepicker("#demo-single-select-datetime", {
	controls: ["calendar", "time"],
	selectMultiple: false,
});

mobiscroll.datepicker("#demo-single-select-timegrid", {
	controls: ["calendar", "timegrid"],
	selectMultiple: false,
});

// Tập trung vào đây
mobiscroll.datepicker("#demo-single-day", {
	min: new Date(),
	controls: ["calendar", "timegrid"],
	display: "inline",
	touchUi: true,
	selectMultiple: false,
	valid: [
		{
			start: formattedDate,
			end: futureFormattedDate,
		},
	],
	valid: [
		{
			start: "02:00",
			end: "06:30",
			recurring: {
				repeat: "daily",
			},
		},
		{
			start: "13:00",
			end: "14:00",
			recurring: {
				repeat: "daily",
			},
		},
	],
	onSet: function (event, inst) {
		// Get the selected date and time
		var selectedDateTime = event.valueText.split(' ');
		var selectedDate = selectedDateTime[0];
		var selectedTime = selectedDateTime[1];

		// Set the value to the hidden input fields
		document.getElementById('selectedDate').value = selectedDate;
		document.getElementById('selectedTime').value = selectedTime;
	}
});


mobiscroll.datepicker("#demo-max-days", {
	controls: ["calendar"],
	display: "inline",
	selectMultiple: true,
	selectMax: 5,
	headerText: "Pick up to 5 days",
});

// Tâp trung vào đây
mobiscroll.datepicker("#demo-counter", {
	controls: ["calendar"],
	display: "inline",
	selectMultiple: true,
	selectCounter: true,
	valid: [
		{
			start: formattedDate,
			end: futureFormattedDate,
		},
	],
});
