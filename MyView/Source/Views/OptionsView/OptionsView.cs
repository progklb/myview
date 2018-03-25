using System;

using MyView.Adapters;
using MyView.Additional;

namespace MyView.Views
{
    /// <summary>
    /// Provides various options for the user to adjust app behaviour.
    /// </summary>
    public partial class OptionsView : AdvancedBaseView
    {
        #region CONSTRUCTOR
        public OptionsView(IntPtr handle) : base(handle) { }
        #endregion


        #region INHERITED METHODS
        public override void AwakeFromNib()
        {
            base.AwakeFromNib();

            IntialiseUI();
        }

        public override void AnimateIn()
        {
            base.AnimateIn();

            UISegCycle.ValueChanged += OnDisplayDurationChanged;
            UISegTransition.ValueChanged += OnTransitionDurationChanged;
            UISegLocation.ValueChanged += OnShowLocationChanged;
            UISegAuthor.ValueChanged += OnShowAuthorChanged;

            UIButtonBlockPhoto.PrimaryActionTriggered += OnBlockPhoto;
            UIButtonBlockAuthor.PrimaryActionTriggered += OnBlockAuthor;
        }

        public override void AnimateOut()
        {
            base.AnimateOut();

            UISegCycle.ValueChanged -= OnDisplayDurationChanged;
            UISegTransition.ValueChanged -= OnTransitionDurationChanged;
            UISegLocation.ValueChanged -= OnShowLocationChanged;
            UISegAuthor.ValueChanged -= OnShowAuthorChanged;

            UIButtonBlockPhoto.PrimaryActionTriggered -= OnBlockPhoto;
            UIButtonBlockAuthor.PrimaryActionTriggered -= OnBlockAuthor;
        }
        #endregion


        #region EVENT HANDLERS - OPTIONS
        void OnDisplayDurationChanged(object sender, EventArgs e)
        {
            SettingsAdapter.SlideshowCycleDurationMode = (DurationModes)(int)UISegCycle.SelectedSegment;
            SettingsAdapter.SaveSettings();
        }

        void OnTransitionDurationChanged(object sender, EventArgs e)
        {
            SettingsAdapter.SlideshowTransitionDurationMode = (DurationModes)(int)UISegTransition.SelectedSegment;
            SettingsAdapter.SaveSettings();
        }

        void OnShowAuthorChanged(object sender, EventArgs e)
        {
            SettingsAdapter.AuthorVisibility = (AuthorVisibilityMode)(int)UISegAuthor.SelectedSegment;
            SettingsAdapter.SaveSettings();
        }

        void OnShowLocationChanged(object sender, EventArgs e)
        {
            SettingsAdapter.LocationVisibility = (LocationVisibilityMode)(int)UISegLocation.SelectedSegment;
            SettingsAdapter.SaveSettings();
        }

        void OnBlockPhoto(object sender, EventArgs e)
        {
            SettingsAdapter.BlockPhoto(SlideshowAdapter.Instance.CurrentImage?.id);
            SettingsAdapter.SaveSettings();
        }

        void OnBlockAuthor(object sender, EventArgs e)
        {
            SettingsAdapter.BlockAuthor(SlideshowAdapter.Instance.CurrentImage?.user.id);
            SettingsAdapter.SaveSettings();
        }
        #endregion


        #region HELPER FUNCTIONS
        void IntialiseUI()
        {
            UISegCycle.SelectedSegment = (int)SettingsAdapter.SlideshowCycleDurationMode;
            UISegTransition.SelectedSegment = (int)SettingsAdapter.SlideshowTransitionDurationMode;

            UISegAuthor.SelectedSegment = (int)SettingsAdapter.AuthorVisibility;
            UISegLocation.SelectedSegment = (int)SettingsAdapter.LocationVisibility;
        }
        #endregion
    }
}