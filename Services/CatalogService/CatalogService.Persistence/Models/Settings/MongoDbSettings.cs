namespace CatalogService.Persistence.Models.Settings
{
    public class MongoDbSettings
    {
        public string DatabaseName { get; set; } = null!;
        public string ConnectionString { get; set; } = null!;
    }
}
