let monthChart;
let dayChart;
let yearChart;

$(function () {
    initPage();
});

function initPage() {
    getChart();
    calendar("ChartDate");

    $("input[name='chartOptions']").change(function () {
        $(".chart").hide();
        $("#" + this.id + "Chart").show();
    });

    $("#ChartDate").change(function () {
        getChart(true);
    });

    $("#searchForm").submit(function (e) {
        e.preventDefault();
        getChart(true);
    });
}

function getChart(update = false) {
    if ($("#searchForm").valid()) { 
        $.post("/api/presence/getChartData", $("#searchForm").serialize())
            .done(function (data) {
                if (update) {
                    dayChart.data.datasets[0].data = data.dayChartDate;
                    dayChart.update();

                    monthChart.data.labels = Array.from(Array(data.daysInMonth).keys()).map(x => ++x);
                    monthChart.data.datasets[0].data = data.monthChartDate;
                    monthChart.update();

                    yearChart.data.datasets[0].data = data.yearChartDate;
                    yearChart.update();
                }
                else {
                    initDayChart(data.dayChartDate);
                    initMonthChart(data.monthChartDate, data.daysInMonth);
                    initYearChart(data.yearChartDate);
                }
            })
            .fail(function () {
                swalWithBootstrapButtons.fire("Oops...", "N�o foi poss�vel carregar as informa��es!\n Se o problema persistir contate o administrador!", "error");
            });
    }
}

function initMonthChart(data, daysInMonth) {
    monthChart = new Chart($("#monthChart"), {
        type: "line",
        data: {
            labels: Array.from(Array(daysInMonth).keys()).map(x => ++x),
            datasets: [{
                label: "Presen\u00E7as",
                backgroundColor: hexToRgba(getStyle("--info"), 10),
                borderColor: getStyle("--info"),
                pointHoverBackgroundColor: "#fff",
                borderWidth: 2,
                data: data
            }]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        min: 0, // it is for ignoring negative step.
                        beginAtZero: true,
                        stepSize: 1
                    }
                }]
            }
        }
    });
}

function initDayChart(data) {
    dayChart = new Chart($("#dayChart"), {
        type: "line",
        data: {
            labels: ["0h", "1h", "2h", "3h", "4h", "5h", "6h", "7h", "8h", "9h", "10h", "11h", "12h", "13h", "14h", "15h", "16h", "17h", "18h", "19h", "20h", "21h", "22h", "23h"],
            datasets: [{
                label: "Presen\u00E7as",
                backgroundColor: hexToRgba(getStyle("--info"), 10),
                borderColor: getStyle("--info"),
                pointHoverBackgroundColor: "#fff",
                borderWidth: 2,
                data: data
            }]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        min: 0, // it is for ignoring negative step.
                        beginAtZero: true,
                        stepSize: 1
                    }
                }]
            }
        }
    });
}

function initYearChart(data) {
    yearChart = new Chart($("#yearChart"), {
        type: "line",
        data: {
            labels: ["Janeiro", "Fevereiro", "Mar\u00E7o", "Abril", "Maio", "Junho", "Julho", "Agosto", "Setembro", "Outubro", "Novembro", "Dezembro"],
            datasets: [{
                label: "Presen\u00E7as",
                backgroundColor: hexToRgba(getStyle("--info"), 10),
                borderColor: getStyle("--info"),
                pointHoverBackgroundColor: "#fff",
                borderWidth: 2,
                data: data
            }]
        },
        options: {
            scales: {
                yAxes: [{
                    ticks: {
                        min: 0, // it is for ignoring negative step.
                        beginAtZero: true,
                        stepSize: 1
                    }
                }]
            }
        }
    });
}