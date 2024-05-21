import { useContext,useEffect, useState, useRef } from "react";
import { UserContext } from "../Contexts";
import { useParams } from "react-router-dom";
import { requests } from "../data/Requests";
import { Editor } from "@monaco-editor/react";
import '../styles/SubmissionPage.css'

function SubmissionDetailPage(){
    const[submission,setSubmission] = useState();
    const[comments,setComments] = useState([]);
    const[postmoderated,setPostmoderated] =useState(false);

    const {user} = useContext(UserContext)

    const params = useParams();
    const editorRef = useRef(null);
    const textareaRef = useRef(null);

    useEffect(()=>{
        console.log("submission useEffect running...")
        requests.GetSubmission(params.id)
        .then(res => res.json())
        .then(data =>{
            console.log("submission data",data);
            setSubmission(data);
        })

        return ()=>{}
    },[params.id,postmoderated])

    useEffect(()=>{
        console.log("comment useeffect runnings")
        if(submission){
            requests.GetCommentsForSubmission(submission.id)
            .then(res=>res.json())
            .then(comments=>setComments(comments))
        }

    },[submission])

    function handleEditorDidMount(editor,monaco){
        editorRef.current = editor;
    }
    function CommentBtnOnClickHandler(){
        let comment = {
            comment_text:textareaRef.current.value,
            user_id: user.id,
            submission_id:submission.id,
            parent_id:null,
        }
        requests.CreateCommentForSubmission(comment)
        .then(res=>res.json())
        .then(data=>console.log('comment id',data.added_comment))
        .then(()=>{
            requests.GetCommentsForSubmission(submission.id)
            .then(res=>res.json())
            .then(comments=>setComments(comments))
            .then(()=>textareaRef.current.value='')
        
        })


    }
    function AcceptBtnOnClickHandler(){
        requests.PostmoderateSubmission(submission.id,true)
        .then(()=>setPostmoderated(!postmoderated))
    }
    function RejectBtnOnClickHandler(){
        requests.PostmoderateSubmission(submission.id,false)
        .then(()=>setPostmoderated(!postmoderated))
    }

    return(
        <div className="submissionPage-main-div">
            <div>
                <Editor
                    height={'90vh'}
                    defaultLanguage="Python"
                    theme="vs-dark"
                    value={submission?.source_code}
                    onMount={handleEditorDidMount}
                    options={{
                        readOnly:true
                    }}
                />
            </div>
            <div className="submissionPage-second-child-div" style={{
                height:'90vh',
                overflowY:'scroll'
            }}>
                <p>Created: {submission?.created_at}</p>
                <p>Finished: {submission?.finished_at}</p>
                <p>auto_verdict: {submission?.auto_verdict === 0 ? 'Rejected':'Accepted'}</p>
                <p>postmoderation_verdict: {submission?.postmoderation_verdict === 0 || submission?.postmoderation_verdict == null? 'Rejected':'Accepted'}</p>
                {
                    submission &&
                    <pre>{atob(submission.result)}</pre>
                }
                {comments?.length > 0 && 
                    <h4 style={{margin:0}}>Comments:</h4>
                }
                {
                    comments?.map((comment)=>{
                        return <div>
                            &nbsp;&nbsp;<b>{comment.username}</b>: {comment.comment_text}
                        </div>
                    })
                }

                <textarea ref={textareaRef}></textarea><br/>
                <button onClick={CommentBtnOnClickHandler}>Comment</button><br/><br/>

                <button onClick={AcceptBtnOnClickHandler}>Accept</button>
                <button onClick={RejectBtnOnClickHandler}>Reject</button>

            </div>
        </div>
    );

}

export default SubmissionDetailPage;