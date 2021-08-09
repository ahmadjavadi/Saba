using SABA_CH.Converter;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations; 

namespace SABA_CH.Enums
{
    [TypeConverter(typeof(EnumDescriptionConverter))]
    public enum CreditTypes
    {
        [Display(Description = "اعتبار سالیانه")]
        AnnualCredit,
        [Display(Description = "اعتبار مازاد")]
        ExcessCredit,

        [Display(Description = "اعتبار کسر از سال")]
        DeductionOfTheYearCredit,

        [Display(Description = "اعتبار نقص کنتور")]
        MeterDefectCredit,

        [Display(Description = "اعتبار تعویض کنتور")]
        ReplaceTheMeterCredit,

        [Display(Description = "اعتبار باقیمانده حجم اولیه")]
        ResidualValidityOfTheInitialVolumeCredit,

        [Display(Description = "سایر")]
        Others
    }
}
