using VSC.Core.DemoDataLinks.Model;
using VSC.Core.Services.Interfaces;
using VSC.Core.ViewModel;
using VSC.Core.WebUI.Controllers;

namespace VSC.Core.DemoDataLinks.WebUI.Controllers
{
    public class InterestController : DataController<Interest>
    {
        public InterestController(IInstanceService<Interest> service, ISessionStateStore store) : base(service, store) { }

        public override async Task<IEnumerable<Interest>> GetIndexRows(SessionViewModel sessionViewModel)
        {
            return await _service.ListPagedFiltered(InstanceId,
               x => x.OrderBy(s => s.Title),
               sessionViewModel.CurrentPage ?? 1,
               sessionViewModel.PageSize);
        }
    }
}
