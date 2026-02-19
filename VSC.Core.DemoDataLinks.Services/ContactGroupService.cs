using Microsoft.EntityFrameworkCore;
using VSC.Core.DemoDataLinks.Data;
using VSC.Core.DemoDataLinks.Model;
using VSC.Core.Services.Interfaces;

namespace VSC.Core.DemoContacts.Services
{
    public class ContactGroupService : DemoDataServiceBase, IInstanceService<ContactGroup>, IDetailService<ContactGroup>
    {
        public ContactGroupService(DemoDatabaseContext context) : base(context) { }

        public async Task<int> Count(Guid instanceId)
        {
            return await _database.ContactGroups.CountAsync(x => x.InstanceId == instanceId);
        }

        public async Task<int> CountFiltered(Guid instanceId, Func<IQueryable<ContactGroup>, IQueryable<ContactGroup>> filterSortQuery)
        {
            //no filter 
            return await Count(instanceId);
        }

        public async Task<ContactGroup?> Create(ContactGroup insert)
        {
            insert.ContactGroupId = Guid.NewGuid();
            insert.Inserted = DateTime.UtcNow;
            _database.ContactGroups.Add(insert);
            await _database.SaveChangesAsync();
            return insert;
        }

        public async Task<ContactGroup?> Delete(Guid instanceId, Guid id)
        {
            var deleted = await _database.ContactGroups
                .FirstOrDefaultAsync(x => x.InstanceId == instanceId && x.ContactGroupId == id);
            if (deleted != null)
            {
                _database.ContactGroups.Remove(deleted);
                await _database.SaveChangesAsync();
            }
            return deleted;
        }

        public async Task<ContactGroup?> GetById(Guid instanceId, Guid id)
        {
            return await _database.ContactGroups
               .FirstOrDefaultAsync(x => x.InstanceId == instanceId && x.ContactGroupId == id);
        }

        public async Task<ContactGroup?> GetDetail(Guid instanceId, Guid id)
        {
            return await _database.ContactGroups
                .Include(x=>x.People)
               .FirstOrDefaultAsync(x => x.InstanceId == instanceId && x.ContactGroupId == id);
        }

        public Task<ContactGroup?> GetQuery(Guid instanceId, Func<IQueryable<ContactGroup>, IQueryable<ContactGroup>> filterQuery)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ContactGroup>> List(Guid instanceId, Func<IQueryable<ContactGroup>, IQueryable<ContactGroup>> filterSortQuery)
        {
            var query = filterSortQuery(_database.ContactGroups
                .Include(x=>x.People)
              .Where(x => x.InstanceId == instanceId));

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<ContactGroup>> ListPagedFiltered(Guid instanceId, Func<IQueryable<ContactGroup>, IQueryable<ContactGroup>> filterSortQuery, int pageNo, int pageSize)
        {
            var query = filterSortQuery(_database.ContactGroups
                .Include(x => x.People)
              .Where(x => x.InstanceId == instanceId))
              .Skip(pageSize  * (pageNo - 1)).Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<ContactGroup?> Update(ContactGroup update)
        {
            var contactGroup = await _database.ContactGroups
                .FirstOrDefaultAsync(x => x.InstanceId == update.InstanceId && x.ContactGroupId == update.ContactGroupId);
            if (contactGroup != null)
            {
                contactGroup.Updated = DateTime.UtcNow;
                contactGroup.LastChangeBy = update.LastChangeBy;
                contactGroup.Title = update.Title;
                await _database.SaveChangesAsync();
            }
            return contactGroup;
        }
    }
}
