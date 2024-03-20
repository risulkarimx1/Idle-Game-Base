using UniRx;

namespace GameCode.Init
{
    public interface IInitProgressReporter
    {
        public ReactiveProperty<float> OnProgressUpdated { get; set; }
    }
}