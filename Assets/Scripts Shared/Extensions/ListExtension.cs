using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace.Extensions
{
    public static class ListExtension
    {
        public static T GetNextRandom<T>(this List<T> list)
        {
            var result = list.OrderBy(_ => Random.value).FirstOrDefault();
            if (result == null)
                return default;

            list.Remove(result);
            return result;
        }
    }
}