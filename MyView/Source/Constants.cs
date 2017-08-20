using UIKit;

namespace MyView
{
	public class Constants
	{
		public static class Colors
		{
			#region PROPERTIES
			public static UIColor Black { get { return new UIColor(0f, 0f, 0f, 1f); } }
			public static UIColor BlackTransparent { get { return new UIColor(0f, 0f, 0f, 0f); } }
			#endregion
		}
		
		public static class Categories
		{
			#region PROPERTIES
			public static string[] List { get; set; } = { "Mountains", "Horses", "Cars", "Forests", "Food", "Space", "People" };
			#endregion
		}
	}
}
