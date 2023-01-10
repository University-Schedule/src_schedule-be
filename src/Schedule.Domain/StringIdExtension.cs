using System;
using System.Linq;

namespace Schedule;

public static class StringIdExtension
{
    public static string GetStringId(this string str, int length)
    {
        var random = new Random();
        
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}