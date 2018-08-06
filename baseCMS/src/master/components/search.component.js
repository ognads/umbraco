import * as client from '../data/client';

$("#search").autocomplete({
    source: `/umbraco/api/searchapi/search?term=` + $("#search").val(),
    minLength: 2
})


//$("#search").on("keyup", async (e) => {
//    let term = $("#search").val();
//    let response = await client.makeRequest(`/umbraco/api/searchapi/search?term=${term}`,"POST");
//})
