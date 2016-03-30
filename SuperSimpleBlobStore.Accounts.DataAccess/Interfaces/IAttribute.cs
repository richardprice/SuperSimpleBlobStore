namespace SuperSimpleBlobStore.Accounts.DataAccess.Common
{
    public interface IAttribute
    {
        int Id { get; set; }
        string Name { get; set; }
        int SortOrder { get; }
    }
}
