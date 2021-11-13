using Contacts_WPF.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Contacts_WPF
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    ContactsCollection Contacts = new ContactsCollection();

    ///Тип Сортировки
    Comparison<Contact> SortType = ((a, b) => { return a.FullName.CompareTo(b.FullName); });

    ///Counter для Contacts
    public int CounterID = 0;

    public MainWindow()
    {
      InitializeComponent();

      lb_ContactBox.ItemsSource = Contacts.Contacts;

      CounterID = Contacts.GetMaxID() + 1;
    }

    private void Tb_FindContact_TextChanged(object sender, TextChangedEventArgs e)
    {
      //Поиск контактов по Имени и Номерам
      if (tb_FindContact.Text.Length != 0)
      {
        //Присваиваем только подходяшие контакты
        lb_ContactBox.ItemsSource = Contacts.Contacts.Where(c =>
            c.FullName.ToUpper().Contains(tb_FindContact.Text.ToUpper()) ||
            (c.Numbers.FirstOrDefault(n => n.Contains(tb_FindContact.Text)) != null ? true : false)
        );
      }
      else
      {
        //Присваиваем все контакты
        lb_ContactBox.ItemsSource = Contacts.Contacts;
      }
    }

    private void Window_Closing(object sender, CancelEventArgs e)
    {
      //SaveJson
      Contacts.SaveToJson();
    }

    private void Lb_ContactBox_KeyDown(object sender, KeyEventArgs e)
    {
      //Remove Selected Contacts
      if (e.Key == Key.Delete)
      {
        if (lb_ContactBox.SelectedItems.Count != 0)
        {
          List<int> selectedIDs = new List<int>();
          for (int i = 0; i < lb_ContactBox.SelectedItems.Count; i++)
          {
            selectedIDs.Add(((Contact)lb_ContactBox.SelectedItems[i]).ID);
          }
          //Remove Contacts
          Contacts.RemoveContactsByID(selectedIDs);
          lb_ContactBox.Items.Refresh();
        }
      }
      //Refresh Getting Contacts From json
      else if ((Keyboard.Modifiers == ModifierKeys.Control) && (e.Key == Key.R))
      {
        Contacts.GetContactsFromJson();
        Contacts.Sort(SortType);
        lb_ContactBox.ItemsSource = Contacts.Contacts;
      }
    }
    private void Btn_AddContact_Click(object sender, RoutedEventArgs e)
    {
      ContactInfo w = new ContactInfo();
      w.Fill(new Contact(), ContactsCollection.ContactTypes);
      w.ShowDialog();

      //Getting Contact From
      var tContact = w.GetContact();
      tContact.ID = CounterID++;
      //Добавляем контакт если он ен пустой
      if (!tContact.IsEmpty())
        Contacts.Add(tContact);

      //Sorting Contacts
      Contacts.Sort(SortType);
      //Contacts.SortByFullName();
      lb_ContactBox.Items.Refresh();

      //SaveJson
      Contacts.SaveToJson();
    }
    private void Btn_EditContact_Click(object sender, RoutedEventArgs e)
    {
      if (lb_ContactBox.SelectedIndex != -1)
      {
        ContactInfo w = new ContactInfo();
        w.Fill((Contact)lb_ContactBox.SelectedItem, ContactsCollection.ContactTypes);
        w.ShowDialog();

        //Getting Contact From
        Contacts.Contacts.Remove((Contact)lb_ContactBox.SelectedItems[0]);
        var tContact = w.GetContact();
        tContact.ID = CounterID++;
        Contacts.Add(tContact);

        //Sorting Contacts
        Contacts.Sort(SortType);
        lb_ContactBox.Items.Refresh();

        //SaveJson
        Contacts.SaveToJson();

      }
    }

    private void Btn_DeleteContact_Click(object sender, RoutedEventArgs e)
    {
      //Remove Selected Contacts
      if (lb_ContactBox.SelectedItems.Count != 0)
      {
        List<int> selectedIDs = new List<int>();
        for (int i = 0; i < lb_ContactBox.SelectedItems.Count; i++)
        {
          selectedIDs.Add(((Contact)lb_ContactBox.SelectedItems[i]).ID);
        }

        //Remove Contacts
        Contacts.RemoveContactsByID(selectedIDs);
        lb_ContactBox.Items.Refresh();
        
        //SaveJson
        Contacts.SaveToJson();
      }
    }

    private void Mi_SortByFullName_Click(object sender, RoutedEventArgs e)
    {
      SortType = ((a, b) => { return a.FullName.CompareTo(b.FullName); });
      Contacts.Sort(SortType);
      lb_ContactBox.Items.Refresh();
    }

    private void Mi_SortByFirstName_Click(object sender, RoutedEventArgs e)
    {
      SortType = ((a, b) => { return a.FirstName.CompareTo(b.FirstName); });
      Contacts.Sort(SortType);
      lb_ContactBox.Items.Refresh();
    }

    private void Mi_SortByFatherName_Click(object sender, RoutedEventArgs e)
    {
      SortType = ((a, b) => { return a.FatherName.CompareTo(b.FatherName); });
      Contacts.Sort(SortType);
      lb_ContactBox.Items.Refresh();
    }

    private void Mi_SortByNumbers_Click(object sender, RoutedEventArgs e)
    {
      SortType = ((a, b) => { return a.Number.CompareTo(b.Number); });
      Contacts.Sort(SortType);
      lb_ContactBox.Items.Refresh();
    }

    private void Mi_SortByBirthDay_Click(object sender, RoutedEventArgs e)
    {
      SortType = ((a, b) => { return a.BirthDate.CompareTo(b.BirthDate); });
      Contacts.Sort(SortType);
      lb_ContactBox.Items.Refresh();
    }
  }
}
