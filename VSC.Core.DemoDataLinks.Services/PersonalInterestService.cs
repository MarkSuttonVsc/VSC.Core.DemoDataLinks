using Microsoft.EntityFrameworkCore;
using VSC.Core.DemoDataLinks.Data;
using VSC.Core.DemoDataLinks.Model;
using VSC.Core.Services.Interfaces;

namespace VSC.Core.DemoContacts.Services
{
    public class PersonalInterestService : DemoDataServiceBase, ILinkService<PersonalInterest>
    {
        public PersonalInterestService(DemoDatabaseContext context) : base(context) { }

        public async Task<PersonalInterest?> AddReverse(Guid instanceId, Guid sourceId, Guid targetId, Guid userId)
        {
            //this is the Interest->Person (reverse) direction for the relationship
            return await Add(instanceId, targetId, sourceId, userId);
        }

        public async Task<PersonalInterest?> Add(Guid instanceId, Guid personId, Guid interestId, Guid userId)
        {
            //this will not attempt to insert a duplicate within the instance
            var duplicate = await _database.PersonalInterests
                .FirstOrDefaultAsync(x =>
                       x.InstanceId == instanceId
                    && x.PersonId == personId
                    && x.InterestId == interestId);
            if (duplicate != null)
            {
                return duplicate;
            }
            var insert = new PersonalInterest
            {
                PersonalInterestId = Guid.NewGuid(),
                InstanceId = instanceId,
                PersonId = personId,
                InterestId = interestId,
                Inserted = DateTime.UtcNow,
                LastChangeBy = userId
            };         
            _database.PersonalInterests.Add(insert);
            await _database.SaveChangesAsync();
            return insert;
        }

        public async Task<PersonalInterest?> Remove(Guid instanceId, Guid id)
        {
            var deleted = await _database.PersonalInterests
                .FirstOrDefaultAsync(x => x.InstanceId == instanceId && x.PersonalInterestId == id);
            if (deleted != null)
            {
                _database.PersonalInterests.Remove(deleted);
                await _database.SaveChangesAsync();
            }
            return deleted;
        }
    }
}
