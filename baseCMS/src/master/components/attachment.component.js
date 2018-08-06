import axios from "axios";
import * as client from '../data/client';
import * as alert from "../ui/alert";
import FormData from "form-data";

if ($("#attachments").length !== 0) {

    let existedFileNames = [];
    let willExistedFileOverwrited = true;
    let isThereExistedFile = false;


    $(document).ready(async () => {

        await updateAlreadyExistedAttachments();

    });

    // ALREADY EXISTED ATTACHMENTS
    // ======================

    //Updating already existed attachments.
    async function updateAlreadyExistedAttachments() {
        let attachments = await getAlreadyExistedAttachments();
        addAttachmentsToPage(attachments);

        //Adding event to delete buttons for attachments
        $('.delete-button').on("click", function () {

            let contentId = $(this).attr("data-id");

            let contentName = $("#" + contentId).text();

            deleteWarning(contentId, contentName);
        });

        //Adding event to download buttons for attachments
        $('.download-button').on("click", function () {

            let parentId = $("#attachments").data('id');

            let contentId = $(this).attr("data-id");
            let contentName = $("#" + contentId).text();

            download(parentId, contentName);
        });

    }

    //Getting already existed attachments from api.
    async function getAlreadyExistedAttachments() {
        const response = await client.makeRequest('/umbraco/api/attachmentapi/getattachmentsbyparent?parentId=' + $("#attachments").data('id'));
        if (response.data) {
            return response.data;
        } else {
            return false;
        }

    }

    //Putting already existed attachments to the page.
    function addAttachmentsToPage(attachments) {
        const attachmentsFrame = $("#attachments_row");
        $("#attachments_row").empty();

        existedFileNames = [];

        for (var i = 0; i < attachments.length; i++) {

            //If UpdatedAt for attachment is min. value then we change it with '-'
            if (attachments[i].UpdatedAt === "0001-01-01T00:00:00")
            {
                attachments[i].UpdatedAt = "-";
            }

            let attachmentElement = `<tr>
                <td><span id="${attachments[i].ID}">${attachments[i].AttachmentName}</span></td>
                <td><span class="badge alert-success pull-left">${attachments[i].AttachmentVersion}</span></td>
                <td title="${attachments[i].CreatedBy}">${attachments[i].CreatedAt.replace(/\T/g, ' ')}</td>
                <td title="${attachments[i].UpdatedBy}">${attachments[i].UpdatedAt.replace(/\T/g, ' ')}</td>
                <td>
                        <button type="button" class="btn pull-right delete-button" data-id="${attachments[i].ID}" aria-hidden="true"><i class="fa fa-trash"></i></button>
                        <a href="/umbraco/surface/attachmentsurface/download?parentId=${ $("#attachments").data('id')}&filename=${attachments[i].AttachmentName }" target="_blank">
                            <button type="button" class="btn pull-right" aria-hidden="true"><i class="fa fa-download"></i></button>
                        </a>                          
                </td>
            </tr>`;

            //Keeping record of already existed attachment name to use other processes
            existedFileNames.push(attachments[i].AttachmentName);

            attachmentsFrame.append(attachmentElement);

        }

    }



    // UPLOADS
    // ======================


    var dropZone = document.getElementById('drop-zone');
    var uploadForm = document.getElementById('js-upload-form');

    const allowedFileTypes = [
        'text/plain',
        'application/pdf',
        'application/vnd.ms-excel',
        'application/vnd.openxmlformats-officedocument.spreadsheetml.sheet',
        'application/msword',
        'image/jpeg',
        'image/png',
    ]

    //Starting upload processes
    var startUpload = async function (files = null) {
        willExistedFileOverwrited = true;
        isThereExistedFile = false;
        let existedFileNamesUploaded = [];

        //Generating FormData object to submit with files.
        var formData = new FormData();
        formData.append("parentId", $("#attachments").data('id'));

        //Appending files to FormData object
        for (var x = 0; x < files.length; x++) {

            //File Size Validation
            if (files[x].size > 6000000) {
                //Resetting file inputs
                resetFileInputs();

                alert.showToast('Validation', 'Individual file size must be under 6MB !! ', 'fa fa-ban', 'alert alert-danger')
                return;

            }

            //File Type Validation
            if (allowedFileTypes.indexOf(files[x].type) === -1) {
                //Resetting file inputs
                resetFileInputs();

                alert.showToast('Validation', 'File is not supported supported file types: pdf, text, excel, csv, jpeg, png ', 'fa fa-ban', 'alert alert-danger')
                return;
            }

            //Controlling if file already exist
            if (existedFileNames.indexOf(files[x].name) !== -1) {

                existedFileNamesUploaded.push(files[x].name);
            }

            formData.append("filesToUpload[]", files[x]);
        }


        if (existedFileNamesUploaded.length > 0) {
            isThereExistedFile = true;
            formData.append("isThereExistedFile", isThereExistedFile);

            await updateWarning(existedFileNamesUploaded, formData);
        } else {
            formData.append("willExistedFileOverwrited", willExistedFileOverwrited);
            formData.append("isThereExistedFile", isThereExistedFile);

            await submitForm(formData);

        }
    }

    //Submitting formData.
    async function submitForm(formData) {

        $("#progress_bar").width('0%');

        //Making request to upload file and create attachment content
        const response = await makeRequest('/umbraco/surface/attachmentsurface/create', 'POST', formData, { 'content-type': 'multipart/form-data' });

        $("#progress_bar").width('0%');

        if (response.data.status === 'Success') {
            //Updating already existed attachments.
            await updateAlreadyExistedAttachments();

            alert.showToast(response.data.status, response.data.message, 'fa fa-check', 'alert alert-success');

        } else {

            let message = response.data.message;
            let files = response.data.files;

            if (files) {
                for (let key in files) {
                    message = message + "<br>" + key + ": " + files[key];
                }

            }

            alert.showToast(response.data.status, message, 'fa fa-ban', 'alert alert-danger')

        }

        //Resetting file inputs
        resetFileInputs();

    }

    async function makeRequest(url, method = 'GET', body = null, headers = {}, params = {}) {

        try {

            //Configuration for progress bar
            const config = {
                onUploadProgress: function (progressEvent) {
                    var percentCompleted = Math.round((progressEvent.loaded * 100) / progressEvent.total);
                    $("#progress_bar").width(percentCompleted + '%');
                }
            };

            const { status, data, statusText } = await axios.post(url, body, config)                 

            return { status, data, statusText };

        } catch (e) {
            console.log(e);
            alert.showToast(e.status, e.message, 'fa fa-ban', 'alert alert-danger');
            loader.clearLoader();
        }
    }

    //Resetting file inputs both drag&drop and browse from computer
    function resetFileInputs() {
        //Deleting file names from above of the drop-zone
        $('#dropped_file_names').html('');
        //Resetting file input
        let fileElem = $("#js-upload-files");
        fileElem.replaceWith(fileElem.val('').clone(true));
    }

    //Adding event listener to attachment form submit button
    uploadForm.addEventListener('submit', function (e) {
        e.preventDefault()

        var uploadFiles = document.getElementById('js-upload-files').files;

        if (uploadFiles.length === 0) {
            alert.showToast("Warning", "There no file(s) to upload", 'fa fa-ban', 'alert alert-danger')

        } else {
            startUpload(uploadFiles)
        }

    })

    dropZone.ondrop = function (e) {

        e.preventDefault();
        this.className = 'upload-drop-zone';

        let fileNameElement = $('#dropped_file_names');
        fileNameElement.empty();

        let fileNames = [];
        for (let i = 0; i < e.dataTransfer.files.length; i++) {
            fileNames[i] = e.dataTransfer.files[i].name;
        }

        fileNameElement.html(fileNames.join());

        startUpload(e.dataTransfer.files)
    }

    dropZone.ondragover = function () {

        this.className = 'upload-drop-zone drop';
        return false;
    }

    dropZone.ondragleave = function () {
        this.className = 'upload-drop-zone';
        return false;
    }

    dropZone.onclick = function () {
        this.data = 'upload-drop-zone';
        return false;
    }

    dropZone.onclick = function () {
        this.data = 'upload-drop-zone';
        return false;
    }

    // DELETE
    // ======================

    let modal = $('#myModal');

    //Delete warning modal
    async function deleteWarning(contentId, contentName) {


        let warningModal = `<div class="modal-header">
        <h4 class="modal-title">Are you sure?</h4>
        <button type="button" class="close" data-dismiss="modal">&times;</button>
        </div>
        <div class="modal-body">
          <p>You are about to delete an attachment : <b>${contentName}</b> </p>
        </div>
        <!-- Modal footer -->
        <div class="modal-footer">
            <button type="button" class="btn btn-danger" data-dismiss="modal" id="delete">Delete</button>
        </div>`;
        $('.modal-content').html(warningModal);
        modal.modal('toggle');
        $('#delete').click(async () => {

            const response = await client.makeRequest(`/umbraco/surface/attachmentsurface/delete?contentId=${contentId}`, 'DELETE');

            if (response.data.status === 'Success') {

                //Updating already existed attachments.
                await updateAlreadyExistedAttachments();
                
                alert.showToast(response.data.status, response.data.message, 'fa fa-check', 'alert alert-success');

            } else {
                alert.showToast(response.data.status, response.data.message, 'fa fa-ban', 'alert alert-danger')
            }


        })
    }

    // DOWNLOAD
    // ======================

    //MAking request to download attachment
    async function download(parentId, filename) {
        await client.makeRequest(`/umbraco/surface/attachmentsurface/download?parentId=${parentId}&filename=${filename}`);
    }

    // UPDATE
    // ======================

    //Update warning modal for fileName conflicts
    async function updateWarning(existedFileNamesUploaded, formData) {
        let tick = true;
        let warningModal = `<div class="modal-header">
        <h4 class="modal-title">You Have File(s) That Already Exist </h4>
        <button type="button" class="close" data-dismiss="modal" id="warning_close">&times;</button>
        </div>
        <div class="modal-body">
          <p>Conflicted File Names : <b>${existedFileNamesUploaded.join()}</b></p>
          <p>Choose one of options below for already existed files</p>
            <input type="radio" name="over-write-option" id="overwrite" checked> Overwrite existed file(s)<br>
            <input type="radio" name="over-write-option" id="keep_existed"> Keep existed file(s) and add new file(s)<br>

        </div>
        <!-- Modal footer -->
        <div class="modal-footer">
            <button type="button" class="btn btn-danger" data-dismiss="modal" id="continue">Continue</button>
        </div>`;
        $('.modal-content').html(warningModal);
        modal.modal('toggle');
        $('#continue').on('click', async function () {

            if (document.getElementById('keep_existed').checked) {
                willExistedFileOverwrited = false;
            }

            formData.append("willExistedFileOverwrited", willExistedFileOverwrited);

            await submitForm(formData);

        });
        $('#warning_close').on('click', function () {
            //Reseting file inputs
            resetFileInputs();
        });

    }
   

}
