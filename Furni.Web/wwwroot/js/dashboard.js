function monthlyFinancialReport() {
    var element = document.getElementById('MonthlyFinancialReport');

    if (!element) {
        return;
    }

    var height = parseInt(KTUtil.css(element, 'height')) || 400; // Default height if not found
    var labelColor = KTUtil.getCssVariableValue('--kt-gray-500') || '#a1a5b7';
    var borderColor = KTUtil.getCssVariableValue('--kt-gray-200') || '#ebedf3';
    var baseColor = KTUtil.getCssVariableValue('--kt-primary') || '#3699ff';
    var baseLightColor = KTUtil.getCssVariableValue('--kt-primary-light') || '#d1e5fc';
    var secondaryColor = KTUtil.getCssVariableValue('--kt-info') || '#3c9bff';
    var revenueColor = '#7239ea'; // Set the color for Revenue

    $.get({
        url: '/Admin/Dashboard/GetMonthlyFinancialReports',
        success: function (data) {
            if (!data || !Array.isArray(data) || data.length === 0) {
                console.error('No data available or incorrect format');
                return;
            }

            // Extracting data
            var months = data.map(item => new Date(item.year, item.month - 1).toLocaleString('default', { month: 'short' }));
            var netProfit = data.map(item => item.totalProfit);
            var revenue = data.map(item => item.totalRevenue);
            var expenses = data.map(item => item.totalCost); // Assume expenses are similar to costs for this example

            var options = {
                series: [{
                    name: 'Net Profit',
                    type: 'bar',
                    stacked: true,
                    data: netProfit
                }, {
                    name: 'Revenue',
                    type: 'bar',
                    stacked: true,
                    data: revenue
                }, {
                    name: 'Expenses',
                    type: 'area',
                    data: expenses
                }],
                chart: {
                    fontFamily: 'inherit',
                    stacked: true,
                    height: height,
                    toolbar: {
                        show: false
                    }
                },
                plotOptions: {
                    bar: {
                        stacked: true,
                        horizontal: false,
                        endingShape: 'rounded',
                        columnWidth: ['12%']
                    }
                },
                legend: {
                    show: false
                },
                dataLabels: {
                    enabled: false
                },
                stroke: {
                    curve: 'smooth',
                    show: true,
                    width: 2,
                    colors: ['transparent']
                },
                xaxis: {
                    categories: months,
                    axisBorder: {
                        show: false
                    },
                    axisTicks: {
                        show: false
                    },
                    labels: {
                        style: {
                            colors: labelColor,
                            fontSize: '12px'
                        }
                    }
                },
                yaxis: {
                    max: Math.max(...netProfit, ...revenue, ...expenses) * 1.1, // Adjust max value based on data
                    labels: {
                        style: {
                            colors: labelColor,
                            fontSize: '12px'
                        }
                    }
                },
                fill: {
                    opacity: 1
                },
                states: {
                    normal: {
                        filter: {
                            type: 'none',
                            value: 0
                        }
                    },
                    hover: {
                        filter: {
                            type: 'none',
                            value: 0
                        }
                    },
                    active: {
                        allowMultipleDataPointsSelection: false,
                        filter: {
                            type: 'none',
                            value: 0
                        }
                    }
                },
                tooltip: {
                    style: {
                        fontSize: '12px'
                    },
                    y: {
                        formatter: function (val) {
                            return '$' + val;
                        }
                    }
                },
                colors: [baseColor, revenueColor, baseLightColor],
                grid: {
                    borderColor: borderColor,
                    strokeDashArray: 4,
                    yaxis: {
                        lines: {
                            show: true
                        }
                    },
                    padding: {
                        top: 0,
                        right: 0,
                        bottom: 0,
                        left: 0
                    }
                }
            };

            var chart = new ApexCharts(element, options);
            chart.render();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error('Error fetching data:', textStatus, errorThrown);
        }
    });
}



function salesThisMonth() {
    var element = document.getElementById('SalesThisMonth');

    if (!element) {
        return;
    }

    // Fetch height and color variables
    var height = parseInt(KTUtil.css(element, 'height'));
    var labelColor = KTUtil.getCssVariableValue('--kt-gray-500') || '#6c757d'; // Default gray color
    var borderColor = KTUtil.getCssVariableValue('--kt-gray-200') || '#e9ecef'; // Default light gray color
    var baseColor = KTUtil.getCssVariableValue('--kt-info') || '#17a2b8'; // Default info color
    var lightColor = KTUtil.getCssVariableValue('--kt-info-light') || '#e0f7fa'; // Default light info color

    $.get({
        url: '/Admin/Dashboard/GetSalesThisMonth',
        success: function (data) {
            // Extract dates and sales from the response
            var dates = data.map(item => new Date(item.daily).toLocaleDateString('en-US', { day: 'numeric', month: 'short' }));
            var sales = data.map(item => parseFloat(item.salesInDay.toFixed(2))); // Round to 2 decimals

            var options = {
                series: [{
                    name: 'Sales',
                    data: sales
                }],
                chart: {
                    fontFamily: 'inherit',
                    type: 'area',
                    height: height,
                    toolbar: {
                        show: false
                    }
                },
                plotOptions: {},
                legend: {
                    show: false
                },
                dataLabels: {
                    enabled: false
                },
                fill: {
                    type: 'solid',
                    opacity: 1
                },
                stroke: {
                    curve: 'smooth',
                    show: true,
                    width: 3,
                    colors: [baseColor]
                },
                xaxis: {
                    categories: dates,
                    axisBorder: {
                        show: false
                    },
                    axisTicks: {
                        show: false
                    },
                    labels: {
                        style: {
                            colors: labelColor,
                            fontSize: '12px'
                        }
                    },
                    crosshairs: {
                        position: 'front',
                        stroke: {
                            color: baseColor,
                            width: 1,
                            dashArray: 3
                        }
                    },
                    tooltip: {
                        enabled: true,
                        formatter: undefined,
                        offsetY: 0,
                        style: {
                            fontSize: '12px'
                        }
                    }
                },
                yaxis: {
                    labels: {
                        style: {
                            colors: labelColor,
                            fontSize: '12px'
                        }
                    }
                },
                states: {
                    normal: {
                        filter: {
                            type: 'none',
                            value: 0
                        }
                    },
                    hover: {
                        filter: {
                            type: 'none',
                            value: 0
                        }
                    },
                    active: {
                        allowMultipleDataPointsSelection: false,
                        filter: {
                            type: 'none',
                            value: 0
                        }
                    }
                },
                tooltip: {
                    style: {
                        fontSize: '12px'
                    },
                    y: {
                        formatter: function (val) {
                            return '$' + val;
                        }
                    }
                },
                colors: [lightColor],
                grid: {
                    borderColor: borderColor,
                    strokeDashArray: 4,
                    yaxis: {
                        lines: {
                            show: true
                        }
                    }
                },
                markers: {
                    strokeColor: baseColor,
                    strokeWidth: 3
                }
            };

            var chart = new ApexCharts(element, options);
            chart.render();
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error('Error fetching data:', textStatus, errorThrown);
        }
    });
}

salesThisMonth();
monthlyFinancialReport();
