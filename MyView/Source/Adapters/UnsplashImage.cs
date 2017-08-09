using System;
using System.Collections.Generic;

namespace MyView
{
    /// <summary>
    /// Represents a deserialised response from Unsplash, along with associated logic.
    /// </summary>
    public partial class UnsplashImage
    {
        public string id { get; private set; }
        public DateTime created_at { get; private set; }
        public DateTime updated_at { get; private set; }
        public int width { get; private set; }
        public int height { get; private set; }
        public string description { get; private set; }

        public UnsplashImageURLS urls { get; private set; }
        public UnsplashImageEXIF exif { get; private set; }
        public UnsplashImageLocation location { get; private set; }
        public List<UnsplashImageCategories> categories { get; private set; }
        public UnsplashImageLinks links { get; private set; }
        public UnsplashUser user { get; private set; }

        public string imagePath { get; set; }
        public byte[] imageData { get; set; }
    }

    public partial class UnsplashImage
    {
        public struct UnsplashImageURLS
        {
            public string raw { get; private set; }
            public string full { get; private set; }
            public string regular { get; private set; }
            public string small { get; private set; }
            public string thumb { get; private set; }
        }

        public struct UnsplashImageLinks
        {
            public string html { get; private set; }
            public string download { get; private set; }
            public string download_location { get; private set; }
        }

        public struct UnsplashImageEXIF
        {
            public string make { get; private set; }
            public string model { get; private set; }
            public string exposure_time { get; private set; }
            public string aperture { get; private set; }
            public string focal_length { get; private set; }
            public int iso { get; private set; }
        }

        public struct UnsplashImageLocation
        {
            public string city { get; private set; }
            public string country { get; private set; }
        }

        public struct UnsplashImageCategories
        {
            public struct UnsplashImageCategoriesLinks
            {
                public string self { get; private set; }
                public string photos { get; private set; }
            }

            public int id { get; private set; }
            public string title { get; private set; }
            public int photo_count { get; private set; }
            public UnsplashImageCategoriesLinks links { get; private set; }
        }
    }
}
