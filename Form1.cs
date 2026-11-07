using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LuckyCafeOrder
{
    public partial class Form1 : Form

    {
        const double TeaPrice = 1.00;
        const double CoffeePrice = 2.00;
        const double JuicePrice = 3.00;
        const double DiscountRate = 0.10;

        double total = 0;
        string luckyMessage = "No prize this time";
        public Form1()
        {
            InitializeComponent();
        }

        private void btnCalculate_Click(object sender, EventArgs e)
        {

            string customername = txtCustomerName.Text;

            if (customername == "")
            {


                MessageBox.Show("Please enter customer name");
                return;
            }
            bool validName = true;
            for(int i =0; i<customername.Length; i++)
            {
                if (char.IsLetter(customername[i]) == false && customername[i] != ' ')
                {
                    validName= false;
                }
            }
            if (validName == false) {
                MessageBox.Show("customer must be only letters");
            }



            int quantity;
            try
            {
                quantity = int.Parse(txtQuantity.Text);
            }
            catch
            {
                MessageBox.Show("Please enter a valid quantity");
                return;
            }
            if (quantity < 1)
            {
                MessageBox.Show("Quantity must be at least 1.");
            }

            double drinkPrice = 0;
            string drinkName = "";
            if (rdoTea.Checked)
            {
                drinkPrice = TeaPrice;
                drinkName = "Tea";
            }
            else if (rdoCoffee.Checked)
            {
                drinkPrice = CoffeePrice;
                drinkName = "Coffee";
            }
            else if (rdoJuice.Checked)
            {
                drinkPrice = JuicePrice;
                drinkName = "Juice";

            }
            else
            {
                MessageBox.Show("Please select a drink");
                return;
            }
            lstOrderList.Items.Clear();
            Random random = new Random();
            Boolean gotPrize = false;
            int prizeNumber = 0;

            for (int i = 1; i <= quantity; i++)
            {
                int luckyNumber = random.Next(1, 51);
                if (luckyNumber == 7 || luckyNumber == 17)
                {
                    gotPrize = true;
                    prizeNumber= luckyNumber;
                }
                string order = i + ") " + customername + " _ " + drinkName +
                    " _ lucky " + luckyNumber + " _ $" + drinkPrice.ToString("0.00");
                lstOrderList.Items.Add(order);
            }
            total = drinkPrice * quantity;
            double totalBeforeDiscount = total;
            if (chkStudentMember.Checked)
            {
                total = total - (total * DiscountRate);
                lstOrderList.Items.Add("Member Discount 10%");
            }
            else {
                lstOrderList.Items.Add("Member Discount 0%");
            }
            lstOrderList.Items.Add("Total Before Discount $" +
                totalBeforeDiscount.ToString("0.00"));
            lstOrderList.Items.Add("Total After Discount $" + total.ToString("0.00"));
         
           

            lblTotalPrice.Text = "$" + total.ToString("0.00");

            if (gotPrize == true)
            {
                luckyMessage = "You got a free cookie!";
                lblLuckyMessage.Text = luckyMessage;
                lstOrderList.Items.Add("You got a free cookie!" + "(" + prizeNumber + ")");


            }
            else
            {
                luckyMessage = "No prize this time";
                lblLuckyMessage.Text = luckyMessage;

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (lstOrderList.Items.Count == 0)
            {
                MessageBox.Show("No order to save");
                return;
            }
            try
            {
                StreamWriter outputFile;
                outputFile = File.AppendText("Order.txt");
                for (int i = 0; i < lstOrderList.Items.Count; i++)
                {
                    outputFile.WriteLine(lstOrderList.Items[i]);
                }
                outputFile.WriteLine("Total price :" + total.ToString("0.00"));
                outputFile.WriteLine("Lucky message " + luckyMessage);
                outputFile.WriteLine("------------------------------");
                outputFile.Close();
                MessageBox.Show("Order saved");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

            }
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (File.Exists("Order.txt") == false)
            {
                MessageBox.Show("No file foud");
                return;
            }
            try
            {
                lstOrderList.Items.Clear();
                StreamReader inputFile;
                inputFile = File.OpenText("Order.txt");
                while (inputFile.EndOfStream == false)
                {
                    string line = inputFile.ReadLine();
                    lstOrderList.Items.Add(line);
                   
                }
                inputFile.Close();
            }
            catch ( Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtCustomerName.Clear();
            txtQuantity.Clear();
            lstOrderList.Items.Clear ();
            lblTotalPrice.Text = "0.00";
            lblLuckyMessage.Text = "No prize this time";
            chkStudentMember.Checked = false;
            rdoCoffee.Checked = false;
            rdoJuice.Checked = false;
            rdoTea.Checked = false;

            txtCustomerName.Focus();


        }
    }
}
