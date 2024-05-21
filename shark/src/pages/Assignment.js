import { useEffect, useState } from "react";
import ReactQuill from "react-quill";
import 'react-quill/dist/quill.snow.css';
import { requests } from "../data/requests";
import WebsiteTree from "../components/WebsiteTree";
import { Editor } from "@monaco-editor/react";
import { Utility } from "../Utility";

export default function Assignment(){
    const [editorValue, setEditorValue] = useState('');// for quill editor
    const [assignments,setAssignments] = useState([])
    const [websites,setWebsites] = useState([])
    const [addedSkeletons,setAddedSkeletons] = useState([])


    const [skeletons,setSkeletons] = useState([])
    const [currentSkeleton,setCurrentSkeleton] = useState(null)

    /*logs*/
    console.log('added',addedSkeletons)
    /*logs*/

    useEffect(()=>{
        console.log("assignment useEffect running..")
       requests.GetAssignmentsForShark() 
       .then(res=>res.json())
       .then(assignments=>{
        setAssignments(assignments);
       })

       requests.GetWebsiteTree()
       .then(res=>res.json())
       .then(data=>{
        setWebsites(data,console.log(data));
       })

       requests.GetSkeletons()
       .then(res=>res.json())
       .then(data=>setSkeletons(data))

        return ()=>{}
    },[])

    function selectOnChangeHandler(event){
        if(event.target.value ===''){
            document.querySelector('#assignment_id').value = '';
            document.querySelector('#assignment_name').value = '';
            document.querySelector('#assignment_points').value ='';
            document.querySelector('#assignment_author').value = '';
            setEditorValue('')
            return;
        }

        requests.GetAssignmentForShark(event.target.value)
        .then(res=>res.json())
        .then(assignment=>{
            document.querySelector('#assignment_id').value = assignment.id;
            document.querySelector('#assignment_name').value = assignment.name;
            document.querySelector('#assignment_points').value = assignment.points;
            document.querySelector('#assignment_author').value = assignment.author;
            setEditorValue(assignment.description)
            //get websites for assignment
            requests.GetWebsitesForAssignment(assignment.id)
            .then(res=>res.json())
            .then(websites=>{
                document.querySelectorAll('[name="assignment-website-checkbox"]').forEach((element)=>{
                    element.checked = false;
                })
                document.querySelectorAll('[id^="pass-"]').forEach((element)=>{
                    element.checked = true;
                })
                websites.map((website)=>{
                    document.querySelector(`#assignment-website-${website.id}`).checked = true;
                    if(website.should_pass){
                        document.querySelector(`#pass-${website.id}`).checked = true;
                    }else{
                        document.querySelector(`#fail-${website.id}`).checked = true;
                    }
                })
            })

            //get skeleton for assignment
            requests.GetSkeletonForAssignment(assignment.id)
            .then(res=>res.json())
            .then(skeletons=>{
                let newArr= skeletons.map((skeleton)=>({
                    id:skeleton.id,
                    name:skeleton.skeleton_name
                }))
                setAddedSkeletons(newArr)
            })

        })
    }

    function handleSkeletonSelectOnChange(event){
        if(event.target.value === ''){
            setCurrentSkeleton(null)
            return;
        }
        let id = event.target.value
        requests.GetSkeleton(id)
        .then(res=>res.json())
        .then(skeleton=>setCurrentSkeleton(skeleton,console.log('from on change',skeleton)))
    }

    function getWebsitesInfo(){ //return list of {id,should_pass}
        var ids = Utility.getCheckedBoxes('assignment-website-checkbox')
        return ids.map((id)=>{
            return {
                id,
                should_pass : document.querySelector(`#pass-${id}`).checked ? true : false 
            };
        })
    }

    function OnCreateHandler(event){
        let skeletons = addedSkeletons.map((skeleton)=>{
            return skeleton.id
        })
        let body = {
            name : document.querySelector('#assignment_name').value,
            points : document.querySelector('#assignment_points').value, 
            author : document.querySelector('#assignment_author').value,
            description : editorValue,
            skeleton_ids:skeletons,
            websites : getWebsitesInfo()
        }
        console.log('body: ',JSON.stringify(body))
        requests.AddAssignment(JSON.stringify(body))

    }

    function onUpdateHandler(event){
        let skeletons = addedSkeletons.map((skeleton)=>{
            return skeleton.id
        })
        let body = {
            id : document.querySelector('#assignment_id').value,
            name : document.querySelector('#assignment_name').value,
            points : document.querySelector('#assignment_points').value, 
            author : document.querySelector('#assignment_author').value,
            description : editorValue,
            skeleton_ids:skeletons,
            websites : getWebsitesInfo()
        }

        console.log('body: ',JSON.stringify(body))
        requests.UpdateAssignment(JSON.stringify(body))

    }
    function OnClickSkeletonAddButton(event){
        if(!currentSkeleton){
            return;
        }
        let obj = {
            id: currentSkeleton.id,
            name: currentSkeleton.skeleton_name
        }
        if(addedSkeletons.some(skeleton=>skeleton.id === obj.id)){
            return;
        } 

        setAddedSkeletons(prevArray => prevArray.concat(obj))
    }
    function OnClickSkeletonDelete(id){
        setAddedSkeletons(prevArray => prevArray.filter((skeleton)=>skeleton.id !== id))
    }


    return(
        <div id="assignment-main">
            <div id="assignment-creation-edit-main">
                <form>
                    <select onChange={selectOnChangeHandler}>
                        <option value="">--none--</option>
                        {
                            assignments.map((assignment)=>{
                                return <option value={assignment.id}>{assignment.name}</option>
                            })
                        }
                    </select><br/><br/>
                    <label>Id:</label><br/>
                    <input id="assignment_id" type="number" disabled></input><br/>
                    <label>Name:</label><br/>
                    <input id="assignment_name" type="text"></input><br/>
                    <label>Points:</label><br/>
                    <input id="assignment_points" type="number"></input><br/>
                    <label>Author:</label><br/>
                    <input id="assignment_author" type="text"></input><br/><br/>
                    <ReactQuill theme="snow" value={editorValue} onChange={setEditorValue}/>
                    <br/><br/><br/>
                </form>
                    <button onClick={OnCreateHandler}>Create</button>&nbsp;
                    <button onClick={onUpdateHandler}>Update</button>&nbsp;
                    <button>Delete</button>
            </div>
            <div id="assignment-skeleton-chooser-main">
                <div style={{margin:5}}>
                    <select id="skeleton-select" onChange={handleSkeletonSelectOnChange}>
                        <option value="">--none--</option>
                    {
                        skeletons.map((skeleton)=>{
                            console.log(skeleton)
                            return (
                                <option value={skeleton.id}>{skeleton.skeleton_name}</option>
                            )
                        })
                    }
                    </select>&nbsp;
                    <button onClick={OnClickSkeletonAddButton}>Add</button>
                    <br/><br/>
                    {
                        addedSkeletons.map((skeleton)=>{
                            return(
                                <div className="skeleton-selected-item">
                                    {skeleton.name}&nbsp; 
                                    <svg onClick={()=>OnClickSkeletonDelete(skeleton.id)} xmlns="http://www.w3.org/2000/svg" x="0px" y="0px" width="15" height="15" viewBox="0 0 32 32">
                                        <path d="M 16 3 C 8.832031 3 3 8.832031 3 16 C 3 23.167969 8.832031 29 16 29 C 23.167969 29 29 23.167969 29 16 C 29 8.832031 23.167969 3 16 3 Z M 16 5 C 22.085938 5 27 9.914063 27 16 C 27 22.085938 22.085938 27 16 27 C 9.914063 27 5 22.085938 5 16 C 5 9.914063 9.914063 5 16 5 Z M 12.21875 10.78125 L 10.78125 12.21875 L 14.5625 16 L 10.78125 19.78125 L 12.21875 21.21875 L 16 17.4375 L 19.78125 21.21875 L 21.21875 19.78125 L 17.4375 16 L 21.21875 12.21875 L 19.78125 10.78125 L 16 14.5625 Z"></path>
                                    </svg>
                                </div>
                            );
                        })
                    }
                    <br/><br/>
                    {
                        currentSkeleton &&
                        <CustomEditor
                            skeleton={currentSkeleton}
                        />
                    }
                </div>
            </div>
            <div id="assignment-website-chooser-main">
                <WebsiteTree websites={websites}/>
            </div>
        </div>
    );
}


function CustomEditor({skeleton}){
    console.log("from editor",skeleton)

    const [fileName,setFileName] = useState('code_before.py')

    const files = {
        'code_before.py':{
            name: 'code_before.py',
            language:'python',
            value:skeleton?.code_before || ''
        },
        'code_after.py':{
            name: 'code_after.py',
            language:'python',
            value:skeleton?.code_after || '' 
        }
    }
    console.log('my file',files)

    const file = files[fileName]


    function handleCodeAfterClick(event){
        setFileName('code_after.py')
        event.target.style.backgroundColor = '#252526'
        event.target.style.borderBottom = '0px solid white'

        let otherTab = document.querySelector('#code_before_tab')
        otherTab.style.backgroundColor='	#3e3e42'
        otherTab.style.borderBottom = '1px solid white'
    }
    function handleCodeBeforeClick(event){
        setFileName('code_before.py')
        event.target.style.backgroundColor = '#252526'
        event.target.style.borderBottom = '0px solid white'

        let otherTab = document.querySelector('#code_after_tab')
        otherTab.style.backgroundColor='	#3e3e42'
        otherTab.style.borderBottom = '1px solid white'
    }

    return(
        <div>
            <div style={{display:'grid',gridTemplateColumns:'1fr 1fr'}}>
                <div id="code_before_tab" className="editor-tab" onClick={handleCodeBeforeClick} style={{borderRight:'1px solid white'}}>Code before</div>
                <div id="code_after_tab" className="editor-tab" onClick={handleCodeAfterClick}  style={{borderBottom:'1px solid white',backgroundColor:'#3e3e42'}}>Code after</div>
            </div>
            <Editor
                key={skeleton.id+file.name}
                height="70vh"
                theme="vs-dark"
                path={file.name}
                language={file.language}
                value={file.value}
                options={{
                    readOnly:true
                }}
            />
        </div>
    );
}