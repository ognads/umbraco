import * as client from '../data/client';
import * as datatable from '../datatable';

// JobPost component
if ($("#jobposts").length !== 0) {
    let page = 0;
    $(document).ready(async () => {
            //Creating table and header of the table
            $('#jobposts').html(`<table id="jobpost" class="table" style="width:100%">
                    <thead>
                        <tr>
                            <th>Job Description</th>
                            <th>Needed Skills</th>
                            <th>Contact Email</th>
                            <th>Number Of Vacancy</th>
                            <th>End Date</th>
                            <th>Url Of Post</th>
                        </tr>
                    </thead>
                </table>`);

            //define columns
            let columns = [
                {
                    data: function (data, type, row) {
                        return "<a href=" + data.PostUmbracoUrl + ">" + data.JobDescription.substr(0, 20) + '..' + "</a>";
                    }
                },
                {
                    data: "NeededSkills",
                    render: function (data, type, row) {
                        return data.substr(0, 20) + '..';
                    }
                },
                { data: "ContactEmail" },
                { data: "NumberOfVacancy" },
                {
                    data: "EndDate", render: function (value) {
                        let date = new Date(value);
                        let day = date.getDate();
                        let month = date.getMonth()+1;
                        let year = date.getFullYear();
                        return day + '/' + month + '/' + year;
                    } },
                { data: "UrlOfPost" },
            ];
            //create table
        datatable.createDatatable("jobpost", '/umbraco/api/jobpostapi/getjobposts?parentId=' + $("#jobposts").data('id') + '&max=20&page=' + page, columns);

        $(window).on("scroll", async () => {
            if (Math.ceil($(window).scrollTop()) + $(window).height() == $(document).height()) {
                const response = await client.makeRequest('/umbraco/api/jobpostapi/getjobposts?parentId=' + $("#jobposts").data('id') + '&max=20&page=' + ++page);
                if (response) {
                    datatable.addData("jobpost", response.data);
                }
            }
        });

    });

    
}

