using VSC.Core.DemoDataLinks.Model;
using VSC.Core.Services.Interfaces;
using VSC.Core.ViewModel;
using VSC.Core.WebUI.Controllers;

namespace VSC.Core.DemoDataLinks.WebUI.Controllers
{
    public class ContactGroupController : DataController<ContactGroup>
    {
        public ContactGroupController(IInstanceService<ContactGroup> service, ISessionStateStore store) : base(service, store) { }

        public override async Task<IEnumerable<ContactGroup>> GetIndexRows(SessionViewModel sessionViewModel)
        {
            return await _service.ListPagedFiltered(InstanceId,
               x => x.OrderBy(s => s.Title),
               sessionViewModel.CurrentPage ?? 1,
               sessionViewModel.PageSize);
        }
    }

}
