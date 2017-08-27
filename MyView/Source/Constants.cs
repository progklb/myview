using System.IO;

using UIKit;

using MyView.Adapters;
using MyView.Additional;

using Mode = MyView.Adapters.SlideshowAdapter.SlideshowModes;

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
			public static string PlaceholderPhoto { get { return Path.Combine(PlaceholderPhotoPath, "PlaceholderPhoto.png"); } }
			
			public static string PlaceholderPhotoPath { get { return "CategoryPlaceholder"; } }
			#endregion
		}
		
		
		public static class Slideshow
		{
			#region PROPERTIES
			public static SlideshowCategory Random { get; set; } = new SlideshowCategory { SlideshowMode = Mode.Random, DisplayName = "Random" };
			
			public static SlideshowCategory[] Categories { get; set; } = { 

				new SlideshowCategory { SlideshowMode = Mode.Query, 	DisplayName = "Mountains", 	QueryString = "mountains" },
				new SlideshowCategory { SlideshowMode = Mode.Query, 	DisplayName = "Horses", 	QueryString = "horses" },
				new SlideshowCategory { SlideshowMode = Mode.Query, 	DisplayName = "Cars", 		QueryString = "cars" },
				new SlideshowCategory { SlideshowMode = Mode.Query, 	DisplayName = "Forests", 	QueryString = "forests" },
				new SlideshowCategory { SlideshowMode = Mode.Query, 	DisplayName = "Food", 		QueryString = "food" },
				new SlideshowCategory { SlideshowMode = Mode.Query, 	DisplayName = "Space", 		QueryString = "space" },
				new SlideshowCategory { SlideshowMode = Mode.Query, 	DisplayName = "People", 	QueryString = "people" }
				
			};
			#endregion
		}
	}
}
