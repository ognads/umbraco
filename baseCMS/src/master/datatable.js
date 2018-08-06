import * as modal from "./ui/modal";
import * as crud from "./crud-operations";


//create data table
//will be added some options later

export function createDatatable(elemId, url, columns) {
    var table = $("#" + elemId).DataTable({
        ajax: {
            url: url,
            type: "GET",
            "contentType": "application/json",
            "dataSrc": ""
        },
        //set row id from data
        rowId: "ID",
        columns: columns,
        select: true,
        //save current state to session
        stateSave: true,
        //set location of buttons
        dom: 'Bfrtip',
        //create buttons
        buttons: [
            {
                extend: "collection",
                text: "Table actions",
                buttons: [{
                    extend: 'copy',
                    text: 'Copy to clipboard'
                }, 'csv', 'excel', 'print', 'selectAll', 'selectNone']
            },
            {
                text: 'Edit',
                className: 'btn btn-warning ' + elemId + 'edit',
                action: function (e, dt, node, config) {
                    //generate form according to operation and element id
                    modal.generateFormModal({
                        operation: 'edit',
                        //ex: employees
                        contentType: elemId,
                        contentId: $('.' + elemId + 'edit').attr('id'),
                        buttonId: 'formSubmitButton'
                    })
                },
                enabled: false
            },
            {
                text: 'Delete',
                className: 'btn btn-danger ' + elemId + 'delete',
                action: function (e, dt, node, config) {
                    //open modal
                    modal.generateWarningModal($('.' + elemId + 'edit').attr('id'), elemId)
                },
                enabled: false
            },
            {
                text: 'Add',
                className: 'btn btn-info ' + elemId + 'add',
                action: function (e, dt, node, config) {
                    modal.generateFormModal({
                        operation: 'add',
                        //ex: employees
                        contentType: elemId,
                        buttonId: 'formSubmitButton'
                    })
                },
                enabled: true
            }
        ],
        //other options
        colReorder: true,
        fixedHeader: true,
        responsive: true,
        paging: false
    });
    //on one row selected enabled edit and delete buttons
    $('#' + elemId).on('select.dt deselect.dt', function () {
        table.buttons(['.' + elemId + 'edit', '.' + elemId + 'delete']).enable(
            table.rows({ selected: true }).indexes().length === 1 ? true : false
        )
        //set id of edit and delete buttons
        $("." + elemId + 'edit').attr('id', table.row({ selected: true }).id())
        $("." + elemId + 'delete').attr('id', table.row({ selected: true }).id())
    });
}
//append data to table
export function addData(elemId, data) {
    $("#" + elemId).DataTable().rows.add(data).draw();
}
