using WorkSpeed.Data.Models;

namespace WorkSpeed.Business.Contexts.Productivity
{
    public static class DataModelExtensions
    {
        public static bool Contains ( this Category category, float? productVolume )
        {
            if ( category.MinVolume.HasValue && category.MaxVolume.HasValue ) {
                return productVolume.HasValue && (productVolume >= category.MinVolume && productVolume < category.MaxVolume);
            }

            if ( category.MinVolume.HasValue ) {
                return productVolume.HasValue && productVolume >= category.MinVolume;
            }

            if ( category.MaxVolume.HasValue ) {
                return productVolume.HasValue && productVolume < category.MaxVolume;
            }

            return true;
        }
    }
}
