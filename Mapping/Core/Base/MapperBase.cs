using DIRS21.Mapping.Core.Interfaces;

namespace DIRS21.Mapping.Core.Base
{
    /// <summary>
    /// Base class for strongly-typed mappers
    /// </summary>
    public abstract class MapperBase<TSource, TTarget> : IMapper
    {
        public abstract string SourceType { get; }
        public abstract string TargetType { get; }

        public object Map(object source)
        {
            if (source == null) 
            {
                return null;
            }
                

            if (!(source is TSource typedSource))
            {
                throw new InvalidCastException(
                    $"Expected source type {typeof(TSource).Name} but received {source.GetType().Name}");
            }

            return MapInternal(typedSource);
        }

        /// <summary>
        /// Implement the actual mapping logic
        /// </summary>
        protected abstract TTarget MapInternal(TSource source);
    }
}