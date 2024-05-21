import { useEffect, useState } from "react";
import { requests } from "../data/requests";

export default function Tools(){
    const [languages,setLanguages] = useState([])
    const [frameworks,setFrameworks] = useState([])

    useEffect(()=>{
        requests.GetLanguages()
        .then(res=>res.json())
        .then(data=>setLanguages(data))

        requests.GetTestFrameworks()
        .then(res=>res.json())
        .then(data=>setFrameworks(data))

        return ()=>{}
    },[])

    function onLanguageAdd(){
        let body= {
            name: document.querySelector('#language-name').value
        }
        requests.AddLanguage(JSON.stringify(body))
    }
    function onLanguageUpdate(){
        let body = {
            name: document.querySelector('#language-name').value,
            id: document.querySelector('#language-id').value
        }
        requests.UpdateLanguage(JSON.stringify(body))
    }
    function onLanguageDelete(){

    }
    function onFrameworkAdd(){
        let language_id;
        let nodes = document.querySelectorAll('[name=language-checkbox]')
        nodes.forEach(node=>{
            if(node.checked){
                language_id = node.value
            }
        })

        let body = {
            id: document.querySelector('#framework-id').value,
            name: document.querySelector('#framework-name').value,
            language_id
        }
        requests.AddFramework(JSON.stringify(body))

    }
    function onFrameworkUpdate(){
        let language_id;
        let nodes = document.querySelectorAll('[name=language-checkbox]')
        nodes.forEach(node=>{
            if(node.checked){
                language_id = node.value
            }
        })

        let body = {
            id: document.querySelector('#framework-id').value,
            name: document.querySelector('#framework-name').value,
            language_id
        }
        requests.UpdateFramework(JSON.stringify(body))

    }
    function onFrameworkDelete(){

    }
    function onChangeLanguage(event){
        if(event.target.value===''){
            return;
        }
        let language = languages.find(language=>language.id == event.target.value)
        console.log(language)
        document.querySelector('#language-id').value = language.id
        document.querySelector('#language-name').value = language.name
    }

    function onChangeFramework(event){
        if(event.target.value===''){
            return;
        }
        let framework = frameworks.find(framework=> framework.testing_framework_id == event.target.value)
        console.log(framework)
        document.querySelector('#framework-id').value = framework.testing_framework_id
        document.querySelector('#framework-name').value = framework.testing_framework_name
        //uncheck the rest
        document.querySelectorAll("[name=language-checkbox]").forEach((element)=>element.checked = false)
        //check the correct checkbox
        document.querySelector(`#language-checkbox-${framework.language_id}`).checked = true

    }
    return(
        <div id="tools-main">
            <div id="language-main">
                <h4>Programming Language</h4>
                <select id="language-select" onChange={onChangeLanguage}>
                    <option value=''>--none--</option>
                    {
                        languages.map((language)=>{
                            return (
                                <option value={language.id}>{language.name}</option>
                            );
                        })
                    }
                </select><br/><br/>
                <label>Id:</label><br/>
                <input id="language-id" type="number" disabled/><br/>
                <label>Language name:</label><br/>
                <input id="language-name" type="text"/>
                <br/><br/>
                <button onClick={onLanguageAdd}>Add</button>&nbsp;
                <button onClick={onLanguageUpdate}>Update</button>&nbsp;
                <button onClick={onLanguageDelete}>Delete</button>
            </div>
            <div id="framework-main">
                <h4>Testing Framework</h4>
                <select id="framework-select" onChange={onChangeFramework}>
                    <option value=''>--none--</option>
                    {
                        frameworks.map((framework)=>{
                            return(
                                <option value={framework.testing_framework_id}>{framework.testing_framework_name}</option>
                            );
                        })
                    }
                </select><br/><br/>
                <label>Id:</label><br/>
                <input id="framework-id" type="number" disabled/><br/>
                <label>Framework name:</label><br/>
                <input id="framework-name" type="text"/><br/><br/>
                <LanguagesList languages={languages}/>
                <br/>
                <button onClick={onFrameworkAdd}>Add</button>&nbsp;
                <button onClick={onFrameworkUpdate}>Update</button>&nbsp;
                <button onClick={onFrameworkDelete}>Delete</button>
            </div>
        </div>
    );
}

function LanguagesList({languages}){
    function onCheckboxClick(event){
        document.querySelectorAll("[name=language-checkbox]").forEach((element)=>{
            if(element.id !== event.target.id){
                element.checked = false
            }
        })

    }

    return(
        <div>
            {
                languages?.map((language)=>{
                    return <div>
                        <input value={language.id} onClick={onCheckboxClick} id={`language-checkbox-${language.id}`} name={`language-checkbox`} type="checkbox"></input>
                        <label>{language.name}</label>
                    </div>
                })
            }
        </div>
    );
}