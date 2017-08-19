using System;
using System.Threading.Tasks;

namespace MyView.Adapters
{
    /// <summary>
    /// Provides images at fixed intervals and defines parameters for display.
    /// </summary>
    class SlideshowController
    {
        #region EVENTS
        /// <summary>
        /// Is raised when an image cycle expires. The parameter carries the new image to display.
        /// </summary>
        public event Action<UnsplashImage> OnImageCycled = delegate { };
        #endregion


        #region PROPERTIES
        /// Whether the slideshow is active.
        public bool Running;
        /// The time in milliseconds between cycles. Changes to this value will only take effect after the current image cycle.
        public int CycleTime { get; set; } = 3000;
        
        /// The time in milliseconds remaining of the current image cycle, based on the <see cref="m_CycleTicks"/> value.
        private int CycleTimeRemaining { get { return (int)(m_CycleTicks / TimeSpan.TicksPerMillisecond); } }
        #endregion
        
        
        #region VARIABLES
        /// A storage variable for tracking cycle time.
        private long m_CycleTicks;
        #endregion


        #region PUBLIC API
        public void Start()
        {
            Running = true;
            StartService().ConfigureAwait(false);
        }

        public void Stop()
        {
            Running = false;
        }
        #endregion


        #region HELPERS
        async Task StartService()
        {
            var imageSet = new string[] { "cars", "food", "forests", "horses", "mountains", "people", "space" };
            var imageURLS = new string[] { 
				"xCmvrpzctaQ", 
				"OVlFXzeAoqQ", 
				"kSmTaltv9KU",
				"7HVGbM4JilI",
				"OTd55EeZMT4",
				"PB-14uh_lyg",
				"PHIgYUGQPvU",
				"anhQGEYbnV4",
				"AMGMIHBIT5g",
				"L5aI2jU0i50",
				"29seD7tA",
				"Oj9z9dvGh6E",
				"iq0EoeLNoy8",
				"6vjJZYYIiBw",
				"eCjij29oPc",
				"fBkkiSWvnmM",
				"JknoLnbr7hI",
				"Z1COpZVLB0Y" 
			};
			
			// Working schemas:
			//"https://images.unsplash.com/photo-1452457807411-4979b707c5be"
			//"https://unsplash.com/photos/SoC1ex6sI4w/download";
			
			//imageURLS[rng.Next(0, imageURLS.Length-1)]     

            Random rng = new Random();
            int timeRemaining;
            
            // Download the next image. Note that we take the current time, initiate image download, and then wait the remaining time until
            // raising the cycle event. In this manner, download time does not affect the cycling time (unless the download exceeds the cycle time,
            // in which case we immediately cycle to the downloaded image as soon as it is available).
            do
            {
            	m_CycleTicks = DateTime.Now.Ticks;
                
				var unsplashImage = await UnsplashAdapter.Instance.GetRandomPhotoAsync();
                unsplashImage.imageData = await UnsplashAdapter.DownloadPhotoAsync(new UnsplashImage() { id = imageURLS[rng.Next(0, imageURLS.Length-1)] }, UnsplashAdapter.UriMode.PhotoID);
                //TODO Push into history
                
				m_CycleTicks = DateTime.Now.Ticks - m_CycleTicks;                
                     
				timeRemaining = CycleTimeRemaining;
                await Task.Delay(timeRemaining > 0 ? timeRemaining : 0);
                
                OnImageCycled(unsplashImage);
            }
            while (Running);
        }
        #endregion
    }
}
