using System;
using System.Threading.Tasks;

using MyView.Additional;
using MyView.Unsplash;

namespace MyView.Adapters
{
    /// <summary>
    /// Provides images at fixed intervals and defines parameters for display.
    /// </summary>
    public class SlideshowAdapter
    {
    	#region TYPES
    	/// <summary>
    	/// The different slideshow modes supported.
    	/// </summary>
    	public enum SlideshowModes
    	{
    		Random,
    		Query
    	}
    	#endregion
    	
    	
        #region EVENTS
        /// Is raised when an image cycle expires. The parameter carries the new image to display.
        public event Action<UnsplashImage> OnImageCycled = delegate { };
        /// Is raised when the slideshow mode is changed. The parameter carries the new mode's display name.
        public event Action<string> OnCategoryChanged = delegate { };
        /// Is raised when an error occurs. The provided string paramter passes a message that describes the error.
        public event Action<string> OnErrorThrown = delegate { };
        #endregion


        #region PROPERTIES
        /// The default slideshow category if there is no <see cref="CurrentCategory"/> assigned.
        public SlideshowCategory DefaultCategory { get; private set; } = Constants.Slideshow.Random;
        /// The currently active slideshow category.
        public SlideshowCategory CurrentCategory { get; private set; } = null;
		/// The latest image to display.
        public UnsplashImage CurrentImage  { get; private set; }
        
        /// Whether the slideshow is active.
        public bool IsRunning { get; private set; }
        /// The time in milliseconds between cycles. Changes to this value will only take effect after the current image cycle.
        public int CycleTime { get; set; } = 10000;
        /// The duration in milliseconds of transitions between images.
        public int TransitionDuration { get; set; } = 2000;
        /// The interval between retrying server requests after a failure.
        public int RetryTimeout { get; set; } = 5000;
        #endregion
        
        
        #region VARIABLES
        /// Timer used to track cycle time.
        private Timer m_Timer;
        /// Indicates that the slideshow category was changed.
        private bool m_CategoryChanged;
        #endregion


        #region PUBLIC API
        /// <summary>
        /// Starts the slideshow service. This requests images based on the current <see cref="CurrentCategory"/>, delivering new images via <see cref="OnImageCycled"/>.
        /// </summary>
        public void Start()
        {
        	UnsplashAdapter.OnErrorThrown += RaiseOnErrorThrown;
        	
            m_Timer = new Timer();
            
            IsRunning = true;
            StartServiceAsync().ConfigureAwait(false);
        }
		
		/// <summary>
		/// Stops an ongoing slideshow service.
		/// </summary>
        public void Stop()
        {
        	UnsplashAdapter.OnErrorThrown -= RaiseOnErrorThrown;
        	
            IsRunning = false;
        }
        
        /// <summary>
		/// Sets the behaviour of this slideshow controller. The value used is any value in <see cref="Constants.Slideshow"/>.
		/// </summary>
        public void SetSlideshowCategory(SlideshowCategory category)
        {
	        m_CategoryChanged = category != CurrentCategory;
        	CurrentCategory = category;
        	OnCategoryChanged(CurrentCategory.DisplayName);
        }
        #endregion


        #region SLIDESHOW
        void HandleCategoryChanged()
        {
        	Stop();
        	Start();
        }
        
        /// <summary>
        /// A looping service that will continually download and deliver images to listeners.
        /// </summary>
        async Task StartServiceAsync()
        {
// Provide an override for DEBUG mode to save bandwidth. Because developers have Internet limits too!            
#if !DEBUG
			var customSize = UnsplashImage.UnsplashImageSizes.Small;
#else
			var customSize = UnsplashImage.UnsplashImageSizes.Default;
#endif
            // Download the next image. Note that we take the current time, initiate image download, and then wait the remaining time until
            // raising the cycle event. In this manner, download time does not affect the cycling time (unless the download exceeds the cycle time,
            // in which case we immediately cycle to the downloaded image as soon as it is available).
            
            // IMPROVE
            // Note that breaks have been added should the mode be changed mid-cycle.
            // This breaks out of this service and starts a new one, causing a soon-as-possible cleanup and display of the new category.
            // Note that we don't cancel mid-download because this causes an exception when using "await" (the awaiting is waiting for a finished Task from the downloader)
            // and changing this to a non-Task-based approach requires quite a bit of rework.
            
            IsRunning = true;
        	bool firstRun = true;
            
            do
            {
            	m_CategoryChanged = false;
            	
            	m_Timer.Start();
				var unsplashImage = await RequestImageAsync();
				
				if (m_CategoryChanged) { break; }
					
				if (unsplashImage != null)
				{
					unsplashImage.custom.imageData = await UnsplashAdapter.DownloadPhotoAsync(unsplashImage, customSize);
					                	
                	// For the start we want to display the image as soon as it is downloaded.
                	// Thereafter, we will download the next image but wait for the cycle timeout before displaying it.
                	if (firstRun)
                	{
	                	UpdateImage(unsplashImage);
	                	firstRun = false;
                	}
                	else
                	{             		
                		while (CycleTime + TransitionDuration - m_Timer.GetElapsedTime() > 0)
                		{	
							if (m_CategoryChanged) { break; }
                			await Task.Delay(100);
                		}
                		
						if (m_CategoryChanged) { break; }
                	
	                	UpdateImage(unsplashImage);
                	}
                }
                else
                {
                	await Task.Delay(RetryTimeout);
                }
            }
            while (IsRunning);
            
            if (m_CategoryChanged)
            {
            	HandleCategoryChanged();
            }
        }
        
        /// <summary>
        /// Requests an image based on the current slideshow mode.
        /// </summary>
        async Task<UnsplashImage> RequestImageAsync()
        {
        	var category = CurrentCategory;
        	if (category == null)
        	{
        		Console.WriteLine("No category has been assigned. Defaulting to Random.");
        		category = DefaultCategory;
        	}
        	
        	switch (category.SlideshowMode)
        	{
        		case SlideshowModes.Random:
        			return await UnsplashAdapter.Instance.GetRandomPhotoAsync();
        		case SlideshowModes.Query:
        			return await UnsplashAdapter.Instance.GetRandomQueryAsync(CurrentCategory.QueryString);
        			
    			default:
    				throw new NotImplementedException($"There is currently no support for the selected slideshow mode: {CurrentCategory.SlideshowMode}.");
        	}
        }
        
        /// <summary>
        /// Sets the provided image as the current image and delivers the image to listeners.
        /// </summary>
        /// <param name="image">Image.</param>
        void UpdateImage(UnsplashImage image)
        {
        	CurrentImage = image;
	        OnImageCycled(CurrentImage);
        }
        #endregion
        
        
        #region HELPERS
        /// <summary>
        /// Raises the <see cref="OnErrorThrown"/> event with the provided message.
        /// </summary>
        /// <param name="message">Message.</param>
        void RaiseOnErrorThrown(string message)
        {
        	OnErrorThrown(message);
        }
        #endregion
    }
}
