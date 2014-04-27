using Microsoft.Phone.Shell;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WorkTajm.ViewModel
{
    class Progress
    {
        private ProgressIndicator progress;
        public Progress()
        {
            progress = new ProgressIndicator();
            progress.IsVisible = true;
            progress.IsIndeterminate = true;
            progress.Value = 0.5;            
            SystemTray.ProgressIndicator = progress;
            SystemTray.Opacity = 0.5;
        }

        public string Text
        {
            set
            {
                if (value == null)
                {
                    progress.IsIndeterminate = true;
                }
                else
                {
                    progress.IsIndeterminate = false;
                    progress.Text = value;
                    progress.IsIndeterminate = true;
                }
            }
        }

        public double Value
        {
            set
            {
                progress.Value = value;
                progress.IsIndeterminate = false;
            }
        }

        public bool Visible
        {
            set
            {
                progress.IsVisible = false;
            }
        }

        ~Progress()
        {
            //progress.IsVisible = false;
        }
    }
}
