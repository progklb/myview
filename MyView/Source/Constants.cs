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
            public static SlideshowCategory Random { get; set; } = new SlideshowCategory { SlideshowMode = Mode.Random, DisplayName = "Random", AssetName = "Random" };

			// Important: SlideshowCategory.DisplayName is used to dynamically locate the thumbnail that represents the category. 
			// If a change is made to the display name, the image resource must be renamed accordingly.
			// TODO Consider implementing a "SourceName" property to abstract from the display name.
			public static SlideshowCategory[] Categories { get; set; } = { 
                new SlideshowCategory { SlideshowMode = Mode.Query,    DisplayName = "Animals",        AssetName = "Animals",       QueryString = "animals" },
                new SlideshowCategory { SlideshowMode = Mode.Query,    DisplayName = "Architecture",   AssetName = "Architecture",  QueryString = "architecture" },
                new SlideshowCategory { SlideshowMode = Mode.Query,    DisplayName = "Aviation",       AssetName = "Aviation",      QueryString = "aviation%20planes" },
                new SlideshowCategory { SlideshowMode = Mode.Query,    DisplayName = "Bokeh",          AssetName = "Bokeh",         QueryString = "bokeh" },
                new SlideshowCategory { SlideshowMode = Mode.Query,    DisplayName = "Business",       AssetName = "Business",      QueryString = "business%20office" },
                new SlideshowCategory { SlideshowMode = Mode.Query,    DisplayName = "Cars",           AssetName = "Cars",          QueryString = "cars" },
                new SlideshowCategory { SlideshowMode = Mode.Query,    DisplayName = "Cities",         AssetName = "Cities",        QueryString = "city" },
                new SlideshowCategory { SlideshowMode = Mode.Query,    DisplayName = "Food & Drink",   AssetName = "Food&Drink",    QueryString = "food%20drink%20coffee" },
                new SlideshowCategory { SlideshowMode = Mode.Query,    DisplayName = "Motorcycles",    AssetName = "Motorcycles",   QueryString = "motorcycles" },
                new SlideshowCategory { SlideshowMode = Mode.Query,    DisplayName = "Music",          AssetName = "Music",         QueryString = "music" },
                new SlideshowCategory { SlideshowMode = Mode.Query,    DisplayName = "Objects",        AssetName = "Objects",       QueryString = "objects" },
                new SlideshowCategory { SlideshowMode = Mode.Query,    DisplayName = "Ocean",          AssetName = "Ocean",         QueryString = "ocean" },
                new SlideshowCategory { SlideshowMode = Mode.Query,    DisplayName = "Outdoors",       AssetName = "Outdoors",      QueryString = "landscape%20mountain%20forest" },
                new SlideshowCategory { SlideshowMode = Mode.Query,    DisplayName = "People",         AssetName = "People",        QueryString = "love%20people%20friends" },
                new SlideshowCategory { SlideshowMode = Mode.Query,    DisplayName = "Retro",          AssetName = "Retro",         QueryString = "retro" },
                new SlideshowCategory { SlideshowMode = Mode.Query,    DisplayName = "Roads",          AssetName = "Roads",         QueryString = "roads" },
                new SlideshowCategory { SlideshowMode = Mode.Query,    DisplayName = "Seasons",        AssetName = "Seasons",       QueryString = "winter%20spring%20autumn" },
                new SlideshowCategory { SlideshowMode = Mode.Query,    DisplayName = "Sports",         AssetName = "Sports",        QueryString = "extreme%20sport" },
                new SlideshowCategory { SlideshowMode = Mode.Query,    DisplayName = "Stars",          AssetName = "Stars",         QueryString = "stars" },
                new SlideshowCategory { SlideshowMode = Mode.Query,    DisplayName = "Technology",     AssetName = "Technology",    QueryString = "technology" },
			};
			#endregion
		}
	}
}
