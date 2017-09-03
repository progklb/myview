using System;
using System.Collections.Generic;

namespace MyView.Unsplash
{
    /// <summary>
    /// Represents a deserialised response from Unsplash, along with associated logic.
    /// </summary>
    public partial class UnsplashImage
    {
    	#region PROPERTIES
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
        /// The predominant color of this image. ??
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
        
        /// Store of MyView-specific data. This has nothing to do with Unsplash API, but is here as convenience
        /// in order to keep the image object a singular package.
        public UnsplashCustomValues custom { get; set; } = new UnsplashCustomValues();
        #endregion
        
        
        #region CONSTRUCTORS
        public UnsplashImage()
        {
        }
        
        public UnsplashImage(byte[] imageData, string filepath = "")
        {
        	custom.imagePath = filepath;
        	custom.imageData = imageData;
        }
        #endregion
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
    	
    	/// <summary>
    	/// Endpoints of various image sizes.
    	/// </summary>
        public class UnsplashImageURLS
        {
            public string raw { get; set; }
            public string full { get; set; }
            public string regular { get; set; }
            public string small { get; set; }
            public string thumb { get; set; }
            /// Note that this property is only returned when specifying a custom size in the Unspash query.
            public string custom { get; set; }
        }

        public class UnsplashImageLinks
        {
            public string html { get; set; }
            public string download { get; set; }
            public string download_location { get; set; }
        }

		/// <summary>
		/// Camera data related to this image.
		/// </summary>
        public class UnsplashImageEXIF
        {
            public string make { get; set; }
            public string model { get; set; }
            public string exposure_time { get; set; }
            public string aperture { get; set; }
            public string focal_length { get; set; }
            public int iso { get; set; }
        }

		/// <summary>
		/// The location at which this image was captured.
		/// </summary>
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
        
        /// <summary>
        /// Unsplash image sizes that can be specified.
        /// These correspond to the endpoints contained in <see cref="UnsplashImageURLS"/>
        /// </summary>
        public enum UnsplashImageSizes
        {
        	Default,
        	
        	Full,
        	Regular,
        	Small,
        	Thumb,
        	Custom
        }
    }
}
