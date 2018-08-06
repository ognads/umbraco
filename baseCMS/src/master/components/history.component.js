import * as client from '../data/client';

//Time ago
import TimeAgo from 'javascript-time-ago';
import en from 'javascript-time-ago/locale/en';
TimeAgo.locale(en);
const timeAgo = new TimeAgo('en-US');


const el = $("table#history");
const contentId = $("#selfId").data('id');

if (el.length !== 0) {
    $(document).ready(async () => {
        //get histories
        const url = "/umbraco/api/historyapi/gethistory?contentId=" + contentId + "&max=10&page=1&prop=All";

        const response = await client.makeRequest(url, "GET");
        if (response) {
            //create table with response
            let thead = `  <thead>
                                <th>Field</th>
                                <th>Old Value</th>
                                <th>New Value</th>
                                <th>Date</th>
                            </thead>`;
            let tbody = '<tbody>';

            $.grep(response.data, function (data) {

                let date = new Date(data.UpdateDate.slice(0,-1));

                tbody += '<tr>';
                tbody += '<td>' + data.FieldName + '</td>';
                tbody += '<td>' + data.OldValue + '</td>';
                tbody += '<td>' + data.NewValue + '</td>';
                tbody += '<td> <i class="fa fa-clock-o"></i> ' + timeAgo.format(date) + '</td>';
                tbody += '</tr>';
            });

            tbody += '</tbody>';

            el.append(thead);
            el.append(tbody);
        }

    });
}
