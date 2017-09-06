using UIKit;

namespace MyView
{
	/// <summary>
	/// Allows us to set a focus override for any focus environment (<see cref="UIViewController"/> , <see cref="UIView"/>, or any derived types).
	/// This is to be implemented in an overriden <see cref="UIView.PreferredFocusedView"/> property of a view.
	/// </summary>
	public static class FocusTarget
	{
		#region PROPERTIES
		/// Reference to a view that should be focused upon.
		public static UIView TargetView { get; private set; }
		#endregion
		
		
		#region PUBLIC API
		/// <summary>
		/// Assigns a preferred view to this instance.
		/// </summary>
		/// <param name="target">The view we wish to focus on.</param>
		public static void SetFocusTarget(UIView target)
		{
			TargetView = target;
		}
		
		/// <summary>
		/// Unassigns the preferred view from this instance.
		/// </summary>
		public static void UnsetFocusTarget()
		{
			TargetView = null;
		}
		#endregion
	}
}
