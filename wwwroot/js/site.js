// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(function(){
    $('.table').DataTable();
   $('.deleteBtn').click(function(e) {
       Swal.fire({
           title: 'Are you sure?',
           text: "You won't be able to revert this!",
           icon: 'warning',
           showCancelButton: true,
           confirmButtonColor: '#3085d6',
           cancelButtonColor: '#d33',
           confirmButtonText: 'Yes, delete it!'
           }).then((result) => {
           if (result.isConfirmed) {
               Swal.fire(
               'Deleted!',
               'Your file has been deleted.',
               'success'
               )
               var btn = $(this);
               var id = btn.data("id");
               $('#courseId').val(id);
               $('#courseDeleteForm').submit();
           }
        

       });
   })

})