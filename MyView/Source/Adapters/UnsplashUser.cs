namespace MyView.Adapters
{
    public class UnsplashUser
    {
        public class UnsplashUserLinks
        {
            public string self { get; private set; }
            public string html { get; private set; }
            public string photos { get; private set; }
            public string likes { get; private set; }
            public string portfolio { get; private set; }
        }

        public string id { get; private set; }
        public string username { get; private set; }
        public string portfolio_url { get; private set; }
        public string bio { get; private set; }
        public string location { get; private set; }
        public int total_likes { get; private set; }
        public int total_photos { get; private set; }
        public int total_collections { get; private set; }
        public UnsplashUserLinks links { get; private set; }
    }
}
