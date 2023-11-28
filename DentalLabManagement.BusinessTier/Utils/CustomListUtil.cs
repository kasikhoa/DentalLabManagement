namespace DentalLabManagement.BusinessTier.Utils
{
    public static class CustomListUtil
    {
        public static (List<int> idsToRemove, List<int> idsToAdd, List<int> idsToKeep) SplitIdsToAddAndRemove(List<int> oldIds, List<int> newIds)
        {
            List<int> idsToAdd = new List<int>(newIds);
            List<int> idsToRemove = new List<int>(oldIds);
            List<int> idsToKeep = new List<int>();

            //A list of Id that is contain deleted ids but does not contain new ids added
            List<int> listWithOutIdsToAdd = new List<int>();

            //This logic help to split new ids, keep old ids and deleted ids
            newIds.ForEach(x =>
            {
                oldIds.ForEach(y =>
                {
                    if (x.Equals(y))
                    {
                        listWithOutIdsToAdd.Add(x);
                        idsToAdd.Remove(x);
                    }
                });
            });

            //This help clarify what ids to keep, using to update case
            idsToKeep = listWithOutIdsToAdd;

            //This logic help to remove old ids, keep only ids to remove
            oldIds.ForEach(x =>
            {
                listWithOutIdsToAdd.ForEach(y =>
                {
                    if (x.Equals(y)) idsToRemove.Remove(x);
                });
            });

            //foreach (var x in newIds)
            //{
            //    if (oldIds.Contains(x))
            //    {
            //        idsToRemove.Remove(x);
            //        idsToAdd.Remove(x);
            //        idsToKeep.Add(x);
            //    }
            //}

            return (idsToRemove, idsToAdd, idsToKeep);
        }

        //public static (List<int> idsToRemove, List<int> idsToAdd, List<int> idsToKeep) SplitIdsToAddAndRemove(List<int> oldIds, List<int> newIds)
        //{
        //    List<int> idsToAdd = newIds.Except(oldIds).ToList();
        //    List<int> idsToRemove = oldIds.Except(newIds).ToList();
        //    List<int> idsToKeep = oldIds.Intersect(newIds).ToList();

        //    return (idsToRemove, idsToAdd, idsToKeep);
        //}
    }
}
