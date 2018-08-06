import * as client from "../data/client";
import * as alert from '../ui/alert';


const modal = $('#myModal');

export const generateFormModal =
    async (form = {operation: 'add', contentType: 'entity', contentId: 0, buttonId: ''}) => {
        const response = await client
            .makeRequest(`/umbraco/surface/${form.contentType}surface/get${form.operation}form?contentId=${form.contentId}`);

        $('.modal-content').html(response.data);
        //set validator for form
        $.validator.unobtrusive.parse($('#' + `${form.buttonId}`).closest('form'));
        modal.modal('toggle');
    };
// A warning modal for delete actions
export const generateWarningModal = (contentId, contentType) => {
    const warningModal = `<div class="modal-header">
    <h4 class="modal-title">Delete ${contentType}</h4>
    <button type="button" class="close" data-dismiss="modal">&times;</button>
</div>
<div class="modal-body">
  <p>Are you sure?</p>
</div>
<!-- Modal footer -->
<div class="modal-footer">
    <button type="button" class="btn btn-danger" data-dismiss="modal" id="delete">Delete</button>
</div>`;
    $('.modal-content').html(warningModal);
    modal.modal('toggle');
    $('#delete').click(async () => {
        const response = await client.makeRequest(`/umbraco/api/commonapi/delete?contentId=${contentId}`, 'DELETE');
        console.log(response);
        if (response.status === 204) {
            //alert.showToast('Success', `${contentType} is successfully deleted`, 'fa fa-check', 'alert alert-success');
            $("#" + `${contentType}`).DataTable().ajax.reload();
            $('[data-task-id='+contentId+']').remove()
        }

    })

};

