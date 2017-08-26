using UIKit;

using MyView.Additional;

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
			public static Category Random { get; set; } = new Category { DisplayName = "Random" };
			
			public static Category[] Categories { get; set; } = { 

				new Category { DisplayName = "Mountains", 	QueryString = "mountains" },
				new Category { DisplayName = "Horses", 		QueryString = "horses" },
				new Category { DisplayName = "Cars", 		QueryString = "cars" },
				new Category { DisplayName = "Forests", 	QueryString = "forests" },
				new Category { DisplayName = "Food", 		QueryString = "food" },
				new Category { DisplayName = "Space", 		QueryString = "space" },
				new Category { DisplayName = "People", 		QueryString = "people" }
				
			};
			#endregion
		}
	}
}
