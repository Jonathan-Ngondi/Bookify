using Application.Abstractions.Data;
using Bogus;
using Dapper;
using Domain.Apartments;

namespace Bookify.API.Extensions
{
    public static class SeedDataExtensions
    {
        public static void SeedData(this IApplicationBuilder app)
        {
            using var scope = app.ApplicationServices.CreateScope();

            var sqlConnectionFactory = scope.ServiceProvider.GetRequiredService<ISqlConnectionFactory>();
            using var connection = sqlConnectionFactory.CreateConnection();

            var faker = new Faker();

            List<object> apartments = new();
            for (var i=0; i <100; i++)
            {
                apartments.Add(new
                {
                    Id = Guid.NewGuid(),
                    Name = faker.Company.CompanyName(),
                    Description = faker.Lorem.Paragraph(),
                    Country = faker.Address.Country(),
                    State = faker.Address.State(),
                    ZipCode = faker.Address.ZipCode(),
                    City = faker.Address.City(),
                    Street = faker.Address.StreetAddress(),
                    PriceAmount = faker.Finance.Amount(100, 1000),
                    PriceCurrency = "USD",
                    CleaningFeeAmount = faker.Random.Decimal(10, 200),
                    CleaningFeeCurrency = "USD",
                    Amenities = new List<int> { (int)Amenity.Parking, (int)Amenity.MountainView, (int)Amenity.Gym, (int)Amenity.WiFi },
                    LastBookedOn = DateTime.MinValue
                });
              
            }

            const string sql = """
                INSERT INTO public.apartments (
                    id, name, description, address_coutry, address_state, address_zip_code, 
                    address_city, address_street, price_amount, price_currency, 
                    cleaning_fee_amount, cleaning_fee_currency, last_booked_on_utc, amenities
                )
                VALUES (
                    @Id, @Name, @Description, @Country, @State, @ZipCode, 
                    @City, @Street, @PriceAmount, @PriceCurrency, 
                    @CleaningFeeAmount, @CleaningFeeCurrency, @LastBookedOn, @Amenities
                );
                """;

            connection.Execute(sql, apartments);
        }
    }
}
