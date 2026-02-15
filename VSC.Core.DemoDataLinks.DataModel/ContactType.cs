using System.ComponentModel.DataAnnotations.Schema;
using VSC.Core.DataModel.Interfaces;

namespace VSC.Core.DemoDataLinks.Model;

public partial class ContactType : IDataModel, IRowRevisionHistory, IMultiTenantKey
{
    
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

    //IDataModel
    public Guid ContactTypeId { get; set; }

    public virtual ICollection<ContactData> ContactData { get; set; } = new List<ContactData>();

    public string RowTypeTitle => "Contact Type";

    public string RowTypeTitlePlural => "Contact Types";

    public string ControllerName => "ContactType";

    public bool CanDelete => ContactData.Count() == 0;

    public bool HasDetail => false;

    public bool HasLinks => false;
    public Dictionary<string, IDataLinkDefinition>? DataLinkDefinitions => null;

    public bool DoNotPage => false;

    public int PageSize => 20;

    public int PagerSpan => 10;

    public Dictionary<string, IDataFilterDefinition>? FilterDefinitions => null;

    public bool IsSearchExclusive => false;
    public Dictionary<string, IDataSearchDefinition>? SearchDefinitions => null;

    public Guid Key => ContactTypeId;

    public string FullTitle => Title;
}
