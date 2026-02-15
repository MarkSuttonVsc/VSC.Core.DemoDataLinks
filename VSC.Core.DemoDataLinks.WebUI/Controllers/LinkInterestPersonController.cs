using VSC.Core.DemoDataLinks.Model;
using VSC.Core.Services.Interfaces;
using VSC.Core.WebUI.Controllers;

namespace VSC.Core.DemoDataLinks.WebUI.Controllers
{
    public class LinkInterestPersonController : LinkDataController<PersonalInterest, Interest, Person>
    {
        public LinkInterestPersonController(ILinkService<PersonalInterest> linkService, 
            IInstanceService<Interest> sourceService, 
            IInstanceService<Person> targetService) 
            : base(linkService, sourceService, targetService)
        {
        }
    }
}
