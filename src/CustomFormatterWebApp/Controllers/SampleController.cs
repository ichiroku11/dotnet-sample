using CustomFormatterWebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace CustomFormatterWebApp.Controllers;

[Route("api/[controller]")]
public class SampleController : ControllerBase {
	[HttpPost]
	public async Task<Sample> PostAsync([FromBody] Sample sample) {
		await Task.CompletedTask;
		return sample;
	}
}
