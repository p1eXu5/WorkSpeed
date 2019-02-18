using System;
using System.Drawing;
using System.Drawing.Imaging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WorkSpeed.Data.Models;

namespace WorkSpeed.Data.DataContexts.Configurations
{
    public class AvatarConfiguration : IEntityTypeConfiguration< Avatar >
    {
        public void Configure ( EntityTypeBuilder< Avatar > builder )
        {
            builder.ToTable("Avatars", "dbo");

            builder.HasKey( p => p.Id );
            builder.Property( p => p.Id ).UseSqlServerIdentityColumn();

            //builder.Property( p => p.Picture ).HasColumnType( "varbinary(max)" );
            builder.Property( p => p.Stride ).HasColumnType( "int" ).IsRequired();
            builder.Property( p => p.Width ).HasColumnType( "int" ).IsRequired();
            builder.Property( p => p.Height ).HasColumnType( "int" ).IsRequired();

            builder.HasMany( p => p.Employees ).WithOne( e => e.Avatar );

            var bitmap = Properties.Resources.default_face;
            var rect = new Rectangle( 0, 0, bitmap.Width, bitmap.Height );
            var bitmapData = bitmap.LockBits( rect, ImageLockMode.ReadWrite, bitmap.PixelFormat );
            IntPtr firstPixel = bitmapData.Scan0;
            var size = bitmapData.Stride * bitmap.Height;
            var array = new byte[ size ];
            System.Runtime.InteropServices.Marshal.Copy( firstPixel, array, 0, size );

            builder.HasData( new[] {
                new Avatar { Id = 1, Picture = array, Stride = bitmapData.Stride, Width = bitmapData.Width, Height = bitmapData.Height }
            });

            bitmap.UnlockBits( bitmapData );
        }
    }
}
