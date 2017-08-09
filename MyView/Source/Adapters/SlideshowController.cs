using System;
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
            while (Running)
            {
                OnImageCycled(null);
                await Task.Delay(CycleTime);
                OnImageCycled(null);
                await Task.Delay(CycleTime);
            }
        }
        #endregion
    }
}
