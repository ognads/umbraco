import * as LocalStorage from '../data/localstorage';
import * as client from '../data/client';
import {getConfiguration} from "../data/configuration";
import {generateWarningModal} from "./modal";

// Creates a board component
const board = $("#board");
board.hide();
if (board.length !== 0) {
    $(document).ready(async () => {
        let App = function () {
            async function init() {
                await preset();
                addStatusColumns();
                draggable();
                droppable();
                openCard();
                await printNotes();
                setUpStatusToggling();
            }

// Adds the static statuses to the localstorage
            async function preset() {
                $('#remove').on('click', function (e) {
                    e.preventDefault();
                });

                const defaultCandidate = {
                    id: 1,
                    title: 'Cem Pesket',
                    status: 'phone-interview'
                };

                if (!LocalStorage.get('appInitialized', true)) {
                    LocalStorage.set('taskCounter', 1);
                    const configs = await getConfiguration();
                    //const {data} = await client.makeRequest('/umbraco/api/commonapi/getconfig');
                    const values = configs.recruitmentStates;
                    const statuses = values.split(',').map(value => {
                        const statusValue = value.replace('[', '').replace(']', '').replace(new RegExp('"', 'g'), '');
                        const statusKey = statusValue.toLowerCase().replace(' ', '-');
                        return {key: statusKey, value: statusValue};
                    });
                    LocalStorage.set('status', JSON.stringify(statuses));
                    LocalStorage.set('candidate', JSON.stringify(defaultCandidate));
                    //LocalStorage.set('appInitialized', true);
                }
            }

// Sets the status columns with the values from localstorage
            function addStatusColumns() {

                const statusArr = JSON.parse(LocalStorage.get('status'));
                const headerObj = $('header ul');
                const myDashboard = $('#dashboard');
                statusArr.map(function (item) {
                    const newLi = $('<li>' + item.value + '</li>');
                    newLi.attr('data-id', item.key);
                    headerObj.append(newLi);

                    const newDiv = $('<div id=' + item.key + ' class=' + item.key + '></div>');
                    myDashboard.append(newDiv);
                });

            }

// Event handling for dragging items
            function draggable() {
                $('.board-card').draggable({
                    handle: 'div',
                    revert: false,
                    helper: function (e) {
                        //Cloning element, to enable draggable elements move out of scrollable parent element.
                        const original = $(e.target).hasClass("ui-draggable") ? $(e.target) : $(e.target).closest(".ui-draggable");
                        return original.clone().css({
                            width: original.width()
                        });
                    },
                    scroll: false,
                    cursor: 'move',
                    start: function (event, ui) {
                    },
                    stop: function (event, ui) {
                    }
                });
            }

// Event handling for dropped items
            function droppable() {
                const droppableConfig = {
                    tolerance: 'pointer',
                    drop: async function (event, ui) {
                        const elm = ui.draggable,
                            parentId = elm.parent().attr('id'),
                            currentId = $(this).attr('id'),
                            taskId = elm.data('task-id');

                        if ($(this).attr('id') === 'remove') {
                            //Deletes task
                            elm.remove();
                            LocalStorage.remove('task-' + taskId);
                            $('#removed-task-notification').text('Task removed successfully').removeClass('hide');


                            setTimeout(function () {
                                $('#removed-task-notification').text('').addClass('hide');
                            }, 3000);
                        } else {
                            //Moves task
                            if (parentId !== currentId) {
                                $(elm).detach().removeAttr('style').appendTo(this);

                                const candidate = JSON.parse(LocalStorage.get('candidate'));
                                candidate.status = currentId;

                                LocalStorage.set('candidate', JSON.stringify(candidate));
                                await client
                                    .makeRequest(`/umbraco/api/candidateapi/updatecandidatestatus?candidateId=${taskId}&status=${candidate.status}`, 'POST')
                            }
                        }

                        $(this).removeClass('dragged-over');
                    },
                    over: function (event, ui) {
                        $(this).addClass('dragged-over');
                    },
                    out: function (event, ui) {
                        $(this).removeClass('dragged-over');
                    }
                };

                $('#dashboard > div, #remove').droppable(droppableConfig);
            }

            function openCard() {
                $(document).on('click', '.expand-card', function (e) {
                    $(this).parent().toggleClass('expanded');
                    e.preventDefault();
                });
            }

// Gets all the candidates from server
            async function getAllCandidates(positionId) {
                return await client.makeRequest('/umbraco/api/candidateapi/getcandidateswithstatus?contentId=' + positionId);
            }

            function generateCard(candidate) {

                const card = `<div class="board-card" data-task-id="${candidate.ID}">
              <div class="card card-info card-outline" style="cursor: move;">
              <div class="card-header"  style="padding-left: 0.25rem">
              <button class="btn btn-link" style="color: red; float: right; padding: unset" id="delete-candidate-${candidate.ID}"><span class="fa fa-trash"></span></button>
              <h3 class="card-title" style="font-size: 1rem"><a href="${candidate.CandidateUrl}">${candidate.CandidateName} ${candidate.CandidateSurname}</a></h3>
              </div>
              <!-- /.card-header -->
              <div class="card-body">
               ${candidate.ResourceDescription}
               
               ${candidate.CandidateSkills}
               
             </div>
              <!-- /.card-body -->
              </div>
              <!-- /.card -->
          
              </div>`;

                $('#dashboard #' + candidate.CandidateStatus).append(card);
                $('#delete-candidate' + '-' + candidate.ID).click(async () => {
                    generateWarningModal(candidate.ID, 'candidate');
                    //await printNotes()
                })
            }

// Writes all candidate on the proper statuses
            async function printNotes() {

                const status = JSON.parse(LocalStorage.get('status'));
                const positionId = LocalStorage.get('positionId');
                const l = status.length;
                let newList = [];
                const candidates = await App.getAllNotes(positionId);
                const list = candidates.data;

                for (let i = 0; i < l; i++) {
                    let filteredList = list.filter((data) => {
                        return data.CandidateStatus === status[i].key
                    });
                    newList.push(...filteredList)
                }
                if (newList && newList.length !== 0) {
                    console.log('new list is not empty');
                    for (let i = 0; i < newList.length; i++) {
                        generateCard(newList[i]);
                        draggable();
                    }
                    adjustSizes(status.length, newList.length);
                } else {
                    $('.board-wrapper').html('No candidates');
                }
                if (!$('#toggle-position').hasClass('closed'))
                    board.show();

            }

            function setUpStatusToggling() {
                const toggleButton = $('#toggle-position');
                const modal = $('#myModal');
                toggleButton.click(async () => {


                    const positionId = LocalStorage.get('positionId');

                    if (toggleButton.hasClass('closed')) {
                        const warningModal = `<div class="modal-header">
                    <h4 class="modal-title">Open Position</h4>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">
                    <p>Are you sure?</p>
                    </div>
                    <!-- Modal footer -->
                    <div class="modal-footer">
                      <button type="button" class="btn btn-danger" data-dismiss="modal" id="open-position">Ok</button>
                    </div>`;
                        $('.modal-content').html(warningModal);
                        modal.modal('toggle');
                        $('#open-position').click(async () => {
                            toggleButton.removeClass('closed');
                            toggleButton.addClass('open');
                            const response = await client.makeRequest(`/umbraco/api/positionapi/toggleposition?positionId=${positionId}&status=open`, 'POST');
                            if (response.status === 204) {
                                $('.board-card').remove();
                                await printNotes();
                                $('#board').show();
                                $('#toggle-position').html('close position');
                            }
                        });


                    } else if (toggleButton.hasClass('open')) {
                        const warningModal = `<div class="modal-header">
                    <h4 class="modal-title">Close Position</h4>
                    <button type="button" class="close" data-dismiss="modal">&times;</button>
                    </div>
                    <div class="modal-body">
                    <p>Are you sure?</p>
                    </div>
                    <!-- Modal footer -->
                    <div class="modal-footer">
                      <button type="button" class="btn btn-danger" data-dismiss="modal" id="close-position">Ok</button>
                    </div>`;
                        $('.modal-content').html(warningModal);
                        modal.modal('toggle');

                        $('#close-position').click(async () => {
                            toggleButton.removeClass('open');
                            toggleButton.addClass('closed');
                            const response = await client.makeRequest(`/umbraco/api/positionapi/toggleposition?positionId=${positionId}&status=closed`, 'POST');
                            if (response.status === 204) {
                                $('#board').hide();
                                $('#toggle-position').html('open position');
                            }
                        });

                    }

                });
            }


            return {
                init: init,
                getAllNotes: getAllCandidates
            }
        }();

        App.init();
    });
}

function adjustSizes(statusLength, dataLength) {
    const el = $("div.ui-droppable");
    const header = $("header.clear");
    el.height(300 * dataLength);
    el.css({width: 100 / statusLength + '%'});
    header.find("li").width(100 / statusLength + '%');
    header.width(100 + '%')
}






