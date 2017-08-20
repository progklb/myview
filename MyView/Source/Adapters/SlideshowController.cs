using System;
using System.Threading.Tasks;

using MyView.Additional;

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
        /// <summary>
        /// Is raised when an error occurs. The provided string paramter passes a message that describes the error.
        /// </summary>
        public event Action<string> OnErrorThrown = delegate { };
        #endregion


        #region PROPERTIES
        /// The latest image to display.
        public UnsplashImage CurrentImage  { get; private set; }
        /// A cache of previous images.
        public RandomAccessQueue<UnsplashImage> History { get; private set; } = new RandomAccessQueue<UnsplashImage>(5);
        
        /// Whether the slideshow is active.
        public bool IsRunning;
        /// The time in milliseconds between cycles. Changes to this value will only take effect after the current image cycle.
        public int CycleTime { get; set; } = 10000;
        /// The duration in milliseconds of transitions between images.
        public int TransitionDuration { get; set; } = 2000;
        
        public int RetryTimeout { get; set; } = 5000;
        #endregion
        
        
        #region VARIABLES
        /// Timer used to track cycle time.
        private Timer m_Timer;
        #endregion


        #region PUBLIC API
        public void Start()
        {
        	UnsplashAdapter.OnErrorThrown += RaiseOnErrorThrown;
        	
            IsRunning = true;
            m_Timer = new Timer();
            StartService().ConfigureAwait(false);
        }

        public void Stop()
        {
        	UnsplashAdapter.OnErrorThrown -= RaiseOnErrorThrown;
        	
            IsRunning = false;
        }
        #endregion


        #region HELPERS
        async Task StartService()
        {
        	bool firstRun = true;
        	
            // Download the next image. Note that we take the current time, initiate image download, and then wait the remaining time until
            // raising the cycle event. In this manner, download time does not affect the cycling time (unless the download exceeds the cycle time,
            // in which case we immediately cycle to the downloaded image as soon as it is available).
            do
            {
            	m_Timer.Start();
                
				var unsplashImage = await UnsplashAdapter.Instance.GetRandomPhotoAsync();
				
				if (unsplashImage != null)
				{
                	unsplashImage.custom.imageData = await UnsplashAdapter.DownloadPhotoAsync(unsplashImage);
					m_Timer.Stop();
                	
                	// For the first image, display it as soon as it has been downloaded.
                	// Wait just a minute! Perhaps we should not.
                	// Have a special start up routine for the main menu ? We need to figure something out here. Think about the timing for God's sake.
                	if (firstRun)
                	{
	                	UpdateImage(unsplashImage);
	                	await Task.Delay(m_Timer.GetElapsedTime() + TransitionDuration);
	                	firstRun = false;
                	}
                	else
                	{
	                	await Task.Delay(m_Timer.GetElapsedTime() + TransitionDuration);
	                	UpdateImage(unsplashImage);
                	}
                }
                else
                {
                	await Task.Delay(RetryTimeout);
                }
            }
            while (IsRunning);
        }
        
        void UpdateImage(UnsplashImage image)
        {
        	CurrentImage = image;
            History.Enqueue(CurrentImage, true);
	        OnImageCycled(CurrentImage);
        }
        
        void RaiseOnErrorThrown(string message)
        {
        	OnErrorThrown(message);
        }
        #endregion
    }
}
