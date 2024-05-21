namespace lynx
{
    public static partial class Queries
    {
        //Seed queries
        public const string DatabaseExists = "SELECT * FROM sys.databases WHERE name = kino";
        public const string SeedQuery = "";
        //Queries for Group

        public const string AddGroup = "INSERT INTO dbo.usergroup (name,description) OUTPUT inserted.group_id VALUES (@name,@description)";

        public const string DeleteGroup = "DELETE FROM dbo.usergroup WHERE group_id = @id";

        public const string GetAllGroups = "SELECT g.group_id as id ,g.* FROM dbo.usergroup g";

        public const string GetGroupById = "SELECT g.group_id as id ,g.* FROM dbo.usergroup g WHERE group_id = @id";

        public const string UpdateGroup = "UPDATE dbo.usergroup SET name = @name, description = @description WHERE group_id = @id";

        public const string AddUserToGroup = "INSERT INTO dbo.a_group_user (user_id,group_id) VALUES (@userid,@groupid)";

        public const string AddGroupToGroup = "DELETE U\r\nFROM dbo.a_group_user U\r\nWHERE U.group_id = @groupid_1\r\nAND EXISTS (\r\n    SELECT 1\r\n    FROM dbo.a_group_user UU \r\n    WHERE UU.group_id = @groupid_2\r\n    AND uu.user_id = u.user_id\r\n);\r\n\r\nUPDATE dbo.a_group_user SET\r\n    group_id = @groupid_1\r\nWHERE group_id = @groupid_2;";
            //"DELETE FROM dbo.a_group_user WHERE user_id IN " +
            //"(SELECT user_id FROM dbo.a_group_user " +
            //"WHERE group_id = @groupid_2) " +
            //"AND  group_id = @groupid_1; " +
            //"INSERT INTO dbo.a_group_user (user_id,group_id) " +
            //"SELECT user_id, @groupid_2 FROM dbo.a_group_user " +
            //"WHERE group_id = @groupid_2";

        public const string GetGroupsForContest = "select a.group_id as id,* from a_contest_group a " +
            "left join usergroup b on a.group_id = b.group_id where a.contest_id = @contest_id";

        public const string GetUsersForGroup = "select a.user_id as id,* from a_group_user a " +
            "left join appuser b on a.user_id = b.user_id where a.group_id = @group_id";

        public const string DeleteUsersForGroup = "delete from kino.dbo.a_group_user where group_id = @id";

        //Queries for Assignments
        public const string GetAssignment = "select assignment_id as id, * from kino.dbo.assignment where assignment_id = @id";
        public const string GetAllAssignments = "select assignment_id as id,* from assignment";

        public const string CreateAssignment = "insert into kino.dbo.assignment (parent_id,name,description,points,website_link) OUTPUT inserted.assignment_id values " +
                                                "(@parent_id,@name,@description,@points,@website_link)";
        public const string UpdateAssignment = "update kino.dbo.assignment set name=@name,description=@description,points=@points where assignment_id=@id";

        public const string DeleteAssignment = "delete from kino.dbo.assignment where assignment_id=@id";
        //Queries for Submission

        public const string GetSubmissionById = "SELECT s.submission_id as id, s.* FROM dbo.submission s WHERE submission_id = @submission_id";

        public const string CreateSubmission = "INSERT INTO kino.dbo.submission (user_id,test_fw_id,lang_fw,assignment_id,assignment_parent_id,language_id,source_code,status,created_at) " +
                                               "OUTPUT inserted.submission_id " +
                                               "VALUES (@user_id,@test_fw_id,@lang_fw,@assignment,@assignment_parent,@language,@sourcecode,@status,GETDATE())";

        public const string GetSubmissionsForAssignmentForUser = "select submission_id as id,* from submission " +
                                "where user_id = @user_id and assignment_id = @assignment_id";


        public const string GetUserSubmissionQueue = 
            "SELECT s.submission_id,a.name as assignment_name,s.auto_verdict,s.postmoderation_verdict,l.name,s.created_at as submitted_at " +
            "FROM dbo.submission s " +
            "LEFT JOIN dbo.assignment a ON s.assignment_id = a.assignment_id " +
            "LEFT JOIN dbo.language l ON s.language_id = l.language_id " +
            "WHERE s.user_id = @userid";

        public const string GetAllSubmissionsForAssignmentForContest = "select submission_id as id,* from dbo.submission s " +
                                "left join dbo.a_contest_assignment aca on s.assignment_id = aca.assignment_id " +
                                "left join dbo.contest c on aca.contest_id = c.contest_id " +
                                "where c.contest_id = @contest_id and s.assignment_id = @assignment_id";

        public const string GetSubmissionsForUserForContest = "select submission_id as id,* from submission s " +
            "left join a_group_user agu on agu.user_id = s.user_id " +
            "left join a_contest_group acg on acg.group_id = agu.group_id " +
            "where s.user_id = @user_id and acg.contest_id = @contest_id";

        public const string CreateComment = "insert into kino.dbo.comment (comment_parent_id,submission_id,user_id,comment_text) output inserted.comment_id values " +
            "(@parent_id,@submission_id,@user_id,@comment_text)";
        public const string GetCommentsForSubmission = "select * from comment c " +
            "left join dbo.appuser au on au.user_id = c.user_id "+
            "where submission_id=@submission_id";
        public const string PostmoderateSubmission = "update dbo.submission set postmoderation_verdict=@accept where submission_id=@submission_id";

        //Queries for Contest
        public const string CreateContestFromContest = 
            "DECLARE @temptable TABLE (contest_id INT); " +
            "DECLARE @temp INT; " +
            "INSERT INTO dbo.contest (name,description,opening_time,closing_time,is_active) " +
            "OUTPUT inserted.contest_id INTO @temptable " +
            "OUTPUT inserted.contest_id " +
            "VALUES (@name,@description,@openingtime,@closingtime,@isactive) " +
            "SELECT @temp = contest_id FROM @temptable " +
            "INSERT INTO dbo.a_contest_assignment (assignment_id,contest_id) " +
            "SELECT assignment_id, @temp FROM dbo.a_contest_assignment WHERE contest_id = @contestid";

        public const string AddContest = "INSERT INTO dbo.contest (name,description,opening_time,closing_time,is_active) " +
                                        "OUTPUT inserted.contest_id " +
                                        "VALUES (@name,@description,@openingtime,@closingtime,@isactive)";

        public const string DeleteContest = "DELETE FROM dbo.contest WHERE contest_id = @id";

        public const string GetContest = "SELECT c.contest_id as id,c.* FROM dbo.contest c WHERE contest_id = @id";

        public const string GetContests = "SELECT c.contest_id as id,c.* FROM dbo.contest c";

        public const string GetContestAssignments = "SELECT a.assignment_id as id,* FROM dbo.a_contest_assignment ca \r\nLEFT JOIN dbo.assignment a ON ca.assignment_id = a.assignment_id \r\nWHERE contest_id = @contestid";
        //"SELECT a.assignment_id,a.name AS assignment_name,a.points FROM dbo.a_contest_assignment ca " +
        //"LEFT JOIN dbo.assignment a ON ca.assignment_id = a.assignment_id " +
        //"WHERE contest_id = @contestid";

        public const string GetContestRankings = "select username,t.user_id,sum(points) as total_points from appuser au\r\n right join (\r\n\tselect s.assignment_id,user_id,max(a.points) as points from submission s\r\n\tleft join assignment a on a.assignment_id  = s.assignment_id\r\n\tleft join a_contest_assignment aca on s.assignment_id = aca.assignment_id\r\n\twhere contest_id = @contestid and postmoderation_verdict = 1\r\n\tgroup by s.assignment_id,user_id\r\n\t\r\n) t on t.user_id = au.user_id\r\ngroup by username,t.user_id order by total_points desc"; 
            //"SELECT c.contest_id,s.user_id, u.username,SUM(a.points) total_points " +
            //"FROM dbo.submission s " +
            //"LEFT JOIN dbo.appuser u ON s.user_id = u.user_id " +
            //"LEFT JOIN dbo.assignment a ON s.assignment_id = a.assignment_id " +
            //"LEFT JOIN dbo.a_contest_assignment ca ON a.assignment_id = ca.assignment_id " + 
            //"LEFT JOIN dbo.contest c ON ca.contest_id = c.contest_id " +
            //"WHERE ca.contest_id = @contestid " +
            //"GROUP BY u.username,c.contest_id,s.user_id,u.username " +
            //"ORDER BY total_points DESC";

        public const string AddGroupToContest = "INSERT INTO dbo.a_contest_group (group_id,contest_id) VALUES (@groupid,@contestid)";
        public const string AddAssignmentsToContest = "INSERT INTO dbo.a_contest_assignment (assignment_id,contest_id) VALUES (@assignmentid,@contestid)";

        public const string GetContestsForUser = "select c.contest_id as id,name,description,is_active,opening_time,closing_time from dbo.contest c " +
                                                "left join dbo.a_contest_group acg on c.contest_id = acg.contest_id " +
                                                "left join dbo.a_group_user agu on agu.group_id = acg.group_id " +
                                                "where agu.user_id = @id";

        public const string GetAssignmentsForContest = "select a.assignment_id as id,a.* from a_contest_assignment aca " +
                                                        "right join assignment a on a.assignment_id = aca.assignment_id " +
                                                        "where aca.contest_id = @contestid";

        public const string UpdateContest = "update kino.dbo.contest set name=@name,description=@description,opening_time=@openingtime,closing_time=@closingtime,is_active=@isactive " +
                                            "where contest_id = @id";
        public const string DeleteGroupsForContest = "delete from kino.dbo.a_contest_group where contest_id = @contestid";
        public const string DeleteAssignmentsForContest = "delete from kino.dbo.a_contest_assignment where contest_id = @contestid";

        public const string GetContestsForAssignment = "select c.contest_id as id,name,is_active from a_contest_assignment \r\nleft join contest c on c.contest_id = a_contest_assignment.contest_id\r\nwhere assignment_id = @assignment_id";


        //Queries for Role

        public const string CreateRole = "INSERT INTO dbo.role (name) VALUES (@name)";

        public const string DeleteRoleById = "DELETE FROM dbo.role WHERE role_id = @id";

        public const string DeleteRoleByName = "DELETE FROM dbo.role WHERE name = @name";

        public const string RoleExists = "SELECT CASE " +
                                        "WHEN EXISTS(SELECT * FROM dbo.role WHERE name = @name) THEN CAST(1 AS BIT)" +
                                        "ELSE CAST(0 AS BIT)" +
                                        "END";
        public const string AddUserToRole = "INSERT INTO dbo.a_appuser_role (user_id,role_id) VALUES (@userid,@roleid)";
        //Queries for User

        public const string GetUserRoles = "SELECT b.name FROM dbo.a_appuser_role a " +
            "LEFT JOIN dbo.role b ON a.role_id = b.role_id " +
            "WHERE a.user_id = @userid";

        //Queries for Auth

        const string Level_1_role = "1";
        public static string Register = "DECLARE @tempid INT;" +
            "DECLARE @temptable TABLE (user_id INT);" +
            "INSERT INTO dbo.appuser (username, email, password)" +
            "OUTPUT inserted.user_id INTO @temptable " +
            "VALUES (@username, @email, @password) " +
            "SELECT @tempid = user_id FROM @temptable " +
            $"INSERT INTO dbo.a_appuser_role (user_id,role_id) VALUES (@tempid,{Level_1_role}) " +
            "SELECT user_id as id, username, email, password FROM dbo.appuser WHERE user_id = @tempid";

        public const string Login = "SELECT user_id as id, username, email, password FROM dbo.appuser WHERE email = @email AND password = @password ";

        //Queries for User
        public const string GetUsers = "select user_id as id,username,email from kino.dbo.appuser";


    }
}
