let datatable;

$(document).ready(function(){
   loadDataTable(); 
});

function loadDataTable(){
    datatable = $('#tblBrands').DataTable({
        "ajax":{
            "url":"/Admin/Brand/GetAll"
        },
        "columns":[
            {"data":"name", "width":"20%"},
            {"data":"description", "width":"40%"},
            {
                "data":"active",
                "render":function(data){
                    if(data==true){
                        return "Active";
                    }else{
                        return "Inactive";
                    }
                },"width":"20%"
            },
            {
                "data":"id",
                "render":function(data){
                    return `
                        <div class="text-center">
                            <a href="/Admin/Brand/Upsert/${data}" class="btn btn-success text-white" style="cursor:pointer">
                                <i class="bi bi-pencil-square"></i>
                            </a>
                            <a onclick=Delete("/Admin/Brand/Delete/${data}") class="btn btn-danger text-white" style="cursor:pointer">
                                <i class="bi bi-trash3-fill"></i>
                            </a>
                        </div>
                    `;
                },"width":"20%"
            }
        ]
    });
}

function Delete(url){
    swal({
        title: "Are you sure to delete the brand?",
        text: "Once deleted, you will not be able to recover the data!",
        icon: "warning",
        buttons: true,
        dangerMode: true,
    }).then((willDelete)=>{
        if(willDelete){
            $.ajax({
                type:"POST",
                url:url,
                success:function(data){
                    if(data.success){
                        toastr.success(data.message);
                        datatable.ajax.reload();
                    }else{
                        toastr.error(data.message);
                    }
                }
            });
        }
    });
}