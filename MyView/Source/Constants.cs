using System.IO;

using UIKit;

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
			public static string PlaceholderPhoto { get { return Path.Combine(PlaceholderPhotoPath, "_PlaceholderPhoto.png"); } }
			public static string StartUpPhoto { get { return Path.Combine(StartUpPath, "StartUp.jpg"); } }
			
			
			public static string PlaceholderPhotoPath { get { return "CategoryPlaceholder"; } }
			public static string StartUpPath { get { return "StartUp"; } }
			#endregion
		}
		
		public static class Slideshow
		{
			#region PROPERTIES
			public static SlideshowCategory Random { get; set; } = new SlideshowCategory { SlideshowMode = Mode.Random, DisplayName = "Random" };

			// Important: SlideshowCategory.DisplayName is used to dynamically locate the thumbnail that represents the category. 
			// If a change is made to the display name, the image resource must be renamed accordingly.
			// TODO Consider implementing a "SourceName" property to abstract from the display name.
			public static SlideshowCategory[] Categories { get; set; } = { 
				new SlideshowCategory { SlideshowMode = Mode.Query, 	DisplayName = "Animals", 		QueryString = "animals" },
				new SlideshowCategory { SlideshowMode = Mode.Query, 	DisplayName = "Architecture", 	QueryString = "architecture" },
				new SlideshowCategory { SlideshowMode = Mode.Query, 	DisplayName = "Aviation",		QueryString = "aviation%20planes" },
				new SlideshowCategory { SlideshowMode = Mode.Query, 	DisplayName = "Bokeh", 			QueryString = "bokeh" },
				new SlideshowCategory { SlideshowMode = Mode.Query, 	DisplayName = "Cars", 			QueryString = "cars" },
				new SlideshowCategory { SlideshowMode = Mode.Query, 	DisplayName = "Cities", 		QueryString = "city" },
				new SlideshowCategory { SlideshowMode = Mode.Query, 	DisplayName = "Food & Drink", 	QueryString = "food%20drink%20coffee" },
				new SlideshowCategory { SlideshowMode = Mode.Query, 	DisplayName = "Motorcycles", 	QueryString = "motorcycles" },
				new SlideshowCategory { SlideshowMode = Mode.Query, 	DisplayName = "Music", 			QueryString = "music" },
				new SlideshowCategory { SlideshowMode = Mode.Query, 	DisplayName = "Nature", 		QueryString = "landscape%20mountain%20forest" },
				new SlideshowCategory { SlideshowMode = Mode.Query, 	DisplayName = "Objects", 		QueryString = "objects" },
				new SlideshowCategory { SlideshowMode = Mode.Query, 	DisplayName = "Ocean", 			QueryString = "ocean" },
				new SlideshowCategory { SlideshowMode = Mode.Query, 	DisplayName = "Business", 		QueryString = "business%20office" },
				new SlideshowCategory { SlideshowMode = Mode.Query, 	DisplayName = "People", 		QueryString = "love%20people%20friends" },
				new SlideshowCategory { SlideshowMode = Mode.Query, 	DisplayName = "Retro", 			QueryString = "retro" },
				new SlideshowCategory { SlideshowMode = Mode.Query, 	DisplayName = "Roads", 			QueryString = "roads" },
				new SlideshowCategory { SlideshowMode = Mode.Query, 	DisplayName = "Seasons", 		QueryString = "winter%20spring%20autumn" },
				new SlideshowCategory { SlideshowMode = Mode.Query, 	DisplayName = "Sports", 		QueryString = "extreme%20sport" },
				new SlideshowCategory { SlideshowMode = Mode.Query, 	DisplayName = "Stars", 			QueryString = "stars" },
				new SlideshowCategory { SlideshowMode = Mode.Query, 	DisplayName = "Technology", 	QueryString = "technology" },
			};
			#endregion
		}
	}
}
