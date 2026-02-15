using VSC.Core.DemoDataLinks.Model;
using VSC.Core.Services.Interfaces;
using VSC.Core.WebUI.Controllers;

namespace VSC.Core.DemoDataLinks.WebUI.Controllers
{
    public class LinkPersonInterestController : LinkDataController<PersonalInterest, Person, Interest>
    {
        public LinkPersonInterestController(ILinkService<PersonalInterest> linkService, 
            IInstanceService<Person> sourceService, 
            IInstanceService<Interest> targetService) 
            : base(linkService, sourceService, targetService)
        {
        }
    }
}
