namespace AutoMarket.Domain;

public class Vehicle : Entity
{
    public Vehicle(string listingId, int year, decimal price)
    {
        ListingId = listingId;
        Year = year;
        Price = price;
    }   

    public string ListingId { get; private set; }
    public int Year { get; private set; }
    public decimal Price { get; private set; }
}
