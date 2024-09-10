namespace HospitalManagementSystem.Helpers
{
    public static class StringExtensions
    {
        public static string GetShort(this string value, int maxLength = 50)
            => value.Length > maxLength
            ? string.Concat(value.AsSpan(0, maxLength), "...")
            : value;
    }
}
