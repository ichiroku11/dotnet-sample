using Microsoft.ApplicationInsights.AspNetCore.TelemetryInitializers;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AzureAppInsightsWebApp;

// テレメトリに追加情報を含めるサンプル
/*
	参考
	https://docs.microsoft.com/ja-jp/azure/azure-monitor/app/api-filtering-sampling#addmodify-properties-itelemetryinitializer

	TelemetryInitializerの実装がある
	https://github.com/microsoft/ApplicationInsights-dotnet/tree/develop/NETCORE/src/Microsoft.ApplicationInsights.AspNetCore/TelemetryInitializers

	AspNetCoreEnvironmentTelemetryInitializer
	EnvironmentをTelemetryに含めるInitializer
	https://github.com/microsoft/ApplicationInsights-dotnet/blob/develop/NETCORE/src/Microsoft.ApplicationInsights.AspNetCore/TelemetryInitializers/AspNetCoreEnvironmentTelemetryInitializer.cs

	ClientIpHeaderTelemetryInitializer
	クライアントのIPアドレスをTelemetryに含めるInitializer
	https://github.com/microsoft/ApplicationInsights-dotnet/blob/develop/NETCORE/src/Microsoft.ApplicationInsights.AspNetCore/TelemetryInitializers/ClientIpHeaderTelemetryInitializer.cs
*/
/// <summary>
/// 
/// </summary>
// ITelemetryInitializerを実装するのもあり
public class SampleTelemetryInitializer : TelemetryInitializerBase {
	private const string _propertyName = "sample";

	public SampleTelemetryInitializer(IHttpContextAccessor httpContextAccessor)
		: base(httpContextAccessor) {
	}

	protected override void OnInitializeTelemetry(
		HttpContext platformContext, RequestTelemetry requestTelemetry, ITelemetry telemetry) {

		// 適当なサンプル
		var propertyValue = "xyz";

		if (!requestTelemetry.Properties.ContainsKey(_propertyName)) {
			requestTelemetry.Properties.Add(_propertyName, propertyValue);
		}

		if (telemetry is ISupportProperties properties && !properties.Properties.ContainsKey(_propertyName)) {
			properties.Properties.Add(_propertyName, propertyValue);
		}
	}
}
