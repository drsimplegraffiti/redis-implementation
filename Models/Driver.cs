

using System.ComponentModel.DataAnnotations;

namespace CachingRedis.Models;
public class Driver
{
    // use Identity for Id
    [Key ]
    public int Id { get; set; }
    public required string Name { get; set; }

    public int DriverNb { get; set; }
}
