namespace SuperSimpleBlobStore.Common
{
    public class Attribute
    {
        public Attribute(int id, string name, int sortOrder)
        {
            this.Id = id;
            this.Name = name;
            this.SortOrder = sortOrder;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int SortOrder { get; set; }

        public Attribute ToAttribute()
        {
            return this;
        }
    }
}
