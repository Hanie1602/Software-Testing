document.addEventListener('DOMContentLoaded', function () {
    function updateSlots() {
        var interval = document.getElementById("timeInterval").value;
        var startHour = 7;
        var endHour = 18; // Extend to 18:00 for the 1-hour intervals
        var totalSlots = Math.ceil((endHour - startHour) * (60 / interval));
        var slotsPerColumn = Math.ceil(totalSlots / 5);
        var checkboxContainer = document.getElementById("checkboxContainer");
        checkboxContainer.innerHTML = "";
        var prefix = interval === "30" ? "short" : "long";
        var count = 1;

        var slots = [];

        for (var i = startHour; i < endHour; i += interval / 60) {
            var startMinute = i % 1 ? 30 : 0;
            var nextIntervalHour = Math.floor(i + interval / 60);
            var nextIntervalMinute = (i + interval / 60) % 1 ? 30 : 0;

            // Calculate the integer value based on the time slot
            var value;
            if (interval === "30") {
                value = (Math.floor(i) - startHour) * 2 + (startMinute === 30 ? 1 : 0) + 1;
            } else if (interval === "60") {
                value = (Math.floor(i) - startHour) + 23;
            }

            slots.push({
                value: value,
                text: `${pad(Math.floor(i))}:${pad(startMinute)} - ${pad(nextIntervalHour)}:${pad(nextIntervalMinute)}`
            });
        }

        var columns = Array(5).fill().map(() => []);

        slots.forEach((slot, index) => {
            var columnIndex = index % 5;
            columns[columnIndex].push(slot);
        });

        columns.forEach((column, colIndex) => {
            var columnDiv = document.createElement("div");
            column.forEach(slot => {
                var formCheck = document.createElement("div");
                formCheck.className = "form-check";

                var checkbox = document.createElement("input");
                checkbox.type = "checkbox";
                checkbox.className = "form-check-input"; // Apply the custom class
                checkbox.name = "TimeSlots"; // Use the name TimeSlots for all checkboxes
                checkbox.id = "time-" + prefix + "-" + pad(count);
                checkbox.value = slot.value;

                var label = document.createElement("label");
                label.htmlFor = checkbox.id;
                label.className = "custom-font"; // Apply the custom font class
                label.appendChild(document.createTextNode(slot.text));

                formCheck.appendChild(checkbox);
                formCheck.appendChild(label);

                columnDiv.appendChild(formCheck);

                count++;
            });
            checkboxContainer.appendChild(columnDiv);
        });
    }

    document.getElementById("timeInterval").addEventListener("change", updateSlots);

    // Trigger the change event to load default slots
    updateSlots();
});

function pad(n) {
    return n < 10 ? "0" + n : n;
}

function logSelectedValues() {
    var selectedValues = [];
    var checkboxes = document.querySelectorAll('input[name="TimeSlots"]:checked');
    checkboxes.forEach(function (checkbox) {
        selectedValues.push(parseInt(checkbox.value, 10));
    });
    console.log("Selected TimeSlots:", selectedValues);
}