// WARNING
//
// This file has been generated automatically by Xamarin Studio from the outlets and
// actions declared in your storyboard file.
// Manual changes to this file will not be maintained.
//
using Foundation;
using System;
using System.CodeDom.Compiler;
using UIKit;

namespace MyView.Views
{
    [Register ("LogoView")]
    partial class LogoView
    {
        [Outlet]
        [GeneratedCode ("iOS Designer", "1.0")]
        UIKit.UIImageView UIImageLogo { get; set; }

        void ReleaseDesignerOutlets ()
        {
            if (UIImageLogo != null) {
                UIImageLogo.Dispose ();
                UIImageLogo = null;
            }
        }
    }
}