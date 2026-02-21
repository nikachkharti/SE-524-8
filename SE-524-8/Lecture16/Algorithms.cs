namespace Lecture16
{
    public class Algorithms
    {
        public static int Count(List<int> list, int elementToCount)
        {
            throw new NotImplementedException();
        }

        public static List<T> Sort<T>(List<T> allCars) where T : IComparable<T>
        {
            for (int i = 0; i < allCars.Count - 1; i++)
            {
                for (int j = i + 1; j < allCars.Count; j++)
                {
                    if (allCars[j].CompareTo(allCars[i]) == -1)
                    {
                        var temp = allCars[j];
                        allCars[j] = allCars[i];
                        allCars[i] = temp;
                    }
                }
            }

            return allCars;
        }

        //public static List<T> Take<T>(ICollection<T> allSortedCars, int max)
        //{
        //    List<T> result = new();

        //    for (int i = 0; i < max; i++)
        //    {
        //        result.Add(allSortedCars[i]);
        //    }

        //    return result;
        //}

        public static IEnumerable<T> NikasFindAll<T>(IEnumerable<T> source, Func<T, bool> predicate)    /*<-- დელეგატი*/
        {
            List<T> result = new();

            foreach (var item in source)
            {
                if (predicate(item))
                    result.Add(item);
            }

            return result;
        }


        public static T NikasFirstOrDefault<T>(IEnumerable<T> source, T elementToFind)
        {
            foreach (var v in source)
            {
                if (v.Equals(elementToFind))
                    return v;
            }

            return default;
        }
        public static void NikasForeach<T>(IEnumerable<T> source)
        {
            var sourceEnumerator = source.GetEnumerator();

            while (sourceEnumerator.MoveNext())
            {
                Console.WriteLine(sourceEnumerator.Current);
            }
        }
        public static T NikasLastOrDefault<T>(IEnumerable<T> source, Predicate<T> predicate)
        {
            T lastElement = default;

            foreach (var item in source)
            {
                if (predicate(item))
                    lastElement = item;
            }

            return lastElement;

        }
        public static IEnumerable<TDestination> NikasSelect<TSource, TDestination>(IEnumerable<TSource> source, Func<TSource, TDestination> selector)
        {
            List<TDestination> result = new();

            foreach (var item in source)
            {
                result.Add(selector(item));
            }

            return result;
        }



        public static int FindFirstIndex<T>(List<T> intList, T elementToFind)
        {
            for (int i = 0; i < intList.Count; i++)
            {
                if (intList[i].Equals(elementToFind))
                    return i;
            }

            return -1;
        }

        public static int FindLastIndex<T>(List<T> intList, T elementToFind)
        {
            for (int i = intList.Count - 1; i >= 0; i--)
            {
                if (intList[i].Equals(elementToFind))
                    return i;
            }

            return -1;
        }

        public static bool FindAnyNumberExist<T>(List<T> intList, T elementToFind)
        {
            for (int i = 0; i < intList.Count; i++)
            {
                if (intList[i].Equals(elementToFind))
                    return true;
            }

            return false;
        }

        public static bool FindAllNumberExist<T>(List<T> intList, T elementToFind)
        {
            for (int i = 0; i < intList.Count; i++)
            {
                if (!intList[i].Equals(elementToFind))
                    return false;
            }

            return true;
        }








        //["1","2","3","Nika","Nika","Nika"] => 1 2 3
    }
}
