import * as client from '../data/client';
//Time ago
import TimeAgo from 'javascript-time-ago';
import en from 'javascript-time-ago/locale/en';
TimeAgo.locale(en);

const timeAgo = new TimeAgo('en-US');
let listOfComments;
let openResponseTab;
let page = 0;
let responsePartial;
let selfId;

// Comment component
if ($("#commentSection").length !== 0) {

        $(document).ready(async () => {
            $(window).on("scroll", async () => {
                if (Math.ceil($(window).scrollTop()) + $(window).height() === $(document).height()) {
                    const response = await client.makeRequest('/umbraco/api/Commentapi/getCommentsById?parentId=' + $("#selfId").data('id') + '&max=10&page=' + ++page);
                    if (response) {
                        for (var singleResponse in response.data) {
                            addCommentToList(response.data[singleResponse]);
                            addSubSection(response.data[singleResponse].Id);
                        }
                    }
                    else {
                        let content = (
                            "<li>"
                            + "Nothing to see here"
                            + "</li>");
                        listOfComments.append(content);
                    }
                }
            });

        });

        $("#commentSection").css("margin-left", "5%");
        $("#commentSection").css("margin-right", "5%");
        $(document).ready(async () => {
            selfId = $("#selfId").data('id');
            firstGet();
        });
    }

  // Acquires the first comments
async function firstGet() {
    openResponseTab = null;
    page = 0;
    const response = await client.makeRequest('/umbraco/api/Commentapi/getCommentsById?parentId=' + $("#selfId").data('id') + '&max=10&page=' + page);
    responsePartial = await client.makeRequest('/umbraco/surface/Commentsurface/getCommentAddForm');
    if (response) {
        clearInside("commentSection");
        $("#commentSection").append(`
                <ul id="listOfComments" class="comments-list"> </ul>
            `);
        clearInside("listOfComments");
        listOfComments = $('#listOfComments');
        addReplyCommentToBase();
        for (var singleResponse in response.data) {
            addCommentToList(response.data[singleResponse]);
            addSubSection(response.data[singleResponse].Id);
        }
    }
}
function addReplyCommentToBase() {

    $("#commentSection").prepend(' <hr><div style="white-space: nowrap;display: inline-block;"><button class="btn btn-light" style="float:right;margin-right:2%;font-size:20px" id= "commentReply' + selfId + '" data-id="' + selfId + '" >New Comment<span style="font-size:20px;margin-left:7%" class="fa fa-comment"></span></button></div>');
    $('#commentReply' + selfId).on(("click"), function (e) {
        openResponsePartial(selfId);
    });
}
//Adds the comment to the content and inserts it to the appropiate section
function addCommentToList(comment) {
    let creationTime = new Date(comment.CreationTime);
    let content = (
        '<li id=' + 'comment' + comment.Id + '>'
        + '<div class="comment-avatar" style="background:white;"> <img class="personImage" src="' + comment.EmployeePhoto + '" alt=""></div>'
        + '  <div class="comment-box">'
        + '      <div class="comment-head">'
        + '          <h6 style="color:#828282;"> ' + comment.Creator + '</h6>'
        + '         <span>' + timeAgo.format(creationTime, 'twitter')+ '</span>'
        + '         <i class="fa fa-reply" id="commentReply' + comment.Id+'" data-id="' + comment.Id + '"+></i>'
        + '         </div>'
        + '         <div class="comment-content"> ' + comment.Description + '</div>'
        + '</div > '
        + '</div>'
        +'</li>'
    );
    if (comment.ParentComment !== 0) {
        $('#' + 'subSection' + comment.ParentComment).append(content);
    }
    else {
        listOfComments.append(content);
    }
    $('#commentReply' + comment.Id).on(("click"), function (e) {
        openResponsePartial(comment.Id);
    });
}
//Adds a section under the given Id
function addSubSection(id) {
    let content = '<ul id=' + 'subSection' + id + ' class="comments-list reply-list"> </ul>';
    $('#' + 'comment' + id).append(content);
    //listOfComments.append(content);
}
//Opens a response model
async function openResponsePartial(id) {
    if (openResponseTab) {
        openResponseTab.remove();
    }
    if (responsePartial) {
        const photoUrl = await client.makeRequest("/umbraco/surface/memberservice/GetCurrentMemberMedia");
        if (id !== selfId) {
            await $('#' + 'subSection' + id).prepend(responsePartial.data);
        }
        else {
            await listOfComments.prepend(responsePartial.data);
        }
        openResponseTab = $("#commentResponse");
        $("#commentParentId").attr("value", id);
        document.getElementById("responseState").addEventListener('DOMSubtreeModified', function () {
                firstGet();
        });
        $("#memberImage").attr("src", photoUrl.data);
        //$('formSubmitButton').on('change',function () {
        //    alert("changes");
        //    firstGet();
        //});
    }
}
function clearInside(id) {
    document.getElementById(id).innerHTML = "";
}
