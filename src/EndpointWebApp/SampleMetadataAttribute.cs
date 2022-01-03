namespace EndpointWebApp;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class SampleMetadataAttribute : Attribute, ISampleMetadata {
	public SampleMetadataAttribute(int value) {
		Value = value;
	}

	public int Value { get; }
}
