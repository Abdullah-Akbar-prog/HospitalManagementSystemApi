namespace Hospital.Domain.Common
{
    public static class Roles
    {
        public const string Admin = "Admin";
        public const string Patient = "Patient";
        public const string Doctor = "Doctor";

        public static readonly string[] All = { Admin, Patient, Doctor };
        public static readonly string[] RegistrableNonAdmin = { Patient, Doctor };
    };
};
