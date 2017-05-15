namespace FleetAssist.Data.StringLength.Complaint
{
    public static class ComplaintStringLength
    {
        public const int RaisedByName = 200;
        public const int RaisedByEmail = 200;
        public const int RaisedByPhone = 200;
        public const int Description = 2000;
        public const int ComplaintOutcomeSummary = 500;
    }
    public static class ComplaintOutcomeStringLength
    {
        public const int Name = 100;
    }
    public static class ComplaintRaisedAgainstStringLength
    {
        public const int Name = 100;
    }
    public static class ComplaintRaisedByStringLength
    {
        public const int Name = 100;
    }
    public static class ComplaintRaisedViaStringLength
    {
        public const int Name = 100;
    }
}

namespace FleetAssist.Data.StringLength.Customer
{
    public static class CustomerStringLength
    {
        public const int Name = 50;
        public const int LegalName = 100;
        public const int TradingName = 100;
        public const int Description = 1000;
        public const int CompanyRegistrationNumber = 8;
        public const int Website = 1000;
        public const int OpeningHours = 100;
    }
    public static class CustomerAddressStringLength
    {
        public const int Name = 100;
        public const int Address1 = 255;
        public const int Address2 = 255;
        public const int Address3 = 255;
        public const int Address4 = 255;
        public const int Address5 = 255;
        public const int Postcode = 8;
    }
    public static class CustomerBusinessTypeStringLength
    {
        public const int Name = 35;
    }
    public static class CustomerContactStringLength
    {
        public const int Name = 100;
        public const int JobTitle = 100;
        public const int LandLineNumber = 50;
        public const int MobileNumber = 50;
        public const int Email = 150;
        public const int Notes = 500;
    }
    public static class CustomerPhoneNumberStringLength
    {
        public const int Name = 100;
        public const int PhoneNumber = 25;
        public const int Notes = 500;
    }
    public static class CustomerPhoneNumberTypeStringLength
    {
        public const int Name = 100;
    }
    public static class FormerCustomerCompanyStringLength
    {
        public const int Name = 100;
        public const int CompanyRegistrationNumber = 8;
    }
    public static class ProductStringLength
    {
        public const int Name = 100;
    }
}
