namespace EndpointWebApp;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class SampleMetadataAttribute(int value) : Attribute, ISampleMetadata {
	public int Value { get; } = value;
}
