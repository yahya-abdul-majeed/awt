import { useContext, useEffect, useRef, useState } from "react";
import { UserContext } from "../Contexts";
import { requests } from "../data/Requests";
import { Modal,Box,Typography } from "@mui/material";
import $ from 'jquery'
import {Utility} from '../Utility'

$.DataTable = require('datatables.net-dt');

function QueuePage(){
    console.log("DataTable ",$.active);

    const tableRef = useRef();
    const table = useRef(null);

    const[queue,setQueue] = useState(null);
    const [tableInitialized,setTableInitialilzed] = useState(false);
    const {user} = useContext(UserContext);


    useEffect(()=>{
        console.log("queue page useEffect running");

        if(user){
            (async ()=>{
                var res_queue = await ((await requests.GetUserSubmissionQueue(user.id)).json());
                setQueue(res_queue,console.log("queue items: ",res_queue));
            })();
        }

        return ()=>{}

    },[user]);

    useEffect(() => {
        if(queue && !tableInitialized){
            setTableInitialilzed(true);
            table.current = $(tableRef.current).DataTable(
                {
                        data: queue,
                        columns: [
                            { data: 'assignment_name',title:'assignment_name',
                                render: (data)=>{
                                    return Utility.capitalizeFirstLetter(data)
                                }
                            },
                            { data: 'submission_id' ,title:'submission_id' },
                            { data: 'auto_verdict' ,title:'auto_verdict' ,
                                render: function ( data, type, row ) {
                                    return 'pass' //this does not count in filtering?
                                }
                            },
                            { data: 'postmoderation_verdict' ,title:'postmoderation_verdict',
                                render: ()=>{
                                    return 'none'
                                }
                            },
                            { data: 'submitted_at' ,title:'submitted_at',
                                render: (data,type,row)=>{
                                    const p = document.createElement('a');
                                    p.textContent = data;
                                    p.style.color = 'blue'
                                    p.style.margin = '0px'
                                    p.addEventListener('mouseover',()=>{
                                        p.style.textDecoration = 'underline';
                                        p.style.cursor = 'pointer'
                                    })
                                    p.addEventListener('mouseout',()=>{
                                        p.style.textDecoration = 'none';
                                    })
                                    p.setAttribute('href',`/submission/${row.submission_id}`)
                                    p.setAttribute('target','_blank')
                                    return p;
                                }
                            }
                        ],
                        paging:true,
                        searching:true,
                        ordering:true
                }
                )
        }
        return function() {
        }
    },[queue,tableInitialized])

    useEffect(()=>{
        if(table.current){
            table.current.clear();
            table.current.rows.add(queue).draw()
        }

        return ()=>{

        }
    },[queue])


    return(
        <div style={{margin:'20px 50px',padding:'0px 10px'}}>
            <table ref={tableRef} id="myTable" style={{width:'100%'}} className="display table table-hover table-bordered table-condensed">
            </table>

        </div>
    );
}

export default QueuePage;