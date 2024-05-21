import { useState,useEffect, useRef } from "react";
import { requests } from "../data/requests";
import { Editor } from "@monaco-editor/react";

export default function Skeleton(){
    const [skeletons,setSkeletons] = useState([])
    const [testFrameworks,setTestFrameworks] = useState([])

    const codebefore_ref = useRef(null)
    const codeafter_ref = useRef(null)

    function handleCodeBeforeEditorDidMount(editor,monaco){
        codebefore_ref.current = editor;
    }
    function handleCodeAfterEditorDidMount(editor,monaco){
        codeafter_ref.current = editor;
    }

    useEffect(()=>{

        requests.GetSkeletons()
        .then(res=>res.json())
        .then(data=>setSkeletons(data))

        requests.GetTestFrameworks()
        .then(res=>res.json())
        .then(data=>setTestFrameworks(data))

        return ()=>{}
    },[])

    function SkeletonSelectOnChange(event){
        let id = event.target.value
        requests.GetSkeleton(id)
        .then(res=>res.json())
        .then(skeleton=>{
            document.querySelector('#skeleton-form-id').value = skeleton.id
            document.querySelector('#skeleton-form-name').value = skeleton.skeleton_name
            document.querySelector('#test-framework-select').value = skeleton.testing_fw_id
            codebefore_ref.current.setValue(skeleton.code_before)
            codeafter_ref.current.setValue(skeleton.code_after)
        })

    }

    function onSkeletonCreate(){
        let name = document.querySelector('#skeleton-form-name').value
        let testing_framework_id = document.querySelector('#test-framework-select').value
        let code_before = codebefore_ref.current.getValue()
        let code_after = codeafter_ref.current.getValue()

        let body = {name,testing_framework_id,code_after,code_before}
        console.log(body)

        requests.AddSkeleton(JSON.stringify(body))
    }
    function onSkeletonUpdate(){
        let id = document.querySelector('#skeleton-form-id').value
        let name = document.querySelector('#skeleton-form-name').value
        let testing_framework_id = document.querySelector('#test-framework-select').value
        let code_before = codebefore_ref.current.getValue()
        let code_after = codeafter_ref.current.getValue()

        let body = {id,name,testing_framework_id,code_after,code_before}
        console.log(body)

        requests.UpdateSkeleton(JSON.stringify(body))

    }
    function onSkeletonDelete(){

    }

    return(
        <div id="skeleton-main">
            <div id="skeleton-create-form">
                <select id="skeleton-select" onChange={SkeletonSelectOnChange}>
                    <option value=''>--none--</option>
                    {
                        skeletons.map((skeleton)=>{
                            return <option value={skeleton.id}>{skeleton.skeleton_name}</option>
                        })
                    }
                </select><br/><br/>
                <label>Id:</label><br/>
                <input id="skeleton-form-id" type="number" disabled/><br/>
                <label>Name:</label><br/>
                <input id="skeleton-form-name" type="text"/><br/>
                <label>Testing Framework:</label><br/>
                <select id="test-framework-select">
                    {
                        testFrameworks.map((framework)=>{
                            return <option value={framework.testing_framework_id}>{framework.testing_framework_name}/{framework.language_name}</option>
                        })
                    }
                </select><br/><br/><br/><br/><br/><br/>
                <button onClick={onSkeletonCreate}>Create</button><br/>
                <button onClick={onSkeletonUpdate}>Update</button><br/>
                <button onClick={onSkeletonDelete}>Delete</button>
            </div>
            <div id="code-before-editor">
                <div className="tab">Code before</div>
                <Editor
                    height="70vh"
                    theme="vs-dark"
                    language={'python'}
                    onMount={handleCodeBeforeEditorDidMount}
                />
            </div>
            <div id="code-after-editor">
                <div className="tab">Code after</div>
                <Editor
                    height="70vh"
                    theme="vs-dark"
                    language={'python'}
                    onMount={handleCodeAfterEditorDidMount}
                />

            </div>
        </div>
    );
}