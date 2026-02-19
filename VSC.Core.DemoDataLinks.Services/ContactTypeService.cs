using Microsoft.EntityFrameworkCore;
using VSC.Core.DemoDataLinks.Data;
using VSC.Core.DemoDataLinks.Model;
using VSC.Core.Services.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace VSC.Core.DemoContacts.Services
{
    public class ContactTypeService : DemoDataServiceBase, IInstanceService<ContactType>
    {
        public ContactTypeService(DemoDatabaseContext context) : base(context) { }

        public async Task<int> Count(Guid instanceId)
        {
            return await _database.ContactTypes.CountAsync(x => x.InstanceId == instanceId);
        }

        public async Task<int> CountFiltered(Guid instanceId, Func<IQueryable<ContactType>, IQueryable<ContactType>> filterSortQuery)
        {
            //no filters
            return await Count(instanceId);
        }

        public async Task<ContactType?> Create(ContactType insert)
        {
            insert.ContactTypeId = Guid.NewGuid();
            insert.Inserted = DateTime.UtcNow;
            _database.ContactTypes.Add(insert);
            await _database.SaveChangesAsync();
            return insert;
        }

        public async Task<ContactType?> Delete(Guid instanceId, Guid id)
        {
            var deleted = await _database.ContactTypes
                .FirstOrDefaultAsync(x => x.InstanceId == instanceId && x.ContactTypeId == id);
            if (deleted != null)
            {
                _database.ContactTypes.Remove(deleted);
                await _database.SaveChangesAsync();
            }
            return deleted;
        }

        public async Task<ContactType?> GetById(Guid instanceId, Guid id)
        {
            return await _database.ContactTypes
                .FirstOrDefaultAsync(x => x.InstanceId == instanceId && x.ContactTypeId == id);
        }

        public Task<ContactType?> GetQuery(Guid instanceId, Func<IQueryable<ContactType>, IQueryable<ContactType>> filterQuery)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ContactType>> List(Guid instanceId, Func<IQueryable<ContactType>, IQueryable<ContactType>> filterSortQuery)
        {
            var query = filterSortQuery(_database.ContactTypes
                .Include(x => x.ContactData)
              .Where(x => x.InstanceId == instanceId));
              return await query.ToListAsync();
        }

        public async Task<IEnumerable<ContactType>> ListPagedFiltered(Guid instanceId, Func<IQueryable<ContactType>, IQueryable<ContactType>> filterSortQuery, int pageNo, int pageSize)
        {
            var query = filterSortQuery(_database.ContactTypes
               .Include(x => x.ContactData)
                .Where(x => x.InstanceId == instanceId)) //must have )) BEFORE paging               
                .Skip((pageNo-1)*pageSize).Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<ContactType?> Update(ContactType update)
        {
            var contactType = await _database.ContactTypes
                .FirstOrDefaultAsync(x => x.InstanceId == update.InstanceId && x.ContactTypeId == update.ContactTypeId);
            if (contactType != null)
            {
                contactType.Updated = DateTime.UtcNow;
                contactType.LastChangeBy = update.LastChangeBy;
                contactType.Title = update.Title;
                await _database.SaveChangesAsync();
            }
            return contactType;
        }
    }
}
