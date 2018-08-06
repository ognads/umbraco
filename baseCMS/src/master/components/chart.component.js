import * as client from '../data/client';

let selfId;
let singleResponse;
let chartRow;
// Chart component
if ($("#charts").length !== 0) {
    console.log("hey");
    $(document).ready(async () => {
        selfId = $("#selfId").data('id');
        getCharts();
    });
}

//Acquires the components
async function getCharts() {
    //gets a list of url for the chart controllers
    const response = await client.makeRequest('/umbraco/api/ChartApi/GetChart?id=' + selfId);
    if (response) {
        openRow();
        //For every url it throws a request and prints to the screen
        for (var singleData in response.data) {
            singleResponse = await client.makeRequest(response.data[singleData]);
            await addChart(singleResponse.data);
        }
    }
}
// Adds it to the responsible section
function addChart(chart) {
    chartRow.append(chart);
}
//opens a row in the charts for more responsiveness
function openRow() {
    $("#charts").append('<div class="row" id="chartRow"></div>');
    chartRow = $("#chartRow");
}
