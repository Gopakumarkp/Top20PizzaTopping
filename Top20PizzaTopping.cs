using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace PizzaToppingCombinations
{
    class Top20PizzaTopping
    {
        #region "Public Methods"

        /// <summary>
        /// Function that reads the order that will download orders directly from http://files.olo.com/pizzas.json
        /// and output pizza topping combinations as a list
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, int> GetPizzaToppingCombinationsToOrderedList()
        {
            try
            {
                //Read the orders from http://files.olo.com/pizzas.json
                var result = new HttpClient().GetAsync("http://files.olo.com/pizzas.json").Result.Content.ReadAsStringAsync().Result;

                if (result == null)
                {
                    MessageBox.Show("Cannot Open http://files.olo.com/pizzas.json ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }


                //Deserialize json object  into a list of objects
                List<Pizza> pizzaOrders = JsonConvert.DeserializeObject<List<Pizza>>(result);

                if (pizzaOrders == null || pizzaOrders.Count == 0)
                {
                    MessageBox.Show("Cannot read http://files.olo.com/pizzas.json", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                //list that returns ordered pizza topping combinations
                Dictionary<string, int> SortedToppings = new Dictionary<string, int>();

                //iterating the order
                foreach (Pizza pizza in pizzaOrders)
                {
                    foreach (string topping in pizza.Toppings)
                    {
                        //add first topping combinations
                        if (SortedToppings.ContainsKey(topping) == false)
                            SortedToppings.Add(topping, 1);
                        else
                            //append the list
                            SortedToppings[topping] += 1;
                    }

                }

                //return the list
                return SortedToppings;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return null;                
            }

        }
        /// <summary>
        /// function that convert top 20 list to a string 
        /// </summary>
        /// <returns></returns>
        public string GetTop20PizzaToppingCombinations()
        {
            try
            {
                StringBuilder top20List = new StringBuilder();

                //get the list
                Dictionary<string, int> PizzaList = GetPizzaToppingCombinationsToOrderedList();

                if (PizzaList != null)
                {
                    //string that return the the top 20


                    //iterating the list
                    foreach (KeyValuePair<string, int> topping in PizzaList.OrderByDescending(key => key.Value).Take(20))
                    {
                        //aappending to the list 
                        top20List.AppendLine(string.Format("{0} - {1}",
                                             topping.Key.FirstLetterToUpperCase() //convert the first letter of topping to uppercase
                                             , topping.Value));
                    }

                }
                if (top20List.Length == 0)
                {
                    return "Failed to read http://files.olo.com/pizzas.json";
                }

                return top20List.ToString();
            }

            catch (Exception ex)
            {
                return ex.Message;               
            }

        }

        #endregion "Public Methods"
    }

    #region "Helper Classes"

    /// <summary>
    /// Object that deserialize object json
    /// </summary>
    partial class Pizza
    {
        public List<string> Toppings = new List<string>();
    }

    /// <summary>
    /// extension class to a string that convert first letter of a string upper case
    /// </summary>
    public static class StringExtensions
    {
        public static string FirstLetterToUpperCase(this string s)
        {
            if (string.IsNullOrEmpty(s))
                throw new ArgumentException("No first letter in the string");

            char[] a = s.ToCharArray();
            a[0] = char.ToUpper(a[0]);
            return new string(a);
        }
    }

    #endregion "Helper Classes"
}
