using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EndpointWebApp {
	// https://docs.microsoft.com/ja-jp/aspnet/core/fundamentals/routing?view=aspnetcore-3.1#guidance-for-library-authors
	// エンドポイントのメタデータは、インターフェイスとして定義するとよいみたい
	// そのインターフェイスは属性として実装するとよいみたい

	public interface ISampleMetadata {
		int Value { get; }
	}
}
