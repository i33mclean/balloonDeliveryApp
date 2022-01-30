// Programmer: Ian McLean
// Project: McLean_3
// Due Date: 07/02/2021
// Description: Individual Assigment #3
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

namespace McLean_3
{
    public partial class Form1 : Form
        // Declare constants
    {
        const decimal homeDeliveryCharge = 7.50M;
        const decimal singleCharge = 9.95M;
        const decimal halfDozenCharge = 35.95M;
        const decimal DozenCharge = 65.95M;
        const decimal personalMessage = 2.50M;
        const decimal salesTax = 0.07M;
        const decimal extrasCharge = 9.50M;
               

        // Execute when program starts
        public Form1()
        {
            InitializeComponent();
            specialOccasionComboBox.SelectedIndex = (-1);
            deliveryDateTextBox.Text = DateTime.Now.ToString("MM/dd/yyyy");
            titleComboBox.Focus();
            PopulateBoxes();
            ResetForm();
            UpdateTotals();
            toolTip1.SetToolTip(this.displaySummaryButton, "Display Summary");
            toolTip1.SetToolTip(this.clearFormButton, "Clear Form");
            toolTip1.SetToolTip(this.exitProgramButton, "Exit Program");
        }

        // Update Totals Method
        private void UpdateTotals()
        {
            var subTotal = 0M;
            if (homeDeliveryRadioButton.Checked)
            {
                subTotal += homeDeliveryCharge; 
            }
            if (singleRadioButton.Checked)
            {
                subTotal += singleCharge;
            }
            if (halfDozenRadioButton.Checked)
            {
                subTotal += halfDozenCharge;
            }
            if (dozenRadioButton.Checked)
            {
                subTotal += DozenCharge;
            }
            if (personalMessageCheckBox.Checked)
            {
                subTotal += personalMessage;
            }

            subTotal += extrasListBox.SelectedItems.Count * extrasCharge;

            subTotalLabel.Text = subTotal.ToString("c");

            salesTaxLabel.Text = (subTotal * salesTax).ToString("c");

            orderTotalMaskedTextBox.Text = (Convert.ToDouble(subTotal * (1 + salesTax))).ToString("C");


        }
        
        // Reset Form Method
        private void ResetForm()
        {
            // Clear form click event

            titleComboBox.SelectedIndex = (-1);
            firstNameTextBox.Text = "";
            lastNameTextBox.Text = "";
            streetTextBox.Text = "";
            cityTextBox.Text = "";
            stateTextBox.Text = "";
            zipMaskedTextBox.Text = "";
            phoneNumberMaskedTextBox.Text = "";
            deliveryDateTextBox.Text = "";
            specialOccasionComboBox.SelectedIndex = (-1);
            personalMessageCheckBox.Checked = false;
            personalMessageTextBox.Text = "";
            subTotalLabel.Text = "";
            salesTaxLabel.Text = "";
            orderTotalMaskedTextBox.Text = "";

            titleComboBox.Focus();
        }

        // Populate Boxes Method
        private void PopulateBoxes()
        {
            try
            {
                var specialOccasions = File.ReadAllLines("Occasions.txt");
                foreach (var occasion in specialOccasions)
                {

                    specialOccasionComboBox.Items.Add(occasion);
                }
                var extras = File.ReadAllLines("Extras.txt");
                foreach (var extra in extras)
                {

                    extrasListBox.Items.Add(extra);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Exception occurred when populating boxes.{Environment.NewLine}{ex.Message}", "Exception Occurred", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Displaying $7.50 when the Home Delivery Radio Button is checked
        private void homeDeliveryRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (homeDeliveryRadioButton.Checked)
            {
                priceLabel.Text = homeDeliveryCharge.ToString();
            }
            else
            {
                priceLabel.Text = "";
            }

            UpdateTotals();
        }
        // Clear Form click event
        private void clearFormButton_Click(object sender, EventArgs e)
        {
            ResetForm();
        }
        
        // Exit Button click event
        private void exitProgramButton_Click(object sender, EventArgs e)
        {
            DialogResult dialog = MessageBox.Show("Are you sure you wish to quit?", "Do you want to quit?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialog == DialogResult.Yes)
            {
                this.Close();
            }

        }

        // Radio Button Check Changed event
        private void singleRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            UpdateTotals();
        }

        // Display Summary Click Event
        private void displaySummaryButton_Click(object sender, EventArgs e)
        {
            var bundleSize = singleRadioButton.Checked ? "Single Bundle" : halfDozenRadioButton.Checked ? "Half-Dozen Bundle" : dozenRadioButton.Checked ? "Dozen Bundle" : "";
            var stringBuilder = new StringBuilder();

            stringBuilder.AppendLine("Customer Name: " + titleComboBox.SelectedIndex + " " + firstNameTextBox.Text + " " + lastNameTextBox.Text);
            stringBuilder.AppendLine("Customer Street: " + streetTextBox.Text + " " + cityTextBox.Text + ", " + stateTextBox.Text + " " + zipMaskedTextBox.Text);
            stringBuilder.AppendLine("Customer Phone: " + phoneNumberMaskedTextBox.Text);
            stringBuilder.AppendLine("Delivery Date: " + deliveryDateTextBox.Text);
            stringBuilder.AppendLine("Delivery Type: " + storePickUpRadioButton.Checked + ", " + homeDeliveryRadioButton.Checked);
            stringBuilder.AppendLine("Bundle Size: " + bundleSize);
            stringBuilder.AppendLine("Occasion: " + specialOccasionComboBox.SelectedIndex);
            stringBuilder.AppendLine("Extras:");
            foreach (object item in extrasListBox.SelectedItems)
            {
                stringBuilder.AppendLine(" " + item.ToString());
            }
            stringBuilder.AppendLine("Message: " + personalMessageTextBox.Text);
            stringBuilder.AppendLine("Order Subtotal: " + subTotalLabel.Text);
            stringBuilder.AppendLine("Sales Tax Amount: " + salesTaxLabel.Text);
            stringBuilder.AppendLine("Order Total: " + orderTotalMaskedTextBox.Text);

            MessageBox.Show(stringBuilder.ToString(), "Bonnie's Balloons Order Summary", MessageBoxButtons.OKCancel);
        }
    }
}
