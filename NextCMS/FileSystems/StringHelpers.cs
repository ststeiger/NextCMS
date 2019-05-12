
namespace NextCMS.FileSystems
{
    public static class StringHelpers
    {

        public static bool IsStartOf(this string start, string instance)
        {
            if (instance == null)
                return false;

            return instance.StartsWith(start);
        }

        public static bool IsEndOf(this string start, string instance)
        {
            if (instance == null)
                return false;

            return instance.EndsWith(start);
        }

    }
}
