import * as client from '../data/client';
import * as datatable from '../datatable';
// Inventory component
const el = $("#inventories");
if (el.length !== 0) {
    //most of the written codes above will be changed with inventory related codes
    let page = 0;

    $(document).ready(async () => {
        el.html(`<table id="inventory" class="table" style="width:100%">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Assigned To</th>
                            <th>Invoice</th>
                            <th>Warranty End Date</th>
                         </tr>
                    </thead>
                </table>`);

            //define columns
            let columns = [
                {
                    data: function (data, type, dataToSet) {
                        return "<a href=" + data.InventoryUrl + ">" + data.Name + "</a>";
                    }
                },
                {data: "AssignedTo"},
                {data: "Invoice"},
                {data: "WarrantyEndDate"}
            ];
            //create table

            datatable.createDatatable("inventory", '/umbraco/api/inventoryapi/getinventories?contentId=' + $("#inventories").data('id') + '&max=20&page=' + page, columns);
            //}


        $(window).on("scroll", async () => {
            if (Math.ceil($(window).scrollTop()) + $(window).height() == $(document).height()) {
                const response = await client.makeRequest('/umbraco/api/inventoryapi/getinventories?contentId=' + $("#inventories").data('id') + '&max=20&page=' + ++page);
                if (response) {
                    datatable.addData("inventory", response.data);
                }
            }
        });
    })
}




