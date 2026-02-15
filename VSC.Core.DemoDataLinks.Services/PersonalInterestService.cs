using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using VSC.Core.DemoDataLinks.Data;
using VSC.Core.DemoDataLinks.Model;
using VSC.Core.Services.Interfaces;

namespace VSC.Core.DemoContacts.Services
{
    public class PersonalInterestService : DemoDataServiceBase, ILinkService<PersonalInterest>
    {
        public PersonalInterestService(DemoDatabaseContext context) : base(context) { }

        public async Task<PersonalInterest?> Add(Guid instanceId, Guid sourceId, Guid targetId, Guid userId)
        {
            //this will not attempt to insert a duplicate within the instance
            var duplicate = await _database.PersonalInterests
                .FirstOrDefaultAsync(x =>
                       x.InstanceId == instanceId
                    && x.PersonId == sourceId
                    && x.InterestId == targetId);
            if (duplicate != null)
            {
                return duplicate;
            }
            var insert = new PersonalInterest
            {
                PersonalInterestId = Guid.NewGuid(),
                InstanceId = instanceId,
                PersonId = sourceId,
                InterestId = targetId,
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
