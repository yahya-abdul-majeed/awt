var baseURL = 'https://localhost:7168/api/';
let all_users = null;

function editGroupSelectorOnChangeHandler(event){
    let id= $(event.target).val()
    console.log(id)
    fetch(baseURL+`Group/GetGroup/${id}`)
    .then(res=>res.json())
    .then(group=>{
        $('#groupEditId').val(group.id)
        $('#groupEditName').val(group.name)
        $('#groupEditDescription').val(group.description)
    });
    //users
    fetch(baseURL+`User/GetUsersForGroup/${id}`)
    .then(res=>res.json())
    .then(users=>{
        $('#group-edit-user-list').find("input").each(function(){
            $(this).prop('checked',false);
        })
        users.map(user=>{
            $(`#edit-group-user-list-item-${user.id}`).prop('checked',true);
        })

    })


}


function groupCreateSubmitHandler(event){
    event.preventDefault()
    let groupName = $('#groupName')
    let groupDescription = $('#groupDescription')

    var checkedUsers = getCheckedBoxes('users-checkbox')
    var checkedGroups = getCheckedBoxes('groupwindow-group-checkbox')

    fetch(baseURL + `group/addgroup`,{
        method:'POST',
        headers:{
            "content-type":"application/json"
        },
        body:JSON.stringify({
            name:groupName.val(),
            description:groupDescription.val()
        })
    })
    .then(res=>res.json())
    .then(data=>{
        if(checkedUsers){
            console.log("checkedusers",checkedUsers)
            fetch(baseURL+`group/adduserstogroup?groupid=${data.added_group}`,{
                method:'POST',
                headers:{
                    "content-type":'application/json'
                },
                body:JSON.stringify(checkedUsers)
            })
        }
        if(checkedGroups){
            console.log("checkedgroups",checkedGroups)
            checkedGroups.map((group)=>{
                fetch(baseURL+`group/addgrouptogroup?groupid_1=${data.added_group}&groupid_2=${group}`)
            })

        }
    })
}

function searchUsers(){
    var search_term = $('#user-search-box').val() 
    console.log("search term ",search_term)
    var new_users = all_users.filter((user)=>user.username.includes(search_term) || user.email.includes(search_term))
    let UsersList = $('#add-users-list');
    UsersList.empty();
    new_users.map((user)=>{
        let listItem = $(`
            <div id='user-list-item-${user.id}'>
                <input type='checkbox' name='users-checkbox' value='${user.id}'/>
                <label>
                    <p><b>${user.username}</b></p>
                    <p>${user.email}</p>
                </label>
            </div> 
        `);
        UsersList.append(listItem);
    })

}

$(function(){
    
    function InitiateUsersList(){
        fetch(baseURL + `User/GetUsers`)
        .then(res=>res.json())
        .then(users=>{
            all_users = users;
            let UsersList = $('#add-users-list');
            users.map((user)=>{
                let listItem = $(`
                    <div id='user-list-item-${user.id}'>
                        <input type='checkbox' name='users-checkbox' value='${user.id}'/>
                        <label>
                            <p><b>${user.username}</b></p>
                            <p>${user.email}</p>
                        </label>
                    </div> 
                `);
                UsersList.append(listItem);
            })
        })
    }
    InitiateUsersList();

    function InitiateGroupsList(){
        fetch(baseURL+`group/getgroups`)
        .then(res=>res.json())
        .then(groups=>{
            let groupsList= $('#add-groups-list')
            groups.map((group,index)=>{
                let listItem= `
                <div>
                    <input type='checkbox' id='groupwindow-group-${group.name}' name='groupwindow-group-checkbox' value='${group.id}'/>
                    <label for='groupwindow-group-${group.name}'>${group.name}</label>
                </div>
                ` 
            groupsList.append(listItem)
            })
        })
    }
    InitiateGroupsList();

    function InitiateEditGroupUsersList(){
        fetch(baseURL + `User/GetUsers`)
        .then(res=>res.json())
        .then(users=>{
            // all_users = users;
            let UsersList = $('#group-edit-user-list');
            users.map((user)=>{
                let listItem = $(`
                    <div id="edit-group-user-list-item-parent-${user.id}">
                        <input type='checkbox' id='edit-group-user-list-item-${user.id}' name='edit-group-users-checkbox' value='${user.id}'/>
                        <label>
                            <p><b>${user.username}</b></p>
                            <p>${user.email}</p>
                        </label>
                    </div> 
                `);
                UsersList.append(listItem);
            })
        })
    }
    InitiateEditGroupUsersList();
    function InitiateEditGroupSelector(){
        fetch(baseURL+`Group/GetGroups`)
        .then(res=>res.json())
        .then(groups=>{
            let element = $('#edit-group-select')
            groups.map((group)=>{
                let item = $(`
                    <option value=${group.id}>${group.name}</option>
                `);
                element.append(item);
            })
        })
    }
    InitiateEditGroupSelector();

    $('#refresh-group-create').on('click',function(){
        $('#add-users-list').empty();
        $('#add-groups-list').empty();

        InitiateGroupsList();
        InitiateUsersList();

    });
    $('#refresh-group-edit').on('click',refreshGroupEdit)

    function refreshGroupEdit(){
        $('#group-edit-user-list').empty();
        $('#edit-group-select').empty();
        $('#groupEditId').val('')
        $('#groupEditName').val('')
        $('#groupEditDescription').val('')

        $('#edit-group-select').append($(`<option value=''>--none--</option>`))
        InitiateEditGroupSelector();
        InitiateEditGroupUsersList();



    }

    $('#group-create-link-navbar').on('click',function(){
        $('#winGroup-body').show()
        $('#winGroup-body-edit').hide()

    })
    $('#group-edit-link-navbar').on('click',function(){
        $('#winGroup-body-edit').show()
        $('#winGroup-body').hide()
    })

    $('#group-edit-update-btn').on('click',function(){
        let userids = getCheckedBoxes('edit-group-users-checkbox')

        let id = $('#groupEditId').val()
        let name= $('#groupEditName').val()
        let description= $('#groupEditDescription').val()

        let body = JSON.stringify({
            group:{
                name,
                description
            },
            userids: userids?.map((userid)=> parseInt(userid)) || [] 
        });
        console.log("body here",body)

        fetch(baseURL+`Group/UpdateGroup/${id}`,{
            method:'PUT',
            headers:{
                "content-type":"application/json"
            },
            body
        })

        console.log("fetch finish")
    })

    $('#group-edit-delete-btn').on('click',function(){
        let id = $('#groupEditId').val()
        fetch(baseURL+`group/deleteGroup/${id}`,{
            method:'DELETE'
        })
        .then(()=>refreshGroupEdit())
    })
})