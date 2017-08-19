using System;
using System.Collections.Generic;

namespace MyView.Adapters
{
    /// <summary>
    /// Represents a deserialised response from Unsplash, along with associated logic.
    /// </summary>
    public partial class UnsplashImage
    {
    	/// Unique ID of this image.
        public string id { get; set; }
        /// The date and time at which this image was created.
        public DateTime created_at { get; set; }
        /// The date and time at which this image was last updated.
        public DateTime updated_at { get; set; }
        /// The width of this photo in pixels.
        public int width { get; set; }
        /// The height of this photo in pixels.
        public int height { get; set; }
        /// A description of the photograph.
        public string description { get; set; }
        public string color { get; set; }

        /// Camera exif information for this image.
        public UnsplashImageEXIF exif { get; set; } = new UnsplashImageEXIF();
		/// The location at which this image was captured.    
        public UnsplashImageLocation location { get; set; } = new UnsplashImageLocation();
        /// The categories to which this image belongs.
        public List<UnsplashImageCategories> categories { get; set; } = new List<UnsplashImageCategories>();
        /// Public weblinks that point to various sizes of this image.
        public UnsplashImageURLS urls { get; set; } = new UnsplashImageURLS();
        /// Weblinks that allow viewing and downloading of this image.
        public UnsplashImageLinks links { get; set; } = new UnsplashImageLinks();
        /// Information pertaining to the author of this image.
        public UnsplashUser user { get; set; } = new UnsplashUser();
        
        public UnsplashCustomValues custom { get; set; } = new UnsplashCustomValues();
    }

    public partial class UnsplashImage
    {
    	/// <summary>
    	/// A custom class that stores MyView-specific data.
    	/// </summary>
    	public class UnsplashCustomValues
    	{
    		/// The local storage location of this image after download.
        	public string imagePath { get; set; }
        	/// The raw byte information that makes up this image.
        	public byte[] imageData { get; set; }
    	}
    	
        public class UnsplashImageURLS
        {
            public string raw { get; set; }
            public string full { get; set; }
            public string regular { get; set; }
            public string small { get; set; }
            public string thumb { get; set; }
        }

        public class UnsplashImageLinks
        {
            public string html { get; set; }
            public string download { get; set; }
            public string download_location { get; set; }
        }

        public class UnsplashImageEXIF
        {
            public string make { get; set; }
            public string model { get; set; }
            public string exposure_time { get; set; }
            public string aperture { get; set; }
            public string focal_length { get; set; }
            public int iso { get; set; }
        }

        public class UnsplashImageLocation
        {
            public string city { get; set; }
            public string country { get; set; }
        }

        public class UnsplashImageCategories
        {
            public class UnsplashImageCategoriesLinks
            {
                public string self { get; set; }
                public string photos { get; set; }
            }

            public int id { get; set; }
            public string title { get; set; }
            public int photo_count { get; set; }
            public UnsplashImageCategoriesLinks links { get; set; } = new UnsplashImageCategoriesLinks();
        }
    }
}
