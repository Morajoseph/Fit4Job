namespace Fit4Job.Enums
{
    public enum CompanySize
    {
        [Display(Name = "Startup")]
        Startup,

        [Display(Name = "1-10 employees")]
        OneToTen,

        [Display(Name = "11-50 employees")]
        ElevenToFifty,

        [Display(Name = "51-200 employees")]
        FiftyOneToTwoHundred,

        [Display(Name = "201-500 employees")]
        TwoHundredOneToFiveHundred,

        [Display(Name = "500+ employees")]
        FiveHundredPlus
    }
}