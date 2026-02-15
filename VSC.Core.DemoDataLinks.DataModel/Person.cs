using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using VSC.Core.DataModel;
using VSC.Core.DataModel.Interfaces;

namespace VSC.Core.DemoDataLinks.Model;

public partial class Person : IDataModel, IRowRevisionHistory, IMultiTenantKey
{
    public Guid PersonId { get; set; }

    //IMultiTenantKey
    public Guid InstanceId { get; set; }

    //IRowRevisionHistory
    public DateTime Inserted {  get; set; }
    public DateTime? Updated { get; set; }
    public Guid LastChangeBy { get; set; }

    //Data Model properties (in the database)

    [DisplayName("First Name")]
    public string FirstName { get; set; } = null!;

    [DisplayName("Last Name")]
    [Required]
    public string LastName { get; set; } = null!;

    [DisplayName("Contact Group")]
    public Guid? ContactGroupId { get; set; }

    public virtual ICollection<ContactData> ContactData { get; set; } = new List<ContactData>();

    public virtual ContactGroup? ContactGroup { get; set; }

    public virtual ICollection<PersonalInterest> PersonalInterests { get; set; } = new List<PersonalInterest>();

    //IFullTitle
    [NotMapped]
    public string? Reference { get; set; }

    [NotMapped]
    public string Title { get => LastName; set { } }

    //IDataModel
    public string RowTypeTitle => "Person";

    public string RowTypeTitlePlural => "People";

    public string ControllerName => "People";

    public bool CanDelete => true;

    public bool HasDetail => true;

    public bool HasLinks => false;
    public Dictionary<string, IDataLinkDefinition>? DataLinkDefinitions => null;

    public bool DoNotPage => false;

    public int PageSize => 15;

    public int PagerSpan => 10;

    public Dictionary<string, IDataFilterDefinition> FilterDefinitions =>
        new Dictionary<string, IDataFilterDefinition>
        {
            { "ContactGroupFilter", new DataFilterDefinition { RowTypeTitle = "Contact Group", HasAny=true } }
        };

    public bool IsSearchExclusive => false; //OR
    public Dictionary<string, IDataSearchDefinition>? SearchDefinitions =>
        new Dictionary<string, IDataSearchDefinition>
        {
            { "LastNameSearch", new DataSearchDefinition { Caption = "Last Name", IsCaseSensitive=false, IsAnyPosition=false } },
        
            { "FirstNameSearch", new DataSearchDefinition { Caption = "First Name", IsCaseSensitive=false, IsAnyPosition=false } }
        };

public Guid Key => PersonId;

    public string FullTitle => $"{FirstName} {LastName}".Trim();
}
