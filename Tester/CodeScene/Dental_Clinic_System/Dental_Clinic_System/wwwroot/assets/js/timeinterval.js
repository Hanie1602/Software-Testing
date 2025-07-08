document.getElementById("timeInterval").addEventListener("change", function () {
	var interval = this.value;
	var startHour = 8;
	var endHour = 14;
	var checkboxContainer = document.getElementById("checkboxContainer");
	checkboxContainer.innerHTML = "";
	var prefix = interval === "30" ? "short" : "long";
	var count = 1;

	for (var i = startHour; i < endHour; i += interval / 60) {
		var startMinute = i % 1 ? 30 : 0;
		var nextIntervalHour = Math.floor(i + interval / 60);
		var nextIntervalMinute = (i + interval / 60) % 1 ? 30 : 0;
		var checkbox = document.createElement("input");
		checkbox.type = "checkbox";
		checkbox.name = "time-" + prefix + "-" + pad(count);
		checkbox.id = "time-" + prefix + "-" + pad(count);
		checkbox.value = i + "-" + nextIntervalHour + ":" + nextIntervalMinute;

		var label = document.createElement("label");
		label.htmlFor = checkbox.name;
		label.appendChild(checkbox);
		label.appendChild(
			document.createTextNode(
				pad(Math.floor(i)) +
					":" +
					pad(startMinute) +
					" - " +
					pad(nextIntervalHour) +
					":" +
					pad(nextIntervalMinute)
			)
		);

		checkboxContainer.appendChild(label);
		checkboxContainer.appendChild(document.createElement("br"));
		count++;
	}
});

function pad(n) {
	return n < 10 ? "0" + n : n;
}
