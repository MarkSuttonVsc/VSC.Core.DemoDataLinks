using VSC.Core.DemoDataLinks.Model;
using VSC.Core.Services.Interfaces;
using VSC.Core.WebUI.Controllers;

namespace VSC.Core.DemoDataLinks.WebUI.Controllers
{
    public class ContactGroupController : DataController<ContactGroup>
    {
        public ContactGroupController(IInstanceService<ContactGroup> service, ISessionStateStore store) : base(service, store) { }
    }

}
