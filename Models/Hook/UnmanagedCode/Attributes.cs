namespace MapleReaper.Models.Hook
{
    public enum Attributes : uint
    {
        DWMWA_NCRENDERING_ENABLED = 1,      // [get] Is non-client rendering enabled/disabled
        DWMWA_NCRENDERING_POLICY,           // [set] DWMNCRENDERINGPOLICY - Non-client rendering policy - Enable or disable non-client rendering
        DWMWA_TRANSITIONS_FORCEDISABLED,    // [set] Potentially enable/forcibly disable transitions
        DWMWA_ALLOW_NCPAINT,                // [set] Allow contents rendered In the non-client area To be visible On the DWM-drawn frame.
        DWMWA_CAPTION_BUTTON_BOUNDS,        // [get] Bounds Of the caption button area In window-relative space.
        DWMWA_NONCLIENT_RTL_LAYOUT,         // [set] Is non-client content RTL mirrored
        DWMWA_FORCE_ICONIC_REPRESENTATION,  // [set] Force this window To display iconic thumbnails.
        DWMWA_FLIP3D_POLICY,                // [set] Designates how Flip3D will treat the window.
        DWMWA_EXTENDED_FRAME_BOUNDS,        // [get] Gets the extended frame bounds rectangle In screen space
        DWMWA_HAS_ICONIC_BITMAP,            // [set] Indicates an available bitmap When there Is no better thumbnail representation.
        DWMWA_DISALLOW_PEEK,                // [set] Don't invoke Peek on the window.
        DWMWA_EXCLUDED_FROM_PEEK,           // [set] LivePreview exclusion information
        DWMWA_CLOAK,                        // [set] Cloak Or uncloak the window
        DWMWA_CLOAKED,                      // [get] Gets the cloaked state Of the window. Returns a DWMCLOACKEDREASON object
        DWMWA_FREEZE_REPRESENTATION,        // [set] BOOL, Force this window To freeze the thumbnail without live update
        PlaceHolder1,
        PlaceHolder2,
        PlaceHolder3,
        DWMWA_ACCENTPOLICY = 19
    }
}