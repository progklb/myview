using System;
using System.Threading.Tasks;

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
		private const int IMAGE_INTERFACE_TIMEOUT = 5000;
		#endregion
		
		
		#region PROPERTIES
		/// The current mode that we are viewing the app in.
		public ApplicationModes CurrentMode { get; private set; }
		#endregion
        
        
		#region VARIABLES
		// Additional UI elements
		private CategorySelectView m_Select;
		private HeaderView m_Header;
		private FooterView m_Footer;
		
		/// Provides images for display on this page
        private SlideshowController m_Slideshow;

		/// A cache of the two image views for displaying images to the user
		private UIImageView[] m_ImageViews;
		/// The index of the currently display image view.
		private int m_CurrentImageIdx;
		
		/// A reference to an active alert controller. This prevents multiple errors trying to create multiple controllers.
		private UIAlertController m_AlertController;
		#endregion


		#region CONSTRUCTOR
		public MainViewController(IntPtr handle) : base(handle) { }
		#endregion


		#region INHERITED METHODS
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();

			m_Slideshow = new SlideshowController();
						
			InitialiseInterface();
			InitialiseControls();
		}
		
		public override void ViewWillAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			
			// Start image provider
            m_Slideshow.Start();
            m_Slideshow.OnImageCycled += SetBackground;
            m_Slideshow.OnImageCycled += SetFooter;
            m_Slideshow.OnModeChanged += SetHeader;
            m_Slideshow.OnErrorThrown += ShowAlert;
            
			m_Slideshow.SetSlideshowMode(SlideshowController.SlideshowModes.Random);
		}
		
		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
			
			m_Slideshow.Stop();
		}
		#endregion
		
		
		#region SETUP
		/// <summary>
		/// Initialises the interface for the first time.
		/// </summary>
		void InitialiseInterface()
		{	
			CurrentMode = ApplicationModes.CategorySelect;
			
			m_Select = BaseView.CreateView<CategorySelectView>(this.View, null, false);
			m_Select.SetItemSelectedCallback(OnCategoryItemSelected);
			m_Select.Slideshow = m_Slideshow;
			
			m_Header = BaseView.CreateView<HeaderView>(this.View);
			m_Footer = BaseView.CreateView<FooterView>(this.View);
			
			m_ImageViews = new UIImageView[] { UIImageBackground1, UIImageBackground2 };
			
			UIImageBackground1.Alpha = 0f;
			UIImageBackground2.Alpha = 0f;
		}
		
		/// <summary>
		/// Initialises gesture recognizers for capturing input.
		/// </summary>
		void InitialiseControls()
		{	
			var menuRecognizer = new UITapGestureRecognizer(OnRemoteMenuClicked);
			menuRecognizer.AllowedPressTypes = new NSNumber[] { NSNumber.FromInt64((Int64)UIPressType.Menu) };
			View.AddGestureRecognizer(menuRecognizer);
			
			var selectRecognizer = new UITapGestureRecognizer(OnRemoteSelectClicked);
			selectRecognizer.AllowedPressTypes = new NSNumber[] { NSNumber.FromInt64((Int64)UIPressType.Select) };
			View.AddGestureRecognizer(selectRecognizer);
		}
		#endregion
		
		
		#region EVENT HANDLERS
		void OnRemoteMenuClicked()
		{
			var old = CurrentMode;
			
			if (CurrentMode == ApplicationModes.ImageView)
			{
				CurrentMode = ApplicationModes.CategorySelect;
				m_Footer.AnimateOut();
				m_Header.AnimateIn();
				m_Select.AnimateIn();
			}
			else if (CurrentMode == ApplicationModes.ImageDetails)
			{
				CurrentMode = ApplicationModes.CategorySelect;
				m_Select.AnimateIn();
				m_Footer.AnimateOut();
			}
			else if (CurrentMode == ApplicationModes.CategorySelect)
			{
				CurrentMode = ApplicationModes.ImageDetails;
				m_Select.AnimateOut();
				m_Footer.AnimateIn();
				// TODO Start timeout
			}
		}
		
		void OnRemoteSelectClicked()
		{
			var old = CurrentMode;
			
			if (CurrentMode == ApplicationModes.ImageView)
			{
				CurrentMode = ApplicationModes.ImageDetails;
				//m_Header.ShowBackingGradient = true;
				m_Header.AnimateIn();
				m_Footer.AnimateIn();
				// TODO Start timeout
			}
			else if (CurrentMode == ApplicationModes.ImageDetails)
			{
				CurrentMode = ApplicationModes.ImageView;
				m_Header.AnimateOut();
				m_Footer.AnimateDim();
				// TODO Stop timeout
			}
		}
		
		void OnCategoryItemSelected()
		{
			m_Select.AnimateOut();
			m_Footer.AnimateIn();
			// TODO Start timeout
			
			Console.WriteLine("CatItemSelected");
		}
		
		async Task StartImageInterfaceTimeout()
		{	
			await Task.Delay(IMAGE_INTERFACE_TIMEOUT);
			// TODO Check if still valid + Hide elements
		}
		#endregion


		#region USER INTERFACE
		/// <summary>
		/// Sets the provided image as the displayed image. This intiates a transition from the current image to the new image.
		/// </summary>
		/// <param name="image">Image to display.</param>
		void SetBackground(UnsplashImage image)
		{	
			// Fade out old view
			UIView.Animate(
				m_Slideshow.TransitionDuration / 1000f,
				() => { m_ImageViews[m_CurrentImageIdx].Alpha = 0f; }
			);
			
			// Increment to next view
            m_CurrentImageIdx = (++m_CurrentImageIdx % m_ImageViews.Length);
			
			// Fade in new view
			UIView.Animate(
				m_Slideshow.TransitionDuration / 1000f, 
				() => { m_ImageViews[m_CurrentImageIdx].Alpha = 1f; }
			);
			
			// Assign new image
			var data = NSData.FromArray(image.custom.imageData);
			m_ImageViews[m_CurrentImageIdx].Image = UIImage.LoadFromData(data);
		}
		
		/// <summary>
		/// Sets the items to display on the footer view according to the provided image.
		/// </summary>
		/// <param name="image">Image that should be represented.</param>
		void SetFooter(UnsplashImage image)
		{
			m_Footer.SetAuthorText(image.user.name, image.user.username);
		}
		
		/// <summary>
		/// Sets the items to display on the header view.
		/// </summary>
		/// <param name="categoryText">The text that should be displayed in the header.</param>
		void SetHeader(string categoryText)
		{
			m_Header.SetCategoryText(categoryText);
		}
		
		/// <summary>
		/// Presents an alert to the user, indicating that something went wrong.
		/// </summary>
		/// <param name="message">Alert message to display.</param>
		void ShowAlert(string message)
		{
			if (m_AlertController == null)
			{
				m_AlertController = UIAlertController.Create(ALERT_HEADER, message, UIAlertControllerStyle.Alert);
				PresentViewController(m_AlertController, true, completionHandler: () => { m_AlertController = null; });
			}
		}
		#endregion
	}
}