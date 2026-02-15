using Microsoft.EntityFrameworkCore;
using VSC.Core.DemoDataLinks.Data;
using VSC.Core.DemoDataLinks.Model;
using VSC.Core.Services.Interfaces;

namespace VSC.Core.DemoContacts.Services
{
    public class InterestService : DemoDataServiceBase, IInstanceService<Interest>, IDetailService<Interest>
    {
        public InterestService(DemoDatabaseContext context) : base(context) { }

        public async Task<int> Count(Guid instanceId)
        {
            return await _database.Interests.CountAsync(x => x.InstanceId == instanceId);
        }

        public async Task<int> CountFiltered(Guid instanceId, Func<IQueryable<Interest>, IQueryable<Interest>> filterSortQuery)
        {
            //no filters
            return await Count(instanceId);
        }

        public async Task<Interest?> Create(Interest insert)
        {
            insert.InterestId = Guid.NewGuid();
            insert.Inserted = DateTime.UtcNow;
            _database.Interests.Add(insert);
            await _database.SaveChangesAsync();
            return insert;
        }

        public async Task<Interest?> Delete(Guid instanceId, Guid id)
        {
            var deleted = await _database.Interests
                .FirstOrDefaultAsync(x => x.InstanceId == instanceId && x.InterestId == id);
            if (deleted != null)
            {
                _database.Interests.Remove(deleted);
                await _database.SaveChangesAsync();
            }
            return deleted;
        }

        public async Task<Interest?> GetById(Guid instanceId, Guid id)
        {
            return await _database.Interests
                .FirstOrDefaultAsync(x => x.InstanceId == instanceId && x.InterestId == id);
        }

        public async Task<Interest?> GetDetail(Guid instanceId, Guid id)
        {
            return await _database.Interests
                .Include(x=>x.PersonalInterests).ThenInclude(x=>x.Person)
                .FirstOrDefaultAsync(x => x.InstanceId == instanceId && x.InterestId == id);
        }

        public Task<Interest?> GetQuery(Guid instanceId, Func<IQueryable<Interest>, IQueryable<Interest>> filterQuery)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Interest>> List(Guid instanceId, Func<IQueryable<Interest>, IQueryable<Interest>> filterSortQuery)
        {
            return await _database.Interests
                .Include(x=>x.PersonalInterests)
              .Where(x => x.InstanceId == instanceId)
              .OrderBy(x => x.Title).ToListAsync();
        }

        public async Task<IEnumerable<Interest>> ListPagedFiltered(Guid instanceId, Func<IQueryable<Interest>, IQueryable<Interest>> filterSortQuery, int pageNo, int pageSize)
        {
            var query = filterSortQuery(_database.Interests
                .Include(x => x.PersonalInterests)
              .Where(x => x.InstanceId == instanceId)
              .OrderBy(x => x.Title));

            return await query.ToListAsync();
        }

        public async Task<Interest?> Update(Interest update)
        {
            var Interest = await _database.Interests
                .FirstOrDefaultAsync(x => x.InstanceId == update.InstanceId && x.InterestId == update.InterestId);
            if (Interest != null)
            {
                Interest.Updated = DateTime.UtcNow;
                Interest.LastChangeBy = update.LastChangeBy;
                Interest.Title = update.Title;
                Interest.Description = update.Description;
                await _database.SaveChangesAsync();
            }
            return Interest;
        }
    }
}
