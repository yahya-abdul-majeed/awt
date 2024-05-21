namespace crow;

public static class Queries{
        public const string GetSubmission = 
                "SELECT submission_id as id, assignment_parent_id, source_code,test_fw_id,lang_fw " +
                "FROM submission WHERE submission_id = @submission_id";
        public const string GetSubmissionContext = 
                "SELECT code_before, code_after FROM assignment a " +
                "LEFT JOIN a_assignment_skeleton aas ON a.assignment_id =  aas.assignment_id " +
                "LEFT JOIN skeleton s ON s.skeleton_id = aas.skeleton_id " +
                "WHERE a.assignment_id = @assignment_id and s.testing_fw_id = @testing_fw_id " +
                "SELECT hosted_at as url, aaw.should_pass FROM website w " +
                "LEFT JOIN a_assignment_website aaw ON w.website_id = aaw.website_id " +
                "WHERE assignment_id = @assignment_id "; 

        public const string OnSubmissionStart = "update dbo.submission set status=@status,execution_host=@execution_host,created_at=@started_at where submission_id=@id";
        public const string OnSubmissionEnd = "update dbo.submission set status=@status,finished_at=@finished_at,result=@result,auto_verdict=@auto_verdict where submission_id=@id";



}