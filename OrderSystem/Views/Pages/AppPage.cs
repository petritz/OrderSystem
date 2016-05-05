using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OrderSystem.Views.Pages
{
    public abstract class AppPage : Page
    {
        private bool loadedView;
        private bool loadedResources;

        public AppPage()
        {
            loadedView = false;
            loadedResources = false;
        }

        public abstract void LoadView();

        public abstract void LoadResources();

        public abstract void ReloadResources();

        public bool LoadedResources
        {
            get { return loadedResources; }
            set { loadedResources = value; }
        }

        public bool LoadedView
        {
            get { return loadedView; }
            set { loadedView = value; }
        }
    }
}