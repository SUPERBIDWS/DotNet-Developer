namespace S4Pay.Extension
{
    public static class StringExtension
    {
        public static string Underscore(this string rule, int count = 50)
        {
            return rule.PadRight(count, '=');
        }
    }
}