namespace TRMDesktopUI.ViewModels
{
    using Caliburn.Micro;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class SalesViewModel: Screen
    {
        private BindingList<string> product;

        public BindingList<string> Products
        {
            get { return product; }
            set
            { 
                product = value; 
                NotifyOfPropertyChange(() => Products);
            }
        } 
        
        private BindingList<string> cart;

        public BindingList<string> Cart
        {
            get { return cart; }
            set
            {
                cart = value; 
                NotifyOfPropertyChange(() => Cart);
            }
        }
                

        private string itemQuantity;

        public string ItemQuantity
        {
            get { return itemQuantity; }
            set { 
                itemQuantity = value;
                NotifyOfPropertyChange(() => Products);
            }
        }

        public string Subtotal { get { return "$0.00"; } }

        public string Tax { get { return "$0.00"; } }

        public string Total { get { return "$0.00"; } }

        public bool CanAddToCart
        {
            get
            {
                bool output = false;

                //TODO: Make sure something is selected.
                //TODO: Make sure item is in stock.
                               
                return output;
            }
        }

        public void AddToCart()
        {

        } 
        
        public bool CanRemoveFromCart
        {
            get
            {
                bool output = false;

                //TODO: Make sure something is selected.
                               
                return output;
            }
        }

        public void RemoveFromCart()
        {

        }

        public bool CanCheckout
        {
            get
            {
                bool output = false;

                //TODO: Make sure there is something in cart.

                return output;
            }
        }

        public void Checkout()
        {

        }

    }
}
