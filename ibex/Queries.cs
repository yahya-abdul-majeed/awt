using ibex.Models.DTO;

namespace ibex
{
    public static class Queries
    {
        //queries for assignment

        public const string GetSubmissionContext =
            "SELECT code_before, code_after FROM assignment a " +
            "LEFT JOIN a_assignment_skeleton aas ON a.assignment_id =  aas.assignment_id " +
            "LEFT JOIN skeleton s ON s.skeleton_id = aas.skeleton_id " +
            "WHERE a.assignment_id = @assignment_id and s.testing_fw_id = @testing_fw_id " +
            "SELECT hosted_at as url, aaw.should_pass FROM website w " +
            "LEFT JOIN a_assignment_website aaw ON w.website_id = aaw.website_id " +
            "WHERE assignment_id = @assignment_id ";

        public const string GetAssignments = "select w.hosted_at as website_link,a.assignment_id as id,* from ibex.dbo.assignment a " +
            "left join ibex.dbo.a_assignment_website aaw on a.assignment_id = aaw.assignment_id " +
            "left join ibex.dbo.website w on aaw.website_id = w.website_id " +
            "where aaw.is_primary = 1";

        public const string GetAssignment = "select w.hosted_at as website_link,a.assignment_id as id,* from ibex.dbo.assignment a " +
            "left join ibex.dbo.a_assignment_website aaw on a.assignment_id = aaw.assignment_id " +
            "left join ibex.dbo.website w on aaw.website_id = w.website_id " +
            "where aaw.is_primary = 1 and " +
            "a.assignment_id = @id";

        public const string GetAssignmentsForShark = "select assignment_id as id,* from ibex.dbo.assignment";
        public const string GetAssignmnetForShark = "select assignment_id as id,* from ibex.dbo.assignment where assignment_id=@id";

        public const string CreateAssignment = "insert into ibex.dbo.assignment (name,description,points,author) OUTPUT inserted.assignment_id values " +
            "(@name,@description,@points,@author)";
        public const string RemoveAssignmentSkeletons = "delete from ibex.dbo.a_assignment_skeleton where assignment_id=@id";
        public const string RemoveAssignmentWebsites = "delete from ibex.dbo.a_assignment_website where assignment_id=@id";
        public const string UpdateAssignment = "update ibex.dbo.assignment set name=@name,description=@description,points=@points,author=@author " +
            "where assignment_id=@id";

        //Queries for website
        public const string GetParentWebsites = "select website_id as id, * from ibex.dbo.website where parent_id is NULL";
        public const string GetChildWebsites = "select website_id as id, * from ibex.dbo.website where parent_id=@id";
        public const string GetWebsitesForAssignment = "select w.website_id as id, * from ibex.dbo.website w " +
            "left join ibex.dbo.a_assignment_website aaw on aaw.website_id = w.website_id " +
            "where aaw.assignment_id = @id";
        public const string GetWebsites = "select website_id as id,* from ibex.dbo.website";
        public const string GetWebsite = "select website_id as id,* from ibex.dbo.website where website_id = @id";
        public const string AddWebsite = "insert into ibex.dbo.website (parent_id,repository,hosted_at,description) OUTPUT inserted.website_id values (@parent_id,@repository,@hosted_at,@description)";
        public const string UpdateWebsite = "update ibex.dbo.website set hosted_at=@hosted_at,parent_id=@parent_id,repository=@repository,description=@description where website_id=@id";

        //Queries for skeleton
        public const string GetSkeletons = "select s.skeleton_id as id,s.name as skeleton_name,tf.name as testing_fw_name,l.name as language_name,* from skeleton s\r\nleft join testing_framework tf on s.testing_fw_id = tf.testing_fw_id\r\nleft join language l on l.language_id = tf.language_id";
        public const string GetSkeleton = "select s.skeleton_id as id,s.name as skeleton_name,* from skeleton s where s.skeleton_id=@id";
        public const string GetSkeletonsForAssignment = "select s.skeleton_id as id,s.name as skeleton_name,tf.name as testing_fw_name,l.name as language_name,* from skeleton s\r\nleft join testing_framework tf on s.testing_fw_id = tf.testing_fw_id\r\nleft join language l on l.language_id = tf.language_id " +
            "left join a_assignment_skeleton aas on s.skeleton_id= aas.skeleton_id " +
            "where aas.assignment_id = @id";
        public const string AddSkeleton = "insert into ibex.dbo.skeleton (testing_fw_id,code_before,code_after,name) values (@testing_framework_id,@code_before,@code_after,@name)";
        public const string UpdateSkeleton = "update ibex.dbo.skeleton set testing_fw_id=@testing_framework_id,code_before=@code_before,code_after=@code_after,name=@name where skeleton_id=@id";
        public const string GetLanguages = "select language_id as id,name from ibex.dbo.language";

        //Queries for Bugs
        public const string GetBugs = "select bug_id as id,* from ibex.dbo.bug";
        public const string GetBugsForWebsite = "select bug_id as id,* from ibex.dbo.bug where website_id = @id";
        public const string RemoveBugsOfWebsite = "delete from ibex.dbo.bug where website_id = @id";

        //Queries for testing tools
        public const string GetTestingFrameworks = "select l.name as language_name,tf.name as testing_framework_name,tf.language_id,tf.testing_fw_id as testing_framework_id from testing_framework tf\r\nleft join language l on tf.language_id = l.language_id\r\n";
        public const string AddLanguage = "insert into ibex.dbo.language (name) values (@name)";
        public const string UpdateLanguage = "update ibex.dbo.language set name=@name where language_id=@id";
        public const string AddFramework = "insert into ibex.dbo.testing_framework (language_id,name) values (@language_id,@name)";
        public const string UpdateFramework = "update ibex.dbo.testing_framework set name=@name,language_id=@language_id where testing_fw_id=@id";
    }
}
