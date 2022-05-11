using Orleans.Concurrency;

using ZooHub.Sample.Shared;

namespace ZooHub.Sample.Server.Grains
{
    [Immutable]
    public record AnimalNotification(AnimalId Id, Animal? Animal);
}
