namespace FaceSort
{
    public interface IConfiguration
    {
        string SubscriptionKey { get; }
        string UriBase { get; }
        string ImagesPath { get; }
    }
}