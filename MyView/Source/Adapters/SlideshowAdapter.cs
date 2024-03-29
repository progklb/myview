using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using MyView.Additional;
using MyView.Unsplash;

namespace MyView.Adapters
{
	// TODO The handling of category changes is very tacked-on here.
	// If we ever get work on an update, refactor this class to be more elegant.
	
    /// <summary>
    /// Provides images at fixed intervals and defines parameters for display.
    /// </summary>
    public sealed class SlideshowAdapter
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


        #region CONSTANTS
        /// The predefined cycle times short, medium, long, corresponding respectively to <see cref=" DurationModes"/>.
        private readonly int[] CYCLE_TIMES = { 10000, 20000, 30000 };
        /// The predefined transition duration times short, medium, long, corresponding respectively to <see cref=" TransitionDurations"/>.
        private readonly int[] TRANS_DURATIONS = { 1000, 2000, 3000 };
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
        /// An existing instance of this adapter. If no instance exists, one will be instantiated and returned.
        public static SlideshowAdapter Instance { get => GetAdapterInstance(); }

        /// The default slideshow category if there is no <see cref="CurrentCategory"/> assigned.
        public SlideshowCategory DefaultCategory { get; private set; } = Constants.Slideshow.Random;
        /// The currently active slideshow category.
        public SlideshowCategory CurrentCategory { get; private set; } = null;
        
		/// The current list of images returned by the server.
        public List<UnsplashImage> CurrentList { get; private set; }
		/// The latest image to display.
        public UnsplashImage CurrentImage { get; private set; }
        
        /// Whether the slideshow is active.
        public bool IsRunning { get; private set; }

        /// The time in milliseconds between cycles.
        public int CycleDuration { get => CYCLE_TIMES[(int)SettingsAdapter.SlideshowCycleDurationMode]; }
        /// The duration in milliseconds of transitions between images.
        public int TransitionDuration { get => TRANS_DURATIONS[(int)SettingsAdapter.SlideshowTransitionDurationMode]; }

        /// The interval between retrying server requests after a failure.
        public int RetryTimeout { get => 5000; }
        #endregion


        #region VARIABLES
        private static SlideshowAdapter m_Instance;

        /// Timer used to track cycle time.
        private Timer m_Timer;
		/// The index of the <see cref="CurrentImage"/> from <see cref="CurrentList"/>
        private int m_CurrentImageIndex;
        #endregion


        #region PUBLIC API
        /// <summary>
        /// Starts the slideshow service. This requests images based on the current <see cref="CurrentCategory"/>, delivering new images via <see cref="OnImageCycled"/>.
        /// </summary>
        public void Start()
        {
        	if (!IsRunning)
        	{
                UnsplashAdapter.CustomSize = UnsplashAdapter.SizingParameters.Full;
	        	UnsplashAdapter.OnErrorThrown += RaiseOnErrorThrown;
	        	
	            m_Timer = new Timer();
	            
	            IsRunning = true;
	            StartServiceAsync().ConfigureAwait(false);
			}
			else
			{
        		Console.WriteLine($"[{nameof(SlideshowAdapter)}] Cannot start the slideshow as there is already a slideshow running.");
			}
        }
		
		/// <summary>
		/// Stops an ongoing slideshow service.
		/// </summary>
        public void Stop()
        {
        	if (IsRunning)
        	{
	        	UnsplashAdapter.OnErrorThrown -= RaiseOnErrorThrown;
	        	
	            IsRunning = false;
        	}
        	else
        	{
        		Console.WriteLine($"[{nameof(SlideshowAdapter)}] Cannot stop slideshow as it is not currently running.");
        	}
        }
        
        /// <summary>
		/// Sets the behaviour of this slideshow controller. The value used is any value in <see cref="Constants.Slideshow"/>.
		/// </summary>
        public void SetSlideshowCategory(SlideshowCategory category)
        {
        	CurrentCategory = category;
        	OnCategoryChanged(CurrentCategory.DisplayName);
        	
        	Console.WriteLine("Set slideshow category : {0}", category.DisplayName);
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
#if DEBUG
			var customSize = UnsplashImage.UnsplashImageSizes.Default;
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
        	
        	// Cache the current category for comparison to check that the current category has not changed.
            var category = CurrentCategory;
            bool HasCategoryChanged()
            {
            	return category != CurrentCategory;
            }
        	
        	UnsplashImage unsplashImage;
        	
            do
            {
Start:
            	// Pull new list of images from server if we are starting a new slideshow or if we have displayed all.
            	if (firstRun || (m_CurrentImageIndex == CurrentList.Count - 1))
            	{
		            m_CurrentImageIndex = 0;
        			CurrentList = await RequestImagesAsync();        			
            	}
            	else
				{
					m_CurrentImageIndex++;
				}
				
				if (HasCategoryChanged()) { goto Finished; } // Break out incase of category change
            	
            	m_Timer.Start();
					
				if (CurrentList != null && CurrentList.Count != 0)
				{
					unsplashImage = CurrentList[m_CurrentImageIndex];

                    // If we have chosen an item that is in the blocked lists, return to start to increment to the next image.
                    if (SettingsAdapter.IsBlockedAuthor(unsplashImage.user?.id) || SettingsAdapter.IsBlockedPhoto(unsplashImage.id))
                    {
                        goto Start;
                    }

					unsplashImage.custom.imageData = await UnsplashAdapter.DownloadPhotoAsync(unsplashImage, customSize);
					
					if (unsplashImage.custom.imageData != null)
					{
						// For the start we want to display the image as soon as it is downloaded.
	                	// Thereafter, we will download the next image but wait for the cycle timeout before displaying it.
	                	if (firstRun)
	                	{
		                	UpdateImage(unsplashImage);
		                	firstRun = false;
	                	}
	                	else
	                	{
	                		while (CycleDuration + TransitionDuration - m_Timer.GetElapsedTime() > 0)
	                		{	
								if (HasCategoryChanged()) { goto Finished; } // Break out of inner loop incase of category change
	                			await Task.Delay(100);
	                		}
	                	
		                	UpdateImage(unsplashImage);
	                	}
					}
					else
					{
                		await Task.Delay(RetryTimeout);
					}
                }
                else
                {
                	await Task.Delay(RetryTimeout);
                }
            }
            while (IsRunning);
            
Finished:
            // If we have broken out due to category change, handle this appropriately.
            if (HasCategoryChanged()) 
            {
            	HandleCategoryChanged();
            }
        }
        
         /// <summary>
        /// Requests a list of images based on the current slideshow mode.
        /// </summary>
        async Task<List<UnsplashImage>> RequestImagesAsync()
        {
        	var category = CurrentCategory;
        	if (category == null)
        	{
        		Console.WriteLine($"[{nameof(SlideshowAdapter)}] No category has been assigned. Defaulting to Random.");
        		category = DefaultCategory;
        	}
        	
        	switch (category.SlideshowMode)
        	{
        		case SlideshowModes.Random:
        			return await UnsplashAdapter.Instance.GetRandomListAsync();
        		case SlideshowModes.Query:
        			return await UnsplashAdapter.Instance.GetRandomListAsync(CurrentCategory.QueryString);
        			
    			default:
    				throw new NotImplementedException($"[{nameof(SlideshowAdapter)}] There is currently no support for the selected slideshow mode: {CurrentCategory.SlideshowMode}.");
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
        		Console.WriteLine($"[{nameof(SlideshowAdapter)}] No category has been assigned. Defaulting to Random.");
        		category = DefaultCategory;
        	}
        	
        	switch (category.SlideshowMode)
        	{
        		case SlideshowModes.Random:
        			return await UnsplashAdapter.Instance.GetRandomPhotoAsync();
        		case SlideshowModes.Query:
        			return await UnsplashAdapter.Instance.GetRandomPhotoAsync(CurrentCategory.QueryString);
        			
    			default:
    				throw new NotImplementedException($"[{nameof(SlideshowAdapter)}] There is currently no support for the selected slideshow mode: {CurrentCategory.SlideshowMode}.");
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
        static SlideshowAdapter GetAdapterInstance()
        {
            if (m_Instance == null)
            {
                m_Instance = new SlideshowAdapter();
            }

            return m_Instance;
        }

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
