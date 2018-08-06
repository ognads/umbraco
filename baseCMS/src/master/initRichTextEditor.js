import * as trumbowyg from 'trumbowyg';

/**
 * Generic options for the rich text editor
 */
const rteOptions = {
    // btns: [
    //     ['formatting'],
    //     ['strong', 'em', 'del'],
    //     ['superscript', 'subscript'],
    //     ['link'],
    //     ['insertImage'],
    //     ['justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull'],
    //     ['unorderedList', 'orderedList'],
    //     ['horizontalRule'],
    //     ['removeformat'],
    //     ['viewHTML']
    // ],
    btnsDef: {
        justify: {
            dropdown: ['justifyLeft', 'justifyCenter', 'justifyRight', 'justifyFull'],
            ico: 'justifyLeft',
            hasIcon: true
        }
    },
    btns: [['strong', 'em', 'del'],'justify',  ['unorderedList', 'orderedList'], 'horizontalRule'],
    hideButtonTexts: true,
    minimalLinks: true,
    autogrow: true,
    svgPath: false
};

/**
 * This will change what the jquery will use as a selctor
 * if the need arises can be changed to id or another class
 */
const richTextName=".richtext";


/**
 * Initilizes all of the textareas into a rich text
 * if you want to add more please call initilizeRichTextEditor by hand
 */
$(document).ready(function () {
    addTextAreaListener();
});

/**
 * Adds a listener for the @param richTextName
 */
function addTextAreaListener() {
    $(document).on('click', richTextName, function() {
        initilizeRichTextEditor();
    });
}

/**
 initilizes rich text editor for every element that has
 the class of "richtext"
 For futher reference if you want to create a new rich text editor
 please make sure that it has the "richtext" class
 */
export function initilizeRichTextEditor() {
    $(richTextName).trumbowyg(rteOptions);
    $("button[title=justify]").attr("class", "trumbowyg-justify-button trumbowyg-justifyCenter-button");
}
/**
 * Disables all of the rich text editors
 */
export function disableRichTextEditor() {
    $(richTextName).trumbowyg('disable');
}
/**
 * Gets the text of the richtext as html
 * Currently not needed
 * @param {*} id  id of t he content which will produce the html
 */
export function getRichTextEditorContent(id) {
    return $("#" + id).trumbowyg('html');
}
