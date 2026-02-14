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

        public static List<T> Take<T>(List<T> allSortedCars, int max)
        {
            List<T> result = new();

            for (int i = 0; i < max; i++)
            {
                result.Add(allSortedCars[i]);
            }

            return result;
        }

        public static List<T> FindAllMatchingElements<T>(List<T> intList, T elementToFind)
        {
            List<T> result = new();

            for (int i = 0; i < intList.Count; i++)
            {
                if (intList[i].Equals(elementToFind))
                    result.Add(intList[i]);
            }

            return result;
        }

        public static T FindFirstElement<T>(List<T> intList, T elementToFind)
        {
            for (int i = 0; i < intList.Count; i++)
            {
                if (intList[i].Equals(elementToFind))
                    return intList[i];
            }

            return default;
        }

        public static T FindLastElement<T>(List<T> intList, T elementToFind)
        {
            for (int i = intList.Count - 1; i >= 0; i--)
            {
                if (intList[i].Equals(elementToFind))
                    return intList[i];
            }

            return default;
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

        //public static List<TDestination> TransformNumbers<TSource, TDestination>(List<TSource> stringData)
        //{
        //    List<TDestination> result = new();

        //    for (int i = 0; i < stringData.Count; i++)
        //    {
        //        result.Add(int.Parse(stringData[i]));
        //    }

        //    return result;
        //}





        //["1","2","3","Nika","Nika","Nika"] => 1 2 3
    }
}
