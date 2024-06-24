[Serializable]
public class StringtooLongException : Exception
{
	public StringtooLongException() { }
	public StringtooLongException(string message) : base(message) { }
	public StringtooLongException(string message, Exception inner) : base(message, inner) { }
	protected StringtooLongException(
	  System.Runtime.Serialization.SerializationInfo info,
	  System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
}