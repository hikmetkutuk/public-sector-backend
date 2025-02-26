using System.ComponentModel;

namespace Domain.Enums;

public enum OrganisationType
{
    [Description("Fakülte")] Category = 0,

    [Description("Merkez")] Headquarters = 1,

    [Description("Daire Başkanlığı")] DepartmentDirectorate = 2,

    [Description("Daire Başkanlığı Müdürlük")]
    DepartmentSection = 3,

    [Description("Başmüdürlük")] ProvincialDirectorate = 11,

    [Description("Müdürlük")] Directorate = 12,

    [Description("Kısım Müdürlüğü")] SectionDirectorate = 13,

    [Description("Şube")] Branch = 14,

    [Description("Şeflik")] ChiefUnit = 15,
}