using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace OpenApiWebApi.Models;

/// <summary>
/// モンスターカテゴリ
/// </summary>
public enum MonsterCategory {
	/// <summary>
	/// 不明
	/// </summary>
	Unknown = 0,

	/// <summary>
	/// スライム系
	/// </summary>
	[Display(Name = "スライム系")]
	Slime,

	/// <summary>
	/// けもの系
	/// </summary>
	[Display(Name = "けもの系")]
	Animal,

	/// <summary>
	/// 鳥系
	/// </summary>
	[Display(Name = "鳥系")]
	Fly,
}
