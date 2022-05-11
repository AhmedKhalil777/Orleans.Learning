using Orleans;

using ZooHub.Sample.Shared;

namespace ZooHub.Sample.Server.Grains
{
    public class CageGrain : Grain, ICageGrain
    {
        public Task RegisterAsync(AnimalId id)
        {
            throw new NotImplementedException();
        }
    }

    public interface ICageGrain : IGrainWithGuidKey
    {
        Task RegisterAsync(AnimalId id);
    }
}
