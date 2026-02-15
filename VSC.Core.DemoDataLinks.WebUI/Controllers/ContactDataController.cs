using VSC.Core.DemoDataLinks.Model;
using VSC.Core.Services.Interfaces;
using VSC.Core.WebUI.Controllers;

namespace VSC.Core.DemoDataLinks.WebUI.Controllers
{
    public class ContactDataController : DataController<ContactData>
    {
        public ContactDataController(IInstanceService<ContactData> service, ISessionStateStore store) : base(service, store) { }
    }
}
