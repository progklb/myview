using System;
using System.Threading.Tasks;

using Foundation;
using UIKit;

using MyView.Adapters;
using MyView.Additional;
using MyView.Views;

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
		#endregion
		
		
		#region PROPERTIES
		/// The current mode that we are viewing the app in.
		public ApplicationModes CurrentMode { get; private set; }
		
		/// The duration of transitions between displaying images.
        public double TransitionDuration { get; set; } = 2;
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
		#endregion


		#region CONSTRUCTOR
		public MainViewController(IntPtr handle) : base(handle) { }
		#endregion


		#region INHERITED METHODS
		public override void ViewDidLoad()
		{
			base.ViewDidLoad();
			
			InitialiseInterface();
			ShowSelectionInterface(true);

			m_Slideshow = new SlideshowController();
		}
		
		public override void ViewWillAppear(bool animated)
		{
			base.ViewDidAppear(animated);
			
			// Start image provider
            m_Slideshow.Start();
            m_Slideshow.OnImageCycled += SetBackground;
            
            UnsplashAdapter.OnErrorThrown += ShowAlert;
		}
		
		public override void ViewWillDisappear(bool animated)
		{
			base.ViewWillDisappear(animated);
			
			m_Slideshow.Stop();
		}
		#endregion


		#region USER INTERFACE
		/// <summary>
		/// Initialises the interface for the first time.
		/// </summary>
		void InitialiseInterface()
		{			
			m_Select = BaseView.CreateView<CategorySelectView>(this.View);
			m_Header = BaseView.CreateView<HeaderView>(this.View);
			m_Footer = BaseView.CreateView<FooterView>(this.View);
			
			m_ImageViews = new UIImageView[] { UIImageBackground1, UIImageBackground2 };
			
			UIImageBackground1.Alpha = 0f;
			UIImageBackground2.Alpha = 0f;
		}

		/// <summary>
		/// Shows or hides the category selection interface.
		/// </summary>
		/// <param name="show">If set to <c>true</c> show.</param>
		void ShowSelectionInterface(bool show)
		{
			if (show)
			{
				m_Header.ShowBackingGradient = false;
				m_Header.AnimateIn();
				m_Select.AnimateIn();
			}
			else
			{
				m_Header.AnimateOut();
				m_Select.AnimateOut();
			}
		}

		/// <summary>
		/// Shows or hides the image view interface.
		/// </summary>
		/// <param name="show">If set to <c>true</c> show.</param>
		void ShowImageInterface(bool show)
		{
			if (show)
			{
				m_Header.ShowBackingGradient = true;
				m_Header.AnimateIn();
				m_Footer.AnimateIn();
			}
			else
			{
				m_Header.AnimateOut();
				m_Footer.AnimateOut();
			}
		}

		/// <summary>
		/// Sets the provided image as the displayed image. This intiates a transition from the current image to the new image.
		/// </summary>
		/// <param name="image">Image to display.</param>
		void SetBackground(UnsplashImage image)
		{	
			// Fade out old view
			UIView.Animate(
				TransitionDuration, 
				() => { m_ImageViews[m_CurrentImageIdx].Alpha = 0f; }
			);
			
			// Increment to next view
            m_CurrentImageIdx = (++m_CurrentImageIdx % m_ImageViews.Length);
			
			// Fade in new view
			UIView.Animate(
				TransitionDuration, 
				() => { m_ImageViews[m_CurrentImageIdx].Alpha = 1f; }
			);
			
			// Assign new image
			var data = NSData.FromArray(image.custom.imageData);
			m_ImageViews[m_CurrentImageIdx].Image = UIImage.LoadFromData(data);
		}
		
		/// <summary>
		/// Presents an alert to the user, indicating that something went wrong.
		/// </summary>
		/// <param name="message">Message.</param>
		void ShowAlert(string message)
		{
			var alert = UIAlertController.Create(ALERT_HEADER, message, UIAlertControllerStyle.Alert);
			PresentViewController(alert, true, completionHandler: null);
		}
		#endregion
		
		
		#region APPLICATION MODE
		/// <summary>
		/// Sets the viewing mode of the app.
		/// </summary>
		/// <param name="mode">Mode.</param>
		public void SetMode(ApplicationModes mode)
		{
			// Sanity check to ensure that our transition makes sense.
			if 	(CurrentMode == mode || 
				(CurrentMode == ApplicationModes.CategorySelect && mode == ApplicationModes.ImageDetails) ||
				(CurrentMode == ApplicationModes.ImageDetails && mode == ApplicationModes.CategorySelect))
			{
				Console.WriteLine($"Illegal mode selection. Cannot transition directly from {CurrentMode} to {mode}");
				return;
			}
			
			var oldMode = mode;
			CurrentMode = mode;
			
			switch (CurrentMode)
			{
				case ApplicationModes.CategorySelect:
					ShowSelectionInterface(false);
					break;
			}
		}
		#endregion
	}
}