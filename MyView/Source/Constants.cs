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
		
		public static class Images
		{
			#region PROPERTIES
			public static string PlaceholderPhoto { get { return "PlaceholderPhoto.png"; } }
			#endregion
		}
		
		
		public static class Slideshow
		{
			#region PROPERTIES
			public static string[] Categories { get; set; } = { "Mountains", "Horses", "Cars", "Forests", "Food", "Space", "People" };
			public static string Random { get; set; } = "Random";
			#endregion
		}
	}
}
