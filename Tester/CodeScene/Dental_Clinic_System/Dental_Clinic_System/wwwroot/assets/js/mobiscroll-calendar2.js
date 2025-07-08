document.addEventListener("DOMContentLoaded", function () {
    console.log("DOM fully loaded and parsed");

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

    console.log("Formatted Date: ", formattedDate);
    console.log("Future Formatted Date: ", futureFormattedDate);

    // Check if mobiscroll is loaded
    if (typeof mobiscroll !== 'undefined') {
        console.log("Mobiscroll is loaded");

        mobiscroll.setOptions({
            theme: "ios",
            themeVariant: "light",
            locale: mobiscroll.localeVi,
        });

        mobiscroll.datepicker('#demo-counter', {
            controls: ['calendar'],
            display: 'inline',
            selectMultiple: true,
            selectCounter: true, // Shows the count of selected dates
            valid: [{
                start: formattedDate,
                end: futureFormattedDate
            }],
            onChange: function (event, inst) {
                console.log("onChange event fired"); // Debug log

                // Inspect the structure of event.value
                console.log("Event value:", event.value);

                // Get the list of selected dates
                var selectedDates = event.value.map(function (dateObj) {
                    // Assuming dateObj is a Date object
                    let year = dateObj.getFullYear();
                    let month = String(dateObj.getMonth() + 1).padStart(2, "0");
                    let day = String(dateObj.getDate()).padStart(2, "0");
                    return `${year}-${month}-${day}`;
                });

                // Set the value to the hidden input field or handle as needed
                document.getElementById('Dates').value = selectedDates.join(', ');
                console.log("Selected dates set in hidden input: " + selectedDates.join(', ')); // Debug log
            }
        });

    } else {
        console.error("Mobiscroll is not loaded");
    }
});

