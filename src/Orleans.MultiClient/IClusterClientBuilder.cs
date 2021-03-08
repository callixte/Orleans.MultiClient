namespace Orleans.MultiClient
{
    public  interface IClusterClientBuilder
    {
        bool IsLocal { get; }
        IGrainFactory Build();
        IClusterClient BuildAsClient();
    }
}
