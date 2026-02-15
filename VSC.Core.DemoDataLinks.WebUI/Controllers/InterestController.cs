using VSC.Core.DemoDataLinks.Model;
using VSC.Core.Services.Interfaces;
using VSC.Core.WebUI.Controllers;

namespace VSC.Core.DemoDataLinks.WebUI.Controllers
{
    public class InterestController : DataController<Interest>
    {
        public InterestController(IInstanceService<Interest> service, ISessionStateStore store) : base(service, store) { }
    }
}
