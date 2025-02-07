using System.ComponentModel;

namespace Domain.Enums;

public enum OrganisationType
{
    Category = 0,

    Headquarters = 1, // Merkez

    DepartmentDirectorate = 2, // Daire Başkanlığı

    DepartmentSection = 3, // Daire Başkanlığı Müdürlük

    ProvincialDirectorate = 11, // Başmüdürlük

    Directorate = 12, // Müdürlük

    SectionDirectorate = 13, // Kısım Müdürlüğü

    Branch = 14, // Şube

    ChiefUnit = 15, // Şeflik
}