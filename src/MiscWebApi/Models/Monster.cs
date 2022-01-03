using System.ComponentModel.DataAnnotations;

namespace MiscWebApi.Models;

public class Monster {
	[Required]
	public int Id { get; set; }
	[Required]
	public string Name { get; set; } = "";
}
