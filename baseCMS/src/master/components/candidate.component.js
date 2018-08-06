import * as client from '../data/client';
import * as datatable from '../datatable';

// Employee component
if ($("#candidates").length !== 0) {
    let page = 0;
    $(document).ready(async () => {
        $('#candidates').html(`<table id="candidate" class="table" style="width:100%">
                    <thead>
                        <tr>
                            <th>Name</th>
                            <th>Surname</th>
                            <th>Email</th>
                            <th>Phone Number</th>
                            <th>Gender</th>
                            <th>BirthDay</th>
                        </tr>
                    </thead>
                </table>`);

        //define columns
        let columns = [
            {
                data: function (data, type, dataToSet) {
                    return "<a href=" + data.CandidateUrl + ">" + data.CandidateName + "</a>";
                }
            },
            { data: "CandidateSurname" },
            { data: "CandidateEmail" },
            { data: "PhoneNumber" },
            { data: "CandidateGender" },
            {
                data: "CandidateBirthday", render: function (value) {
                    let date = new Date(value);
                    let day = date.getDate();
                    let month = date.getMonth()+1;
                    let year = date.getFullYear();
                    return day + '/' + month + '/' + year;
                }
            }
        ];
        //create table
        datatable.createDatatable("candidate", '/umbraco/api/candidateapi/getcandidates?contentId=' + $("#selfId").data('id') + '&max=20&page=' + page, columns);

        $(window).on("scroll", async () => {

            if (Math.ceil($(window).scrollTop()) + $(window).height() == $(document).height()) {
                const response = await client.makeRequest('/umbraco/api/candidateapi/getcandidates?contentId=' + $("#selfId").data('id') + '&max=20&page=' + ++page);
                if (response) {
                    datatable.addData("candidate", response.data);
                }
            }
        });

        
    });


}

