import * as client from '../data/client';
import {initilizeRichTextEditor} from '../initRichTextEditor';

// Sets the edit mode to false at the start
window.editMode = false;

$(document).ready(() => {
    const el = $('.editable');
    // Pencil icon appended next to the element
    const $editIcon = $('<i class="fa fa-pencil" style="cursor:pointer"></i>');
    $editIcon.appendTo(el);
    el.click(async (e) => {
        const id = e.currentTarget.attributes.id.nodeValue;
        const clickedEl = $(`#${id}`);
        if (editMode === false) {
            const clickedElCopy = clickedEl.clone();
            clickedEl.html(await getEditForm($('#contentType').val(), clickedEl.data('id'), id));

            const buttons = $('.buttons');
            // Submit and Close buttons appended on click
            buttons.append('<button class="btn btn-danger" id="closeEdit">Close</button>');
            $('textarea#' + id).addClass('richtext');
            initilizeRichTextEditor();
            window.editMode = true;
            //Close event handling
            $('#closeEdit').click(() => {
                clickedEl.html(clickedElCopy.html());
                buttons.html('');
                cancelEditMode()
            });
            //Submit event handling
            $('.submit').click(() => {
                localStorage.setItem('editedItem', JSON.stringify({id: id, newValue: $('input#' + id).val()}));
                localStorage.setItem('editedItemArea', JSON.stringify({id: id, newValue: $('textarea#' + id).val()}))
            });
            $('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
                clickedEl.html(clickedElCopy.html());
                buttons.html('');
                cancelEditMode()
            });
        }
    });
});

// Cancels edit mode
function cancelEditMode() {
    setTimeout(() => {
        editMode = false
    }, 100);
}
// Sends request for the detail form
async function getEditForm(contentType, contentId, fieldName) {
    const form = await client
        .makeRequest(`/umbraco/surface/${contentType}surface/geteditdetailform?contentId=${contentId}&fieldName=${fieldName}`);
    const $form = $(form.data);
    const $outerEl = $(`<div class="col-md-5"></div>`);
    $form.children().each((index, input) => {
        if ($(input)[0].id.toLowerCase() !== fieldName.toLowerCase()
            && $(input)[0].name !== '__RequestVerificationToken'
            && $(input)[0].name !== 'ID'
            && $(input)[0].className !== 'buttons'
            && $(input)[0].className !== 'col-md-5'
            && $(input)[0].nodeName !== 'SPAN') {
            $(input).hide();
        } else {
            $(input).wrap($outerEl)
        }
    });
    return $form;
}



