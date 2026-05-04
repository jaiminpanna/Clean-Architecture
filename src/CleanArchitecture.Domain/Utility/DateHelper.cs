namespace CleanArchitecture.Domain.Utility
{
    public static class DateHelper
    {
        public static int GetAge(this DateTime dateOfBirth)
        {
            var today = DateTime.Today;

            int age = today.Year - dateOfBirth.Year;

            if (dateOfBirth.Date > today.AddYears(-age))
                age--;

            return age;
        }

        public static readonly string[] DateFormats =
        [
            // ISO fromats
            "yyyy-MM-dd",
            "yyyy/MM/dd",

            // US formats
            "MM/dd/yyyy",
            "M/d/yyyy",

            // EU / India formats
            "dd/MM/yyyy",
            "d/M/yyyy",

            // With separators variations
            "MM-dd-yyyy",
            "dd-MM-yyyy",

            // Compact formats
            "yyyyMMdd",
            "ddMMyyyy",
            "MMddyyyy",

            // With time 
            "yyyy-MM-ddTHH:mm:ss",
            "yyyy-MM-ddTHH:mm:ssZ",
            "yyyy-MM-ddTHH:mm:ss.fffZ",
            "yyyy-MM-dd HH:mm:ss",

            // With time but no seconds
            "yyyy-MM-ddTHH:mm",
            "yyyy-MM-dd HH:mm",

            // RFC / JSON style
            "ddd, dd MMM yyyy HH:mm:ss 'GMT'",
            "ddd, dd MMM yyyy HH:mm:ss"
        ];

    }
}
