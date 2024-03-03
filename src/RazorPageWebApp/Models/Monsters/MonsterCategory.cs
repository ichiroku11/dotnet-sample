using System.ComponentModel.DataAnnotations;

namespace RazorPageWebApp.Models.Monsters;

public enum MonsterCategory {
	None = 0,

	[Display(Name = "スライム系")]
	Slime,

	[Display(Name = "けもの系")]
	Animal,

	[Display(Name = "鳥系")]
	Fly,
}
