import * as datatable from '../datatable';
// Social security component
const el = $("#social-medias");
if (el.length !== 0) {
    //most of the written codes above will be changed with entity related codes
    let page = 0;
    let getLocation = function (href) {
        let l = document.createElement("a");
        l.href = href;
        return l;
    };
    $(document).ready(async () => {
        el.html(`<table id="socialmedia" class="table" style="width:100%">
                    <thead>
                        <tr>
                            <th>Title</th>
                            <th>Url</th>
                        </tr>
                    </thead>
                </table>`);

        //define columns
        let columns = [
            {data: "Name"},
            {
                data: function (data, type, dataToSet) {
                    return "<a href=" + data.Url + "><img style='width:20px;height:20px' src=" + "'" + getLocation(data.Url).protocol + "//" + getLocation(data.Url).hostname + "/favicon.ico'" +"/>   " + data.Url + "</a>";
                }
            }
        ];
        //create table

        datatable.createDatatable("socialmedia", '/umbraco/api/commonapi/getsocialmedia?contentId=' + el.data('id') + '&max=20&page=' + page, columns);
        //}
    });
    
}




