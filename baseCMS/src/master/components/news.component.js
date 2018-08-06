import * as client from '../data/client';
import * as alert from "../ui/alert";

//Time ago
import TimeAgo from 'javascript-time-ago';
import en from 'javascript-time-ago/locale/en';
TimeAgo.locale(en);

const timeAgo = new TimeAgo('en-US');

let page = 0;

//This variable is showing that there is operation on the search input.
let searchOperationFlag = false;

if ($("#news").length !== 0) {

    $(document).ready(async () => {

        //Fetching news objects and adding to page
        getNewsObjects();

        $("#news_search").keyup(async function () {
            searchOperationFlag = true;
            await getNewsObjects();

        });

        // If there is search operation then we are canceling lazy loading,
        $(window).on("scroll", async () => {
            if (Math.ceil($(window).scrollTop()) + $(window).height() === $(document).height() && !searchOperationFlag) {

                const term = $('#news_search').val();

                let newsObjects = await client.makeRequest('/umbraco/api/newsapi/getnews?parentId=' + $("#news").data('id') + '&max=10&page=' + ++page + '&searchTerm=' + term);
                if (newsObjects.data) {
                    //Adding news to the page
                    addNewsToPage(newsObjects.data);

                } else {
                    alert.showToast('Error', 'News could not be fetched from api', 'fa fa-ban', 'alert alert-danger')
                }
            }
        });

    });

}

//Fetching news objects and adding to page
async function getNewsObjects() {
    const newsObjects = await fetchNews();
    addNewsToPage(newsObjects);

}

function addNewsToPage(newsObjects) {

    //If there is search operation then we are voidind the news container
    if (searchOperationFlag) {
        $('#news_container').html('');
        page = 0;
    }

    //Looping in newObjects for date groups
    for (var newsDate in newsObjects) {
        let timeLineHeader = ``;
        let timeLineBody = ``;

        if (newsObjects.hasOwnProperty(newsDate)) {
            timeLineHeader = `<li class="time-label">
                    <span class="bg-info">
                        ${newsDate}
                    </span>
                </li>`;

            //Looping in dates for new objects
            for (let j = 0; j < newsObjects[newsDate].length; j++) {

                //If create date and update date are not same it means this is updated
                let updatedText = 'CREATED';
                if (newsObjects[newsDate][j].UpdatedAt !== newsObjects[newsDate][j].CreatedAt) {
                    updatedText = 'UPDATED';
                }

                let updateTime = new Date(newsObjects[newsDate][j].UpdatedAt);
                timeLineBody += `<li>
                        <i class="fa fa-envelope bg-primary"></i>
                        <div class="timeline-item">
                            <span class="time"><b>${updatedText}</b>  <i class="fa fa-clock-o"></i> ${timeAgo.format(updateTime)}</span>
                            <h3 class="timeline-header"><a href="#">${newsObjects[newsDate][j].CreatedBy}</a> ${newsObjects[newsDate][j].Title}</h3>
                            <div class="timeline-body" style="padding-left: 20px;">
                                ${newsObjects[newsDate][j].Description}
                            </div>
                        </div>
                    </li>`;

            }
        }

        $("#news_container").append(timeLineHeader);
        $("#news_container").append(timeLineBody);

        searchOperationFlag = false;

    }

}

//Getting news from api.
async function fetchNews() {
    const term = $('#news_search').val();
    const newsObjects = await client.makeRequest('/umbraco/api/newsapi/getnews?parentId=' + $("#news").data('id') + '&max=10&page=' + page + '&searchTerm=' + term);
    if (newsObjects.data) {
        return newsObjects.data;
    } else {
        alert.showToast('Error', 'News could not be fetched from api ', 'fa fa-ban', 'alert alert-danger');
    }

}
