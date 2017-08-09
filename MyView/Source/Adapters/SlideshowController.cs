using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyView
{
    /// <summary>
    /// Provides images at fixed intervals and defines parameters for display.
    /// </summary>
    class SlideshowController
    {
        #region EVENTS
        public event Action<UnsplashImage> OnImageCycled = delegate { };
        #endregion


        #region PROPERTIES
        public bool Running;
        public int CycleTime { get; set; } = 3000;
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

            Random rng = new Random();
            
            while (Running)
            {
                var unsplashImage = await UnsplashAdapter.Instance.GetRandomPhotoAsync();
                unsplashImage.imageData = await UnsplashAdapter.DownloadPhotoAsync(        imageURLS[rng.Next(0, imageURLS.Length-1)]     , UnsplashAdapter.UriMode.PhotoID);
                
                //TODO Push into history
                
                OnImageCycled(unsplashImage);
                
                await Task.Delay(CycleTime);
            }
        }
        #endregion
    }
}
