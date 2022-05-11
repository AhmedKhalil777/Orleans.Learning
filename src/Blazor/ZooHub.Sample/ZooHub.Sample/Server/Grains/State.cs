namespace ZooHub.Sample.Server.Grains
{
    /// <summary>
    /// Represent a generic container to a Grain State
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class State<T>
    {
        public T? Item { get; set; }
    }
}
