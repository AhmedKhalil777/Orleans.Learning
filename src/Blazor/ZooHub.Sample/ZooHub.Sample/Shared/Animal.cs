namespace ZooHub.Sample.Shared
{
    [StronglyTypedId(backingType: StronglyTypedIdBackingType.Guid)]
    public partial struct AnimalId { }

    public class Animal
    {
        public Guid OwnerId { get; set; }
        public AnimalId Id { get; set; }
        public string? AnimalClass { get; set; }
        public string? Name { get; set; }

        /// <summary>
        /// Describe what is the level of that animal can hunt, if 0 => can not kill (Prey)
        /// if Animal Predatory > other Animal, then Predatory will kill the week
        /// if 2 even from the same class (2 will live)
        /// If 2 even from different classes (Randomly one kill another)
        /// </summary>
        public int PredatoryLevel { get; set; }
    }


}
