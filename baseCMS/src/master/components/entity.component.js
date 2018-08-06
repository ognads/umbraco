import * as client from '../data/client';
import * as datatable from '../datatable';
// Entity component
if ($("#entities").length !== 0) {
    //most of the written codes above will be changed with entity related codes
    let page = 0;

    $(document).ready(async () => {
        //const response = await client.makeRequest('/umbraco/api/employeeapi/getemployees?contentId=' + $("#employees").data('id') +'&max=20&page=' + page);
        //if (response) {
            $('#entities').html(`<table id="entity" class="table" style="width:100%">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th></th>
                        </tr>
                    </thead>
                </table>`);

            //define columns
            let columns = [
                {data: "Name"},
                {
                    "targets": 1,
                    "data": "EmployeeListUrl",
                    "render": function ( data ) {
                        return '<a href="'+data+'">Detail</a>';
                    }
                }
               ];
            //create table

        datatable.createDatatable("entity", '/umbraco/api/entityapi/getentities?contentId=' + 1525 + '&max=20&page=' + page, columns);
        //}

        $(window).on("scroll", async () => {
            if (Math.ceil($(window).scrollTop()) + $(window).height() == $(document).height()) {
                const response = await client.makeRequest('/umbraco/api/entityapi/getentities?contentId=' + 1525 + '&max=20&page=' + ++page);
                if (response) {
                    datatable.addData("entity", response.data);
                }
            }
        });

    });

}




