﻿using Extensionator;
using Godot;
using System.Runtime.InteropServices;

namespace GodotExtensionator {
    public static class OSExtension {

        /// <summary>
        /// Attempts to open a specified URL in the default web browser or platform-specific way.
        /// </summary>
        /// <param name="url">The URL to open (must be a valid URL string).</param>
        /// <remarks>
        /// This function opens the provided URL in the system's default web browser or uses a platform-specific method for handling external links. It first checks if the provided URL is valid using the `IsValidUrl` method. If valid, it performs platform-specific processing:
        ///   - Web platform: encodes the URL using `URIEncode` for proper handling.
        /// It then uses the `OS.ShellOpen` method to open the URL in the appropriate application.
        /// </remarks>
        public static void OpenExternalLink(string url) {
            if (url.IsValidUrl()) {
                if (OS.GetName().EqualsIgnoreCase("Web"))
                    url = url.URIEncode();

                OS.ShellOpen(url);
            }
        }

        /// <summary>
        /// Generates a unique identifier string based on the current Unix time and a random component.
        /// </summary>
        /// <returns>A string representing the generated unique identifier.</returns>
        /// <remarks>
        /// This function combines the current Unix time obtained from `Time.GetUnixTimeFromSystem` and a random value between 100 and 999 to create a unique identifier string. The Unix time provides a base value that changes frequently, while the random component adds further uniqueness. The result is formatted as a string by concatenating the Unix time multiplied by 1000 and the random value.
        /// </remarks>
        public static string GenerateRandomIdFromUnixTime() => $"{Time.GetUnixTimeFromSystem() * 1000}{100 + GD.Randi() % 899 + 1}";

        /// <summary>
        /// Checks if multithreading for rendering is enabled in the project settings.
        /// </summary>
        /// <returns>True if multithreading is enabled, false otherwise.</returns>
        /// <remarks>
        /// This function retrieves the value of the project setting "rendering/driver/threads/thread_model" and checks if it's equal to 2. A value of 2 typically indicates that multithreading for rendering is enabled in the project settings. This information can be useful for checking the current threading configuration.
        /// </remarks>
        public static bool IsMultithreadingEnabled() => ProjectSettings.GetSetting("rendering/driver/threads/thread_model").Equals(2);

        #region Platform detectors
        /// <summary>
        /// Checks if the current platform is considered a mobile device.
        /// </summary>
        /// <returns>False if the platform is web-based, otherwise a Variant containing the result of the JavaScript evaluation (true or false).</returns>
        /// <remarks>
        /// This function attempts to detect if the application is running on a mobile device. It first checks if the platform has the "web" feature, indicating a web-based environment. If not, it uses the `JavaScriptBridge.Eval` method to evaluate a regular expression against the `navigator.userAgent` property (available in web contexts). The regular expression checks for common mobile device user agent patterns. If a match is found, the function returns a Variant containing `true`, otherwise it returns `false`.
        /// </remarks>
        public static Variant IsMobile() {
            if (OS.HasFeature("web"))
                return JavaScriptBridge.Eval("/Android|webOS|iPhone|iPad|iPod|BlackBerry|IEMobile|Opera Mini/i.test(navigator.userAgent)", true);

            return false;
        }

        /// <summary>
        /// Checks if the current operating system is SteamOS, indicating a Steam Deck device.
        /// </summary>
        /// <returns>True if the OS name (case-insensitive) is "SteamOS" or contains any hardware specification related to SteamDeck, false otherwise.</returns>
        public static bool IsSteamDeck() 
            => OS.GetDistributionName().EqualsIgnoreCase("SteamOS") || 
                RenderingServer.GetRenderingDevice().GetDeviceName().Contains("radv vangogh", StringComparison.CurrentCultureIgnoreCase) ||
                OS.GetProcessorName().Contains("amd custom apu 0405", StringComparison.CurrentCultureIgnoreCase);

        /// <summary>
        /// Checks if the current project is an exported release build.
        /// </summary>
        /// <returns>True if the project is an exported release build, false otherwise.</returns>
        /// <remarks>
        /// This function relies on the presence of the "template" feature, which is typically only available in exported builds.
        /// </remarks>
        public static bool IsExportedRelease() => OS.HasFeature("template");

        /// <summary>
        /// Checks if the project is running in the Godot editor.
        /// </summary>
        /// <returns>True if the project is running in the editor, false otherwise.</returns>
        /// <remarks>
        /// This function is a convenient way to differentiate between editor and exported builds.
        /// </remarks>
        public static bool IsEditor() => !IsExportedRelease();

        /// <summary>
        /// Checks if the current operating system is Windows.
        /// </summary>
        /// <returns>True if the operating system is Windows, false otherwise.</returns>
        public static bool IsWindows() => RuntimeInformation.IsOSPlatform(OSPlatform.Windows);

        /// <summary>
        /// Checks if the current operating system is Linux.
        /// </summary>
        /// <returns>True if the operating system is Linux, false otherwise.</returns>
        public static bool IsLinux() => RuntimeInformation.IsOSPlatform(OSPlatform.Linux);

        /// <summary>
        /// Checks if the current operating system is Mac.
        /// </summary>
        /// <returns>True if the operating system is Mac, false otherwise.</returns>
        public static bool IsMac() => RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        #endregion
    }

}