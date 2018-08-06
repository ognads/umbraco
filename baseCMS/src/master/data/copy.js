import ClipboardJS from 'clipboard';
//import TurndownService from 'turndown';
const copyHtml = $('#copy-element').html();
//const turnDownService = new TurndownService();
//const markdown = turnDownService.turndown(copyHtml);
//$('#copy-target').text(markdown);

const clipboard = new ClipboardJS('#copy');

clipboard.on('success', function(e) {
    console.log(e);
    //alert('Copied to clipboard')
});
clipboard.on('error', function(e) {
    console.log(e);
});
