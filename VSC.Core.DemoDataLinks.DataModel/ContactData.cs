using System.ComponentModel.DataAnnotations.Schema;
using VSC.Core.DataModel.Interfaces;

namespace VSC.Core.DemoDataLinks.Model;

public partial class ContactData : IDataModel, IRowRevisionHistory, IMultiTenantKey
{
    public Guid ContactDataId { get; set; }

    //IMultiTenantKey
    public Guid InstanceId { get; set; }

    //IRowRevisionHistory
    public DateTime Inserted { get; set; }
    public DateTime? Updated { get; set; }
    public Guid LastChangeBy { get; set; }

    [NotMapped]
    public string? Reference { get; set; }
    [NotMapped]
    public string Title { get; set; } = "";

    public Guid PersonId { get; set; }

    public Guid ContactTypeId { get; set; }

    public string Detail { get; set; } = null!;

    public virtual ContactType ContactType { get; set; } = null!;

    public virtual Person Person { get; set; } = null!;

    public Guid Key => ContactTypeId;

    public string RowTypeTitle => "Contact Data";

    public string RowTypeTitlePlural => "ContactData";

    public string ControllerName => "ContactDataController";

    public bool CanDelete => true;

    public bool HasDetail => false;

    public bool HasLinks => false;
    public Dictionary<string, IDataLinkDefinition>? DataLinkDefinitions => throw new NotImplementedException();

    public bool DoNotPage => throw new NotImplementedException();

    public int PageSize => throw new NotImplementedException();

    public int PagerSpan => throw new NotImplementedException();

    public Dictionary<string, IDataFilterDefinition>? FilterDefinitions => throw new NotImplementedException();

    public bool IsSearchExclusive => false;

    public Dictionary<string, IDataSearchDefinition>? SearchDefinitions => throw new NotImplementedException();

    public string FullTitle => throw new NotImplementedException();
}
