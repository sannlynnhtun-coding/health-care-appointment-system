window.chartInterop = {
    renderChart: function (containerId, options) {
        // Apply Emerald Green Theme Defaults
        const defaultOptions = {
            colors: ['#10b981', '#34d399', '#6ee7b7', '#059669', '#065f46'],
            chart: {
                style: {
                    fontFamily: 'Inter, sans-serif'
                },
                backgroundColor: 'transparent'
            },
            title: {
                style: {
                    color: '#0f172a',
                    fontWeight: 'bold'
                }
            },
            credits: {
                enabled: false
            },
            plotOptions: {
                series: {
                    borderRadius: 4
                }
            }
        };

        const finalOptions = Highcharts.merge(defaultOptions, options);
        Highcharts.chart(containerId, finalOptions);
    }
};
