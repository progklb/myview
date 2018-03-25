// WARNING
//
// This file has been generated automatically by Visual Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace MyView.Views
{
    [Register ("OptionsView")]
    partial class OptionsView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton UIButtonBlockPhoto { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIButton UIButtonBlockUser { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISegmentedControl UISegAuthor { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISegmentedControl UISegCycle { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISegmentedControl UISegLocation { get; set; }

        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UISegmentedControl UISegTransition { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (UIButtonBlockPhoto != null) {
                UIButtonBlockPhoto.Dispose ();
                UIButtonBlockPhoto = null;
            }

            if (UIButtonBlockUser != null) {
                UIButtonBlockUser.Dispose ();
                UIButtonBlockUser = null;
            }

            if (UISegAuthor != null) {
                UISegAuthor.Dispose ();
                UISegAuthor = null;
            }

            if (UISegCycle != null) {
                UISegCycle.Dispose ();
                UISegCycle = null;
            }

            if (UISegLocation != null) {
                UISegLocation.Dispose ();
                UISegLocation = null;
            }

            if (UISegTransition != null) {
                UISegTransition.Dispose ();
                UISegTransition = null;
            }
        }
    }
}