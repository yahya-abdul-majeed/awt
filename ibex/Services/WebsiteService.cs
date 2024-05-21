using Dapper;
using ibex.Data;
using ibex.Models;
using ibex.Models.DTO;

namespace ibex.Services
{
    public interface IWebsiteService
    {
        Task<Website> GetWebsite(int id);
        Task<IEnumerable<Website>> GetWebsites();
        Task<IEnumerable<Website>> GetParentWebsites();
        Task DeleteWebsite(int id);
        Task UpdateWebsite(UpdateWebsiteDTO website);
        Task AddWebsite(AddWebsiteDTO website);
        Task<IEnumerable<WebsiteNodeDTO>> GetWebsiteTree();
        Task<IEnumerable<Website>> GetWebsitesForAssignment(int assignment_id);

    }
    public class WebsiteService : IWebsiteService
    {
        private readonly DapperContext _context;
        public WebsiteService(DapperContext context)
        {
            _context = context;
        }
        public async Task AddWebsite(AddWebsiteDTO website)
        {
            using(var connection = _context.CreateConnection())
            {
                //create a website and return id
                var parameters = new
                {
                    website.parent_id,
                    website.hosted_at,
                    website.repository,
                    website.description
                };
                var website_id = await connection.QuerySingleAsync<int>(Queries.AddWebsite, parameters);
                //add bugs to this website
                if(website.bugs.Count > 0)
                {
                    string query = "insert into ibex.dbo.bug (description,commit_link,author,website_id,name) values " +
                        "(@description,@commit_link,@author,@id,@name)";
                    foreach(var bug in website.bugs)
                    {
                        await connection.ExecuteAsync(query, new
                        {
                            bug.name,
                            bug.description,
                            bug.commit_link,
                            bug.author,
                            id = website_id
                        });
                    }
                }
            }
        }

        public Task DeleteWebsite(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Website>> GetParentWebsites()
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Website>(Queries.GetParentWebsites);
            }
        }

        public async Task<Website> GetWebsite(int id)
        {
            using (var connection = _context.CreateConnection())
            {
                return await connection.QuerySingleAsync<Website>(Queries.GetWebsite, new {id});
            }
        }

        public async Task<IEnumerable<Website>> GetWebsites()
        {
            using(var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Website>(Queries.GetWebsites);
            }
        }

        public async Task<IEnumerable<Website>> GetWebsitesForAssignment(int assignment_id)
        {
            using(var connection = _context.CreateConnection())
            {
                return await connection.QueryAsync<Website>(Queries.GetWebsitesForAssignment, new { id = assignment_id });
            }
        }

        public async Task<IEnumerable<WebsiteNodeDTO>> GetWebsiteTree()
        {
            using(var connection = _context.CreateConnection())
            {
                var parents = await connection.QueryAsync<WebsiteNodeDTO>(Queries.GetParentWebsites);
                foreach(var parent in parents)
                {
                    parent.children = (await connection.QueryAsync<WebsiteNodeDTO>(Queries.GetChildWebsites,new {id = parent.id})).ToList();
                }
                return parents;
            }
        }

        public async Task UpdateWebsite(UpdateWebsiteDTO website)
        {
            using(var connection = _context.CreateConnection())
            {
                //update website
                var parameters = new
                {
                    website.id,
                    website.parent_id,
                    website.hosted_at,
                    website.repository,
                    website.description
                };
                await connection.ExecuteAsync(Queries.UpdateWebsite, parameters);
                //remove bugs of this website
                await connection.ExecuteAsync(Queries.RemoveBugsOfWebsite, new {id = website.id});
                //add bugs to this website
                if(website.bugs.Count > 0)
                {
                    string query = "insert into ibex.dbo.bug (description,commit_link,author,website_id,name) values " +
                        "(@description,@commit_link,@author,@id,@name)";
                    foreach(var bug in website.bugs)
                    {
                        await connection.ExecuteAsync(query, new
                        {
                            bug.name,
                            bug.description,
                            bug.commit_link,
                            bug.author,
                            id = website.id
                        });
                    }
                }
            }
        }
    }
}
