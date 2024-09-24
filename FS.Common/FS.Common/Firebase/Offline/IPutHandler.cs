namespace HM.Firebase.Database.Offline
{
    using HM.Firebase.Database.Query;

    using System.Threading.Tasks;

    public interface IPutHandler<in T>
    {
        Task PutAsync(ChildQuery query, string key, OfflineEntry entry);
    }
}
