import * as modal from "./ui/modal"
import "crud-operations"
import {CrudOperations} from "./crud-operations";

const form = {contentType: 'entity'};

$('.entities-add').click(async function () {

    await modal.generateFormModal({operation: CrudOperations.ADD,...form});
    modal.openModal();
});
$('.entities-delete').click(async function () {

    await modal.generateFormModal({operation: CrudOperations.DELETE,...form,contentId: $(this).attr('id')});
    modal.openModal();
});
$('.entities-update').click(async function () {

    await modal.generateFormModal({operation: CrudOperations.UPDATE,...form,contentId: $(this).attr('id')});
    modal.openModal();
});

