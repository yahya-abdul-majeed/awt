using Dapper;
using lynx.Data;
using lynx.Models;
using lynx.Models.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Net.Mime;

namespace lynx.Services
{
    public interface IContestService
    {
        Task<GetContestDTO?> GetContest(int id);
        Task<IEnumerable<GetContestDTO>> GetContests();
        Task<int> AddContest(AddContestDTO contest);
        Task<int> AddContest(AddContestDTO contest, int contestid);
        Task<int> DeleteContest(int id);
        Task<IEnumerable<Assignment>> GetContestAssignments(int contestid);
        Task<IEnumerable<ContestRankingItem>> GetContestRankings(int contestid);
        Task<int> AddGroupToContest(int contestid, int groupid);
        Task<int> AddGroupsToContest(int contestid,string[] groupid);
        Task<int> AddAssignmentsToContest(List<AssignmentAdditionDTO> additions);
        Task<int> AddRemoteAssignmentsToContest(List<int> remote_ids,int contest_id);
        Task<IEnumerable<GetContestDTO>> GetContestsForUser(int id);
        Task<int> EditContest(int id,Contest contest, string[] groups, string[] assignments,
            bool updateContest=true,bool updateGroups=true,bool updateAssignments=true);
        Task<IEnumerable<GetContestForAssignmentDTO>> GetContestsForAssignment(int assignment_id);

    }
    public class ContestService : IContestService
    {
        private readonly DapperContext _context;
        private readonly IAssignmentService _assignmentService;
        private readonly IbexClient _ibexClient;
        public ContestService(DapperContext context,IAssignmentService assignmentService, IbexClient ibexClient)
        {
            _context = context;
            _assignmentService = assignmentService;
            _ibexClient = ibexClient;
        }

        public async Task<int> AddAssignmentsToContest(List<AssignmentAdditionDTO> additions) 
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(Queries.AddAssignmentsToContest,additions);
            }
        }
        public async Task<int> AddRemoteAssignmentsToContest(List<int> remote_ids,int contest_id) 
        {
            //1 -> we must request assignments in bulk from ibex, using ibexClient
            var assignments = await _ibexClient.GetSpecificAssignments(remote_ids);
            //2 -> we must add assignments to lynx in bulk
            var ids = (await _assignmentService.CreateAssignmentsBulk(assignments.ToList()));
            var objs = ids.Select(id=>new AssignmentAdditionDTO
            {
                assignmentid = id,
                contestid = contest_id
            });
            return await AddAssignmentsToContest(objs.ToList());

        }

        public async Task<int> AddContest(AddContestDTO contest)
        {
            var parameters = new
            {
                name = contest.name,
                description = contest.description,
                openingtime = contest.opening_time,
                closingtime = contest.closing_time,
                isactive = contest.is_active
            };

            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleAsync<int>(Queries.AddContest, parameters);
            }
            //using (var connection = _context.CreateConnection())
            //{
            //    return await connection.ExecuteAsync(Queries.AddContest, parameters);
            //}
        }

        public async Task<int> AddContest(AddContestDTO contest, int contestid)
        {
            var parameters = new 
            {
                contestid,
                name = contest.name,
                description = contest.description,
                openingtime = contest.opening_time,
                closingtime = contest.closing_time,
                isactive = contest.is_active
            };
            using(var connection = _context.CreateConnection())
            {
                var res = await connection.QuerySingleAsync<int>(Queries.CreateContestFromContest, parameters);
                return res;
            }
        }

        public async Task<int> AddGroupsToContest([FromBody] int contestid, [FromBody] string[] groupid)
        {
            if (groupid.IsNullOrEmpty())
                return 0;
            string AddGroupsToContest = "INSERT INTO dbo.a_contest_group (group_id,contest_id) VALUES ";
            foreach (var id in groupid)
            {
                AddGroupsToContest += $"({id},{contestid}),";
            }
            AddGroupsToContest = AddGroupsToContest.Remove(AddGroupsToContest.Length - 1);
            using(var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(AddGroupsToContest); 
            }
        }

        public async Task<int> AddGroupToContest(int contestid, int groupid)
        {
            var parameters = new {contestid, groupid};
            using(var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(Queries.AddGroupToContest, parameters);
            } 
        }

        public async Task<int> DeleteContest(int id)
        {
            var parameters = new
            {
                id
            };

            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(Queries.DeleteContest, parameters);
            }
        }

        public async Task<GetContestDTO?> GetContest(int id)
        {
            var parameters = new
            {
                id
            };

            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<GetContestDTO>(Queries.GetContest, parameters);
            }
        }

        public async Task<IEnumerable<Assignment>> GetContestAssignments(int contestid)
        {
            var parameters = new
            {
               contestid 
            };

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Assignment>(Queries.GetContestAssignments, parameters);
            }
        }

        public async Task<IEnumerable<ContestRankingItem>> GetContestRankings(int contestid)
        {
            var parameters = new
            {
               contestid 
            };

            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<ContestRankingItem>(Queries.GetContestRankings, parameters);
            }
        }

        public async Task<IEnumerable<GetContestDTO>> GetContests()
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<GetContestDTO>(Queries.GetContests);
            }
        }

        public async Task<IEnumerable<GetContestDTO>> GetContestsForUser(int id)
        {
            var parameters = new { id };
            using(var connection = _context.CreateConnection())
            {
                var contests = await connection.QueryAsync<GetContestDTO>(Queries.GetContestsForUser,parameters);
                foreach(var contest in contests)
                {
                     contest.Assignments = await connection.QueryAsync<Assignment>(Queries.GetAssignmentsForContest, new {contestid=contest.id});
                }

                return contests;
            }
        }

        public async Task<int> EditContest(int id,Contest contest, string[] groups, string[] assignments, bool updateContest = true, bool updateGroups = true, bool updateAssignments = true)
        {
            try
            {
                using(var connection = _context.CreateConnection())
                {
                    if (updateContest)
                    {
                        await connection.ExecuteAsync(Queries.UpdateContest, new
                        {
                            id,
                            name = contest.name,
                            description = contest.description,
                            openingtime = contest.opening_time,
                            closingtime = contest.closing_time,
                            isactive = contest.is_active    
                        });
                    }
                    if (updateGroups)
                    {
                        await connection.ExecuteAsync(Queries.DeleteGroupsForContest,new {contestid = id});
                        await AddGroupsToContest(id,groups);
                    }
                    if (updateAssignments)
                    {
                        await connection.ExecuteAsync(Queries.DeleteAssignmentsForContest,new {contestid = id});
                        var objs = assignments.Select(assignment_id=>new AssignmentAdditionDTO
                        {
                            assignmentid = Int32.Parse(assignment_id),
                            contestid = id
                        });
                        await AddAssignmentsToContest(objs.ToList());
                    }
                }
                return 1;

            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public async Task<IEnumerable<GetContestForAssignmentDTO>> GetContestsForAssignment(int assignment_id)
        {
            using(var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<GetContestForAssignmentDTO>(Queries.GetContestsForAssignment, new {assignment_id});
            }
        }
    }
}
