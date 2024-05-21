import { Editor } from "@monaco-editor/react";
import { useContext, useEffect, useRef, useState } from "react";
import { Layout,LayoutPanel} from "rc-easyui";
import { Box,Tabs,Tab,Typography} from "@mui/material";
import { styled } from '@mui/material/styles';
import { useParams,Link } from "react-router-dom";
import {UserContext} from '../Contexts.js'
import { requests } from "../data/Requests.js";
import { Utility } from "../Utility.js";
import '../styles/EditorPage.css'

function EditorPage(){
    const [result, setResult] = useState(null);
    const [currentAssignment,setCurrentAssignment] = useState(null)
    const [submissions,setSubmissions] = useState()
    const [value, setValue] = useState(0);
    const editorRef = useRef(null);

    const {user} = useContext(UserContext);
    console.log("my user",user)
    const params = useParams();
    console.log("params,",params)


    useEffect(()=>{
      console.log("editor useeffect running");
      (async () =>{
        var res_assignment= await (await requests.GetAssignment(params.assignment_id)).json();
        setCurrentAssignment(res_assignment)
        console.log("my response",res_assignment);

      })();


      return ()=>{
        console.log("cleanup editor")
      }
    },[])

    useEffect(()=>{
      console.log("second useeffect running");
      if(user && currentAssignment){

        (async () =>{
          var res_submissions = await (await requests.GetSubmissionsForAssignmentForUser(user?.id,currentAssignment?.id)).json();
          setSubmissions(res_submissions)
          console.log("my submissions",res_submissions);

        })();
      }

      return ()=>{}
    },[currentAssignment,user,result])


    function handleEditorDidMount(editor,monaco){
        editorRef.current = editor;
    }

    function submit(){
      var submission= {
        source_code: editorRef.current.getValue(),
        assignment_parent_id:4,
        test_fw_id: 1,
        user_id: user.id,
        assignment_id:currentAssignment.id,
        language_id:1,
        status: 1 
      }
      requests.CreateSubmission(submission)
      .then(res => res.json())
      .then(data => {
        var decodedstr = atob(data.result_file)
        setResult(decodedstr,console.log(decodedstr))
        setValue(1)
      })
      .catch(err=>console.log(err))
    }

    const handleChange = (event, newValue) => {
        setValue(newValue);
    };


    return <div style={{height:'90vh',marginTop:5}}>
        <Layout> 
            {/* <LayoutPanel region="east" split style={{ width: 500,minWidth: 300}}>
                <EditorComplement submit={submit}/>
            </LayoutPanel> */}
            <LayoutPanel region="east" split style={{ width: 550,minWidth: 0}}>
                <Box sx={{ width: '100%' }}>
                    <Box sx={{ bgcolor: '#fff' }}>
                        <AntTabs value={value} onChange={handleChange} aria-label="ant example">
                            <AntTab label="Task" />
                            <AntTab label="Result" />
                            <AntTab label="Submissions" />
                        </AntTabs>
                        <Box sx={{ p: 3 }} />
                    </Box>
                    <CustomTabPanel value={value} index={0}>
                      {
                        currentAssignment && 
                        <div>
                          <h2>{Utility.capitalizeFirstLetter(currentAssignment.name)}</h2>
                          <div style={{margin:'20px'}}>
                            <p>Description: {Utility.capitalizeFirstLetter(currentAssignment.description)}</p>
                            <p>Website under test: <a href={"https://"+currentAssignment.website_link} target="_blank">{currentAssignment.website_link}</a></p>
                            <p><i>{currentAssignment.points} points</i></p>
                            <select >
                              <option>python3/unittest</option>
                              <option>dotnet6/Nunit3</option>
                            </select>
                            <br/><br/>
                            <button onClick={submit} type="submit">Submit</button>
                          </div>
                        </div>
                      }
                    </CustomTabPanel>
                    <CustomTabPanel value={value} index={1}>
                      <pre>
                        {
                          result ?
                          <h3 style={{margin:0}}>{result?.passed ? 'Passed':'Failed'}</h3>:
                          <p>No result to show...</p>
                        }
                        {result}
                      </pre>
                    </CustomTabPanel>
                    <CustomTabPanel value={value} index={2}>
                      <div style={{display:'grid',justifyContent:'center'}}> 
                      {
                        submissions?.map((submission,index)=>{
                          return (
                            <div key={index} className="editorpage-submission-item-main">
                              <h5>#{index+1}</h5>
                              <div className="timings">
                                <p>Created at: {submission.created_at}</p>
                                <p>Finished at: {submission.finished_at}</p>
                              </div>
                              <div >
                                {
                                  true ? (
                                    <Link to= {`/submission/${submission.id}`} target='_blank'>
                                  <svg onClick={()=>console.log("svg clicked")} xmlns="http://www.w3.org/2000/svg" x="0px" y="0px" width="40" height="40" viewBox="0 0 48 48">
                                    <path fill="#c8e6c9" d="M44,24c0,11.045-8.955,20-20,20S4,35.045,4,24S12.955,4,24,4S44,12.955,44,24z"></path>
                                    <path fill="#4caf50" d="M34.586,14.586l-13.57,13.586l-5.602-5.586l-2.828,2.828l8.434,8.414l16.395-16.414L34.586,14.586z"></path>
                                  </svg></Link>

                                  ) : (
                                    <svg xmlns="http://www.w3.org/2000/svg" x="0px" y="0px" width="40" height="40" viewBox="0 0 48 48">
                                      <path fill="#f44336" d="M44,24c0,11.045-8.955,20-20,20S4,35.045,4,24S12.955,4,24,4S44,12.955,44,24z"></path>
                                      <path fill="#fff" d="M29.656,15.516l2.828,2.828l-14.14,14.14l-2.828-2.828L29.656,15.516z"></path>
                                      <path fill="#fff" d="M32.484,29.656l-2.828,2.828l-14.14-14.14l2.828-2.828L32.484,29.656z"></path>
                                    </svg>
                                  )
                                }
                              </div>
                            </div>
                          )
                        })
                      }
                      </div>
                    </CustomTabPanel>
                </Box>
            </LayoutPanel>
            <LayoutPanel region="center">
                <Editor
                    height={'100vh'}
                    defaultLanguage="Python"
                    defaultValue ={"print(\"Hello yahya\")"} 
                    theme="vs-dark"
                    onMount={handleEditorDidMount}
                />
            </LayoutPanel>
        </Layout>
    </div>
}

var data = "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum."

function CustomTabPanel(props) {
  const { children, value, index, ...other } = props;

  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`simple-tabpanel-${index}`}
      aria-labelledby={`simple-tab-${index}`}
      {...other}
    >
      {value === index && (
        <Box sx={{ p: 3 }}>
          <Typography>{children}</Typography>
        </Box>
      )}
    </div>
  );
}

function a11yProps(index) {
  return {
    id: `simple-tab-${index}`,
    'aria-controls': `simple-tabpanel-${index}`,
  };
}

const AntTabs = styled(Tabs)({
  borderBottom: '1px solid #e8e8e8',
  '& .MuiTabs-indicator': {
    backgroundColor: '#1890ff',
  },
});

const AntTab = styled((props) => <Tab disableRipple {...props} />)(({ theme }) => ({
  textTransform: 'none',
  minWidth: 0,
  [theme.breakpoints.up('sm')]: {
    minWidth: 0,
  },
  fontWeight: theme.typography.fontWeightRegular,
  marginRight: theme.spacing(1),
  color: 'rgba(0, 0, 0, 0.85)',
  fontFamily: [
    '-apple-system',
    'BlinkMacSystemFont',
    '"Segoe UI"',
    'Roboto',
    '"Helvetica Neue"',
    'Arial',
    'sans-serif',
    '"Apple Color Emoji"',
    '"Segoe UI Emoji"',
    '"Segoe UI Symbol"',
  ].join(','),
  '&:hover': {
    color: '#40a9ff',
    opacity: 1,
  },
  '&.Mui-selected': {
    color: '#1890ff',
    fontWeight: theme.typography.fontWeightMedium,
  },
  '&.Mui-focusVisible': {
    backgroundColor: '#d1eaff',
  },
}));

const StyledTabs = styled((props) => (
  <Tabs
    {...props}
    TabIndicatorProps={{ children: <span className="MuiTabs-indicatorSpan" /> }}
  />
))({
  '& .MuiTabs-indicator': {
    display: 'flex',
    justifyContent: 'center',
    backgroundColor: 'transparent',
  },
  '& .MuiTabs-indicatorSpan': {
    maxWidth: 40,
    width: '100%',
    backgroundColor: '#635ee7',
  },
});

const StyledTab = styled((props) => <Tab disableRipple {...props} />)(
  ({ theme }) => ({
    textTransform: 'none',
    fontWeight: theme.typography.fontWeightRegular,
    fontSize: theme.typography.pxToRem(15),
    marginRight: theme.spacing(1),
    color: 'rgba(255, 255, 255, 0.7)',
    '&.Mui-selected': {
      color: '#fff',
    },
    '&.Mui-focusVisible': {
      backgroundColor: 'rgba(100, 95, 228, 0.32)',
    },
  }),
);

export default EditorPage;