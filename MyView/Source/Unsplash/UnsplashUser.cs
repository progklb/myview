namespace MyView.Adapters
{
    public partial class UnsplashUser
    {
        public string id { get; set; }
        public string username { get; set; }
        public string name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string portfolio_url { get; set; }
        public string bio { get; set; }
        public string location { get; set; }
        public int total_likes { get; set; }
        public int total_photos { get; set; }
        public int total_collections { get; set; }
        
        /// Weblinks to this user's pages.
        public UnsplashUserLinks links { get; set; } = new UnsplashUserLinks();
        /// Various sizes for this user's profile image.
        public UnsplashUserProfileImage profile_image { get; set; } = new UnsplashUserProfileImage();
    }
    
    public partial class UnsplashUser
    {
    	public class UnsplashUserLinks
        {
            public string self { get; set; }
            public string html { get; set; }
            public string photos { get; set; }
            public string likes { get; set; }
            public string portfolio { get; set; }
        }
        
        public class UnsplashUserProfileImage
        {
            public string small { get; set; }
            public string medium { get; set; }
            public string large { get; set; }
        }
    }
}
