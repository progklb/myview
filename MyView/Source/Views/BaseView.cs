using System;

using Foundation;
using UIKit;
using CoreGraphics;
using CoreAnimation;
using ObjCRuntime;

namespace MyView
{
	/// <summary>
    /// Custom <see cref="UIView"/> base class that has convenient functionality.
	/// </summary>
	public class BaseView : UIView
	{
		#region PROPERTIES
		public nfloat FadeInDuration { get; set; } = 0.25f;
		public nfloat FadeOutDuration { get; set; } = 0.5f;
		#endregion
		
		
		#region CONSTRUCTOR
		public BaseView(IntPtr handle) : base(handle) { }
		#endregion


		#region PUBLIC API
		/// <summary>
        /// Creates a new view of the specified type.
        /// </summary>
        /// <returns>The view.</returns>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
		/// <param name="parentView">The view onto which to attach this view newly created view.</param>
		/// <param name="viewID">Optional view identifier, if this differs from the class name.</param>
		/// <param name="initiallyHidden">Optional flag to indicate that this view should be created and then immediately hidden.</param>
        public static T CreateView<T>(UIView parentView, string viewID = null, bool initiallyHidden = true) where T : BaseView
        {
        	var newView = Create<T>(viewID, initiallyHidden);
        	newView.SizeToView(parentView);
        	parentView.Add(newView);
        	
        	return newView;
        }
		
		/// <summary>
		/// Creates and returns the specified type of view.
		/// Note that the optional <paramref name="viewID"/> can be provided in order to explicitely specify the nib's unique identifier. 
		/// If nothing is provided, the class name of the provided type T will be used as the identifier.
		/// </summary>
		/// <returns>The create.</returns>
		/// <param name="viewID">Optional view identifier, if this differs from the class name.</param>
		/// <param name="initiallyHidden">Optional flag to indicate that this view should be created and then immediately hidden.</param>
		public static T Create<T>(string viewID = null, bool initiallyHidden = true) where T : UIView
		{
			if (viewID == null)
			{
				viewID = typeof(T).Name;
			}

			var array = NSBundle.MainBundle.LoadNib(viewID, null, null);
			var view = Runtime.GetNSObject<T>(array.ValueAt(0));
			view.Hidden = initiallyHidden;
			
			return view;
		}
		
		/// <summary>
		/// Resizes and positions this view to the size and position of the provided view.
		/// </summary>
		/// <param name="view">View to use for sizing and positioning.</param>
		public void SizeToView(UIView view)
        {
			Frame = new CGRect(view.Bounds.Location, new CGSize(view.Frame.Size));
        }
		#endregion
		
		
		#region PUBLIC API - TRANSITIONS
		/// <summary>
		/// Fades this view in from complete transparent to it's default <see cref="UIView.Alpha"/> value.
		/// </summary>
		public void AnimateIn()
		{
			Hidden = false;
			nfloat targetAlpha = 1f;
			Alpha = 0f;
			
			Animate(
				FadeInDuration,
				() => { Alpha = targetAlpha; }
			);
		}
		
		/// <summary>
		/// Fades this view out to completely transparent.
		/// </summary>
		public void AnimateOut()
		{
			Animate(
				FadeOutDuration,
				() => { Alpha = 0; },
				() => { Hidden = true; }
			);
		}
		#endregion
		
		
		#region PUBLIC API - HELPERS
		/// <summary>
		/// Inserts a vertical gradient layer on top of the provided view. The upper color is the <paramref name="startingColor"/>, 
		/// while the lower color is the <see cref="endingColor"/>
		/// </summary>
		/// <param name="view">View to apply gradient too.</param>
		/// <param name="startingColor">Starting color.</param>
		/// <param name="endingColor">Ending color.</param>
		public static void InsertGradient(UIView view, CGColor startingColor, CGColor endingColor)
		{
			var gradient = new CAGradientLayer();
			gradient.Frame = view.Bounds;
			gradient.NeedsDisplayOnBoundsChange = true;
			gradient.MasksToBounds = true;
			gradient.Colors = new CGColor[] { startingColor, endingColor };

			view.Layer.InsertSublayer(gradient, 0);
		}
		#endregion
	}
}
