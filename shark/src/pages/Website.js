import ReactQuill from "react-quill";
import { useEffect, useState } from "react";
import { requests } from "../data/requests";
import parse from 'html-react-parser';

export default function Website(){

    const [editorValue, setEditorValue] = useState('Enter website description here...');// for website editor
    const [editorValue2, setEditorValue2] = useState('Enter bug description here...');// for bug editor
    const [bugs,setBugs] = useState([])
    const [currentBug,setCurrentBug] = useState(null)
    const [parentWebsites,setParentWebsites] = useState([])
    const [websites,setWebsites] = useState([])

    console.log(bugs)

    useEffect(()=>{
        requests.GetParentWebsites()
        .then(res=>res.json())
        .then(websites=>{
            setParentWebsites(websites)
        })

        requests.GetWebsites()
        .then(res=>res.json())
        .then(websites=>setWebsites(websites))

        return ()=>{}
    },[])

    function randomId(length = 6) {
    return Math.random().toString(36).substring(2, length+2);
    }

    function onAddBug(){
        let bug = {
            id : randomId(20),
            name: document.querySelector('#bug-form-name').value,
            author: document.querySelector('#bug-form-author').value,
            commit_link: document.querySelector('#bug-form-commit').value,
            description:editorValue2
        }
        setBugs(bugs => bugs.concat(bug))
        
    }
     function onRemoveBug(id){
        if(id === currentBug?.id){
            setCurrentBug(null)
        }
        setBugs(bugs => bugs.filter((bug)=>bug.id !== id))
     }
    function onBugClick(id){
        let element = document.querySelector('#bug-description-viewer')
        if(element.style.display === 'block' && currentBug?.id === id){
            element.style.display = 'none'
            return;
        }
        setCurrentBug(bugs.find(bug=>bug.id === id))
        element.style.display = 'block'
    }

    function selectWebsiteOnChangeHandler(event){
        setCurrentBug(null)
        let id = event.target.value
        if(id === ''){
            return;
        }

        requests.GetWebsite(id)
        .then(res=>res.json())
        .then(website=>{
            let parent = parentWebsites.find(web=>web.id === website.parent_id) || null
            console.log('parent here',parent)
            document.querySelector('#website-form-id').value = website.id
            document.querySelector('#website-form-url').value = website.hosted_at
            document.querySelector('#website-form-repo').value = website.repository
            document.querySelector('#website-form-parent').value = parent?.id || ''
            setEditorValue(website.description)

            //get bugs for website
            requests.GetBugsForWebsite(website.id)
            .then(res=>res.json())
            .then(bugs=>setBugs(bugs))
        })
    }

    function onCreateWebsite(){
        let parentid = document.querySelector('#website-form-parent').value
        let body = {
            parent_id: parentid === ''? null: parentid,
            hosted_at: document.querySelector('#website-form-url').value,
            repository: document.querySelector('#website-form-repo').value,
            description: editorValue,
            bugs 
        }
        console.log(body)
        requests.AddWebsite(JSON.stringify(body))
    }
    function onUpdateWebsite(){
        let parentid = document.querySelector('#website-form-parent').value
        let body = {
            id: document.querySelector('#website-form-id').value,
            parent_id: parentid === ''? null: parentid,
            hosted_at: document.querySelector('#website-form-url').value,
            repository: document.querySelector('#website-form-repo').value,
            description: editorValue,
            bugs 
        }
        console.log(body)
        requests.UpdateWebsite(JSON.stringify(body))
         
    }
    function onDeleteWebsite(){

    }

    return(
        <div id="website-main">
            <div id="website-create-update">
                <select onChange={selectWebsiteOnChangeHandler}>
                    <option value=''>--none--</option>
                    {
                        websites.map((website)=>{
                            return <option value={website.id}>{website.hosted_at}</option>
                        })
                    }
                </select><br/><br/>
                <label>Id: </label><br/>
                <input id="website-form-id" type="number" disabled/><br/>
                <label>URL: </label><br/>
                <input id="website-form-url" type="text"/><br/>
                <label>Repository: </label><br/>
                <input id="website-form-repo" type="text"/><br/><br/>
                <ReactQuill theme="snow" value={editorValue} onChange={setEditorValue}/><br/>
                <label>Parent :</label>&nbsp;
                <select id="website-form-parent">
                    <option value="">--none--</option>
                    {
                        parentWebsites.map((website)=>{
                            return <option value={website.id}>{website.hosted_at}</option>
                        })
                    }
                </select><br/><br/>
                <h5 style={{margin:'2px 0px'}}>Added bugs</h5>
                <BugsViewer bugs={bugs} onRemoveBug={onRemoveBug} onBugClick={onBugClick}/>
                {/* <div id="bug-description-viewer" style={{display:'none'}}>
                    {
                        currentBug &&
                        parse(currentBug.description)
                    }
                </div> */}
                <br/>
                <button onClick={onCreateWebsite}>Create</button>&nbsp;
                <button onClick={onUpdateWebsite}>Update</button>&nbsp;
                <button onClick={onDeleteWebsite}>Delete</button>
            </div>
            <div id="website-bug-list">
                <div id="bug-list">
                </div>
                <h5>Add a bug</h5>
                {/* <label>Id:</label><br/>
                <input id="bug-form-id" type="number" disabled/><br/> */}
                <label>Name:</label><br/>
                <input id="bug-form-name" type="text"/><br/>
                <label>Author:</label><br/>
                <input id="bug-form-author" type="text"/><br/>
                <label>Commit:</label><br/>
                <input id="bug-form-commit" type="text"/><br/><br/>
                <ReactQuill theme="snow" value={editorValue2} onChange={setEditorValue2}/><br/>
                <button onClick={onAddBug}>Add</button>
            </div>

            <div id="bug-description-viewer" style={{display:'none'}}>
                {
                    currentBug && 
                    <div>
                        <h5>{currentBug?.name}</h5>
                        {
                            currentBug &&
                            parse(currentBug.description)
                        }<br/><br/>
                        <b>Commit : </b>{currentBug?.commit_link}<br/><br/>
                        <b>Author : </b>{currentBug?.author}
                    </div>
                }

            </div>
        </div>
    );
}

function BugsViewer({bugs,onRemoveBug,onBugClick}){


    return(
        <div>
            {
                bugs?.map((bug)=>{
                    let name = bug.name
                    if(name.length > 15){
                        name = name.substring(0,15) + '..'
                    }
                    return(
                        <div id="bug-item">
                            <span onClick={()=>onBugClick(bug.id)} >
                                {name}&nbsp; 
                            </span>
                            <svg onClick={()=>{onRemoveBug(bug.id)}} xmlns="http://www.w3.org/2000/svg" x="0px" y="0px" width="15" height="15" viewBox="0 0 32 32">
                                <path d="M 16 3 C 8.832031 3 3 8.832031 3 16 C 3 23.167969 8.832031 29 16 29 C 23.167969 29 29 23.167969 29 16 C 29 8.832031 23.167969 3 16 3 Z M 16 5 C 22.085938 5 27 9.914063 27 16 C 27 22.085938 22.085938 27 16 27 C 9.914063 27 5 22.085938 5 16 C 5 9.914063 9.914063 5 16 5 Z M 12.21875 10.78125 L 10.78125 12.21875 L 14.5625 16 L 10.78125 19.78125 L 12.21875 21.21875 L 16 17.4375 L 19.78125 21.21875 L 21.21875 19.78125 L 17.4375 16 L 21.21875 12.21875 L 19.78125 10.78125 L 16 14.5625 Z"></path>
                            </svg>
                        </div>
                    );
                })
            }
        </div>
    );
}