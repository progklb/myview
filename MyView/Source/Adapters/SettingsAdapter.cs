using System;

using Foundation;

using MyView.Additional;

namespace MyView.Adapters
{
    /// <summary>
    /// Handles user settings.
    /// </summary>
    public sealed class SettingsAdapter
    {
        #region CONSTANTS
        private const string CUSTOM_SETTINGS = "CustomSettings";

        private const string CYCLE_TIME_KEY = "CycleTime";
        private const string TRANS_DURATION_KEY = "TransDuration";

        private const string AUTHOR_VISIBLITY_KEY = "AuthorVisibility";
        private const string LOCATION_VISIBLITY_KEY = "LocationVisibility";
        #endregion


        #region PROPERTIES
        /// The current duration mode for image cycling.
        public static DurationModes SlideshowCycleDurationMode { get; set; } = DurationModes.Short;
        /// The current duration mode for transitions.
        public static DurationModes SlideshowTransitionDurationMode { get; set; } = DurationModes.Medium;

        /// Whether and how we should display the location text.
        public static AuthorVisibilityMode AuthorVisibility { get; set; } = AuthorVisibilityMode.Selected;
        /// How we should display the author text.
        public static LocationVisibilityMode LocationVisibility { get; set; } = LocationVisibilityMode.Selected;
        #endregion


        #region PUBLIC API
        /// <summary>
        /// Saves the current settings to user preferences.
        /// </summary>
        public static void SaveSettings()
        {
            // Set a settings flag so that we know we have custom settings that should be loaded.
            NSUserDefaults.StandardUserDefaults.SetBool(true, CUSTOM_SETTINGS);

            NSUserDefaults.StandardUserDefaults.SetInt((int)SlideshowCycleDurationMode, CYCLE_TIME_KEY);
            NSUserDefaults.StandardUserDefaults.SetInt((int)SlideshowTransitionDurationMode, TRANS_DURATION_KEY);

            NSUserDefaults.StandardUserDefaults.SetInt((int)AuthorVisibility, AUTHOR_VISIBLITY_KEY);
            NSUserDefaults.StandardUserDefaults.SetInt((int)LocationVisibility, LOCATION_VISIBLITY_KEY);
        }

        /// <summary>
        /// Loads saved user preferences.
        /// </summary>
        public static void LoadSettings()
        {
            // Only load settings if we have custom settings saved. 
            // Otherwise perform no action so that we leave the default values.
            if (NSUserDefaults.StandardUserDefaults.BoolForKey(CUSTOM_SETTINGS))
            {
                SlideshowCycleDurationMode = (DurationModes)(int)NSUserDefaults.StandardUserDefaults.IntForKey(CYCLE_TIME_KEY);
                SlideshowTransitionDurationMode = (DurationModes)(int)NSUserDefaults.StandardUserDefaults.IntForKey(TRANS_DURATION_KEY);

                AuthorVisibility = (AuthorVisibilityMode)(int)NSUserDefaults.StandardUserDefaults.IntForKey(AUTHOR_VISIBLITY_KEY);
                LocationVisibility = (LocationVisibilityMode)(int)NSUserDefaults.StandardUserDefaults.IntForKey(LOCATION_VISIBLITY_KEY);
            }
        }
        #endregion
    }
}
