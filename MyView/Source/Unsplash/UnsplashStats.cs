namespace MyView.Unsplash
{
	/// <summary>
	/// Container object for the response to GET /stats/total
	/// </summary>
	public class UnsplashStats
	{
		public int photos { get; set; }
        public int downloads { get; set; }
        public int views { get; set; }
        public int likes { get; set; }
        public int pixels { get; set; }
        /// The average number of downloads per second for the past 7 days.
        public int downloads_per_second { get; set; }
        /// The average number of views per second for the past 7 days.
        public int views_per_second { get; set; }
        public int developers { get; set; }
        public int applications { get; set; }
        public int requests { get; set; }
	}
}
