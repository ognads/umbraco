import * as client from '../data/client';
import * as datatable from '../datatable';

// Position component
if ($("#positions").length !== 0) {
    let page = 0;
    $(document).ready(async () => {
        //Creating table and header of the table
        $('#positions').html(`<table id="position" class="table" style="width:100%">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Description</th>
                            <th></th>
                        </tr>
                    </thead>
                </table>`);

        //define columns
        //"aoColumnDefs": [{
        //    "aTargets": [0],
        //    "mData": "download_link",
        //    "mRender": function (data, type, full) {
        //        return '<a href="' + data + '">Download</a>';
        //    }
        //}]

        let columns = [

            {
                data: function (data, type, dataToSet) {
                    return "<a href=" + data.PositionUrl + ">" + data.Name + "</a>";
                }
            },
            { data: "Description" },
            {
                data: function (data) {
                    return `<a href="${data.JobPostListUrl}">Job Posts</a>`;
                }
            }

        ];
        //create table
        datatable.createDatatable("position", '/umbraco/api/positionapi/getpositions?parentId=' + $("#positions").data('id') + '&max=20&page=' + page, columns);

        $(window).on("scroll", async () => {
            if (Math.ceil($(window).scrollTop()) + $(window).height() == $(document).height()) {
                const response = await client.makeRequest('/umbraco/api/positionapi/getpositions?parentId=' + $("#positions").data('id') + '&max=20&page=' + ++page);
                if (response) {
                    datatable.addData("position", response.data);
                }
            }
        });

    });


}
