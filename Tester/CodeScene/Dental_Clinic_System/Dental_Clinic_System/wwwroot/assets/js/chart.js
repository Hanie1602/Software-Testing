//const ctx = document.getElementById("myChart");

//let myChart = new Chart(ctx, {
//    data: {
//        datasets: [
//            {
//                type: "line",
//                label: "Numbers of Votes",
//                data: [12, 19, 5, 10, 2, 3],
//                borderWidth: 1,
//                backgroundColor: ["rgba(255, 99, 132, 0.2)"],
//                borderColor: ["rgb(255, 99, 132)"],
//            },
//            {
//                type: "bar",
//                label: "Numbers of Wins",
//                data: [32, 19, 25, 30, 42, 33],
//                borderWidth: 1,
//                backgroundColor: ["rgba(75, 192, 192, 0.2)"],
//            },
//        ],
//        labels: ["Red", "Blue", "Yellow", "Green", "Purple", "Orange"],
//    },
//});

const ctx = document.getElementById("myChart");

let myChart = new Chart(ctx, {
    data: {
        datasets: [
            {
                type: "line",
                label: "Tổng đặt khám thành công",
                data: @Html.Raw(Json.Serialize(Model.MonthlySuccessfulAppointments)),
                borderWidth: 1,
                backgroundColor: ["rgba(75, 192, 192, 0.2)"],
                borderColor: ["rgb(75, 192, 192)"],
            },
            {
                type: "bar",
                label: "Tổng đặt khám thất bại",
                data: @Html.Raw(Json.Serialize(Model.MonthlyFailedAppointments)),
                borderWidth: 1,
                backgroundColor: ["rgba(255, 99, 132, 0.2)"],
            },
        ],
        labels: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"], // Replace with appropriate labels
    },
    options: {
        plugins: {
            legend: {
                display: true,
                position: 'top',
                labels: {
                    color: 'rgba(0, 0, 0, 1)'
                }
            }
        },
        scales: {
            x: {
                ticks: {
                    color: 'rgba(0, 0, 0, 1)'
                },
                grid: {
                    color: 'rgba(0, 0, 0, 0.1)'
                }
            },
            y: {
                beginAtZero: true,
                ticks: {
                    color: 'rgba(0, 0, 0, 1)'
                },
                grid: {
                    color: 'rgba(0, 0, 0, 0.1)'
                }
            }
        }
    }
});