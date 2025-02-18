using System.ComponentModel;

namespace Domain.Enums;

public enum DefinitionType
{
    [Description("Fakülte")] Faculty = 1,

    [Description("Fakülte Bölümü")] FacultyDepartment = 2,

    [Description("Dosya Kodu")] FileCode = 3,

    [Description("Rapor Durumu")] ReportStatus = 4,

    [Description("Rapor Türü")] ReportType = 5,

    [Description("Durum")] Status = 6,

    [Description("Belge Türü")] DocumentType = 7,

    [Description("İş Ünvanı")] JobTitle = 8,

    [Description("İzin Türü")] PermitType = 9,

    [Description("İşlem Nedeni")] TransactionReason = 10,

    [Description("Üniversite")] University = 11,

    [Description("Program Türü")] ProgramType = 12,

    [Description("İşlem Türü")] ProcessType = 13,

    [Description("Günlük Türü")] PerDiemType = 14,

    [Description("Ödeme Türü")] TypeOfPayment = 15,

    [Description("Döviz")] Currency = 16,
}