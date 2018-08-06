export function showToast(title, message, icon, classGroup) {
    const alert = `<div class="${classGroup}" id="toast">
    <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;
    </button>
    <h5>
    <i class="icon ${icon}"></i> 
    ${!!title? title:'Error'}
    </h5>
    ${message}
    </div>`;
    const el = $('#toast-container').html(alert).hide();
    el.fadeIn();
}


$(document).ready(() => {
    $('#toast-open-success').click(() => {
        showToast('Success', 'Something was succesful', 'fa fa-check', 'alert alert-success')
    });
});
$(document).ready(() => {
    $('#toast-open-error').click(() => {
        showToast('Error', 'Something went wrong', 'fa fa-ban', 'alert alert-danger')
    });
});

