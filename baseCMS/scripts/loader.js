let loaderOpen = false;

    function showLoader() {
        const loader = `<div  style="margin-bottom: 0!important;position: unset!important" class="card"  id="loader">
    <div class="overlay" >
      <i class="fa fa-refresh fa-spin" style="position:fixed;"></i>
    </div>
  </div>`;
        const el = $('#loader');
        el.html(loader)
}
function clearLoader() {
    $('#loader').empty()
}
$(document).ready(()=> {
    $('#loader-open').click(()=> {
       if(!loaderOpen) {
           showLoader();
           loaderOpen=true;
           setTimeout(()=> {
               clearLoader();
               loaderOpen=false
           },3000)
       }else {
           clearLoader();
           loaderOpen=false
       }
   })
});