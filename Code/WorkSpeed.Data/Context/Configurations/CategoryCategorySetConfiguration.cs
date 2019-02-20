
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.Context.Configurations
{
    public class CategoryCategorySetConfiguration : IEntityTypeConfiguration< CategoryCategorySet >
    {
        public void Configure ( EntityTypeBuilder< CategoryCategorySet > builder )
        {
            builder.HasKey( ccs => new { ccs.CategoryId, ccs.CategorySetId } );

            builder.HasOne( ccs => ccs.Category ).WithMany( c => c.CategorySets ).HasForeignKey( ccs => ccs.CategoryId );
            builder.HasOne( ccs => ccs.CategorySet ).WithMany( cs => cs.Categories ).HasForeignKey( ccs => ccs.CategorySetId );

            builder.HasData( new CategoryCategorySet[] {
                new CategoryCategorySet { CategorySetId = 1, CategoryId = 1 }, 
                new CategoryCategorySet { CategorySetId = 1, CategoryId = 2 }, 
                new CategoryCategorySet { CategorySetId = 1, CategoryId = 3 }, 
                new CategoryCategorySet { CategorySetId = 1, CategoryId = 4 }, 
                new CategoryCategorySet { CategorySetId = 1, CategoryId = 5 }, 
                new CategoryCategorySet { CategorySetId = 1, CategoryId = 6 }, 
            } );
        }
    }
}
