import * as client from '../data/client';

$(document).ready(async () => {
    if ($("select").data('id') === "tags") {
        const selectTags = $("select[data-id='tags']")
        const contentType = selectTags.attr('id').split('-')[0];
        let url = '/umbraco/api/commonapi/gettags?type=' + contentType;
        const response = await client.makeRequest(url, 'GET');
        let data = { id: 0, text: '0' };
        if (response.data !== '[]') {
            data = $.map(response.data, function (obj) {
                return { id: obj.Text, text: obj.Text };
            });
        }
        selectTags.select2({
            tags: true,
            multiple: true,
            tokenSeparators: [','],
            data: data,
            createTag: function (params) {
                const term = $.trim(params.term);

                if (term === '') {
                    return null;
                }
                return {
                    id: term,
                    text: term,
                    newTag: true
                }
            }

        });
        let vals = $("#temp").data('val');

        if ($.isArray(vals) && vals.length!==0) 
            $("select[data-id='tags']").val($("#temp").data('val')).trigger('change');
        else if (vals && vals.length !== 0)
            $("select[data-id='tags']").val($("#temp").data('val').split(',')).trigger('change');
        $("select[data-id='tags']").on("change", async function (e) {
            const request = await client.makeRequest('/umbraco/api/commonapi/settags?type=' + contentType + '&contentId=' + $("#selfId").data('id') + '&tags=' + $("select[data-id='tags']").val(), 'POST');
        });
    }
})


