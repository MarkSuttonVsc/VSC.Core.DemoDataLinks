using VSC.Core.DemoDataLinks.Data;
using VSC.Core.DemoDataLinks.Model;
using VSC.Core.Services.Interfaces;

namespace VSC.Core.DemoContacts.Services
{
    public class ContactDataService : DemoDataServiceBase, IInstanceService<ContactData>
    {
        public ContactDataService(DemoDatabaseContext context) : base(context) { }

        public Task<int> Count(Guid instanceId)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountFiltered(Guid instanceId, Func<IQueryable<ContactData>, IQueryable<ContactData>> filterSortQuery)
        {
            throw new NotImplementedException();
        }

        public Task<ContactData?> Create(ContactData insert)
        {
            throw new NotImplementedException();
        }

        public Task<ContactData?> Delete(Guid instanceId, Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ContactData?> GetById(Guid instanceId, Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<ContactData?> GetQuery(Guid instanceId, Func<IQueryable<ContactData>, IQueryable<ContactData>> filterQuery)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ContactData>> List(Guid instanceId, Func<IQueryable<ContactData>, IQueryable<ContactData>> filterSortQuery)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<ContactData>> ListPagedFiltered(Guid instanceId, Func<IQueryable<ContactData>, IQueryable<ContactData>> filterSortQuery, int pageNo, int pageSize)
        {
            throw new NotImplementedException();
        }

        public Task<ContactData?> Update(ContactData update)
        {
            throw new NotImplementedException();
        }
    }
}
