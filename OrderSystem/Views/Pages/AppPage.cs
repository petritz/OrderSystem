using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace OrderSystem.Views.Pages
{
    /// <summary>
    /// The base class for every app page showing in the root app. Implements loading mechanism.
    /// </summary>
    public abstract class AppPage : Page
    {
        private bool loadedView;
        private bool loadedResources;

        protected AppPage()
        {
            loadedView = false;
            loadedResources = false;
        }

        /// <summary>
        /// Should be implemented in order to create the view elements
        /// </summary>
        public abstract void LoadView();

        /// <summary>
        /// Should be implemented to load the resources
        /// </summary>
        public abstract void LoadResources();

        /// <summary>
        /// Should be implemented to reload the resources
        /// </summary>
        public abstract void ReloadResources();

        /// <summary>
        /// Determines if the resources were loaded or not
        /// </summary>
        public bool LoadedResources
        {
            get { return loadedResources; }
            set { loadedResources = value; }
        }

        /// <summary>
        /// Determines if the view was loaded or not
        /// </summary>
        public bool LoadedView
        {
            get { return loadedView; }
            set { loadedView = value; }
        }
    }
}