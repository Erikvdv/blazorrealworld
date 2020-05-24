using System;
using System.Text.Json;

namespace Infrastructure.Utils
{
    public static class CompareObjects
    {
        public static bool AreEqual(object? obj1, object? obj2)
        {
            if (obj1 == null && obj2 == null) return true;
            if (obj1 == null || obj2 == null) return false;
            return JsonSerializer.Serialize(obj1) == JsonSerializer.Serialize(obj2);
        }
    }
}
