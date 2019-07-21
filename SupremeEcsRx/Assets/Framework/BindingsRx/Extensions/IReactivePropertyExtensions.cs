using UniRx;

namespace BindingsRx.Extensions
{
    public static class IReactivePropertyExtensions
    {
        public static ReadOnlyReactiveProperty<string> ToTextualProperty<T>(this IReactiveProperty<T> nonStringProperty)
        { return nonStringProperty.Select(x => x.ToString()).ToReadOnlyReactiveProperty(); }
    }
}