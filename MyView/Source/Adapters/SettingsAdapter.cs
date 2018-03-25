using System.Collections.Generic;

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

        private const string BLOCKED_AUTHORS_KEY = "BlockedAuthors";
        private const string BLOCKED_PHOTOS_KEY = "BlockedPhotos";
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

        /// A list of authors that have been blocked by the user.
        private static List<string> BlockedAuthors { get; set; } = new List<string>();
        /// A list of photos that have been blocked by the user.
        private static List<string> BlockedPhotos { get; set; } = new List<string>();
        #endregion


        #region PUBLIC API - SETTINGS
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

            NSUserDefaults.StandardUserDefaults[BLOCKED_AUTHORS_KEY] = NSArray.FromStrings(BlockedAuthors.ToArray());
            NSUserDefaults.StandardUserDefaults[BLOCKED_PHOTOS_KEY] = NSArray.FromStrings(BlockedPhotos.ToArray());
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

                BlockedAuthors = GetList(NSUserDefaults.StandardUserDefaults.StringArrayForKey(BLOCKED_AUTHORS_KEY));
                BlockedPhotos = GetList(NSUserDefaults.StandardUserDefaults.StringArrayForKey(BLOCKED_PHOTOS_KEY));

                System.Console.WriteLine("Blocked photo list: {0}", BlockedPhotos.Count);
                foreach (var id in BlockedPhotos)
                {
                    System.Console.WriteLine("      - " + id);
                }

                System.Console.WriteLine("Blocked author list: {0}", BlockedAuthors.Count);
                foreach (var id in BlockedAuthors)
                {
                    System.Console.WriteLine("      - " + id);
                }
            }
        }
        #endregion


        #region PUBLIC API - BLOCKING
        /// <summary>
        /// Adds the provided author ID to the list of blocked authors.
        /// In order to persist this blocking, <see cref="SaveSettings"/> must be called afterwards.
        /// </summary>
        /// <param name="id">ID of author.</param>
        public static void BlockAuthor(string id)
        {
            if (id != null && !IsBlockedAuthor(id))
            {
                BlockedAuthors.Add(id);
            }
        }

        /// <summary>
        /// Adds the provided photo ID to the list of blocked photos.
        /// In order to persist this blocking, <see cref="SaveSettings"/> must be called afterwards.
        /// </summary>
        /// <param name="id">ID of photo.</param>
        public static void BlockPhoto(string id)
        {
            if (id != null && !IsBlockedPhoto(id))
            {
                BlockedPhotos.Add(id);
            }
        }

        /// <summary>
        /// Checks whether the provided author ID is in the list of blocked authors.
        /// </summary>
        /// <returns><c>true</c>, if the author's ID is in the blocked list, <c>false</c> otherwise.</returns>
        /// <param name="id">ID of author.</param>
        public static bool IsBlockedAuthor(string id)
        {
            return id != null && BlockedAuthors.Contains(id);
        }

        /// <summary>
        /// Checks whether the provided photo ID is in the list of blocked photos.
        /// </summary>
        /// <returns><c>true</c>, if the photo's ID is in the blocked list, <c>false</c> otherwise.</returns>
        /// <param name="id">ID of photo.</param>
        public static bool IsBlockedPhoto(string id)
        {
            return id != null && BlockedPhotos.Contains(id);
        }
        #endregion


        #region HELPER FUNCTIONS
        static List<string> GetList(string[] array)
        {
            return array != null && array.Length > 0 ? new List<string>(array) : new List<string>();
        }
        #endregion
    }
}
