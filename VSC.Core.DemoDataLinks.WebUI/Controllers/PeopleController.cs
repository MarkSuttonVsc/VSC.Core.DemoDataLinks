using Microsoft.AspNetCore.Mvc.Rendering;
using VSC.Core.DemoDataLinks.Model;
using VSC.Core.Services.Interfaces;
using VSC.Core.ViewModel;
using VSC.Core.WebUI.Controllers;

namespace VSC.Core.DemoDataLinks.WebUI.Controllers
{
    public class PeopleController : DataController<Person>
    {
        //for drop downs
        private readonly IInstanceService<ContactGroup> _contactGroupService;

        public PeopleController(IInstanceService<Person> service, IInstanceService<ContactGroup> contactGroupService, 
            ISessionStateStore store)
            : base(service, store)
        {
            _contactGroupService = contactGroupService;
        }

        public override async Task<Dictionary<string, IEnumerable<SelectListItem>>> IndexFilterPreparation(Guid instanceId, Guid? parentKey = null)
        {
            var filters = new Dictionary<string, IEnumerable<SelectListItem>>()
            {
                {
                    "ContactGroupFilter",
                       (await _contactGroupService.List(InstanceId, x=>x))
                        .OrderBy(x => x.Title)
                        .Select(x => new SelectListItem
                        {
                            Text = x.Title,
                            Value = x.Key.ToString()
                        })
                }
            };
            return filters;
        }

        public override async Task<Dictionary<string, IEnumerable<SelectListItem>>> EditDropDownPreparation(Guid instanceId, Guid id)
        {
            return new Dictionary<string, IEnumerable<SelectListItem>>()
            {{
                "ContactGroupList",
                 (await _contactGroupService.List(InstanceId, x=>x))
                    .OrderBy(x => x.Title)
                    .Select(x => new SelectListItem
                    {
                        Text = x.Title,
                        Value = x.Key.ToString(),
                    })
                }
            };
        }

        private static IQueryable<Person> GetFilterSearchFunction(IQueryable<Person> query, SessionViewModel sessionViewModel)
        {
            //this must include the OrderBy for the paging to work correctly

            var filter = sessionViewModel.FilterStates.FirstOrDefault(x => x.FilterName == "ContactGroupFilter" && x.CurrentItem != Guid.Empty);
            var searchFirstName = sessionViewModel.SearchStates.FirstOrDefault(x => x.SearchName == "FirstNameSearch" && !String.IsNullOrWhiteSpace(x.SearchCriterion));
            var searchLastName = sessionViewModel.SearchStates.FirstOrDefault(x => x.SearchName == "LastNameSearch" && !String.IsNullOrWhiteSpace(x.SearchCriterion));

            if (filter != null)
            {
                Guid selectedValue = filter.CurrentItem;
                //there is a filter
                if (searchFirstName != null && searchLastName == null)
                {
                    string firstName = searchFirstName.SearchCriterion ?? "";
                    return query.Where(y => y.ContactGroupId == selectedValue && y.FirstName.StartsWith(firstName))
                        .OrderBy(s=>s.LastName).ThenBy(s=>s.FirstName);
                }
                if (searchFirstName == null && searchLastName != null)
                {
                    string lastName = searchLastName.SearchCriterion ?? "";
                    return query.Where(y => y.ContactGroupId == selectedValue && y.LastName.StartsWith(lastName))
                        .OrderBy(s => s.LastName).ThenBy(s => s.FirstName);
                }
                if (searchFirstName != null && searchLastName != null)
                {
                    string firstName = searchFirstName.SearchCriterion ?? "";
                    string lastName = searchLastName.SearchCriterion ?? "";
                    return query.Where(y => y.ContactGroupId == selectedValue
                            && y.FirstName.StartsWith(firstName)
                            && y.LastName.StartsWith(lastName))
                        .OrderBy(s => s.LastName).ThenBy(s => s.FirstName);
                }

                //no name search
                return query.Where(y => y.ContactGroupId == selectedValue).OrderBy(s => s.LastName).ThenBy(s => s.FirstName);
            }
            else
            {
                //no filter
                if (searchFirstName != null && searchLastName == null)
                {
                    string firstName = searchFirstName.SearchCriterion ?? "";
                    return query.Where(y => y.FirstName.StartsWith(firstName))
                        .OrderBy(s => s.LastName).ThenBy(s => s.FirstName);
                }
                if (searchFirstName == null && searchLastName != null)
                {
                    string lastName = searchLastName.SearchCriterion ?? "";
                    return query.Where(y => y.LastName.StartsWith(lastName))
                    .OrderBy(s => s.LastName).ThenBy(s => s.FirstName);
                }
                if (searchFirstName != null && searchLastName != null)
                {
                    string firstName = searchFirstName.SearchCriterion ?? "";
                    string lastName = searchLastName.SearchCriterion ?? "";
                    return query.Where(y => y.FirstName.StartsWith(firstName) && y.LastName.StartsWith(lastName))
                        .OrderBy(s => s.LastName).ThenBy(s => s.FirstName);
                }
            }
            //no search or filter at all (all values are selected)
            return query.OrderBy(s => s.LastName).ThenBy(s => s.FirstName);
        }


        public override async Task<int> GetIndexCount(SessionViewModel sessionViewModel)
        {
            return await _service.CountFiltered(InstanceId, x=> GetFilterSearchFunction(x, sessionViewModel));
        }

        public override async Task<IEnumerable<Person>> GetIndexRows(SessionViewModel sessionViewModel)
        {
             return await _service.ListPagedFiltered(InstanceId,
                x => GetFilterSearchFunction(x, sessionViewModel),
                sessionViewModel.CurrentPage ?? 1, 
                sessionViewModel.PageSize);                
        }
    }
}
