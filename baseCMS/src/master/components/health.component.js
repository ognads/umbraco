import * as datatable from '../datatable';
// Health component
const el = $("#healths");
if (el.length !== 0) {
    //most of the written codes above will be changed with entity related codes
    let page = 0;

    $(document).ready(async () => {
        el.html(`<table id="health" class="table" style="width:100%">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Description</th>
                        </tr>
                    </thead>
                </table>`);

        //define columns
        let columns = [
            {data: "Title"},
            { data: "Description" }
        ];
        //create table

        datatable.createDatatable("health", '/umbraco/api/commonapi/gethealth?contentId=' + el.data('id') + '&max=20&page=' + page, columns);
        //}
    });
}




