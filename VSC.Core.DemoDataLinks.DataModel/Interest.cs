using System.ComponentModel.DataAnnotations.Schema;
using VSC.Core.DataModel;
using VSC.Core.DataModel.Interfaces;

namespace VSC.Core.DemoDataLinks.Model;

public partial class Interest : IDataModel, IDataLinkModel, IRowRevisionHistory, IMultiTenantKey
{
    public Guid InterestId { get; set; }

    //IMultiTenantKey
    public Guid InstanceId { get; set; }

    //IRowRevisionHistory
    public DateTime Inserted { get; set; }
    public DateTime? Updated { get; set; }
    public Guid LastChangeBy { get; set; }

    //IFullTitle
    [NotMapped]
    public string? Reference { get; set; }

    public string Title { get; set; } = null!;

    public string? Description { get; set; }

    public virtual ICollection<PersonalInterest> PersonalInterests { get; set; } = new List<PersonalInterest>();

    //IDataModel
    
    public string RowTypeTitle => "Interest";

    public string RowTypeTitlePlural => "Interests";

    public string ControllerName => "Interest";

    public bool CanDelete => PersonalInterests.Count() == 0;

    public bool HasDetail => true;

    //has ILinkDataModel
    public bool HasLinks => true;
    public Dictionary<string, IDataLinkDefinition>? DataLinkDefinitions =>
        new Dictionary<string, IDataLinkDefinition>
        {
            { "InterestLink", new DataLinkDefinition {LinkTitle = "Interested Person", LinkControllerName = "LinkInterestPerson" } }            
        };

    public bool DoNotPage => false;

    public int PageSize => 20;

    public int PagerSpan => 10;

    public Dictionary<string, IDataFilterDefinition>? FilterDefinitions => null;

    public bool IsSearchExclusive => false;
    public Dictionary<string, IDataSearchDefinition>? SearchDefinitions => null;

    public Guid Key => InterestId;

    public string FullTitle => Title;
}
