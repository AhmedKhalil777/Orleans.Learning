using Orleans;
using Orleans.Runtime;

using System.Collections.Immutable;

using ZooHub.Sample.Shared;

namespace ZooHub.Sample.Server.Grains
{
    public class AnimalGrain : Grain, IAnimalGrain
    {
        private string GrainType => nameof(AnimalGrain);
        private AnimalId GrainKey => new AnimalId(this.GetPrimaryKey());

        private readonly ILogger<AnimalGrain> _logger;
        private readonly IPersistentState<State> _persistentState;

        public AnimalGrain(ILogger<AnimalGrain> logger,IPersistentState<State> persistentState)
        {
            _logger = logger;
            _persistentState = persistentState;
        }

        public Task<Animal?> GetAsync() => Task.FromResult(_persistentState?.State?.Animal);

        public async Task SetAsync(Animal animal)
        {
            if (animal.Id == GrainKey)
            {
                throw new InvalidOperationException("Grain and State Mismatch");
            }

            _persistentState.State.Animal = animal;
            await _persistentState.WriteStateAsync();


            // register the animal with its owner cage
            await GrainFactory.GetGrain<ICageGrain>(animal.OwnerId)
                .RegisterAsync(animal.Id);

            // for sample debugging
            _logger.LogInformation(
                "{@GrainType} {@GrainKey} now contains {@Animal}",
                GrainType, GrainKey, animal);

            // notify listeners - best effort only
            GetStreamProvider("SMS")
                .GetStream<AnimalNotification>(animal.OwnerId, nameof(IAnimalGrain))
                .OnNextAsync(new AnimalNotification(animal.Id, animal))
                .Ignore();


        }


        public class State
        {
            public Animal? Animal { get; set; }
        }

    }

    
    public interface IAnimalGrain : IGrainWithStringKey
    {
    }
}
