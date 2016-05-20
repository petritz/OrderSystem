using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrderSystemLibrary.Database;
using OrderSystemLibrary.Enums;

namespace OrderSystemLibrary.Models
{
    /// <summary>
    /// This class stoers all model references. The models are initialized when the instance was accessed first.
    /// </summary>
    public class ModelRegistry
    {
        private static ModelRegistry instance;
        private Dictionary<ModelIdentifier, MainModel> registry;

        private ModelRegistry()
        {
            registry = new Dictionary<ModelIdentifier, MainModel>();
            registry.Add(ModelIdentifier.User, new UserModel());
            registry.Add(ModelIdentifier.Product, new ProductModel());
            registry.Add(ModelIdentifier.Order, new OrderModel());
            registry.Add(ModelIdentifier.ProductLine, new ProductLineModel());
            registry.Add(ModelIdentifier.Credit, new CreditModel());
        }

        /// <summary>
        /// Get the model associated to the identifier. This method calls the Instance method.
        /// </summary>
        /// <param name="name">The identifier</param>
        /// <returns>The reference to the model</returns>
        public static MainModel Get(ModelIdentifier name)
        {
            return Instance.GetModel(name);
        }

        /// <summary>
        /// Get the model associated to the identifier.
        /// </summary>
        /// <param name="name">The identifier</param>
        /// <returns>The reference to the model</returns>
        public MainModel GetModel(ModelIdentifier name)
        {
            return registry[name];
        }

        /// <summary>
        /// The instance
        /// </summary>
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