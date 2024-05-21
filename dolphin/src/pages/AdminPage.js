import $ from 'jquery'
import { useEffect } from 'react'

function AdminPage(){

    let Root2 = $('#root2')
    let ContestBtn = $('#contestBtn')
    let GroupBtn = $('#groupBtn')
    let AssignmentBtn = $('#AssignmentBtn')
    console.log("buttons")


    useEffect(()=>{
        Root2.show()
        ContestBtn.on('click',window.createContestWindow);
        GroupBtn.on('click',window.createGroupWindow);
        AssignmentBtn.on('click',window.createAssignmentWindow);

        return ()=>{
            Root2.hide()
            ContestBtn.off('click')
            GroupBtn.off('click')
            AssignmentBtn.off('click')
        }
    },[])

    


    return (
        <>
        </>
    );


}


export default AdminPage;

