using Microsoft.EntityFrameworkCore;
using VSC.Core.DemoDataLinks.Data;
using VSC.Core.DemoDataLinks.Model;
using VSC.Core.Services.Interfaces;

namespace VSC.Core.DemoContacts.Services
{
    public class PeopleDataService : DemoDataServiceBase, IInstanceService<Person>, IDetailService<Person>
    {
        
        public PeopleDataService(DemoDatabaseContext context) 
            : base(context) { }

        public async Task<int> Count(Guid instanceId)
        {
            return await _database.People.CountAsync(x => x.InstanceId == instanceId);
        }

        public async Task<int> CountFiltered(Guid instanceId, Func<IQueryable<Person>, IQueryable<Person>> filterSortQuery)
        {
            var query = filterSortQuery(_database.People.Where(x => x.InstanceId == instanceId));
            return await query.CountAsync();
        }

        public async Task<Person?> Create(Person insert)
        {
            insert.PersonId = Guid.NewGuid();
            insert.Inserted = DateTime.UtcNow;
            if (insert.ContactGroupId == Guid.Empty) insert.ContactGroupId = null;

            _database.People.Add(insert);
            await _database.SaveChangesAsync();
            return insert;
        }

        public async Task<Person?> Delete(Guid instanceId, Guid id)
        {
            var deleted = await _database.People
                .Include(x => x.ContactGroup)
                .FirstOrDefaultAsync(x => x.InstanceId == instanceId && x.PersonId == id);
            if (deleted == null) return null;
            _database.People.Remove(deleted);
            await _database.SaveChangesAsync();
            return deleted;

        }

        public async Task<Person?> GetById(Guid instanceId, Guid id)
        {
            return await _database.People
                .Include(x => x.ContactGroup)
                .FirstOrDefaultAsync(x => x.InstanceId == instanceId && x.PersonId == id);
        }

        public async Task<Person?> GetDetail(Guid instanceId, Guid id)
        {
            return await _database.People
                .Include(x => x.ContactGroup)
                .Include(x => x.ContactData).ThenInclude(x => x.ContactType)
                .Include(x=>x.PersonalInterests).ThenInclude(x=>x.Interest)
                .FirstOrDefaultAsync(x => x.InstanceId == instanceId && x.PersonId == id);
        }

        public Task<Person?> GetQuery(Guid instanceId, Func<IQueryable<Person>, IQueryable<Person>> filterQuery)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<Person>> List(Guid instanceId, Func<IQueryable<Person>, IQueryable<Person>> filterSortQuery)
        {
            var query = filterSortQuery(_database.People
                .Include(x => x.ContactGroup)
                .Where(x => x.InstanceId == instanceId)
                .OrderBy(x => x.LastName).ThenBy(x => x.FirstName));

            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Person>> ListPagedFiltered(Guid instanceId, Func<IQueryable<Person>, IQueryable<Person>> filterSortQuery, int pageNo, int pageSize)
        {
            var query = filterSortQuery(_database.People
                .Include(x=>x.ContactGroup)
                .Include(x => x.PersonalInterests).ThenInclude(x=>x.Interest)
                .Where(x => x.InstanceId == instanceId)
                .OrderBy(x => x.LastName).ThenBy(x=>x.FirstName))
                .Skip(pageSize*(pageNo-1)).Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<Person?> Update(Person update)
        {
            var person = await _database.People
            .Include(x => x.ContactGroup)
                .FirstOrDefaultAsync(x => x.InstanceId == update.InstanceId && x.PersonId == update.PersonId);
            if (person == null) return null;
            if (update.ContactGroupId == Guid.Empty)
            {
                person.ContactGroupId = null;
            }
            else
            {
                person.ContactGroupId = update.ContactGroupId;
            }
            person.Updated = DateTime.UtcNow;
            person.LastChangeBy = update.LastChangeBy;
            person.FirstName = update.FirstName;
            person.LastName = update.LastName;
            await _database.SaveChangesAsync();
            return person;
        }
    }
}
