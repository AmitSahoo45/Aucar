using System.ComponentModel.DataAnnotations.Schema;

namespace AuctionService;

[Table("Items")] // by making this we are calling it a table "Items" in the database, which the entity framework can easily pickup
public class Item
{
    public Guid Id { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    public string Color { get; set; }
    public int Mileage { get; set; }
    public string ImageUrl { get; set; }

    // navigation or related properties so that entity framework can work with it
    public Auction Auction { get; set; }
    public Guid AuctionId { get; set; }
}