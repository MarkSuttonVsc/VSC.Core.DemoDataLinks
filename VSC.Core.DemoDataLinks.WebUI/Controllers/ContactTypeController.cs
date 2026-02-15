using VSC.Core.DemoDataLinks.Model;
using VSC.Core.Services.Interfaces;
using VSC.Core.WebUI.Controllers;

namespace VSC.Core.DemoDataLinks.WebUI.Controllers
{
    public class ContactTypeController : DataController<ContactType>
    {
        public ContactTypeController(IInstanceService<ContactType> service, ISessionStateStore store) : base(service, store) { }
    }
}
