using Domain.Apartments;
using Domain.Bookings;
using Domain.Reviews;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Configurations
{
    internal sealed class ReviewConfiguration : IEntityTypeConfiguration<Review>
    {
        
        public void Configure(EntityTypeBuilder<Review> builder)
        {
            builder.ToTable("reviews");
            
            builder.HasKey(review => review.Id);
            
            builder.Property(review => review.Rating)
                .HasConversion(rating => rating.Value, value => Rating.Create(value).Value);

            builder.Property(review => review.Comment)
                .HasMaxLength(2000)
                .HasConversion(comment => comment.Value, value => new Comment(value));

            builder.HasOne<Apartment>()
                .WithMany()
                .HasForeignKey(review => review.ApartmentId);

            builder.HasOne<Booking>()
                .WithMany()
                .HasForeignKey(review => review.BookingId);

            builder.HasOne<User>()
                .WithMany()
                .HasForeignKey(review => review.UserId);


            builder.Property(review => review.CreatedOnUtc)
                .HasColumnType("timestamp with time zone");
            
            builder.Property<uint>("Version").IsRowVersion();

        }
    }
}
