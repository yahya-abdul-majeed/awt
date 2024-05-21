var ibexBaseURL = 'https://localhost:7235/api/';

async function GetAssignmentsForShark(){
    return fetch(ibexBaseURL+`assignment/getAssignmentsForShark`)
}

async function GetAssignmentForShark(id){
    return fetch(ibexBaseURL+`assignment/getAssignmentForShark/${id}`)
}

async function GetWebsiteTree(){
    return fetch(ibexBaseURL+`Website/GetWebsiteTree`)
}

async function GetSkeletons(){
    return fetch(ibexBaseURL+`Skeleton/GetSkeletons`)
}
async function GetSkeleton(id){
    return fetch(ibexBaseURL+`Skeleton/GetSkeleton/${id}`)
}
async function AddAssignment(body){
    return fetch(ibexBaseURL+`assignment/AddAssignment`,{
        method:'POST',
        headers:{
            'content-type':'application/json'
        },
        body
    })
}
async function UpdateAssignment(body){
    return fetch(ibexBaseURL+`assignment/UpdateAssignment`,{
        method:'PUT',
        headers:{
            'content-type':'application/json'
        },
        body
    })
}
async function GetWebsitesForAssignment(id){
    return fetch(ibexBaseURL+`Website/GetWebsitesForAssignment/${id}`)
}
async function GetSkeletonForAssignment(id){
    return fetch(ibexBaseURL+`Skeleton/GetSkeletonsForAssignment/${id}`)
}
async function GetBugsForWebsite(id){
    return fetch(ibexBaseURL+`bug/getbugsForWebsite/${id}`)
}

async function GetParentWebsites(){
    return fetch(ibexBaseURL+'Website/GetParentWebsites')
}
async function GetWebsites(){
    return fetch(ibexBaseURL+'Website/GetWebsites')
}
async function GetWebsite(id){
    return fetch(ibexBaseURL+`Website/GetWebsite/${id}`)
}
async function AddWebsite(body){
    return fetch(ibexBaseURL+`website/AddWebsite`,{
        method:'POST',
        headers:{
            'content-type':'application/json'
        },
        body
    })
}
async function UpdateWebsite(body){
    return fetch(ibexBaseURL+`website/UpdateWebsite`,{
        method:'POST',
        headers:{
            'content-type':'application/json'
        },
        body
    })
}

async function GetTestFrameworks(){
    return fetch(ibexBaseURL+`Skeleton/GetTestingFrameworks`)
}
async function AddSkeleton(body){
    return fetch(ibexBaseURL+'skeleton/addSkeleton',{
        method:'POST',
        headers:{
            'content-type':'application/json'
        },
        body
    })
}
async function UpdateSkeleton(body){
    return fetch(ibexBaseURL+'skeleton/updateSkeleton',{
        method:'PUT',
        headers:{
            'content-type':'application/json'
        },
        body
    })
}

async function GetLanguages(){
    return fetch(ibexBaseURL+'skeleton/GetLanguages')
}

async function AddLanguage(body){
    return fetch(ibexBaseURL+'tools/addLanguage',{
        method:'POST',
        headers:{
            'content-type':'application/json'
        },
        body
    })
}
async function AddFramework(body){
    return fetch(ibexBaseURL+'tools/addFramework',{
        method:'POST',
        headers:{
            'content-type':'application/json'
        },
        body
    })
}
async function UpdateFramework(body){
    return fetch(ibexBaseURL+'tools/updateFramework',{
        method:'PUT',
        headers:{
            'content-type':'application/json'
        },
        body
    })
}
async function UpdateLanguage(body){
    return fetch(ibexBaseURL+'tools/updateLanguage',{
        method:'PUT',
        headers:{
            'content-type':'application/json'
        },
        body
    })
}

export const requests = {
    GetAssignmentsForShark,
    GetAssignmentForShark,
    GetWebsiteTree,
    GetSkeletons,
    GetSkeleton,
    AddAssignment,
    UpdateAssignment,
    GetSkeletonForAssignment,
    GetWebsitesForAssignment,
    GetBugsForWebsite,
    GetParentWebsites,
    GetWebsites,
    GetWebsite,
    AddWebsite,
    UpdateWebsite,
    GetTestFrameworks,
    AddSkeleton,
    UpdateSkeleton,
    GetLanguages,
    AddLanguage,
    AddFramework,
    UpdateLanguage,
    UpdateFramework
}

