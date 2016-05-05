using OrderSystem.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderSystem.Enums;

namespace OrderSystem.Models
{
    class ModelRegistry
    {
        private static ModelRegistry instance;
        private Dictionary<ModelIdentifier, MainModel> registry;

        // Constructor

        private ModelRegistry()
        {
            registry = new Dictionary<ModelIdentifier, MainModel>();
            registry.Add(ModelIdentifier.User, new UserModel());
            registry.Add(ModelIdentifier.Product, new ProductModel());
            registry.Add(ModelIdentifier.Order, new OrderModel());
            registry.Add(ModelIdentifier.ProductLine, new ProductLineModel());
        }

        // Functions

        public static MainModel Get(ModelIdentifier name)
        {
            return Instance.GetModel(name);
        }

        public MainModel GetModel(ModelIdentifier name)
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