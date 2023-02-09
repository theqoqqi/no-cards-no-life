namespace Core.Choices {
    public abstract class GenericChoice<T> : Choice where T : ChoiceMetadata {

        protected override ChoiceMetadata UpcastedMetadata => Metadata;
        
        protected readonly T Metadata;

        protected GenericChoice(T metadata) {
            Metadata = metadata;
        }
    }
}