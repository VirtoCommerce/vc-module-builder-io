namespace VirtoCommerce.BuilderIO.Core.Models
{
    public class BuilderIOPageChanges
    {
        public BuilderIOPage NewValue { get; set; }
        public BuilderIOPage PreviousValue { get; set; }
        public string ModelName { get; set; }
        public string Operation { get; set; }
    }
}
