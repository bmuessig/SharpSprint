using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using TestStack.White;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.MenuItems;
using TestStack.White.UIItems.WindowStripControls;
using TestStack.White.Factory;
using TestStack.White.UIItems;
using System.Text.RegularExpressions;
using TestStack.White.Recording;
using TestStack.White.UIItemEvents;

namespace SharpSprint.Control
{
    public class RemoteControl
    {
        public Application LayoutApp;
        public Window LayoutWindow;

        public string FileName { get; private set; }
        public string Version { get; private set; }

        Regex TitleRegex = new Regex(@"Sprint-Layout (\d\.\d+)(?: - \[(.*)\])?", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public RemoteControl()
        {

        }

        public bool Launch(
            string Executable = @"C:\Program Files (x86)\Layout60\Layout60.exe",
            string Arguments = "")
        {
            if (!Executable.ToLower().EndsWith(".exe"))
                return false;
            if (!File.Exists(Executable))
                return false;

            LayoutApp = Application.Launch(new System.Diagnostics.ProcessStartInfo(Executable, Arguments));
            return (!LayoutApp.HasExited);
        }

        public bool Attach()
        {
            if (LayoutApp == null)
                return false;
            if (LayoutApp.HasExited)
                return false;

            Window[] windows = LayoutApp.GetWindows().ToArray();
            foreach (Window window in windows)
            {
                if (window.Name.StartsWith("Sprint-Layout"))
                {
                    LayoutWindow = window;
                    return true;
                }
            }

            return false;
        }

        public bool UpdateInfo()
        {
            if (LayoutApp == null || LayoutWindow == null)
                return false;
            if (LayoutApp.HasExited || LayoutWindow.IsClosed)
                return false;

            Match m = TitleRegex.Match(LayoutWindow.Title);
            if (!m.Success)
                return false;
            if (m.Groups.Count != 3)
                return false;

            if (!m.Groups[2].Success || string.IsNullOrWhiteSpace(m.Groups[2].Value))
                FileName = string.Empty;
            else
                FileName = m.Groups[2].Value.Trim();

            if (!m.Groups[1].Success || string.IsNullOrWhiteSpace(m.Groups[1].Value))
            {
                Version = string.Empty;
                return false;
            }

            return true;
        }

        public bool Export(string FileName)
        {
            if (LayoutApp == null || LayoutWindow == null)
                return false;
            if (LayoutApp.HasExited || LayoutWindow.IsClosed)
                return false;

            MenuBar menu = LayoutWindow.Get<MenuBar>(SearchCriteria.ByAutomationId("MenuBar"));
            if (menu == null)
                return false;
            Menu extra = menu.MenuItemBy(SearchCriteria.ByAutomationId("Item 5"));
            if (extra == null)
                return false;

            if (extra.ChildMenus.Count < 3)
                return false;
            Menu export = extra.ChildMenus[extra.ChildMenus.Count - 2];
            if (export == null)
                return false;
            if (!export.Enabled)
                return false;
            export.Click();
            return HandleFileDialog(FileName, false);
        }

        public bool Import(string FileName)
        {
            if (LayoutApp == null || LayoutWindow == null)
                return false;
            if (LayoutApp.HasExited || LayoutWindow.IsClosed)
                return false;

            MenuBar menu = LayoutWindow.Get<MenuBar>(SearchCriteria.ByAutomationId("MenuBar"));
            if (menu == null)
                return false;
            Menu extra = menu.MenuItemBy(SearchCriteria.ByAutomationId("Item 5"));
            if (extra == null)
                return false;

            if (extra.ChildMenus.Count < 3)
                return false;
            Menu import = extra.ChildMenus[extra.ChildMenus.Count - 1];
            if (import == null)
                return false;
            if (!import.Enabled)
                return false;
            import.Click();
            return HandleFileDialog(FileName, true);
        }

        public bool CreateFileName(out string Result, bool UseOriginalFilename = true)
        {
            Result = string.Empty;

            if (LayoutApp == null || LayoutWindow == null)
                return false;
            if (LayoutApp.HasExited || LayoutWindow.IsClosed)
                return false;

            if(FileName == null)
                FileName = string.Empty;

            Result = Path.Combine(Path.GetTempPath(), string.Format("SharpSprint_{0}{1}_{2}.txt",
                LayoutApp.Process.Id.ToString("X8"),
                (!string.IsNullOrWhiteSpace(FileName) && UseOriginalFilename) ? "" : "_" + Path.GetFileNameWithoutExtension(FileName),
                DateTime.Now.Millisecond));

            return true;
        }

        private bool HandleFileDialog(string FileName, bool OpenFile = true)
        {
            if (LayoutApp == null || LayoutWindow == null)
                return false;
            if (LayoutApp.HasExited || LayoutWindow.IsClosed)
                return false;

            if(string.IsNullOrWhiteSpace(FileName))
                return false;

            Window userDialog = LayoutApp.GetWindow(SearchCriteria.ByClassName("#32770"), InitializeOption.NoCache);
            if (userDialog == null)
                return false;

            Button okButton = userDialog.Get<Button>(SearchCriteria.ByAutomationId("1"));

            if (okButton == null)
                return false;

            // Check if the new window has a file name property
            if (!userDialog.Exists<TextBox>(SearchCriteria.ByAutomationId("1148")))
            {
                // Then just click the button and wait
                okButton.Click();

                // WAIT

                // Find the new window
                userDialog = LayoutApp.GetWindow(SearchCriteria.ByClassName("#32770"), InitializeOption.NoCache);
                if (userDialog == null)
                    return false;

                // Find the new button
                okButton = userDialog.Get<Button>(SearchCriteria.ByAutomationId("1"));
            }

            TextBox fileName = userDialog.Get<TextBox>(SearchCriteria.ByAutomationId("1148"));
            Button cancelButton = userDialog.Get<Button>(SearchCriteria.ByAutomationId("2"));
            if (fileName == null || okButton == null || cancelButton == null)
                return false;

            if (OpenFile && !File.Exists(FileName))
            {
                cancelButton.Click();
                return false;
            }
            else
            {
                if (!OpenFile && File.Exists(FileName))
                    File.Delete(FileName);

                fileName.Text = FileName;
                okButton.Click();
            }

            return true;
        }
    }
}
