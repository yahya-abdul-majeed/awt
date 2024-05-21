
var baseURL = 'https://localhost:7168/api/'
var ibexbaseURL = 'https://localhost:7235/api/'


//submit handler for creating contest
function onSubmitHandler(event){
    event.preventDefault()
    console.log("in submit handler");
    let NameInput = document.querySelector('#contestName')
    let DescriptionInput = document.querySelector('#contestDescription')
    let Checkbox = document.querySelector('#publish')
    let OpeningTime = document.querySelector('#opening-time')
    let ClosingTime = document.querySelector('#closing-time')

    let options = {
        method:'POST',
        headers: {
            "content-type":"application/json"
        },
        body:JSON.stringify({
            name: NameInput.value,
            description: DescriptionInput.value,
            is_active: Checkbox.checked,
            opening_time: OpeningTime.value,
            closing_time:ClosingTime.value
        })
    }
    console.log(options.body)

    //check if a contest is selected
    let duplicateContest=false;  
    let duplicateContestId = null;
    if($('#contest-select').val() !== ""){
        duplicateContest = true;
        duplicateContestId = $('#contest-select').val();
    }
    console.log(duplicateContest.toString())
    console.log(duplicateContestId)

    var groupCheckedBoxes = getCheckedBoxes('group-checkbox');
    var localAssigmentsCheckedBoxes = getCheckedBoxes('local-assignment-checkbox')
    var remoteAssigmentsCheckedBoxes = getCheckedBoxes('remote-assignment-checkbox')
    console.log('group checked boxes',groupCheckedBoxes)
    console.log('local checked boxes',localAssigmentsCheckedBoxes)
    console.log('remote checked boxes',remoteAssigmentsCheckedBoxes)

    if(duplicateContest){
        fetch(baseURL+`contest/addcontestfromcontest?contestid=${duplicateContestId}`,options)
        .then(res=>res.json())
        .then(data=>{
            //add groups
            let body = JSON.stringify({
                    groupids: groupCheckedBoxes,
                    contest_id: data.added_contest
                })
            console.log("group body", body)
            fetch(baseURL+'contest/addgroupstocontest',{
                method:'POST',
                headers:{
                    "content-type":"application/json"
                },
                body
            })

        })

    }else{
        fetch(baseURL +'contest/addcontest',options)
        .then(res=>res.json())
        .then(data =>{
            //add groups
            let body = JSON.stringify({
                    groupids: groupCheckedBoxes,
                    contest_id: data.added_contest
                })
            console.log("group body", body)
            fetch(baseURL+'contest/addgroupstocontest',{
                method:'POST',
                headers:{
                    "content-type":"application/json"
                },
                body
            })
            //add local assignments
            body = JSON.stringify(convertToObjectArray(localAssigmentsCheckedBoxes,data.added_contest))
            console.log(body)
            fetch(baseURL+`contest/addassignmentstocontest`,{
                method:'POST',
                headers:{
                    "content-type":"application/json"
                },
                body
            })
            //add remote assignments
            body = JSON.stringify(remoteAssigmentsCheckedBoxes)
            console.log('remote body',body)
            fetch(baseURL+`contest/addRemoteassignmentstocontest?contest_id=${data.added_contest}`,{
                method:'POST',
                headers:{
                    "content-type":"application/json"
                },
                body
            })

        })

    }


}

function editContestOnClickHandler(){

    let contestid = $('#contestEditId').val()

    let NameInput = document.querySelector('#contestEditName')
    let DescriptionInput = document.querySelector('#contestEditDescription')
    let Checkbox = document.querySelector('#Editpublish')
    let OpeningTime = document.querySelector('#edit-opening-time')
    let ClosingTime = document.querySelector('#edit-closing-time')

    let groups = getCheckedBoxes('edit-contest-group-checkbox');
    let assignments = getCheckedBoxes('edit-contest-assignment-checkbox');
    let body = JSON.stringify({
        contest:{
            name: NameInput.value,
            description: DescriptionInput.value,
            is_active: Checkbox.checked,
            opening_time: OpeningTime.value,
            closing_time:ClosingTime.value
        },
        groups: groups || [],
        assignments: assignments || []
    })

    let options = {
        method:'PUT',
        headers:{
            "content-type":"application/json"
        },
        body
    }
    console.log('edit options',options)
    fetch(baseURL+`Contest/UpdateContest/${contestid}`,options)
}


function convertToObjectArray(ids,contestid){
    return ids.map(assignmentid=>({
        assignmentid,
        contestid
    }))
}

function getCheckedBoxes(chkboxName) {
  var checkboxes = document.getElementsByName(chkboxName);
  var checkboxesChecked = [];
  // loop over them all
  for (var i=0; i<checkboxes.length; i++) {
     // And stick the checked ones values onto an array...
     if (checkboxes[i].checked) {
        checkboxesChecked.push(checkboxes[i].value);
     }
  }
  // Return the array if it is non-empty, or null
  return checkboxesChecked.length > 0 ? checkboxesChecked : null;
}

function localAssignmentClickhandler(id){
    console.log('id ',id)
    let element = $(`#local-assignment-detail-div-${id}`);
    if(element.css('display') == 'none'){
        element.css('display','block')
    }
    else{
        element.css('display','none')
    }
}
function editContestAssignmentClickhandler(id){
    console.log('id ',id)
    let element = $(`#edit-contest-assignment-detail-div-${id}`);
    if(element.css('display') == 'none'){
        element.css('display','block')
    }
    else{
        element.css('display','none')
    }
}

function remoteAssignmentClickhandler(id){
    console.log('id ',id)
    let element = $(`#remote-assignment-detail-div-${id}`);
    if(element.css('display') == 'none'){
        element.css('display','block')
    }
    else{
        element.css('display','none')
    }

}
function editContestSelectorOnChangeHandler(event){
    let id= $(event.target).val()
    console.log(id)
    fetch(baseURL+`Contest/GetContest/${id}`)
    .then(res=>res.json())
    .then(contest=>{
        //add to DOM
        console.log(contest)
        let contestEditName = $('#contestEditName');
        let contestEditDescription = $('#contestEditDescription');
        let Editpublish = $('#Editpublish');
        let editOpeningTime = $('#edit-opening-time');
        let editClosingTime = $('#edit-closing-time');
        let contestEditId = $('#contestEditId');    

        contestEditId.val(contest.id)
        contestEditName.val(contest.name)
        contestEditDescription.val(contest.description)
        editOpeningTime.val(contest.opening_time)
        editClosingTime.val(contest.closing_time)
        if(contest.is_active == true){
            Editpublish.prop('checked',true)
        }
        else {
            Editpublish.prop('checked',false)
        }
        //groups
        $('#edit-contest-addremove-groups-items').find("input").each(function(){
            $(this).prop('checked',false);
        })
        fetch(baseURL+`Group/GetGroupsForContest/${contest.id}`)
        .then(res=>res.json())
        .then(groups=>{
            groups.map((group)=>{
                $(`#edit-group-${group.id}`).prop('checked',true);
            })
        })
        //assignments
        $('#edit-contest-assignments-items').find("input").each(function(){
            $(this).prop('checked',false);
        })
        fetch(baseURL+`Contest/GetContestAssignments/${contest.id}`)
        .then(res=>res.json())
        .then(assignments=>{
            assignments?.map((assignment)=>{
                console.log("inside assignment")
                $(`#edit-contest-assignment-${assignment.id}`).prop('checked',true);
            })
        })

    })
}

$(function(){

    
    function InitiateGroupChooser(){

        fetch(baseURL+`group/getgroups`)
        .then(res=>res.json())
        .then(groups=>{
            let fieldsetDiv = $('#group-fieldset-main-div')
            groups.map((group,index)=>{
                let element = `
                <div class='contest-create-group-item' >
                    <input type='checkbox' id='group-${group.name}' name='group-checkbox' value='${group.id}'/>
                    <label for='group-${group.name}'>${group.name}</label>
                </div>
                ` 
            fieldsetDiv.append(element)
            })
        })

    }
    InitiateGroupChooser();
    function InitiateEditContestGroupChooser(){
        fetch(baseURL+`group/getgroups`)
        .then(res=>res.json())
        .then(groups=>{
            let Div = $('#edit-contest-addremove-groups-items')
            groups.map((group,index)=>{
                let element = `
                <div class='contest-edit-group-item' >
                    <input type='checkbox' id='edit-group-${group.id}' name='edit-contest-group-checkbox' value='${group.id}'/>
                    <label for='edit-group-${group.id}'>${group.name}</label>
                </div>
                ` 
            Div.append(element)
            })
        })

    }
    InitiateEditContestGroupChooser();

    function InitiateEditContestAssignments(){
        fetch(baseURL+`assignment/getAssignments`)
        .then(res=>res.json())
        .then(assignments=>{
            let assignmentsDiv = $('#edit-contest-assignments-items');
            assignments.map((assignment)=>{
                let element = $(`
                    <div>
                        <input type='checkbox' id='edit-contest-assignment-${assignment.id}' value='${assignment.id}' name='edit-contest-assignment-checkbox'/>
                    </div>
                `);
                let label = $(`<label>${assignment.name}</label>`);
                let detailDiv = $(`
                    <div id='edit-contest-assignment-detail-div-${assignment.id}' style="display:none">
                        <p>${assignment.description}</p>
                        <p>${assignment.website_link}</p>
                        <p>Points: ${assignment.points}</p>
                    </div>`
                );
                element.append(label);
                element.append(detailDiv);
                let handler = ()=>{
                    editContestAssignmentClickhandler(assignment.id)
                }
                label.on('click',handler);


                assignmentsDiv.append(element);
            })
        })
    }
     InitiateEditContestAssignments();

    function InitiateLocalAssignments(){
        fetch(baseURL+`assignment/getAssignments`)
        .then(res=>res.json())
        .then(assignments=>{
            let assignmentsDiv = $('#local-assignments-items');
            assignments.map((assignment)=>{
                let element = $(`
                    <div>
                        <input type='checkbox' id='local-assignment-${assignment.id}' value='${assignment.id}' name='local-assignment-checkbox'/>
                    </div>
                `);
                let label = $(`<label>${assignment.name}</label>`);
                let detailDiv = $(`
                    <div id='local-assignment-detail-div-${assignment.id}' style="display:none">
                        <p>${assignment.description}</p>
                        <p>${assignment.website_link}</p>
                        <p>Points: ${assignment.points}</p>
                    </div>`
                );
                element.append(label);
                element.append(detailDiv);
                let handler = ()=>{
                    localAssignmentClickhandler(assignment.id)
                }
                label.on('click',handler);


                assignmentsDiv.append(element);
            })
        })
    }
    InitiateLocalAssignments();


    function InitiateRemoteAssignments(){
        fetch(ibexbaseURL+`assignment/getAssignments`)
        .then(res=>res.json())
        .then(assignments=>{
            let assignmentsDiv = $('#remote-assignments-items');
            assignments.map((assignment)=>{
                let element = $(`
                    <div>
                        <input type='checkbox' id='remote-assignment-${assignment.id}' value='${assignment.id}' name='remote-assignment-checkbox'/>
                    </div>
                `);
                let label = $(`<label>${assignment.name}</label>`);
                let detailDiv = $(`
                    <div id='remote-assignment-detail-div-${assignment.id}' style="display:none">
                        <p>${assignment.description}</p>
                        <p>${assignment.website_link}</p>
                        <p>Points: ${assignment.points}</p>
                    </div>`
                );
                element.append(label);
                element.append(detailDiv);
                let handler = ()=>{
                    remoteAssignmentClickhandler(assignment.id)
                }
                label.on('click',handler);


                assignmentsDiv.append(element);
            })

        })

    }
    InitiateRemoteAssignments();

    function InitiateContestSelecter(){ //for duplicating contest
        fetch(baseURL+`Contest/GetContests`)
        .then(res=>res.json())
        .then(contests=>{
            let selectElement = $('#contest-select')
            contests.map((contest)=>{
                let optionElement = $(`
                    <option value='${contest.id}'>${contest.name}</option>
                `)
                selectElement.append(optionElement);
            })
        })
    }
    InitiateContestSelecter();


    function InitiateEditContestSelecter(){
        fetch(baseURL+`Contest/GetContests`)
        .then(res=>res.json())
        .then(contests=>{
            let selectElement = $('#edit-contest-select')
            contests.map((contest)=>{
                let optionElement = $(`
                    <option value='${contest.id}'>${contest.name}</option>
                `)
                selectElement.append(optionElement);
            })
        })
    }
    InitiateEditContestSelecter();

    function refreshEditContest(){
        $('#edit-contest-contestvieweer').find("input").each(function(){
            this.value='';
            $(this).prop('checked',false)
        })
        $('#edit-contest-contestvieweer').find("textarea").each(function(){
            console.log(this.value)
            this.value='';
        })
        $('#edit-contest-select').empty();
        $('#edit-contest-addremove-groups-items').empty();
        $('#edit-contest-assignments-items').empty();

        $('#edit-contest-select').append($(`
            <option value="">--none--</option>
        `))
        InitiateEditContestSelecter();
        InitiateEditContestAssignments();
        InitiateEditContestGroupChooser();

    }


    $('#refresh-contest-create').on('click',function(){
        $('#group-fieldset-main-div').empty();
        $('#local-assignments-items').empty();
        $('#remote-assignments-items').empty();
        $('#contest-select').empty();

        $('#contest-select').append($(`
            <option value="">--none--</option>
        `))
        InitiateContestSelecter()
        InitiateGroupChooser()
        InitiateLocalAssignments()
        InitiateRemoteAssignments()
    })
    $('#refresh-edit-contest').on('click',refreshEditContest)

    $('#contest-create-link-navbar').on('click',function(){
        $('#winContest-body').show()
        $('#winContest-body-edit').hide()

    })
    $('#contest-edit-link-navbar').on('click',function(){
        $('#winContest-body-edit').show()
        $('#winContest-body').hide()
    })
    $('#DeleteContestBtn').on('click',function(){
        let id = $('#contestEditId').val()
        fetch(baseURL+`contest/deleteContest/${id}`,{
            method:'DELETE'
        })
        .then(()=>refreshEditContest())
    })
})