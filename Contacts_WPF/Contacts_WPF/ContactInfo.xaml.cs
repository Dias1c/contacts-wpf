using Contacts_WPF.Objects;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Newtonsoft.Json;
using System.IO;

namespace Contacts_WPF
{
  /// <summary>
  /// Interaction logic for ContactInfo.xaml
  /// </summary>
  public partial class ContactInfo : Window
  {
    public ContactInfo()
    {
      InitializeComponent();
    }

    Contact _Contact = null;

    private List<string> GetNumbersFromLB_Numbers()
    {
      List<string> Numbers = new List<string>();

      for (int i = 0; i < lb_Numbers.Items.Count; i++)
      {
        Numbers.Add(lb_Numbers.Items[i].ToString());
      }

      return Numbers;
    }
    private void FillNumbersInLb_Numbers(List<string> numbers)
    {
      for (int i = 0; i < numbers.Count; i++)
      {
        lb_Numbers.Items.Add(numbers[i]);
      }
    }
    public void Fill(Contact contact, List<string> contactTypes)
    {
      _Contact = contact;
      sp_Info.DataContext = _Contact;
      FillNumbersInLb_Numbers(contact.Numbers);


      cb_ContactType.ItemsSource = contactTypes;
      if (cb_ContactType.Items.Count != 0)
        cb_ContactType.SelectedIndex = cb_ContactType.Items.IndexOf(contact.ContactType);
    }
    public Contact GetContact()
    {
      _Contact.Numbers = GetNumbersFromLB_Numbers();
      return _Contact;
    }
    private void Tb_LastName_TextChanged(object sender, TextChangedEventArgs e)
    {
      _Contact.LastName = $"{tb_LastName.Text}";
      tb_FullName.Text = $"{_Contact.FullName}";
    }

    private void Tb_FirstName_TextChanged(object sender, TextChangedEventArgs e)
    {
      _Contact.FirstName = $"{tb_FirstName.Text}";
      tb_FullName.Text = $"{_Contact.FullName}";
    }

    private void Tb_FatherName_TextChanged(object sender, TextChangedEventArgs e)
    {
      _Contact.FatherName = $"{tb_FatherName.Text}";
      tb_FullName.Text = $"{_Contact.FullName}";
    }

    private void cb_ContactType_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
      _Contact.ContactType = cb_ContactType.SelectedItem.ToString();
    }


    private void lb_Numbers_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.Key == Key.Delete)
      {
        if (lb_Numbers.SelectedItems.Count != 0)
        {
          int selectedIndex = lb_Numbers.SelectedIndex;
          //Remove
          lb_Numbers.Items.RemoveAt(selectedIndex);
        }
      }
    }

    private void Btn_AddNumber_Click(object sender, RoutedEventArgs e)
    {
      if (!String.IsNullOrEmpty(tb_Number.Text))
      {
        if (!lb_Numbers.Items.Contains(tb_Number.Text))
        {
          lb_Numbers.Items.Add(tb_Number.Text);
          tb_Number.Text = "";
        }
      }
    }
    private void Btn_DeleteNumber_Click(object sender, RoutedEventArgs e)
    {
      if (lb_Numbers.SelectedItems.Count != 0)
      {
        int selectedIndex = lb_Numbers.SelectedIndex;
        //Remove
        lb_Numbers.Items.RemoveAt(selectedIndex);
      }
    }

    private void Mi_SetContactFile_Click(object sender, RoutedEventArgs e)
    {
      try
      {
        OpenFileDialog fileDialog = new OpenFileDialog();
        fileDialog.Filter = "json(*.json)|*.json|json(*.json)|*.json.txt";
        if (fileDialog.ShowDialog() ?? false)
        {
          string filePath = fileDialog.FileName;
          var tContact = JsonConvert.DeserializeObject<Contact>(File.ReadAllText(filePath));
          Fill(tContact, ContactsCollection.ContactTypes);
        }
      }
      catch (Exception ex)
      {
        MessageBox.Show($"{ex.Message}", "Error");
      }
    }

    private void Mi_SaveContactFile_Click(object sender, RoutedEventArgs e)
    {
      SaveFileDialog fileDialog = new SaveFileDialog();
      fileDialog.Filter = "json(*.json)|*.json|json(*.json)|*.json.txt";
      if (fileDialog.ShowDialog() ?? false)
      {
        string filePath = fileDialog.FileName;
        File.WriteAllText(filePath, JsonConvert.SerializeObject(_Contact));
      }
    }
  }
}
