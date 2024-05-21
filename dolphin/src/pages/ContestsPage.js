import React, { useContext,useEffect, useState } from "react";
import {Box } from "@mui/material";
import { TreeView,TreeItem,useTreeItem  } from "@mui/x-tree-view";
import { UserContext } from "../Contexts";
import { Utility } from "../Utility";
import Drawer from 'react-modern-drawer'
import 'react-modern-drawer/dist/index.css'
import { requests } from "../data/Requests";

import '../styles/ContestsPage.css'

import clsx from 'clsx';
import Typography from '@mui/material/Typography';
import ExpandMoreIcon from '@mui/icons-material/ExpandMore';
import ChevronRightIcon from '@mui/icons-material/ChevronRight';
import { Link } from "react-router-dom";
import DrawerContent from "../components/DrawerContent";


const CustomContent = React.forwardRef(function CustomContent(props, ref) {
  const {
    classes,
    className,
    label,
    nodeId,
    icon: iconProp,
    expansionIcon,
    displayIcon,
  } = props;

  const {
    disabled,
    expanded,
    selected,
    focused,
    handleExpansion,
    handleSelection,
    preventSelection,
  } = useTreeItem(nodeId);

  const icon = iconProp || expansionIcon || displayIcon;

  const handleMouseDown = (event) => {
    preventSelection(event);
  };

  const handleExpansionClick = (event) => {
    handleExpansion(event);
  };

  const handleSelectionClick = (event) => {
    handleSelection(event);
    props.handleC()
  };

  return (
    // eslint-disable-next-line jsx-a11y/no-static-element-interactions
    <div
      className={clsx(className, classes.root, {
        [classes.expanded]: expanded,
        [classes.selected]: selected,
        [classes.focused]: focused,
        [classes.disabled]: disabled,
      })}
      onMouseDown={handleMouseDown}
      ref={ref}
    >
      {/* eslint-disable-next-line jsx-a11y/click-events-have-key-events,jsx-a11y/no-static-element-interactions */}
      <div onClick={handleExpansionClick} className={classes.iconContainer}>
        {icon}
      </div>
      <Typography
        onClick={handleSelectionClick}
        component="div"
        className={classes.label}
      >
        {label}
      </Typography>
    </div>
  );
});

const CustomTreeItem = React.forwardRef(function CustomTreeItem(props, ref) {
  return <TreeItem ContentComponent={CustomContent} {...props} ref={ref} />;
});

function ContestsPage(){

  const {user} = useContext(UserContext)
  const [contests,setContests] = useState(null);
  const [groups,setGroups] = useState(null);
  const [rankings,setRankings] = useState(null);
  const [currentItem,setCurrentItem] = useState(null);
  const [currentContest,setCurrentContest] = useState(null);

  const [currentItemIsContest,setCurrentItemIsContest] = useState(true);


  useEffect(()=>{
    console.log("Contests page useEffect running...");
    if(user != null){
      (async()=>{
        var res = await requests.GetContestsForUser(user?.id);
        var contests = await res.json();
        setContests(contests)
        if(contests.length > 0){
          setCurrentItem(contests[0])
          setCurrentItemIsContest(true)
        }
        console.log("Contests: ",contests)
      })();

      
      }
  },[user])

  useEffect(()=>{
    if(contests && contests.length > 0){
      requests.GetContestRankings(contests[0].id)
      .then(res=>res.json())
      .then(data=>{setRankings(data,console.log("rankings",data))})

    requests.GetGroupsWithUsersForContest(contests[0].id)
    .then(res=>res.json())
    .then(data=>{
      console.log("groups for contest ",data);
      setGroups(data);
    })

    }


  },[contests])



  function handleContestClick(c_index){
    setCurrentItemIsContest(true)
    setCurrentItem(contests[c_index]);

    //get contest rankings
    requests.GetContestRankings(contests[c_index].id)
    .then(res=>res.json())
    .then(data=>{setRankings(data,console.log("rankings",data))})

    //get groups for contest
    requests.GetGroupsWithUsersForContest(contests[c_index].id)
    .then(res=>res.json())
    .then(data=>{
      console.log("groups for contest ",data);
      setGroups(data);
    })
  }

  function handleAssignmentClick(c_index,a_index){
    console.log("from assignment handler: ", a_index," ",c_index)
    setCurrentItemIsContest(false)
    setCurrentContest(contests[c_index])
    setCurrentItem(contests[c_index].assignments[a_index])
  }
  return(
    <div className="contestspage-div-main">
      <div className="contestspage-div-one">
        <h3>Contests Enrolled</h3>
        {
          contests && 
           <Box sx={{ minHeight: 180, flexGrow: 1, maxWidth: 300 }}>
            <TreeView
              aria-label="icon expansion"
              defaultCollapseIcon={<ExpandMoreIcon />}
              defaultExpandIcon={<ChevronRightIcon />}
            >
              {
                contests?.map((contest,c_index)=>(
                  <CustomTreeItem key={c_index} ContentProps={{handleC:function(){handleContestClick(c_index)}}}  nodeId={'c.'+ c_index.toString()} label={Utility.capitalizeFirstLetter(contest.name)}>
                    {
                      contest.assignments.map((assignment,a_index)=>(
                        <CustomTreeItem key={a_index} ContentProps={{handleC:function(){handleAssignmentClick(c_index,a_index)}}} nodeId={'a.'+a_index.toString()+'.c.'+c_index.toString()} label={Utility.capitalizeFirstLetter(assignment.name)}></CustomTreeItem>
                      ))
                    }
                  </CustomTreeItem>
                ))
              }

            </TreeView>
            </Box>

        }
      </div>
      <div className="contestspage-div-two">
        <SecondDivComponent key={currentItem?.id} groups={groups} contest={currentContest} isContest={currentItemIsContest} Item={currentItem} rankings={rankings}/>
      </div>
  </div>
  );
}

function SubmissionsForTask({assignmentid,contestid,load}){

  const [submissionsForAssignment,setSubmissionsForAssignment] = useState(null);

  useEffect(()=>{

    if(load){
      requests.GetAllSubmissionsForAssignmentForContest(assignmentid,contestid)
      .then(res => res.json())
      .then(data =>{
        setSubmissionsForAssignment(data);
        console.log('subs data ',data);
      })
    }

    return ()=>{}
  },[assignmentid,contestid,load])

  return(
    <div>
      {
        submissionsForAssignment?.map((submission,index)=>{
          return (
            load && 
            <div className="submissions-for-task-main" key={index}>
              <h5>#{index+1}</h5>
              <div className="timings">
                <p>Created at: {submission.created_at}</p>
                <p>Finished at: {submission.finished_at}</p>
                <p>user: {submission.user_id}</p>
              </div>
              {
                true && 
                <div>
                  <Link to={`/submission/${submission.id}`} target="_blank"><img src="./images/checkmark.png" alt="checkmark" width={40} height={40}/></Link>
                </div>
              }
            </div>
          );
        })
      }
    </div>
  );
}

    

function SecondDivComponent({contest,isContest,Item,rankings,groups}){

  const [loadAssignments,setLoadAssignments] = useState(false);
  const [currentGroup,setCurrentGroup] = useState(null);
  const [isOpen, setIsOpen] = useState(false)
  const [userSubmissions,setUserSubmissions] = useState(null);
  const [userClickedInDrawer,setUserClickedInDrawer] = useState(null);

  function handleUserInDrawerClick(userid){
    if(userid === userClickedInDrawer){
      setUserClickedInDrawer(null)
    }
    else{
      setUserClickedInDrawer(userid)
      console.log(Item)
      requests.GetSubmissionsForUserForContest(userid,Item?.id)
      .then(res=>res.json())
      .then(data=>{
        setUserSubmissions(data)
        console.log("user submission ",data)
      })
    }
  }

  if(isContest){
    return(
        Item &&
        <div className="secondDivComponent-main">
          <div className="secondDiv-contests-main">
            <div>
              <h1>{Utility.capitalizeFirstLetter(Item.name)}</h1>
              <p>{Utility.capitalizeFirstLetter(Item.description)}</p>
              <p>Opening time: {Item.opening_time}</p>
              <p>Closing time: {Item.closing_time}</p>
              <p>Is Active: {Utility.capitalizeFirstLetter(Item.is_active.toString())}</p>
            </div>
            <div className="secondDiv-contest-groups">
              <br/>
              {
                groups?.map((group,index)=>{
                  return <div className="secondDiv-contest-groups-item" key={index} onClick={()=>{
                        setCurrentGroup(group)
                        setIsOpen(true)
                      }}>
                        <p><b>{group.name}</b></p>
                    </div>
                })
              }
            </div>
            <Drawer
                open={isOpen}
                direction='right'
                size={310}
                enableOverlay={false}
            >
              <DrawerContent 
                currentGroup={currentGroup}
                userClickedInDrawer={userClickedInDrawer}
                userSubmissions={userSubmissions}
                setIsOpen={setIsOpen}
                handleUserInDrawerClick={handleUserInDrawerClick}
             />
            </Drawer>
          </div>
          <div >
            <h3 style={{width:'200px'}}>Rankings for {Utility.capitalizeFirstLetter(Item.name)}:</h3>
            {
              rankings?.map((rank,index) => (
                <div className="contestPage-rank-item-main">
                  <div>
                    {
                      index === 0 && rank.total_points > 0? <img alt="gold medal" src="./images/goldmedal.png"/>: null
                    }
                    {
                      index === 1 && rank.total_points > 0? <img alt="silver medal" src="./images/silvermedal.png"/>: null
                    }
                    {
                      index === 2 && rank.total_points > 0? <img alt="bronze medal" src="./images/bronzemedal.png"/>: null
                    }
                  </div>
                  <div>
                    <p>{Utility.capitalizeFirstLetter(rank.username)}</p>
                    <p>{rank.total_points}</p>
                  </div>
                </div>
              ))
            }
          </div>
        </div>
    );
  }
  else{
    return(
      <div style={{
        display:'grid',
        gridTemplateColumns:'1fr 1fr'
      }}>
        <div>
          <h1>{Utility.capitalizeFirstLetter(Item.name)}</h1>
          <p>{Utility.capitalizeFirstLetter(Item.description)}</p>
          <p>Points: {Item.points}</p>
          <p>Website: <a href={"https://"+Item.website_link} target="_blank">{Item.website_link}</a></p>
          <button style={{backgroundColor:'lightgreen'}}><Link to={`/editor/${Item.id}`}>Solve</Link></button>
          <button onClick={()=>setLoadAssignments(true)}>Load submissions</button>&nbsp;&nbsp;
          <input type="checkbox" id="unmarked"/>
          <label for="unmarked">Show unmarked only</label>
        </div>
        <div style={{
          overflowY:'scroll',
          height: 600
        }}>
          <h4>Submissions for {Item.name} in {contest.name}</h4>
          <SubmissionsForTask assignmentid={Item?.id} contestid={contest?.id} load={loadAssignments}/>
        </div>
      </div>
    );
  }
}

export default ContestsPage;

