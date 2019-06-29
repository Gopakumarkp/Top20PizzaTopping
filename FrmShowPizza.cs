using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PizzaToppingCombinations
{
    public partial class FrmShowPizza : Form
    {
        public FrmShowPizza()
        {
            InitializeComponent();
        }

        private void btnShowTop20_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            //Calling the function to get top20 pizza topping combinations
            Top20PizzaTopping top20PizzaTopping = new Top20PizzaTopping();
            MessageBox.Show(top20PizzaTopping.GetTop20PizzaToppingCombinations(), "Top 20 Most Frequently Ordered Pizza Topping Combinations");

            Cursor.Current = Cursors.Default;
        }
    }
}
