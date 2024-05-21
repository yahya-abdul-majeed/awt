using Dapper;
using lynx.Data;
using lynx.Models;
using lynx.Models.Auth;
using lynx.Models.DTO;
using System.Security.Cryptography;

namespace lynx.Services
{
    public interface IGroupService
    {
        Task<IEnumerable<GetGroupDTO>> GetAllGroups();
        Task<GetGroupDTO?> GetGroupById(int id);
        Task<int> AddGroup(AddGroupDTO group);
        Task<int> DeleteGroup(int id);
        Task<int> UpdateGroup(int id, UpdateGroupDTO group);
        Task<int> AddUserToGroup(int userid, int groupid);
        Task<int> AddUsersToGroup(List<int> userids, int groupid);
        Task<int> AddGroupToGroup(int groupid_1,int groupid_2); //adds group_2 to group_1
        Task<IEnumerable<Group>> GetGroupsForContest(int contest_id);
        Task<int> EditGroup(int id, Group group, List<int> userids);

    }
    public class GroupService : IGroupService
    {
        private readonly DapperContext _context;
        public GroupService(DapperContext context)
        {
            _context = context;
        }
        public async Task<int> AddGroup(AddGroupDTO group)
        {
            var parameters = new { 
                name = group.name,
                description = group.description
            };

            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleAsync<int>(Queries.AddGroup, parameters);
            }
        }

        public async Task<int> AddGroupToGroup(int groupid_1, int groupid_2)
        {
            var parameters = new
            {
                groupid_1,
                groupid_2
            };
            using(var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(Queries.AddGroupToGroup, parameters);
            }
        }

        public async Task<int> AddUserToGroup(int userid, int groupid)
        {
            var parameters = new
            {
                userid, groupid
            };
            using(var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(Queries.AddUserToGroup, parameters);
            }
        }
        public async Task<int> AddUsersToGroup(List<int> userids, int groupid)
        {
            if (userids.Count == 0)
                return 0;
            var query = "INSERT INTO dbo.a_group_user (user_id,group_id) VALUES ";
            foreach (var userid in userids)
            {
                query += $"({userid},{groupid}),";
            }
            query = query.Remove(query.Length - 1);

            using(var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(query);
            }
        }

        public async Task<int> DeleteGroup(int id)
        {
            var parameters = new { id };

            using(var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(Queries.DeleteGroup, parameters);
            }
        }

        public async Task<IEnumerable<GetGroupDTO>> GetAllGroups()
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<GetGroupDTO>(Queries.GetAllGroups);
            }
        }

        public async Task<GetGroupDTO?> GetGroupById(int id)
        {
            var parameters = new { id };
            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleOrDefaultAsync<GetGroupDTO>(Queries.GetGroupById,parameters);
                
            }
        }

        public async Task<IEnumerable<Group>> GetGroupsForContest(int contest_id)
        {
            var parameters = new { contest_id };

            using (var connection = _context.CreateConnection())
            {
                var groups = await connection.QueryAsync<Group>(Queries.GetGroupsForContest,parameters);
                foreach(var group in groups)
                {
                    group.users = (await connection.QueryAsync<User>(Queries.GetUsersForGroup,new {group_id = group.id})).ToList();
                }
                return groups;
            }
        }

        public async Task<int> UpdateGroup(int id, UpdateGroupDTO group)
        {
            var parameters = new
            {
                id,
                name = group.name,
                description = group.description
            };
            using (var connection = _context.CreateConnection())
            {
                return await connection.ExecuteAsync(Queries.UpdateGroup,parameters);
            }
        }

        public async Task<int> EditGroup(int id, Group group, List<int> userids)
        {
            using (var connection = _context.CreateConnection())
            {
                //update group
                await connection.ExecuteAsync(Queries.UpdateGroup, new
                {
                    id,
                    name = group.name,
                    description = group.description
                });
                //update users for group
                await connection.ExecuteAsync(Queries.DeleteUsersForGroup, new { id });
                await AddUsersToGroup(userids, id);
            }
            return 1;

        }
    }
}
