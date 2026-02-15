using VSC.Core.DemoDataLinks.Data;

namespace VSC.Core.DemoContacts.Services
{
    public class DemoDataServiceBase
    {
        private readonly DemoDatabaseContext _demoDatabaseContext;

        internal DemoDatabaseContext _database => _demoDatabaseContext;

        public DemoDataServiceBase(DemoDatabaseContext demoDatabaseContext)
        {
            _demoDatabaseContext = demoDatabaseContext;
        }
    }
}
