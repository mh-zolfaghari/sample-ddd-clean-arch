namespace Architecture.Shared.Commons;

// Provides default values for various types used across the architecture.
public static class DefaultValues
{
    // Default value for RowVersion, represented as an 8-byte array initialized to zero.
    public static byte[] DefaultRowVersion => new byte[8];
}
