using System;

using Foundation;
using UIKit;

using MyView.Adapters;
using MyView.Additional;
using MyView.Views;
using MyView.Unsplash;

namespace MyView.Screens
{
    /// <summary>
    /// The main screen of the app.
    /// </summary>
    public partial class MainViewController : UIViewController
    {
        #region CONSTANTS
        /// The text displayed as the header of the alert screen.
        private const string ALERT_HEADER = "Oh no!";
        /// The amount of time in milliseconds before automatically hiding the image interface.
        private const int IMAGE_INTERFACE_TIMEOUT = 3000;
        /// The amount of time in milliseconds before automatically hiding the alert message.
        private const int TEMPORARY_ALERT_TIMEOUT = 3000;
        #endregion


        #region PROPERTIES
        /// The current mode that we are viewing the app in.
        public ApplicationModes CurrentMode { get; private set; }

        /// Provides images for display on this page
        private SlideshowAdapter Slideshow { get => SlideshowAdapter.Instance; }

        /// Returns an explicit focus target if one has been set. To trigger a focus on this target, first set it and then call <see cref="UpdateFocusIfNeeded"/>
        public override UIView PreferredFocusedView { get { return FocusTarget.TargetView ?? base.PreferredFocusedView; } }

        /// The enabled state of the gesture recognizer for the remote's select button.
        private bool SelectRecognizerEnabled
        {
            get { return View.GestureRecognizers[1].Enabled; }
            set { View.GestureRecognizers[1].Enabled = value; }
        }
        #endregion


        #region VARIABLES
        // Additional UI elements
        private CategorySelectView m_Select;
        private HeaderView m_Header;
        private FooterView m_Footer;
        private AlertView m_Alert;
        private LogoView m_Logo;
        private OptionsView m_Options;

        /// Keeps track of our last selected category.
        private SlideshowCategory m_LastSelectedCategory;

        /// A cache of the two image views for displaying images to the user
        private UIImageView[] m_ImageViews;
        /// The index of the currently display image view.
        private int m_CurrentImageIdx;
        #endregion


        #region CONSTRUCTOR
        public MainViewController(IntPtr handle) : base(handle) { }
        #endregion


        #region INHERITED METHODS
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            SettingsAdapter.LoadSettings();

            InitialiseInterface();
            InitialiseControls();
            InitialiseSlideshow();

            CurrentMode = ApplicationModes.ImageView;
            SelectRecognizerEnabled = true;
            m_Footer.AnimateDim();
        }

        public override void ViewWillAppear(bool animated)
        {
            base.ViewDidAppear(animated);

            // Start image provider
            Slideshow.Start();

            // Set up the UI to correspond to our current state
            m_LastSelectedCategory = Slideshow.CurrentCategory;
            SetHeader(Slideshow.CurrentCategory);

            SetBackground(new UnsplashImage(UIImage.FromFile(Constants.Images.StartUpPhoto).AsJPEG().ToArray()));
        }

        public override void ViewWillDisappear(bool animated)
        {
            base.ViewWillDisappear(animated);

            Slideshow.Stop();
        }
        #endregion


        #region SETUP
        /// <summary>
        /// Initialises the interface for the first time.
        /// </summary>
        void InitialiseInterface()
        {
            m_Select = BaseView.CreateView<CategorySelectView>(this.View);
            m_Select.AddItemSelectedCallback(OnCategoryItemSelected);
            m_Select.AddItemFocusedCallback(OnCategoryItemFocused);

            m_Header = BaseView.CreateView<HeaderView>(this.View);
            m_Header.ShowBackingGradient = true;

            m_Footer = BaseView.CreateView<FooterView>(this.View);
            m_Logo = BaseView.CreateView<LogoView>(this.View, null, false);

            m_ImageViews = new UIImageView[] { UIImageBackground1, UIImageBackground2 };

            UIImageBackground1.Alpha = 0f;
            UIImageBackground2.Alpha = 0f;
        }

        /// <summary>
        /// Initialises gesture recognizers for capturing input.
        /// </summary>
        void InitialiseControls()
        {
            // NOTE Changing the order that these recognizers are added to the array requires updating the SelectRecognizerEnabled property.

            var menuRecognizer = new UITapGestureRecognizer(OnRemoteMenuClicked);
            menuRecognizer.AllowedPressTypes = new NSNumber[] { NSNumber.FromInt64((Int64)UIPressType.Menu) };
            View.AddGestureRecognizer(menuRecognizer);

            var selectRecognizer = new UITapGestureRecognizer(OnRemoteSelectClicked);
            selectRecognizer.AllowedPressTypes = new NSNumber[] { NSNumber.FromInt64((Int64)UIPressType.Select) };
            View.AddGestureRecognizer(selectRecognizer);

            var touchRecognizer = new UITapGestureRecognizer(OnRemoteTouched);
            touchRecognizer.AllowedPressTypes = new NSNumber[] { };
            touchRecognizer.AllowedTouchTypes = new NSNumber[] { NSNumber.FromInt64((Int64)UITouchType.Indirect) };
            View.AddGestureRecognizer(touchRecognizer);
        }

        /// <summary>
        /// Initialises the slideshow adapter and subscribes this instance to its events.
        /// </summary>
        void InitialiseSlideshow()
        {
            Slideshow.OnImageCycled += SetBackground;
            Slideshow.OnImageCycled += SetFooter;

            // Display alert on error. Successful downloads hide the alert.
            Slideshow.OnErrorThrown += message => { ShowAlert(message); };
            Slideshow.OnImageCycled += HideAlert;

            // Start up logic
            Slideshow.OnImageCycled += HideStartUpLogo;

            Slideshow.SetSlideshowCategory(Slideshow.DefaultCategory);
        }
        #endregion


        #region INPUT EVENT HANDLERS
        void OnRemoteMenuClicked()
        {
            switch (CurrentMode)
            {
                case ApplicationModes.ImageView:
                case ApplicationModes.ImageDetails:
                    ShowOptions();
                    break;

                case ApplicationModes.Options:
                    HideOptions();
                    break;

                case ApplicationModes.CategorySelect:
                    // Close menu, setting our original category.
                    OnCategoryItemSelected(m_LastSelectedCategory);
                    break;
            }
        }

        void OnRemoteSelectClicked()
        {
            switch (CurrentMode)
            {
                case ApplicationModes.ImageView:
                case ApplicationModes.ImageDetails:
                    ShowCategorySelect();
                    break;
            }
        }

        void OnRemoteTouched()
        {
            switch (CurrentMode)
            {
                case ApplicationModes.ImageView:
                    ShowImageDetails();
                    break;

                case ApplicationModes.ImageDetails:
                    ShowImageView();
                    break;
            }
        }
        #endregion



        #region USER INTERFACE - STATES

        // NOTE We disable the "select" recognizer when displaying any interactive overlays as this
        // recognizer will steal "select" input from any overlay view that we are interacting with.

        void ShowOptions()
        {
            CurrentMode = ApplicationModes.Options;
            SelectRecognizerEnabled = false;

            m_Options = BaseView.CreateView<OptionsView>(this.View);
            m_Options.OnPhotoBlocked += id => { ShowAlert("You will never see this photo again", "Photo blocked", TEMPORARY_ALERT_TIMEOUT); };
            m_Options.OnAuthorBlocked += id => { ShowAlert("You will no longer see photos from this author", "Author blocked", TEMPORARY_ALERT_TIMEOUT); };

            m_Options.AnimateIn();
            m_Header.AnimateOut();
            m_Footer.AnimateOut();
        }

        void HideOptions()
        {
            m_Options.AnimateOutAndRemove();
            m_Options = null;

            m_Header.AnimateIn();
            ShowImageDetails();
        }

        void ShowCategorySelect()
        {
            CurrentMode = ApplicationModes.CategorySelect;
            SelectRecognizerEnabled = false;

            SetHeader(null);
            m_Select.AnimateIn();
            m_Header.AnimateIn();
            m_Footer.AnimateOut();
        }

        void ShowImageView()
        {
            CurrentMode = ApplicationModes.ImageView;
            SelectRecognizerEnabled = true;

            m_Header.AnimateOut();
            m_Footer.AnimateDim();
        }

        void ShowImageDetails()
        {
            CurrentMode = ApplicationModes.ImageDetails;
            SelectRecognizerEnabled = true;

            m_Header.AnimateIn();
            m_Footer.AnimateIn();

            // After the timeout, the footer hides and we switch back to the ImageView state.
            m_Footer.StartDimTimeout(() => {
                // Ensure that we are still in the ImageDetails state.
                if (CurrentMode == ApplicationModes.ImageDetails)
                {
                    CurrentMode = ApplicationModes.ImageView;
                    m_Header.AnimateOut();
                }
            });
        }

        void OnCategoryItemSelected(SlideshowCategory category)
        {
            Slideshow.SetSlideshowCategory(category);
            m_LastSelectedCategory = category;

            SetHeader(Slideshow.CurrentCategory);

            m_Select.AnimateOut();
            ShowImageDetails();
        }

        void OnCategoryItemFocused(SlideshowCategory category)
        {
            Slideshow.SetSlideshowCategory(category);
        }
        #endregion


        #region USER INTERFACE - SECONDARY
        /// <summary>
        /// Presents an alert to the user, indicating that something went wrong.
        /// </summary>
        /// <param name="message">Alert message to display.</param>
        void ShowAlert(string message, string title = ALERT_HEADER, int timeout = -1)
        {
            if (m_Alert == null)
            {
                m_Alert = BaseView.CreateView<AlertView>(this.View);
                m_Alert.AnimateIn();
            }

            m_Alert.SetText(title, message);

            if (timeout > 0)
            {
                m_Alert.AnimateOutAndRemove(timeout, () => m_Alert = null);
            }
        }

        /// <summary>
        /// Removes the alert from view and destroys it.
        /// </summary>
        /// <param name="image">Image.</param>
        void HideAlert(UnsplashImage image)
        {
            if (m_Alert != null)
            {
                m_Alert.AnimateOutAndRemove();
                m_Alert = null;
            }
        }

        /// <summary>
        /// Animates out the starting logo.
        /// </summary>
        void HideStartUpLogo(UnsplashImage image)
        {
            Slideshow.OnImageCycled -= HideStartUpLogo;

            m_Logo.AnimateOutAndRemove();

            // Only tell the header to display if we are in the correct mode to do so.
            if (CurrentMode == ApplicationModes.CategorySelect || CurrentMode == ApplicationModes.ImageDetails)
            {
                m_Header.AnimateIn();
            }
        }
        #endregion


        #region USER INTERFACE - HELPERS
        /// <summary>
        /// Sets the provided image as the displayed image. This intiates a transition from the current image to the new image.
        /// </summary>
        /// <param name="image">Image to display.</param>
        void SetBackground(UnsplashImage image)
        {
            // Fade out old view
            UIView.Animate(
                Slideshow.TransitionDuration / 1000f,
                () => { m_ImageViews[m_CurrentImageIdx].Alpha = 0f; }
            );

            // Increment to next view
            m_CurrentImageIdx = (++m_CurrentImageIdx % m_ImageViews.Length);

            // Fade in new view
            UIView.Animate(
                Slideshow.TransitionDuration / 1000f,
                () => { m_ImageViews[m_CurrentImageIdx].Alpha = 1f; }
            );

            // Assign new image
            var data = NSData.FromArray(image.custom.imageData);
            m_ImageViews[m_CurrentImageIdx].Image = UIImage.LoadFromData(data);
        }
        
        /// <summary>
        /// Sets the items to display on the header view.
        /// </summary>
        /// <param name="category">The category that has been selected.</param>
        void SetHeader(SlideshowCategory category = null)
        {
            m_Header.SetCategoryText(category?.DisplayName);
        }

        /// <summary>
        /// Sets the items to display on the footer view according to the provided image.
        /// </summary>
        /// <param name="image">Image that should be represented.</param>
        void SetFooter(UnsplashImage image)
        {
            m_Footer.SetAuthorText(image.user.name, image.user.username);
            m_Footer.SetLocationText(image.location.city, image.location.country);
        }
        #endregion
    }
}