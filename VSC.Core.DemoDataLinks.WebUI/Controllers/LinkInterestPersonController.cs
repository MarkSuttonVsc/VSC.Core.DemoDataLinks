using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using VSC.Core.DataModel.Interfaces;
using VSC.Core.DemoDataLinks.Model;
using VSC.Core.Services.Interfaces;
using VSC.Core.WebUI.Controllers;
using VSC.Core.WebUI.ViewModels;

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

        public override async Task<IEnumerable<Person>> GetLinkTargetList(int limit = -1)
        {
            if (limit > 0)
            {
                return await _targetService.List(InstanceId,
                        x => x.OrderBy(s => s.LastName).ThenBy(s => s.FirstName)
                        .Take(limit));
            }
            //no limit
            return await _targetService.List(InstanceId, x => x.OrderBy(s => s.LastName).ThenBy(s => s.FirstName));
        }

        public override FindLinkViewModel CreateFindLinkViewModel()
        {
            return new FindLinkViewModel
            {
                SearchTermCaption = "Last Name",
                HasSecondSearchTerm = true,
                SearchTermCaption2 = "First Name"
            };
        }

        public override async Task<IEnumerable<IDataModel>> FindLinkTargets(FindLinkViewModel viewModel)
        {
            return await _targetService.List(InstanceId,
                x => x.Where(y => y.LastName.ToLower().StartsWith(viewModel.SearchTerm.ToLower())
                            && y.FirstName.ToLower().StartsWith((viewModel.SearchTerm2 ?? "").ToLower()))
                      .OrderBy(s => s.LastName).ThenBy(s => s.FirstName));
        }

        /// <summary>
        /// This is the Interest->Person (reverse) direction for the relationship
        /// </summary>
        /// <param name="viewModel"></param>
        /// <returns></returns>
        public override async Task<IActionResult> SaveLink(LinkViewModel viewModel)
        {
            await SaveModelStatePreparation(viewModel); //located in IdentityController base - processes the [NoModelStateValidation] attribute.   
            if (ModelState.IsValid)
            {
                var linkDefinition = new PersonalInterest() as IDataLinkDefinition;
                var source = await _sourceService.GetById(InstanceId, viewModel.SourceId);
                var target = await _targetService.GetById(InstanceId, viewModel.SelectedTargetId);
                if (source != null && target != null)
                {
                    //Add method is in the Person->Interest direction - this must be reversed
                    var link = await _linkService.AddReverse(InstanceId, viewModel.SourceId, viewModel.SelectedTargetId, SignedInUserId);
                    Debug.WriteLine(" <--> reverse link write success");
                }
            }
            //cancel / complete
            //return using cross-controller values
            return RedirectToAction(viewModel.FromAction, viewModel.FromController, new { id = viewModel.FromId, tabIndex = viewModel.FromTabIndex });
        }
    }
}
