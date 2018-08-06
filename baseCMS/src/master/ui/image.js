import * as configuration from '../data/configuration'
let personNotFoundImage;
let genericNotFoundImage;
let imageTimeOutLength;
let config;

/**
 * When document is ready it acquires the configuration via ajax call
 * then assigns them to the given variables
 * Lastly, it searches for if there's a errored image
 */
$(document).ready(async() => {
    config = await getConfig();
    personNotFoundImage = config.personImageNotFound;
    genericNotFoundImage = config.genericImageNotFound;
    imageTimeOutLength=config.imageTimeOut;
    checkEverything();
});
//AJAX is currently disabled since we're using axios
// /**
//  * On every ajax call searches for the errored images
//  */
// $(document).ajaxComplete(function () {
//     checkEverything();
// });

async function getConfig() {
    return await configuration.getConfiguration();
}
/**
 * Finds the given class and then if the image is not found changes that image to
 * the given imagesource. This is currently a bit taxing. Use sparingly.
 *
 * @param {*} className class name that the system will search for. Only these will be impacted
 * @param {*} imageSource Images will be changed to this
 */
function checkImageErrors(className, imageSource) {
    let images = document.getElementsByClassName(className);
    Array.prototype.forEach.call(images, function (el) {
        if (el.naturalWidth == 0 && el.alt !="Not Found") {
            el.src = imageSource;
            el.alt="Not Found";
        }
    });
}
/**
 * Checks every image tag in the given context
 * This is done so that every unavailable image is converted to the given icon
 * @param {*} imageSource
 */
function checkEveryImageErrors(imageSource) {
    let images = document.getElementsByTagName("img");
    Array.prototype.forEach.call(images, function (el) {
        if (el.naturalWidth == 0 && el.alt !="Not Found") {
            el.src = imageSource;
            el.alt="Not Found";
        }
    });
}
/**
 * Checks for every given function one by one only if there exist a configuration
 * CheckEveryImageErrors always should be on the top
 */
export async function checkEverything() {
    if (config != undefined && config != null) {
        await checkErrors();
    }
}
/**
 * Checks error for the giiven images
 * waits for the imagetimeoutlength first
 * Any additional images should be added to the last line
 */
function checkErrors() {
    setTimeout(function(){
        checkImageErrors("personImage", personNotFoundImage);
        checkEveryImageErrors(genericNotFoundImage);
    },imageTimeOutLength);
}
