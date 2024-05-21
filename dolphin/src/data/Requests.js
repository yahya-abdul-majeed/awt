
let baseURL = "https://localhost:7168/api/";


async function Login(data){
    console.log("Logging in...");
    console.log("Login data: ",data)

    const options = {
        method:'POST',
        headers:{
           'Content-Type': 'application/json' 
        },
        body: JSON.stringify(data)
    }

    return fetch(baseURL+"auth/login",options)
    .then(res =>res.text())
    .catch(err => console.log("Error from login: ",err))
}

async function Register(data){
    console.log("Registering...")
    console.log("Registration data: ",data)

    const options = {
        method:'POST',
        headers:{
           'Content-Type': 'application/json' 
        },
        body: JSON.stringify(data)
    }

    return fetch(baseURL+"auth/register",options)

}

async function GetContestsForUser(user_id){
    console.log(`Getting Contests for ${user_id}`);

    var response = fetch(baseURL+`contest/getContestsForUser/${user_id}`);
    return response;

}

async function GetAssignment(assignment_id){

    var response = fetch(baseURL + `assignment/getassignment/${assignment_id}`)
    return response

}

async function GetSubmissionsForAssignmentForUser(user_id,assignment_id){
    console.log("user_id",user_id + " assignment_id " + assignment_id);
    var response = fetch(baseURL + `Submission/GetSubmissionsForAssignmentForUser/${user_id}/${assignment_id}`)
    return response;

}

async function GetContestRankings(contest_id){
    console.log("getting rankings for ",contest_id);
    var response = fetch(baseURL + `contest/getContestRankings/${contest_id}`)
    return response;
}

async function GetUserSubmissionQueue(user_id){
    console.log("getting user submission queue");
    var response = fetch(baseURL + `submission/getUserSubmissionQueue/${user_id}`)
    return response;
}

async function GetSubmission(submission_id){
    console.log("getting submission for id ",submission_id)
    var response = fetch(baseURL + `submission/getSubmission/${submission_id}`)
    return response;
}

async function CreateSubmission(submission){
    var options = {
        method: "POST",
        headers:{
            "Content-Type": "application/json",
        },
        body: JSON.stringify(submission)
    }
    var response = fetch(baseURL + `submission/createsubmission`,options)
    return response
}

async function GetAllSubmissionsForAssignmentForContest(assignment_id,contest_id){
    var response = fetch(baseURL + `Submission/GetAllSubmissionsForAssignmentForContest/${assignment_id}/${contest_id}`);
    return response;
}

async function GetGroupsWithUsersForContest(contest_id){
    var response = fetch(baseURL + `Group/GetGroupsForContest/${contest_id}`)
    return response;
}

async function GetSubmissionsForUserForContest(user_id,contest_id){
    var response = fetch(baseURL+ `submission/GetSubmissionsForUserForContest/${user_id}/${contest_id}`)
    return response;
}

async function CreateCommentForSubmission(comment){
    var response = fetch(baseURL+`submission/createcomment`,{
        method:'POST',
        headers:{
            'content-type':'application/json'
        },
        body:JSON.stringify(comment)
    })
    return response;
}
async function GetCommentsForSubmission(submission_id){
    var response = fetch(baseURL+`submission/getCommentsForSubmission/${submission_id}`)
    return response;
}
async function PostmoderateSubmission(id,accept){
    return fetch(baseURL+`submission/postmoderateSubmission/${id}?accept=${accept}`)
}

export const requests = {
    Login,
    Register,
    GetContestsForUser,
    GetAssignment,
    GetSubmissionsForAssignmentForUser,
    GetContestRankings,
    GetUserSubmissionQueue,
    GetSubmission,
    CreateSubmission,
    GetAllSubmissionsForAssignmentForContest,
    GetGroupsWithUsersForContest,
    GetSubmissionsForUserForContest,
    CreateCommentForSubmission,
    GetCommentsForSubmission,
    PostmoderateSubmission
}