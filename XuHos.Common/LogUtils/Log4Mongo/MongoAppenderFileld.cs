using log4net.Layout;

namespace XuHos.Common.Log.Log4Mongo
{
	public class MongoAppenderFileld
	{
		public string Name { get; set; }
		public IRawLayout Layout { get; set; }
	}
}