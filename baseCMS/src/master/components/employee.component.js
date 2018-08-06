import * as client from '../data/client';
import * as datatable from '../datatable';

// Employee component
if ($("#employees").length !== 0) {
    let page = 0;
    $(document).ready(async () => {
        //const response = await client.makeRequest('/umbraco/api/employeeapi/getemployees?contentId=' + $("#employees").data('id')+'&max=20&page='+page);
        //if (response) {
        $('#employees').html(`<table id="employee" class="table" style="width:100%">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Last Name</th>
                            <th>Email</th>
                            <th>Gender</th>
                            <th>Birthday</th>
                            <th>Salary</th>
                        </tr>
                    </thead>
                </table>`);

        //define columns
        let columns = [
            {
                data: function (data, type, dataToSet) {
                    return "<a href=" + data.EmployeeUrl + ">" + data.Name + "</a>";
                }
            },
            { data: "LastName" },
            { data: "Email" },
            { data: "Gender" },
            {
                data: "BirthDay", render: function (value) {
                    let date = new Date(value);
                    let day = date.getDate();
                    let month = date.getMonth()+1;
                    let year = date.getFullYear();
                    return day + '/' + month + '/' + year;
                }
            },
            { data: "Salary" }
        ];
        //create table
        datatable.createDatatable("employee", '/umbraco/api/employeeapi/getemployees?contentId=' + $("#employees").data('id') + '&max=20&page=' + page, columns);
        //}

        $(window).on("scroll", async () => {

            if (Math.ceil($(window).scrollTop()) + $(window).height() == $(document).height()) {
                const response = await client.makeRequest('/umbraco/api/employeeapi/getemployees?contentId=' + $("#employees").data('id') + '&max=20&page=' + ++page);
                if (response) {
                    datatable.addData("employee", response.data);
                }
            }
        });

    });


}

