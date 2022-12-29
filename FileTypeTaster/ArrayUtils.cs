namespace FileTypeTaster;

public static class ArrayUtils
{
    public static bool EndsWith<T>(this T[] haystack, T[] needle) where T : IEquatable<T>
    {
        if (needle.Length > haystack.Length)
        {
            throw new ArgumentException("The sequence you're searching for cannot be longer than the sequence being searched");
        }

        var needlePtr = needle.Length - 1;
        var haystackPtr = haystack.Length - 1;
        var isMatch = true;
        while (needlePtr >= 0 && haystackPtr >= 0)
        {
            if (!needle[needlePtr].Equals(haystack[haystackPtr]))
            {
                // no match, move to next possibility
                isMatch = false;
                break;
            }

            needlePtr--;
            haystackPtr--;
        }

        return isMatch;
    }

    public static bool StartsWith<T>(this T[] haystack, T[] needle) where T : IEquatable<T>
    {
        if (needle.Length > haystack.Length)
        {
            throw new ArgumentException("The sequence you're searching for cannot be longer than the sequence being searched");
        }

        for (var i = 0; i < needle.Length && i < haystack.Length; i++)
        {
            if (!needle[i].Equals(haystack[i]))
            {
                return false;
            }
        }

        return true;
    }
}