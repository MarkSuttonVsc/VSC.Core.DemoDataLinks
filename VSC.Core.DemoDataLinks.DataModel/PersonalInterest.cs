using System.ComponentModel.DataAnnotations.Schema;
using VSC.Core.DataModel.Interfaces;

namespace VSC.Core.DemoDataLinks.Model;

public partial class PersonalInterest : IDataModel, IRowRevisionHistory, IMultiTenantKey
{
    public Guid PersonalInterestId { get; set; }

    //IMultiTenantKey
    public Guid InstanceId { get; set; }

    public Guid PersonId { get; set; }

    public Guid InterestId { get; set; }

    public DateTime Inserted { get; set; }

    public DateTime? Updated { get; set; }

    public Guid LastChangeBy { get; set; }

    public virtual Interest Interest { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;

    public string RowTypeTitle => "Personal Interest";

    public string RowTypeTitlePlural => "Personal Interests";

    public string ControllerName => "PersonalInterest";

    public bool CanDelete => true;

    public bool HasDetail => false;

    public bool HasLinks => false;

    public Dictionary<string, IDataLinkDefinition>? DataLinkDefinitions => null;

    public bool DoNotPage => false;

    public int PageSize => 10;

    public int PagerSpan => 10;

    public Dictionary<string, IDataFilterDefinition>? FilterDefinitions => null;

    public bool IsSearchExclusive => throw new NotImplementedException();

    public Dictionary<string, IDataSearchDefinition>? SearchDefinitions => null;

    public Guid Key => PersonalInterestId;

    //IFullTitle
    [NotMapped]
    public string? Reference { get; set; }
    [NotMapped]
    public string Title { get; set; } = "";

    public string FullTitle => "";
}
