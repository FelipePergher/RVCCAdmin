"use strict";
import "jquery-validation";
import "jquery-validation-unobtrusive";
import "bootstrap-datepicker";
import "bootstrap-datepicker/dist/locales/bootstrap-datepicker.pt-br.min";
import Chart from "chart.js";

export default (function () {

    let monthChart;
    let yearChart;

    global.setupValidator($.validator);

    $(function () {
        initPage();
    });

    function initPage() {
        getChart();

        $("#ChartDate").datepicker({
            clearBtn: true,
            format: "dd/mm/yyyy",
            language: "pt-BR",
            templates: {
                leftArrow: "<span class=\"fas fa-chevron-left\"></span>",
                rightArrow: "<span class=\"fas fa-chevron-right\"></span>"
            }
        });

        $(".chartOptions").click(function () {
            $(".chart").hide();
            $("#" + this.id + "Chart").show();
        });

        $("#ChartDate").change(function () {
            getChart(true);
        });

        $("#searchForm").off("submit").submit(function (e) {
            e.preventDefault();
            getChart(true);
        });
    }

    function getChart(update = false) {
        if ($("#searchForm").valid()) {
            $.post("/api/visitorAttendanceType/getChartData", $("#searchForm").serialize())
                .done(function (data) {
                    if (update) {
                        monthChart.data.labels = Array.from(Array(data.daysInMonth).keys()).map(x => x + 1);
                        monthChart.data.datasets[0].data = data.monthChartDate;
                        monthChart.update();

                        yearChart.data.datasets[0].data = data.yearChartDate;
                        yearChart.update();
                    }
                    else {
                        initMonthChart(data.monthChartDate, data.daysInMonth);
                        initYearChart(data.yearChartDate);
                    }
                })
                .fail(function () {
                    global.swalWithBootstrapButtons.fire("Oops...", "Não foi possível carregar as informações!\n Se o problema persistir contate o administrador!", "error");
                });
        }
    }

    function initMonthChart(data, daysInMonth) {
        monthChart = new Chart($("#monthChart"), {
            type: "line",
            data: {
                labels: Array.from(Array(daysInMonth).keys()).map(x => x + 1),
                datasets: [{
                    label: "Atendimentos",
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
                    label: "Atendimentos",
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

}());
