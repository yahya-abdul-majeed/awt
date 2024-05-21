
function remoteAssignmentCreateClickhandler(id){
    console.log('id ',id)
    let element = $(`#remote-assignment-create-detail-div-${id}`);
    if(element.css('display') == 'none'){
        element.css('display','block')
    }
    else{
        element.css('display','none')
    }
}

function remoteAssignmentCreateInputOnClick(event){
    $('#assignment-create-list-items').find("input").each(function(){
        $(this).prop('checked',false);
        console.log($(this))
    })
    event.target.checked = true;
    fetch(ibexbaseURL+`Assignment/GetAssignment/${event.target.value}`)
    .then(res=>res.json())
    .then(assignment=>{
        $('#editAssignmentName').val(assignment.name)
        $('#editAssignmentParent').val(assignment.id)
        $('#editAssignmentPoints').val(assignment.points)
        $('#editAssignmentWebsite').val(assignment.website_link)
        quill.setText(assignment.description)
        // quill.root.innerHTML = `<b>${assignment.description}</b>`
    })
    

}

$(function(){

    function InitiateAssignmentCreateList(){
        //get remote assignments
        fetch(ibexbaseURL+`assignment/getAssignments`)
        .then(res=>res.json())
        .then(assignments=>{
            let assignmentsDiv = $('#assignment-create-list-items');
            assignments.map((assignment)=>{
                let element = $(`
                    <div>
                        <input onclick="remoteAssignmentCreateInputOnClick(event)" type='checkbox' id='remote-assignment-create-${assignment.id}' value='${assignment.id}' name='remote-assignment-create-checkbox'/>
                    </div>
                `);
                let label = $(`<label>${assignment.name}</label>`);
                let detailDiv = $(`
                    <div id='remote-assignment-create-detail-div-${assignment.id}' style="display:none">
                        <p>${assignment.description}</p>
                        <p>${assignment.website_link}</p>
                        <p>Points: ${assignment.points}</p>
                    </div>`
                );
                element.append(label);
                element.append(detailDiv);
                let handler = ()=>{
                    remoteAssignmentCreateClickhandler(assignment.id)
                }
                label.on('click',handler);


                assignmentsDiv.append(element);
            })

        })

    }
    InitiateAssignmentCreateList();

    function InitiateEditAssignmentSelector(){
        fetch(baseURL + `Assignment/GetAssignments`)
        .then(res=>res.json())
        .then(assignments=>{
            let element = $('#edit-assignment-selector')
            assignments.map((assignment)=>{
                let optionElement = $(`
                    <option value='${assignment.id}'>${assignment.name}</option>
                `)
                element.append(optionElement);
            })
        })
    };
    InitiateEditAssignmentSelector();



    function refreshAssignmentCreate(){
        $('#assignment-create-list-items').empty();
        $('#editAssignmentName').val('')
        $('#editAssignmentParent').val('')
        $('#editAssignmentPoints').val('')
        $('#editAssignmentWebsite').val('')
        quill.setText('')

        InitiateAssignmentCreateList();
    }

    function refreshEditAssignment(){
        $('#edit-assignment-contests-list').empty();
        $('#edit-assignment-selector').empty();
        $('#editAssignmentId2').val('')
        $('#editAssignmentName2').val('')
        $('#editAssignmentParent2').val('')
        $('#editAssignmentPoints2').val('')
        $('#editAssignmentWebsite2').val('')
        quill2.setText('')

        $('#edit-assignment-selector').append($(`<option value=''>--none--</option>`));
        InitiateEditAssignmentSelector();
    }


    $('#assignment-create-link-navbar').on('click',function(){
        $('#winAssignment-body').show()
        $('#winAssignment-body-edit').hide()

    })
    $('#assignment-edit-link-navbar').on('click',function(){
        $('#winAssignment-body-edit').show()
        $('#winAssignment-body').hide()
    })

    $('#refresh-assignment-create').on('click',refreshAssignmentCreate)

    $('#assignment-create-button').on('click',function(){
        let body = JSON.stringify({
            name:$('#editAssignmentName').val(),
            parent_id:$('#editAssignmentParent').val(),
            points:$('#editAssignmentPoints').val(),
            website_link:$('#editAssignmentWebsite').val(),
            description: quill.getText()
        });
        console.log(body);
        fetch(baseURL+`Assignment/CreateAssignment`,{
            method:'POST',
            headers:{
                'content-type':'application/json'
            },
            body
        })
    });

    $('#edit-assignment-selector').on('change',function(){
        fetch(baseURL+`Assignment/GetAssignment/${this.value}`)
        .then(res=>res.json())
        .then(assignment=>{
            $('#editAssignmentId2').val(assignment.id)
            $('#editAssignmentName2').val(assignment.name)
            $('#editAssignmentParent2').val(assignment.parent_id)
            $('#editAssignmentPoints2').val(assignment.points)
            $('#editAssignmentWebsite2').val(assignment.website_link)
            quill2.setText(assignment.description)

            
            fetch(baseURL+`Contest/GetContestsForAssignment/${assignment.id}`)
            .then(res=>res.json())
            .then(contests=>{
                let element = $('#edit-assignment-contests-list')
                element.empty();
                contests.map((contest)=>{
                    let item = $(`
                        <div id='edit-assignment-contests-list-item'>
                            <p><b> ${contest.name}</b></p>
                            <p>Id: ${contest.id}</p>
                            <p>${contest.is_active ? 'Active':'Inactive'}</p>
                        </div>
                    `);
                    element.append(item);
                })
            })

        })
    })

    $('#assignment-update-button').on('click',function(){
        let id = $('#editAssignmentId2').val()
        let body = {
            // id: parseInt(id),
            id,
            name:$('#editAssignmentName2').val() ,
            description:quill2.getText(),
            points:$('#editAssignmentPoints2').val()
        }
        console.log(body);
        fetch(baseURL+`Assignment/UpdateAssignment`,{
            method:'PUT',
            headers:{
                'content-type':'application/json'
            },
            body: JSON.stringify(body)
        })

    })

    $('#assignment-delete-button').on('click',function(){
        let id = $('#editAssignmentId2').val()
        fetch(baseURL+`Assignment/deleteAssignment/${id}`,{
            method:'DELETE'
        })
        .then(()=>refreshEditAssignment())

    });
    
    $('#refresh-assignment-edit').on('click',refreshEditAssignment)

})