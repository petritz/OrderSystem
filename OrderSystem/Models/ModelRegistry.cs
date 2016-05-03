using OrderSystem.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderSystem.Models
{
    class ModelRegistry
    {
        private static ModelRegistry instance;
        private Dictionary<string, MainModel> registry;

        // Constructor

        private ModelRegistry()
        {
            registry = new Dictionary<string, MainModel>();
            registry.Add("user", new UserModel());
            registry.Add("product", new ProductModel());
            registry.Add("order", new OrderModel());
            registry.Add("productLine", new ProductLineModel());
        }
        
        // Functions

        public static MainModel Get(string name)
        {
            return Instance.GetModel(name);
        }

        public MainModel GetModel(string name)
        {
            return registry[name];
        }

        // Properties

        public static ModelRegistry Instance
        {
            get
            {
                if (instance == null) instance = new ModelRegistry();
                return instance;
            }
        }
    }
}
