using System.ComponentModel.DataAnnotations.Schema;
using VSC.Core.DataModel.Interfaces;

namespace VSC.Core.DemoDataLinks.Model;

public partial class ContactGroup : IDataModel, IRowRevisionHistory, IMultiTenantKey
{
    public Guid ContactGroupId { get; set; }

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
    public string FullTitle => Title;
  
    public virtual ICollection<Person> People { get; set; } = new List<Person>();

    public string RowTypeTitle => "Contact Group";

    public string RowTypeTitlePlural => "Contact Groups";

    public string ControllerName => "ContactGroup";

    public bool CanDelete => People.Count() == 0;

    public bool HasDetail => true;

    public bool HasLinks => false;
    public Dictionary<string, IDataLinkDefinition>? DataLinkDefinitions => null;

    public bool DoNotPage => false;

    public int PageSize => 20;

    public int PagerSpan => 10;

    public Dictionary<string, IDataFilterDefinition>? FilterDefinitions => null;

    public bool IsSearchExclusive => false;
    public Dictionary<string, IDataSearchDefinition>? SearchDefinitions => null;

    public Guid Key => ContactGroupId;

    
}
