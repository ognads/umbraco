import axios from "axios";
import * as loader from "../ui/loader";
import * as alert from '../ui/alert';
import * as image from '../ui/image';
//Generic function for all ajax requests
export async function makeRequest(url, method = 'GET', body = null, headers = {}, params = {}) {
    //Generic load start for all ajax requests
    loader.showLoader();
    try {
        const { status, data, statusText } = await axios.request({
            url: url,
            method: method,
            data: body,
            headers: headers,
            params: params
        });
        image.checkEverything();
        //Loader stop
        loader.clearLoader();
        return { status, data, statusText };

    } catch (e) {
        console.log(e);
        alert.showToast(e.status, e.message, 'fa fa-ban', 'alert alert-danger');
        loader.clearLoader();
    }
}


